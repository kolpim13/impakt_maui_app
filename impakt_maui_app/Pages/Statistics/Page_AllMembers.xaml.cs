using impakt_maui_app.VM.Statistics;

namespace impakt_maui_app.Pages.Statistics;

public partial class Page_AllMembers : ContentPage
{
	public Page_AllMembers()
	{
		InitializeComponent();
	}

    void SearchBar_TextChanged(object s, TextChangedEventArgs e)
    {
       
        System.Diagnostics.Debug.WriteLine($"Changed -> '{e.NewTextValue}'");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is VM_AllMembers vm)
        {
            await vm.InitializeAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    protected override bool OnBackButtonPressed()
    {
        base.OnBackButtonPressed();
        if (BindingContext is VM_AllMembers vm)
        {
            vm.CleanUp();
        }
        return false;
    }
}