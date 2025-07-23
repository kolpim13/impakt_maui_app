using CommunityToolkit.Mvvm.ComponentModel;
using impakt_maui_app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM
{
    public partial class VM_Scanner : ObservableObject
    {
        public async Task InitializeAsync()
        {
            /* Get PassTypes from the DB */
            if (!GeneralResources.IsPassTypesObtained)
                await GeneralResources.PassTypes_FromDataBase();

            /* Get ExternalProviders from the DB */
            if (!GeneralResources.IsExternalProvidersObtained)
                await GeneralResources.ExternalProviders_FromDataBase();
        }
    }
}
