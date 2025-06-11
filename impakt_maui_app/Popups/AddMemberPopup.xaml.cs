using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;

namespace impakt_maui_app.Popups
{
    public partial class NewMemberPopup : Popup
    {
        public NewMemberPopup()
        {
            InitializeComponent();
        }

        private async void OnAddMemberlicked(object sender, EventArgs e)
        {
            // Make button inactive during operation
            ButtonAddMember.IsEnabled = false;

            // Coolect data --> validate them --> prepare to be sent
            BackendReq_RegisterNewMember new_member = new BackendReq_RegisterNewMember
            {
                name = NameEntry.Text,
                surname = SurnameEntry.Text,
                email = EmailEntry.Text,
            };

            // Establish connection --> Send collected data --> react on response
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.NewMemberUrl, new_member);
                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Success", "Member added!", "OK");
                }
                else
                {
                    // Add some reaction later
                    ;
                }
            }
            catch (Exception ex)
            {
                ;
            }
            

            // Restore button disregardless result
            ButtonAddMember.IsEnabled = true;
        }
    }
}
