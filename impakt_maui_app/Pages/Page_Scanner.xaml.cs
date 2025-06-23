namespace impakt_maui_app.Pages;

public enum QRScanMode : ushort
{
    None,
    CheckIn,
}

public partial class Page_Scanner : ContentPage
{
	public Page_Scanner()
	{
		InitializeComponent();
	}

    private async void OnClicked_CheckIn(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Page_Scanner_QRScanner?mode=CheckIn");
    }
}