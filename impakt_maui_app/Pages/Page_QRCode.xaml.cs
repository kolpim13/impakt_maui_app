namespace impakt_maui_app.Pages;

public partial class Page_QRCode : ContentPage
{
    private float? _original_brightness;
	public Page_QRCode(string card_id)
	{
        InitializeComponent();
        QrCodeView.Value = card_id;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}