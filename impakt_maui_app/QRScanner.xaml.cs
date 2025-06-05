using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        /* Data will be needed during existance of the page */
        private QRScanType scanType;

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
                    await DisplayAlert("QR Code", value, "OK");
                    await Navigation.PopAsync();

                    // Call the API with chosen previousely option
                    await Task.Delay(1000); // Temporarly emulates databases response

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
        }

        private async void OnToMainPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
            return;
        }
    }
}
