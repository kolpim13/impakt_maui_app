using CommunityToolkit.Maui.Views;
using impakt_maui_app.Schemas;
using impakt_maui_app.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace impakt_maui_app.Popups
{
    public partial class Popup_MemberInfo : Popup
    {
        public Popup_MemberInfo(VM_Popup_MemberInfo vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        public static async Task<Popup_MemberInfo> CreateAsync(string member_id)
        {
            VM_Popup_MemberInfo vm = new VM_Popup_MemberInfo();
            await vm.InitializeAsync(member_id);
            return new Popup_MemberInfo(vm);
        }
    }
}
