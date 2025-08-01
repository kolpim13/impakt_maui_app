namespace impakt_maui_app.Pages.Profile;

public partial class Page_ProfileMember : ContentPage
{
	public Page_ProfileMember()
	{
		InitializeComponent();
	}

    private async void Btn_OnShowQrCodeClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new Page_QRCode(User.Account.CardId));
    }

    private async void Btn_OnLogOutClicked(object? sender, EventArgs e)
    {
        Application.Current.MainPage = new LogInShell();
    }
}