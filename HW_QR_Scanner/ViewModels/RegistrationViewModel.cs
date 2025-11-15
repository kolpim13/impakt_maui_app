using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HW_QR_Scanner.ViewModels
{
    public partial class RegistrationViewModel : ViewModelBase
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        // ...

        /* PROPERTIES */
        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string scannerName;

        [ObservableProperty]
        private string scannerLocation;

        /* COMMANDS */
        [RelayCommand]
        private void RegisterScanner()
        {

        }

        [RelayCommand]
        private void GoToScanPage()
            => _navigation.ShowScan();

        public RegistrationViewModel()
        {
            ;
        }

        /* PRIVATE METHODS */
        // ...
    }
}
