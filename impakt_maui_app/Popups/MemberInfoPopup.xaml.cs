using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace impakt_maui_app.Popups
{
    public partial class MemberInfoPopup : Popup
    {
        public MemberInfoPopup(BackendResp_MemberInfo member_info)
        {
            InitializeComponent();

            // Update data in labels
            NameLabel.Text = member_info.name;
            SurnameLabel.Text = member_info.surname;
            EmailLabel.Text = member_info.email;

            // Optional data need additional check
            PhoneLabel.Text = member_info.phone_number ?? "No data";
            DateOfBirthLabel.Text = member_info.date_of_birth.ToString() ?? "No data";

        }
    }
}
