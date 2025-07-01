using CommunityToolkit.Maui.Views;
using impakt_maui_app.Schemas;
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
        public MemberInfoPopup(Resp_Members_MemberInfo member_info)
        {
            InitializeComponent();

            /* Fill labels */
            LB_Name.Text = member_info.name;
            LB_Surname.Text = member_info.surname;

            LB_PassType.Text = Enum.GetName(typeof(PassType), member_info.pass_type).ToString();
            LB_EntrancesLeft.Text = member_info.entrances_left.ToString();
            LB_PassExpirationDate.Text = member_info.expiration_date.ToString();
        }
    }
}
