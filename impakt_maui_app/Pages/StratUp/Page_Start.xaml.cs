namespace impakt_maui_app.Pages;

public partial class Page_Start : ContentPage
{
	public Page_Start()
	{
		InitializeComponent();
	}

    private async void BTN_LogIn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Page_Login");
    }

    private async void BTN_SignUp_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Page_SignUp");
    }

    private async void LB_ForgotPassword_Tapped(object sender, TappedEventArgs e)
    {

    }
}