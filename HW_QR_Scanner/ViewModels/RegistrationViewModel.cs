using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW_QR_Scanner.Models;
using Impact.Schemas;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private string adminLogin;

        [ObservableProperty]
        private string adminPassword;

        [ObservableProperty]
        private string scannerName;

        [ObservableProperty]
        private string scannerLocation;

        /* COMMANDS */
        [RelayCommand]
        private async Task RegisterScanner()
        {
            string scanner_login = DeviceService.GetDeviceFingerprint();
            string scanner_password = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            Req_Scanner_Register req = new Req_Scanner_Register()
            {
                admin_login = AdminLogin,
                admin_password = AdminPassword,
                scanner_login = scanner_login,
                scanner_password = scanner_password,
                name = ScannerName,
                location = ScannerLocation,
                rsa_public_key = null,
            };

            (Resp_Scanner_Register? rep, bool res, string? error_mes) = await Impact.Backend.Requests.Post_Scanner_Register(req);
            if(!res)
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Failed to register scanner", error_mes, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error)
                    .ShowAsync();
                return;
            }

            DeviceService.SaveCridentials(new AppCridentials
            {
                Username = scanner_login,
                Password = scanner_password,
            });

            await MessageBoxManager
                .GetMessageBoxStandard("Scanner registered", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success)
                .ShowAsync();
        }

        [RelayCommand]
        private void GoToScanPage()
            => _navigation.ShowScan();

        public RegistrationViewModel()
        {
            ;
        }

        /* PRIVATE METHODS */
        // private string 
    }
}
