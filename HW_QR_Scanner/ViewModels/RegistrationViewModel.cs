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

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        [RelayCommand]
        private void RegisterScanner()
        {

        }

        public RegistrationViewModel()
        {
            ;
        }
    }
}
