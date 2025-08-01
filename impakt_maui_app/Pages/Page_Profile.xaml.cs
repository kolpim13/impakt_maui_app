using CommunityToolkit.Maui.Views;
using impakt_maui_app.Popups;
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

        private async void Btn_OnAddMemberClicked(object? sender, EventArgs e)
        {
            Popup_NewMember popup = new Popup_NewMember();
            await Shell.Current.ShowPopupAsync(popup);
        }

        private async void Btn_OnLogOutClicked(object? sender, EventArgs e)
        {
            Application.Current.MainPage = new LogInShell();
        }
    }
}
