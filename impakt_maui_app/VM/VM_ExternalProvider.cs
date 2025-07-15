using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Schemas;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Windows.Input;
using ZXing.Net.Maui;
using System.Text.Json;
using impakt_maui_app.Models;

namespace impakt_maui_app.VM
{
    public partial class VM_ExternalProviders : ObservableObject
    {
        private bool _is_edit_provider; // false - add, true - edit.

        public ObservableCollection<Model_ExternalProvider> Providers { get; } = new();

        [ObservableProperty]
        private Model_ExternalProvider? selectedProvider;

        // New / Modified External Provider 
        [ObservableProperty] private bool isNewModifyMenuVisible = false;
        [ObservableProperty] private string name;
        [ObservableProperty] private string? description;
        [ObservableProperty] private bool isPartialPayment;
        [ObservableProperty] private decimal? partialPayment;

        [ObservableProperty] private bool isDataValidated;

        [RelayCommand]
        public void AddProvider()
        {
            _is_edit_provider = false;

            /* Set default fields values */
            Name = "";
            Description = null;
            IsPartialPayment = false;
            PartialPayment = null;

            IsNewModifyMenuVisible = true;
        }

        [RelayCommand(CanExecute = nameof(can_btn_edit_be_pressed))]
        public void EditProvider(Model_ExternalProvider provider)
        {
            if (provider is null) return;
            _is_edit_provider = true;

            /* Set values of fields according to the chosen provider*/
            Name = provider.Name;
            Description = provider.Description;
            IsPartialPayment = provider.IsPartialPayment;
            PartialPayment = provider.PartialPayment;

            IsNewModifyMenuVisible = true;
        }

        [RelayCommand(CanExecute = nameof(can_btn_delete_be_pressed))]
        public void DeleteProvider()
        {
            // To be done
            ;
        }

        [RelayCommand(CanExecute = nameof(can_add_or_edit_provider))]
        public async void AddOrEdtitProvider()
        {
            if (_is_edit_provider is true)
            {
                edit_provider();
            }
            else
            {
                add_provider();
            }

            IsNewModifyMenuVisible = false;
        }

        [RelayCommand]
        private void CancelAddEditProvider()
        {
            IsNewModifyMenuVisible = false;
        }

        public VM_ExternalProviders()
        {
            /* Get all providers - probably some static data should be used here. */
            get_all_providers();
        }

        private async void add_provider()
        {
            /* Assemble request */
            Req_Create_ExternalProviders req = new Req_Create_ExternalProviders
            {
                name = Name,
                description = Description,
                is_partial_payment = IsPartialPayment,
                partial_payment = PartialPayment,
            };

            /* Send POST request */
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_ExternalProviders_Create, req);
                if (response.IsSuccessStatusCode)
                {
                    /* Add newly added provider to a list */
                    var provider_details = await response.Content.ReadFromJsonAsync<Resp_Instance_ExternalProviders>();

                    Model_ExternalProvider new_provider = new Model_ExternalProvider
                    {
                        Id = provider_details.id,
                        Name = provider_details.name,
                        Description = provider_details.description,
                        IsPartialPayment = provider_details.is_partial_payment,
                        PartialPayment = provider_details.partial_payment,
                        IsDeleted = provider_details.is_deleted,
                    };
                    Providers.Add(new_provider);
                }
            }
            catch (Exception ex)
            {
                ;
            }

            /* Send GET request -> to update list of the all */
        }
        private async void edit_provider()
        {
            /* Assemble request */
            Req_Update_ExternalProviders req = new Req_Update_ExternalProviders
            {
                id = SelectedProvider.Id,
                name = Name,
                description = Description,
                is_partial_payment = IsPartialPayment,
                partial_payment = PartialPayment,
            };

            /* Send PUT request */
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsJsonAsync(Network.Put_ExternalProviders_Update, req);
                if (response.IsSuccessStatusCode)
                {
                    /* Add newly added provider to a list */
                    var provider_details = await response.Content.ReadFromJsonAsync<Resp_Instance_ExternalProviders>();

                    Model_ExternalProvider new_provider = new Model_ExternalProvider
                    {
                        Id = provider_details.id,
                        Name = provider_details.name,
                        Description = provider_details.description,
                        IsPartialPayment = provider_details.is_partial_payment,
                        PartialPayment = provider_details.partial_payment,
                        IsDeleted = provider_details.is_deleted,
                    };

                    /* Update collection */
                    Providers.Remove(SelectedProvider);
                    Providers.Add(new_provider);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private async void get_all_providers()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_ExternalProviders);
                if (response.IsSuccessStatusCode)
                {
                    Providers.Clear();
                    var all_providers = await response.Content.ReadFromJsonAsync<List<Resp_Instance_ExternalProviders>>();
                    foreach (Resp_Instance_ExternalProviders provider in all_providers)
                    {
                        Providers.Add(new Model_ExternalProvider
                        {
                            Id = provider.id,
                            Name = provider.name,
                            Description = provider.description,
                            IsPartialPayment = provider.is_partial_payment,
                            PartialPayment = provider.partial_payment,
                            IsDeleted = provider.is_deleted,
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private bool can_add_or_edit_provider() => 
            !string.IsNullOrEmpty(Name);
        partial void OnNameChanged(string oldValue, string newValue) =>
            AddOrEdtitProviderCommand.NotifyCanExecuteChanged();

        private bool can_btn_delete_be_pressed() => false;
        private bool can_btn_edit_be_pressed() =>
            !(SelectedProvider is null);
        partial void OnSelectedProviderChanged(Model_ExternalProvider? value)
        {
            EditProviderCommand.NotifyCanExecuteChanged(); 
            // DeleteProviderCommand.NotifyCanExecuteChanged();
        } 
    }

    public class Converter_ExternalProviders_Description : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var description = value as string;
            return string.IsNullOrWhiteSpace(description) ? "No description" : description;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Converter_ExternalProviders_PartialPayment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var partial_apyment = value as decimal?;
            return partial_apyment is null ? "NO" : partial_apyment.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
