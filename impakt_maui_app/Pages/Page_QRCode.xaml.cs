namespace impakt_maui_app.Pages;

public partial class Page_QRCode : ContentPage
{
    readonly IScreenBrightness _brightness;

    public Page_QRCode(string card_id)
	{
        InitializeComponent();
        QrCodeView.Value = card_id;
        _brightness = new ScreenBrightnessService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if ANDROID
        _brightness.SetMaximum();
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
#if ANDROID
        _brightness.RestorePreviousValue();
#endif
    }
}