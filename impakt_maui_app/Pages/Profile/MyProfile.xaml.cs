using CommunityToolkit.Maui.Views;
using impakt_maui_app.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Pages.Profile
{
    public partial class MyProfile : ContentPage
    {
        public MyProfile()
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

        private async void Btn_OnLogOutClicked(object? sender, EventArgs e)
        {
            Application.Current.MainPage = new LogInShell();
        }

        private async void Btn_OnShowQrCodeClicked(object? sender, EventArgs e)
        {
            var route = $"{nameof(QRCode)}?CardId={User.Account.CardId}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
