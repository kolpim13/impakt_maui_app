using impakt_maui_app.Schemas;
using impakt_maui_app.VM;
using Maui.DataGrid;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;

namespace impakt_maui_app.Pages;

public partial class Page_Statistics_Admin_InstructorsStatistics : ContentPage
{
    public Page_Statistics_Admin_InstructorsStatistics()
	{
		InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        /* Load first page */
        base.OnAppearing();
    }
}