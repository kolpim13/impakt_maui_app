using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using Java.Security;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Input;
using ZXing.Net.Maui;

namespace impakt_maui_app.VM
{
    public partial class CV_Model_ExternalProvider : ExternalProvider, INotifyPropertyChanged
    {
        private bool isExpanded = false;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    public partial class VM_ExternalProviders : ObservableObject
    {
        private readonly IAlertService _alertService;

       

        public ObservableCollection<CV_Model_ExternalProvider> Providers { get; } = new();

        [NotifyCanExecuteChangedFor(nameof(EditProviderCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteProviderCommand))]
        [ObservableProperty] private CV_Model_ExternalProvider? selectedProvider;
        private CV_Model_ExternalProvider? _previousely_selected_provided;
        
        [RelayCommand]
        private void SelectionChanged(CV_Model_ExternalProvider selected)
        {
            // To pro
            if (selected is null)
                return;

            // When clicked for the first time
            if (_previousely_selected_provided is null)
            {
                _previousely_selected_provided = selected;
                selected.IsExpanded = true;
                return;
            }

            if (_previousely_selected_provided == selected &&
                _previousely_selected_provided.IsExpanded)
            {
                selected.IsExpanded = false;
            }
            else
            {
                selected.IsExpanded = true;
            }

            // Collaps others
            foreach (var item in Providers)
            {
                if (item != selected)
                    item.IsExpanded = false;
            }

            // Update previous selection
            _previousely_selected_provided = selected;
        }

        [ObservableProperty] private bool isAddOrEditProvider; // false - add, true - edit.
        
        [ObservableProperty] private bool isNewModifyMenuVisible = false;

        [NotifyCanExecuteChangedFor(nameof(AddOrEdtitProviderCommand))]
        [ObservableProperty] private string name;

        [ObservableProperty] private string? description;

        [ObservableProperty] private decimal? partialPayment;


        [RelayCommand]
        private async Task CvShowContextMenu()
        {
            if (SelectedProvider is null)
                return;

            string action = await _alertService.ShowActionSHeet($"Choose action for [{SelectedProvider.Name}] Provider?", "Cancel", null, "Delete (Not at the moment)", "Modify");
            switch (action)
            {
                case "Delete":
                    // TBD  
                    break;
                case "Modify":
                    EditProvider(SelectedProvider);
                    break;
            }
        }

        [RelayCommand]
        public void AddProvider()
        {
            IsAddOrEditProvider = false;

            /* Set default fields values */
            Name = "";
            Description = null;
            PartialPayment = null;

            IsNewModifyMenuVisible = true;
        }

        [RelayCommand(CanExecute = nameof(can_btn_edit_be_pressed))]
        private void EditProvider(ExternalProvider provider)
        {
            if (provider is null) return;
            IsAddOrEditProvider = true;

            /* Set values of fields according to the chosen provider*/
            Name = provider.Name;
            Description = provider.Description;
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
            if (IsAddOrEditProvider is true)
            {
                edit_provider();
            }
            else
            {
                add_provider();
            }

            // Update CollectionView by reading data from the database
            get_all_providers();

            // Hide table with an element.
            IsNewModifyMenuVisible = false;
        }

        [RelayCommand]
        private void CancelAddEditProvider()
        {
            IsNewModifyMenuVisible = false;
        }

        public VM_ExternalProviders()
        {
            _alertService = new AlertService();

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
                is_partial_payment = PartialPayment is null || PartialPayment == 0 ? false : true,
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

                    CV_Model_ExternalProvider new_provider = new CV_Model_ExternalProvider
                    {
                        Id = provider_details.id,
                        Name = provider_details.name,
                        Description = provider_details.description,
                        IsPartialPayment = provider_details.is_partial_payment,
                        PartialPayment = provider_details.partial_payment,
                        IsDeleted = provider_details.is_deleted,
                        IsExpanded = false,
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
                is_partial_payment = PartialPayment is null || PartialPayment == 0 ? false : true,
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

                    CV_Model_ExternalProvider new_provider = new CV_Model_ExternalProvider
                    {
                        Id = provider_details.id,
                        Name = provider_details.name,
                        Description = provider_details.description,
                        IsPartialPayment = provider_details.is_partial_payment,
                        PartialPayment = provider_details.partial_payment,
                        IsDeleted = provider_details.is_deleted,
                        IsExpanded = false,
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
                        Providers.Add(new CV_Model_ExternalProvider
                        {
                            Id = provider.id,
                            Name = provider.name,
                            Description = provider.description,
                            IsPartialPayment = provider.is_partial_payment,
                            PartialPayment = provider.partial_payment,
                            IsDeleted = provider.is_deleted,
                            IsExpanded = false,
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
    }

    public class Converter_ExternalProviders_AddOrEditTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var is_add_or_edit = value as bool?;
            return is_add_or_edit is false ? "New External Provider" : "Modify External Provider";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Converter_ExternalProviders_Description : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var description = value as string;
            return string.IsNullOrWhiteSpace(description) ? "---" : description;
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
    public class Converter_ExternalProviders_IsDeleted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var is_deleted = value as bool?;
            return is_deleted is false ? "Active" : "Deleted";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
