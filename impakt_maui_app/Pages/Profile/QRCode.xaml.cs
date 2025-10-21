namespace impakt_maui_app.Pages.Profile;

[QueryProperty(nameof(CardId), "CardId")]
public partial class QRCode : ContentPage
{
    readonly IScreenBrightness _brightness;

    public string? CardId { get; set; } = "0000000000";

    public QRCode()
	{
        InitializeComponent();
        _brightness = new ScreenBrightnessService();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        QrCodeView.Value = CardId;
        Label_CardID.Text = CardId;
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

    private async void OnShareQrClicked(object sender, EventArgs e)
    {
        try
        {
            var screenshot = await QrCodeView.CaptureAsync();
            if (screenshot is null)
            {
                await DisplayAlert("Error", "Couldn’t capture the QR image.", "OK");
                return;
            }

            using var pngStream = await screenshot.OpenReadAsync();

            // 2) Persist to a temporary file (so other apps can read it via FileProvider)
            var fileName = $"impact_member_qr_{DateTime.UtcNow:yyyyMMdd_HHmmssfff}.png";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            using (var fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await pngStream.CopyToAsync(fs);
            }

            // 3) Share via the native Android share sheet.
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Share QR Code",
                File = new ShareFile(filePath)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Share failed", ex.Message, "OK");
        }
    }
}