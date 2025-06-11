using CommunityToolkit.Maui.Views;
using impakt_maui_app.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
//using GameController;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace impakt_maui_app
{
    public partial class QRScanner : ContentPage
    {
        enum QRScanType: ushort
        {
            None = 0,
            CheckIn,
            UpdatePassType,
            GetUserInfo,
        }

        /* Data will be needed during existance of the page */
        private QRScanType scanType;

        private string uptdate_pass_details_url;
        private string get_user_info_url;
        private string checkin_member_url;

        public QRScanner() 
        {
            InitializeComponent();

            /* Set Barcode Reader Options */
            QRCodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormat.QrCode, // Set to recognize EAN-13 barcodes
                AutoRotate = true, 
                Multiple = false,
            };

            /* Data initialzation */
            scanType = QRScanType.None;

            uptdate_pass_details_url = string.Format("{0}/members/update/", Network.URL);
            get_user_info_url = string.Format("{0}/members/{1}/get/member_info", Network.URL, UserInfo.Card_ID);
            checkin_member_url = string.Format("{0}/members/{1}/checkin", Network.URL, UserInfo.Card_ID);
        }

        private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
        {
            // Block possibility to scan untill response come from the backend
            QRCodeReader.IsDetecting = false;

            var value = e.Results.FirstOrDefault()?.Value;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (!string.IsNullOrEmpty(value))
                {
                    HttpClient client = new HttpClient();

                    // Call API depends on choosen parameters
                    switch (scanType)
                    {
                        case QRScanType.CheckIn:
                            try
                            {
                                // Assemble request
                                BackendReq_CheckInMember req = new BackendReq_CheckInMember
                                {
                                    card_id = value,
                                };

                                // Process http request
                                HttpResponseMessage response = await client.PostAsJsonAsync(checkin_member_url, req);
                                if (response.IsSuccessStatusCode)
                                {
                                    await DisplayAlert("Member was checked in", value, "OK");
                                    await Navigation.PopAsync();
                                }
                                else
                                {
                                    string error_message = await response.Content.ReadAsStringAsync();
                                    await DisplayAlert(error_message, value, "OK");
                                    await Navigation.PopAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                            break;

                        case QRScanType.UpdatePassType:
                            try
                            {
                                PassType pass_type = (PassType)Enum.Parse(typeof(PassType), (string)PickerPassType.SelectedItem);
                                BackendReq_UpdatePassDetails pass_details = new BackendReq_UpdatePassDetails
                                {
                                    card_id = value,
                                    pass_type = (int)pass_type,
                                    entrances_left = Convert.ToInt16(PickerPassEntrances.SelectedItem),
                                    expiration_date = DateOnly.FromDateTime(DateTime.Today.AddDays(35)),
                                };

                                // Process http request
                                HttpResponseMessage response = await client.PostAsJsonAsync(uptdate_pass_details_url, pass_details);
                                if (response.IsSuccessStatusCode)
                                {
                                    ;
                                }
                                else
                                {
                                    ;
                                }
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                            break;

                        case QRScanType.GetUserInfo:
                            try
                            {
                                // Assemble request
                                BackendReq_MemberInfo req = new BackendReq_MemberInfo
                                {
                                    card_id = value,
                                };

                                // Process http request
                                HttpResponseMessage response = await client.PostAsJsonAsync(get_user_info_url, req);
                                if (response.IsSuccessStatusCode)
                                {
                                    BackendResp_MemberInfo member_info = await response.Content.ReadFromJsonAsync<BackendResp_MemberInfo>();
                                    await Shell.Current.ShowPopupAsync(new MemberInfoPopup(member_info));
                                }
                                else
                                {
                                    ;
                                }

                                    
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                            
                            break;
                    }

                    // Restore possibility to scan after response is obtained
                    QRCodeReader.IsDetecting = true;
                }
            });
        }

        private void OnAnyScanButtobClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                switch(btn.ClassId)
                {
                    case "CheckIn":
                        scanType = QRScanType.CheckIn;
                        break;
                    case "CheckEntrancesToday":
                        break;
                    case "UpdatePassData":
                        scanType = QRScanType.UpdatePassType;
                        PickerPassType.IsVisible = true;
                        PickerPassEntrances.IsVisible = true;
                        break;
                    case "GetUserInfo":
                        scanType = QRScanType.GetUserInfo;
                        break;
                }
            }

            // Hide current pannel and display scanner
            ButtonPanel.IsVisible = false;
            ScanningPannel.IsVisible = true;
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            // Hide scanner and reveal buttons
            ButtonPanel.IsVisible = true;
            ScanningPannel.IsVisible = false;

            // Restore values
            switch (scanType)
            {
                case QRScanType.CheckIn:
                    break;
                case QRScanType.UpdatePassType:
                    PickerPassType.IsVisible = false;
                    PickerPassEntrances.IsVisible = false;
                    break;
                case QRScanType.GetUserInfo:
                    break;
            }
            scanType = QRScanType.None;
        }

        private async void OnToMainPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
            return;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /* Select picker items */
            PickerPassType.SelectedItem = (string)"No";
            PickerPassEntrances.SelectedItem = (short)4;
        }
    }
}
