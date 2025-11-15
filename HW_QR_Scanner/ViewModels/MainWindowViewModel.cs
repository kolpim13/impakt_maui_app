using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HW_QR_Scanner.ViewModels
{
    public interface INavigation
    {
        void ShowRegistration();
        void ShowScan();
    }

    public partial class MainWindowViewModel : ViewModelBase, INavigation
    {
        private RegistrationViewModel _registrationVm;
        private ScanViewModel _scanVm;

        [ObservableProperty]
        private object currentPage;

        public MainWindowViewModel()
        {
            // Start on registration screen
            ShowRegistration();
        }

        /* COMMANDS */
        [RelayCommand]
        private void ShowRegistrationUI() => ShowRegistration();

        [RelayCommand]
        private void ShowScanUI() => ShowScan();

        /* INTERFACE INavigation */
        public void ShowRegistration()
        {
            _registrationVm = new RegistrationViewModel();
            _registrationVm.SetNavigation(this);
            CurrentPage = _registrationVm;
        }
        public void ShowScan()
        {
            _scanVm = new ScanViewModel();
            _scanVm.SetNavigation(this);
            CurrentPage = _scanVm;
        }
    }
}
