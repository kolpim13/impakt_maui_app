namespace impakt_maui_app.Pages;

public partial class Page_Statistics : ContentPage
{
	public Page_Statistics()
	{
		InitializeComponent();
	}

    private async void OnClicked_AdminSummary(object? sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("Page_Statistics_AdminSummary");
    }
}