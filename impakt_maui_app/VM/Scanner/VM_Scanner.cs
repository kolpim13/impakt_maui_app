using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Pages.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM.Scanner
{
    public partial class VM_Scanner : ObservableObject
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        // ...

        /* PROPERTIES */
        // ...

        /* COMMANDS */
        [RelayCommand]
        private async Task ChooseScanType(string param)
        {
            QRScanMode scan_type;
            Enum.TryParse(param, true, out scan_type);

            if (scan_type == QRScanMode.MemberInfo)
            {
                await Shell.Current.GoToAsync(nameof(Scanner_QR),
                    new Dictionary<string, object>
                    {
                        ["ScanType"] = scan_type,
                    });
            }

            if (scan_type == QRScanMode.UpdatePass)
            {
                await Shell.Current.GoToAsync(nameof(Pages.Scanner.EntryPass));
            }

            if (scan_type == QRScanMode.CheckIn)
            {
                await Shell.Current.GoToAsync(nameof(Pages.Scanner.ExternalProvider));
            }
        }

        public VM_Scanner()
        {
            ;
        }

        /* PUBLIC METHODS (TO BE USED OUTSIDE OF VM) */
        public async Task InitializeAsync()
        {
            ;
        }

        /* PRIVATE METHODS */
        // ...
    }
}
