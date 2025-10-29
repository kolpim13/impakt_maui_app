namespace impakt_maui_app.Pages.Profile;

public partial class Page_ProfileMember : ContentPage
{
	public Page_ProfileMember()
	{
		InitializeComponent();
	}

    private async void Btn_OnShowQrCodeClicked(object? sender, EventArgs e)
    {
        var route = $"{nameof(QRCode)}?CardId={User.Account.CardId}";
        await Shell.Current.GoToAsync(route);
    }

    private async void Btn_OnLogOutClicked(object? sender, EventArgs e)
    {
        Application.Current.MainPage = new LogInShell();
    }
}