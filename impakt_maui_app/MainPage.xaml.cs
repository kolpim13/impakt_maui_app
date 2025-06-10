using System.ComponentModel;
using ZXing.Net.Maui.Controls;
using CommunityToolkit.Maui.Views;
using impakt_maui_app.Popups;

namespace impakt_maui_app
{
    public class MainPage_Bindings_UserInfo : INotifyPropertyChanged
    {
        private static readonly Lazy<MainPage_Bindings_UserInfo> _instance = new(() => new MainPage_Bindings_UserInfo());
        public static MainPage_Bindings_UserInfo Instance => _instance.Value;

        public string Name { get => UserInfo.Name; }
        public string SurName { get => UserInfo.SurName; }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = MainPage_Bindings_UserInfo.Instance;
        }

        private async void OnTempClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//QRScanner");
            return;
        }

        private async void OnAddMemberClicked(object? sender, EventArgs e)
        {
            await Shell.Current.ShowPopupAsync(new NewMemberPopup());
        }
    }
}
