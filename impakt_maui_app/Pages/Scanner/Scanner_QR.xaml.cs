using impakt_maui_app.VM.Scanner;
using ZXing.Net.Maui;

namespace impakt_maui_app.Pages.Scanner;

public partial class Scanner_QR : ContentPage
{
	public Scanner_QR()
	{
		InitializeComponent();
    }

    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs args)
    {
        /* This function exists because "toolkit:EventToCommandBehavior" failed to work on zxing lib */

        if (BindingContext is VM_Scanner_QR vm)
        {
            await vm.BarcodeDetected(args);
        }
    }
}