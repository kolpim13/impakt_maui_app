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
        private string previous_search_text;

        private int page = 0;
        private readonly int page_size = 100;
        private int total = 0;
        private int remaining = 1;

        /* PROPERTIES */
        private ObservableCollection<CV_Model_Member> _allMembers { get; } = new();
        public ObservableCollection<CV_Model_Member> Members { get; private set; } = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePrivilegeCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteMemberCommand))]
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

        [RelayCommand(CanExecute = nameof(can_execute_change_privalege_or_delete_member_command))]
        private async Task ChangePrivilege()
        {
            // Filter all roles that are can not be set [should be less than current role of the user].
            string[] exclude = { };
            foreach (AccountType type in Enum.GetValues(typeof(AccountType)))
            {
                if (User.Account.AccountType <= type)
                {
                    exclude.Append(type.ToString());
                }
            }

            // Get all names of possible account roles exluding filtered values
            string[] roles = Enum.GetNames(typeof(AccountType))
                .Where(v => !exclude.Contains(v))
                .ToArray();

            // Show PopUp with possible variants
            string chosen_variant = await Shell.Current.DisplayActionSheet("Choose Account type:", "Cancel", null, roles);

            // Finish command if cancelled was clicked
            if (chosen_variant is null) { return; }

            // Parse chosen variant --> Send request to backend
            int new_account_type = (int)Enum.Parse(typeof(AccountType), chosen_variant);
            try
            {
                // Assemble request (Only account type is implemented for now).
                Req_Members_ChangePrivileges req = new Req_Members_ChangePrivileges
                {
                    card_id = SelectedMember.CardId,
                    account_type = new_account_type,
                };

                // Send request --> process result
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsJsonAsync(Network.Put_Members_Privileges, req);
                if (response.IsSuccessStatusCode)
                {
                    // Update changed member in the List
                    var updated_member = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();
                    SelectedMember = CV_Model_Member.FromRespMemberModel(updated_member);
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
        }

        [RelayCommand(CanExecute = nameof(can_execute_change_privalege_or_delete_member_command))]
        private async Task DeleteMember()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(Network.Delete_Members(SelectedMember.CardId));
                if (response.IsSuccessStatusCode)
                {
                    // Delete a member from a list
                    _allMembers.Remove(SelectedMember);
                    Members = _allMembers;
                    OnPropertyChanged(nameof(Members));

                    SelectedMember = null;
                    OnPropertyChanged(nameof(SelectedMember));
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
        }

        /* COMMANDS CONDITIONS */
        private bool can_execute_change_privalege_or_delete_member_command()
        {
            // Can be executed only if any member was chosen
            if (SelectedMember is null) { return false; }

            // Only ADMINs and ROOTs can cahnge privileges of others
            if (User.Account.AccountType == AccountType.Instructor)
            {
                return false;
            }
            return true;
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
