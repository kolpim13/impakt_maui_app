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
        }

        /* Data will be needed during existance of the page */
        private QRScanType scanType;

        private string uptdate_pass_details_url;

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
                    // Call API depends on choosen parameters
                    switch(scanType)
                    {
                        case QRScanType.UpdatePassType:
                            PassType pass_type = (PassType)Enum.Parse(typeof(PassType), (string)PickerPassType.SelectedItem);
                            BackendReq_UpdatePassDetails pass_details = new BackendReq_UpdatePassDetails
                            {
                                card_id = value,
                                pass_type = (int)pass_type,
                                entrances_left = Convert.ToInt16(PickerPassEntrances.SelectedItem),
                                expiration_date = DateOnly.FromDateTime(DateTime.Today.AddDays(35)),
                            };

                            HttpClient client = new HttpClient();
                            HttpResponseMessage response = await client.PostAsJsonAsync(uptdate_pass_details_url, pass_details);
                            if (response.IsSuccessStatusCode)
                            {

                            }
                            else
                            {

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
