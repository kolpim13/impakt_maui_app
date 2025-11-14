using CommunityToolkit.Mvvm.ComponentModel;

namespace HW_QR_Scanner.ViewModels
{
    public interface INavigation
    {
        void ShowRegistration();
        //  Show
    }

    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly RegistrationViewModel _homeVm = new();

        [ObservableProperty]
        private object currentViewModel;

        public MainWindowViewModel()
        {
            ShowRegistration();
        }

        public void ShowRegistration()
        {
            CurrentViewModel = _homeVm;
        }

    }
}
