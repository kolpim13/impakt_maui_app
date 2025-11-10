using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM.Scanner
{
    public partial class VM_ExternalProvider : ObservableObject
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        // ...

        /* PROPERTIES */
        public ObservableCollection<ExternalProvider> Providers { get; set; }

        /* COMMANDS */
        [RelayCommand]
        private async Task NavigateToScanQR(ExternalProvider provider)
        {
            /* We do know scan type since we at this page already 
            Have to figure out the external provider. */
            if (provider is null)
                return;

            await Shell.Current.GoToAsync(nameof(Pages.Scanner.Scanner_QR),
                new Dictionary<string, object>
                {
                    ["ScanType"] = QRScanMode.CheckIn,
                    ["ExternalProvider"] = provider,
                });
        }

        public VM_ExternalProvider()
        {
            ;
        }

        /* PUBLIC METHODS (TO BE USED OUTSIDE OF VM) */
        public async Task InitializeAsync() 
        {
            // External Providers were not obtained ? --> pull them from DB
            if (GeneralResources.IsExternalProvidersObtained is false)
            {
                await GeneralResources.ExternalProviders_FromDataBase();
            }

            // Update property
            Providers = GeneralResources.Get_ExternalProviders_AsCollection()
                .Where(p => p.IsDeleted == false)
                .ToObservableCollection();
            Providers.Add(GeneralResources.dummy_provider);
            OnPropertyChanged(nameof(Providers));
        }

        /* PRIVATE METHODS */
        // ...
    }
}
