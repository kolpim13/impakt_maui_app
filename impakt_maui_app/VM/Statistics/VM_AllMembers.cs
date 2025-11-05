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
                    IsExpanded = false,
                };
            }
        }

        /* PRIVATE DATA */
        private const int minimal_search_length = 3;

        private bool is_initialized = false;
        private bool is_data_filtered = false;
        private string previous_search_text;

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
        private void ApplySearchFilter(string query)
        {
            // Search was clicked && text len < 3 --> Show all data
            if (string.IsNullOrEmpty(query) ||
                query.Length < minimal_search_length)
            {
                SearchBar_CleanSearchFilter();
                return;
            }
            
            // Filter data
            if (string.Equals(query, previous_search_text, StringComparison.InvariantCultureIgnoreCase) is true) 
                { return; }

            IEnumerable<CV_Model_Member> filtered = _allMembers.Where(m =>
                (m.FullName?.Contains(query, StringComparison.InvariantCultureIgnoreCase) == true) ||
                (m.AccountType.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) == true)
                );

            Members = filtered.ToObservableCollection();
            previous_search_text = query;
            is_data_filtered = true;

            OnPropertyChanged(nameof(Members));
        }

        [RelayCommand]
        private async Task OpenMemberProfile(CV_Model_Member model)
        {
            // Get Info from DB about the user --> navigate to corresponding page
            Resp_Members_Inst member = await fetch_member(model.CardId);
            var route = $"{nameof(Pages.Profile.MemberProfile)}";
            var args = new Dictionary<string, object>
                {
                    { "Member", member },
                };
            await Shell.Current.GoToAsync(route, args);

            // Update information about the member
            member = await fetch_member(model.CardId);
            if (member is null)
            {
                // Member was deleted
                _allMembers.Remove(model);
                Members.Remove(model);
                // OnPropertyChanged(nameof(Members));
            }
            else
            {
                // Update level of privilege
                model.AccountType = (AccountType)member.account_type;
            }
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

        /* COMMANDS CONDITIONS */

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

        public void SearchBar_CleanSearchFilter()
        {
            // Data already filtered
            if (is_data_filtered is false)
            { return; }

            Members = _allMembers;
            OnPropertyChanged(nameof(Members));

            is_data_filtered = false;
        }

        /* PRIVATE METHODS */
        private async Task<Resp_Members_Inst> fetch_member(string card_id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_Member_Inst(card_id));
                if (response.IsSuccessStatusCode)
                {
                    Resp_Members_Inst member = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();
                    return member;
                }
                else
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                ;
            }
            return null;
        }
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
