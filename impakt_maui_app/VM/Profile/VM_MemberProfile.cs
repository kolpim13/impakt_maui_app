using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM.Profile
{
    public partial class VM_MemberProfile : ObservableObject
    {
        /* PROPERTIES */
        [ObservableProperty]
        private Resp_Members_Inst member;

        public VM_MemberProfile()
        {
            ;
        }

        /* COMMANDS */
        [RelayCommand(CanExecute = nameof(can_execute_change_privalege))]
        private async Task ChangePrivilege()
        {
            // Filter all roles that are can not be set [should be less than current role of the user].
            string[] exclude = { };
            foreach (AccountType type in Enum.GetValues(typeof(AccountType)))
            {
                if ((int)User.Account.AccountType >= (int)type)
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
            if (chosen_variant is null ||
                chosen_variant == "Cancel") { return; }

            // Parse chosen variant --> Send request to backend
            int new_account_type = (int)Enum.Parse(typeof(AccountType), chosen_variant);
            try
            {
                // Assemble request(Only account type is implemented for now).
                Req_Members_ChangePrivileges req = new Req_Members_ChangePrivileges
                {
                    card_id = member.card_id,
                    account_type = new_account_type,
                };

                // Send request --> process result
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsJsonAsync(Network.Put_Members_Privileges, req);
                if (response.IsSuccessStatusCode)
                {
                    // Update changed member in the List
                    var updated_member = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();
                    //SelectedMember = CV_Model_Member.FromRespMemberModel(updated_member);
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

        [RelayCommand(CanExecute = nameof(can_execute_delete_member))]
        private async Task DeleteMember()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(Network.Delete_Members(member.card_id));
                if (response.IsSuccessStatusCode)
                {
                    // Return to previous page
                    MainThread.BeginInvokeOnMainThread(async () =>
                        { await Shell.Current.GoToAsync(".."); });
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
        private bool can_execute_change_privalege()
        {
            if (User.Account.AccountType != AccountType.Root &&
                User.Account.AccountType != AccountType.Admin)
            {
                return false;
            }
            return true;
        }

        private bool can_execute_delete_member()
        {
            if (User.Account.AccountType != AccountType.Root &&
                 User.Account.AccountType != AccountType.Admin)
            {
                return false;
            }
            return true;
        }

        /* PRIVATE METHODS */
    }
}

namespace impakt_maui_app.VM
{
    public class Converter_AcoountTypeIntToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            AccountType temp = (AccountType)Enum.Parse(typeof(AccountType), value.ToString());
            return temp.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && targetType.IsEnum)
                return Enum.Parse(targetType, str, ignoreCase: true);

            return BindableProperty.UnsetValue;
        }
    }
}
