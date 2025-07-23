using impakt_maui_app.VM;

namespace impakt_maui_app.Pages;

public enum QRScanMode : ushort
{
    None,
    CheckIn,
    UpdatePass,
    MemberInfo,
}

public partial class Page_Scanner : ContentPage
{
	public Page_Scanner()
	{
		InitializeComponent();
	}
    
    protected override async void OnAppearing()
    {
        /* Load Pass Types from DB */
        base.OnAppearing();
        if (BindingContext is VM_Scanner vm)
            await vm.InitializeAsync();
    }

    private async void OnClicked_CheckIn(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Page_Scanner_QRScanner?mode=CheckIn");
    }

    private async void OnClicked_UpdatePass(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Page_Scanner_QRScanner?mode=UpdatePass");
    }

    private async void OnClicked_UserInfo(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Page_Scanner_QRScanner?mode=MemberInfo");
    }
}