using impakt_maui_app.VM;

namespace impakt_maui_app.Pages;

public partial class Page_PassTypes : ContentPage
{
	public Page_PassTypes()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        /* Load async */
        base.OnAppearing();
        if (BindingContext is VM_PassTypes vm)
            await vm.InitializeAsync();
    }
}