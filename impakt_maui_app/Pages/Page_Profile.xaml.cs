using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Pages
{
    public partial class Page_Profile : ContentPage
    {
        public Page_Profile()
        {
            InitializeComponent();
        }

        private async void Btn_OnExternalProvidersClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Page_ExternalProvider");
        }

        private async void Btn_OnPassTypesClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Page_PassTypes");
        }
    }
}
