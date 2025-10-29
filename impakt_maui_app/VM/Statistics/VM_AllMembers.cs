using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Pages;
using impakt_maui_app.Popups;
using impakt_maui_app.Schemas;
using Maui.DataGrid;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;

namespace impakt_maui_app.VM.Statistics
{
    public partial class VM_AllMembers : ObservableObject
    {
        /* DEFINITIONS */
        public partial class CV_Model_Member : ObservableObject
        {
            public string CardId { get; set; }
            public string FullName { get; set; }
            public AccountType AccountType { get; set; }
            public string? Phone { get; set; }
            public string Email { get; set; }
            public DateOnly RegistrationDate { get; set; }

            [ObservableProperty]
            private bool isExpanded;

            public static CV_Model_Member FromRespMemberModel(Resp_Members_Inst resp_model)
            {
                return new CV_Model_Member
                {
                    CardId = resp_model.card_id,
                    FullName = $"{resp_model.name.Trim()} {resp_model.surname.Trim()}",
                    AccountType = (AccountType)resp_model.account_type,
                    Phone = resp_model.phone_number,
                    Email = resp_model.email,
                    RegistrationDate = resp_model.registration_date,
                    IsExpanded = false,
                };
            }
        }

        /* PRIVATE DATA */
        private const int minimal_search_length = 3;

        private bool is_initialized = false;
        private bool is_data_filtered = false;

        private int page = 0;
        private readonly int page_size = 100;
        private int total = 0;
        private int remaining = 1;

        /* PROPERTIES */
        private ObservableCollection<CV_Model_Member> _allMembers { get; } = new();
        public ObservableCollection<CV_Model_Member> Members { get; private set; } = new();

        [ObservableProperty]
        private CV_Model_Member selectedMember;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string searchText = string.Empty;

        /* COMMANDS */
        [RelayCommand]
        private async Task LoadTableData()
        {
            if (IsLoading || remaining <= 0 || is_data_filtered)
                return;

            IsLoading = true;

            // Fetch data from DB --> Convert data to be used in CollectionView
            var members = await fetch_members();
            foreach (var member in members)
            {
                var m = CV_Model_Member.FromRespMemberModel(member);
                _allMembers.Add(m);
            }
            Members = _allMembers;
            OnPropertyChanged(nameof(Members));

            IsLoading = false;
        }

        [RelayCommand]
        private void ApplySearchFilter(int source)
        {
            // Source of the coomand is text was changed --> check only for empty string
            if (source == 1 &&
                string.IsNullOrEmpty(SearchText))
            {
                // Data was filtered --> reset all filtered data
                Members = _allMembers;
                OnPropertyChanged(nameof(Members));

                is_data_filtered = false;
                return;
            }

            // Search was clicked && there is no text --> do nothing
            if (string.IsNullOrEmpty(SearchText) ||
                SearchText.Length < minimal_search_length)
            {
                return;
            }

            // Get all data filtered
            IEnumerable<CV_Model_Member> filtered;
            filtered = _allMembers.Where(m =>
                (m.FullName?.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) == true) ||
                (m.AccountType.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) == true)
                );

            Members = filtered.ToObservableCollection();
            OnPropertyChanged(nameof(Members));

            is_data_filtered = true;
        }

        [RelayCommand]
        private void Test(int source)
        {
            if (string.IsNullOrEmpty(SearchText) ||
                SearchText.Length < minimal_search_length)
            {
                return;
            }
            return;
        }

        [RelayCommand]
        private async Task OpenQrCodePage(string CardId)
        {
            var route = $"{nameof(Pages.Profile.QRCode)}?CardId={CardId}";
            await Shell.Current.GoToAsync(route);
        }

        [RelayCommand]
        private void ExpandCollapsMember(CV_Model_Member member)
        {
            if (member is null) 
                return;
            member.IsExpanded = !member.IsExpanded;
        }

        [RelayCommand]
        private async Task AddMember()
        {
            Popup_NewMember popup = new Popup_NewMember();
            var member = await Shell.Current.ShowPopupAsync(popup);

            // If member was returned --> add it to a list
            if (member is not null)
            {
                var m = CV_Model_Member.FromRespMemberModel((Resp_Members_Inst)member);
                _allMembers.Add(m); 
            }

            Members = _allMembers;
            OnPropertyChanged(nameof(Members));
        }

        [RelayCommand]
        private async Task ChangePrivilage()
        {

        }

        [RelayCommand]
        private async Task DeleteMember()
        {

        }

        public VM_AllMembers() 
        {
            ;
        }

        /* PUBLIC METHODS (TO BE USED OUTSIDE OF VM) */
        public async Task InitializeAsync()
        {
            if (is_initialized)
                return;

            await LoadTableData();
            is_initialized = true;
        }

        public void CleanUp()
        {
            if (!is_initialized)
                return;

            _allMembers.Clear();
            SearchText = string.Empty;
            is_initialized = false;
        }

        /* PRIVATE METHODS */
        private async Task<List<Resp_Members_Inst>> fetch_members()
        {
            List<Resp_Members_Inst> result = new();

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_Members_Instances(page, page_size));
                if (response.IsSuccessStatusCode)
                {
                    var paginated = await response.Content.ReadFromJsonAsync<Resp_Paginated_Members_Instances>();
                    
                    page = paginated.page;
                    total = paginated.total;
                    remaining = paginated.remaining;
                    result = paginated.items;
                }
            }
            catch (Exception ex)
            {
                ;
            }

            return result;
        }
    }
}
