using impakt_maui_app.VM.Scanner;

namespace impakt_maui_app.Pages.Scanner;

public partial class ExternalProvider : ContentPage
{
	public ExternalProvider()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        /* To move this code on creation of the page somehow ? */
        base.OnAppearing();
        if (BindingContext is VM_ExternalProvider vm)
        {
            await vm.InitializeAsync();
        }
    }
}