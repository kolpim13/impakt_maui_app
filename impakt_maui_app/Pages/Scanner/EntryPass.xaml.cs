using impakt_maui_app.VM.Scanner;

namespace impakt_maui_app.Pages.Scanner;

public partial class EntryPass : ContentPage
{
	public EntryPass()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        /* To move this code on creation of the page somehow ? */
        base.OnAppearing();
        if (BindingContext is VM_EntryPass vm)
        {
            await vm.InitializeAsync();
        }
    }
}