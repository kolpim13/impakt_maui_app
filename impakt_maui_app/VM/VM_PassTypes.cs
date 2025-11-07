using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM
{
    public partial class VM_PassTypes : ObservableObject
    {
        private readonly ExternalProvider dummy_provider = new ExternalProvider
        {
            Id = -1,
            Name = "No Provider",
            IsPartialPayment = false,
            IsDeleted = true,
        };
        private bool _is_edit_pass_type;

        public ObservableCollection<EntryPass> PassTypes { get; } = new();

        [ObservableProperty]
        private EntryPass? selectedPassType;

        /* To be used for edit / add functionality. */
        [ObservableProperty] private bool isFormVisible = false;
        [ObservableProperty] private string name; 
        [ObservableProperty] private string? description;
        [ObservableProperty] private decimal price;
        [ObservableProperty] private int? validityDays;
        [ObservableProperty] private int? maximumEntries;

        [ObservableProperty] private bool requiresExernalAuth;
        public ObservableCollection<ExternalProvider> ExternalProviders { get; } = new();
        [ObservableProperty] private ExternalProvider? selectedExternalProvider;

        [ObservableProperty] private bool isExtEventPass;
        [ObservableProperty] private string? extEventCode;
        

        [RelayCommand]
        private void AddPassTypeForm()
        {
            _is_edit_pass_type = false;
            IsFormVisible = true;
        }

        [RelayCommand(CanExecute = nameof(can_btn_edit_be_pressed))]
        private void EditPassTypeForm()
        {
            _is_edit_pass_type = true;

            /* Set values of forms from selected pass */
            Name = SelectedPassType.Name;
            Description = SelectedPassType.Description;
            Price = SelectedPassType.Price;
            ValidityDays = SelectedPassType.ValidityDays;
            MaximumEntries = SelectedPassType.MaximumEntries;
            RequiresExernalAuth = SelectedPassType.RequiresExternalAuth;
            SelectedExternalProvider = ExternalProviders.FirstOrDefault(provider => provider.Id == SelectedPassType?.ExternalProviderId, dummy_provider);                                                         
            IsExtEventPass = SelectedPassType.IsExtEventPass;
            ExtEventCode = SelectedPassType.ExtEventCode;

            IsFormVisible = true;
        }

        [RelayCommand(CanExecute = nameof(can_btn_delete_be_pressed))]
        private void DeletePassType()
        {
            ;
        }

        [RelayCommand(CanExecute = nameof(is_btn_ok_available))]
        private async void BtnOk()
        {
            if (_is_edit_pass_type is true)
            {
                edit_pass_type();
            }
            else
            {
                add_pass_type();
            }

            IsFormVisible = false;
        }

        [RelayCommand]
        private void BtnCancel()
        {
            IsFormVisible = false;
        }
        public VM_PassTypes()
        {
            ;
        }

        public async Task InitializeAsync()
        {
            /* Is called from the View class. */

            /* Get ExternalProviders from the database */
            await GeneralResources.ExternalProviders_FromDataBase();
            GeneralResources.Get_ExternalProviders_AsCollection(ExternalProviders);

            /* Get PassTypes from the database */
            await GeneralResources.PassTypes_FromDataBase();
            GeneralResources.Get_PassTypes_AsCollection(PassTypes);

            /* Add "NO" ExternalProvider option --> Set it as default choice */
            ExternalProviders.Add(dummy_provider);
            SelectedExternalProvider = ExternalProviders.First(provider => provider.Id == dummy_provider.Id);
        }

        /* FUNCTIONALITY */
        private Req_PassTypes_Create req_create_from_forms() =>
            new Req_PassTypes_Create
            {
                name = Name,
                description = Description,
                price = Price,
                validity_days = ValidityDays,
                maximum_entries = MaximumEntries,
                requires_external_auth = RequiresExernalAuth,
                external_provider_name = RequiresExernalAuth is true ? SelectedExternalProvider.Name : null,
                external_provider_id = RequiresExernalAuth is true ? SelectedExternalProvider.Id : null,
                is_ext_event_pass = IsExtEventPass,
                ext_event_code = IsExtEventPass is true ? ExtEventCode : null,
            };
        private Req_PassTypes_Update req_update_from_forms() =>
            new Req_PassTypes_Update
            {
                id = SelectedPassType.Id,
                name = Name,
                description = Description,
                price = Price,
                validity_days = ValidityDays,
                maximum_entries = MaximumEntries,
                requires_external_auth = RequiresExernalAuth,
                external_provider_name = RequiresExernalAuth is true ? SelectedExternalProvider.Name : null,
                external_provider_id = RequiresExernalAuth is true ? SelectedExternalProvider.Id : null,
                is_ext_event_pass = IsExtEventPass,
                ext_event_code = IsExtEventPass is true ? ExtEventCode : null,
            };

        private async void edit_pass_type()
        {
            Req_PassTypes_Update req = req_update_from_forms();
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsJsonAsync(Network.Put_PassTypes_Update, req);
                if (response.IsSuccessStatusCode)
                {
                    /* Update collection on this page */
                    var pass_details = await response.Content.ReadFromJsonAsync<Resp_PassTypes_Inst>();
                    EntryPass new_pass = EntryPass.From_Resp_Inst(pass_details);
                    
                    PassTypes.Remove(SelectedPassType);
                    PassTypes.Add(new_pass);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private async void add_pass_type()
        {
            Req_PassTypes_Create req = req_create_from_forms();
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_PassTypes_Create, req);
                if (response.IsSuccessStatusCode)
                {
                    /* Update collection on this page */
                    var pass_details = await response.Content.ReadFromJsonAsync<Resp_PassTypes_Inst>();
                    EntryPass new_pass = EntryPass.From_Resp_Inst(pass_details);
                    PassTypes.Add(new_pass);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        /* CONDITIONS */
        private bool can_btn_edit_be_pressed() =>
            !(SelectedPassType is null);
        partial void OnSelectedPassTypeChanged(EntryPass? value)
        {
            EditPassTypeFormCommand.NotifyCanExecuteChanged();
        }

        private bool can_btn_delete_be_pressed() =>
            false;
        private bool is_btn_ok_available()
        {
            /* Some checks should be added */
            if (string.IsNullOrEmpty(Name?.Trim()) ||
                (RequiresExernalAuth && SelectedExternalProvider is null) ||
                IsExtEventPass && string.IsNullOrEmpty(ExtEventCode?.Trim()))
                { return false; }
            return true;
        }

        /* PROPERTIES NOTIFICATIONS */
        partial void OnSelectedExternalProviderChanged(ExternalProvider? value) =>
            RequiresExernalAuth = SelectedExternalProvider.Id != dummy_provider.Id;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            /* Update Command: Btn OK */
            if (e.PropertyName == nameof(Name) ||
                e.PropertyName == nameof(RequiresExernalAuth) ||
                e.PropertyName == nameof(SelectedExternalProvider) ||
                e.PropertyName == nameof(IsExtEventPass) ||
                e.PropertyName == nameof(ExtEventCode))
            {
                BtnOkCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public class Converter_PassTypes_NullToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* param -> What to show instead of null. 
               value -> asumed value. */
            string? param = parameter is not null ? parameter as string : "";
            string? str = value as string;
            return string.IsNullOrWhiteSpace(str) ? param : str;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
