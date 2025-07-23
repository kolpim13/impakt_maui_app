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
    public partial class VM_Popup_MemberInfo : ObservableObject
    {
        [ObservableProperty] private string? fullName;
        [ObservableProperty] private string? passTypeName;
        [ObservableProperty] private int? entriesLeft;
        [ObservableProperty] private DateOnly? expirationDate;
        [ObservableProperty] private string? externalProviderName;
        [ObservableProperty] private string? extEventCode;
        public ObservableCollection<Model_MemberPass> MemberPasses { get; } = new();

        public VM_Popup_MemberInfo()
        {
            ;
        }
        public async Task InitializeAsync(string member_id)
        {
            /* Get all valid MemberPasses */
            Model_Member? member = await GeneralResources.Get_Member_FromDB(member_id);
            if (member is null)
            {
                FullName = "User NOT exist in database";
                return;
            }

            await GeneralResources.Get_MemberPass_AsCollection_FromDB(MemberPasses, member_id);

            /* Update Fields */
            FullName = $"{member.Name} {member.Surname}";
        }
    }
}
