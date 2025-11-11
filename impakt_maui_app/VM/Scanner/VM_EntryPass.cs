using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM.Scanner
{
    public partial class VM_EntryPass : ObservableObject
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        // ...

        /* PROPERTIES */
        public ObservableCollection<Models.EntryPass> EntryPasses { get; set; }

        /* COMMANDS */
        [RelayCommand]
        private async Task NavigateToScanQR(EntryPass pass)
        {
            /* We do know scan type since we at this page already 
            Have to figure out the external provider. */
            await Shell.Current.GoToAsync(nameof(Pages.Scanner.Scanner_QR),
                new Dictionary<string, object>
                {
                    ["ScanType"] = QRScanMode.UpdatePass,
                    ["EntryPass"] = pass,
                });
        }

        public VM_EntryPass()
        {
            ;
        }

        /* PUBLIC METHODS (TO BE USED OUTSIDE OF VM) */
        public async Task InitializeAsync()
        {
            // External Providers were not obtained ? --> pull them from DB
            if (GeneralResources.IsPassTypesObtained is false)
            {
                await GeneralResources.PassTypes_FromDataBase();
            }

            // Update property
            EntryPasses = GeneralResources.Get_PassTypes_AsCollection()
                .Where(p => p.IsDeleted == false)
                .ToObservableCollection();
            OnPropertyChanged(nameof(EntryPasses));
        }

        /* PRIVATE METHODS */
        // ...

    }
}