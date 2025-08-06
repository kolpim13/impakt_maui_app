using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Pages
{
    public partial class Page_Login : ContentPage
    {
        public Page_Login()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object? sender, EventArgs e)
        {

            // Collect data from forms
            string? username = UsernameEntry.Text?.Trim();
            string? password = PasswordEntry.Text;

            // Validate both username && password were entered
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Fill both: username and password", "OK");
                return;
            }

            try
            {
                // Assemble post request --> send it.
                Req_LogIn_Username req = new Req_LogIn_Username
                {
                    username = username,
                    password = password,
                };

                HttpClient _httpClient = new HttpClient();
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(Network.Post_LogIn, req);
                var member_info = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();

                // Validate the response
                if (response.IsSuccessStatusCode)
                {
                    // response.Content.ReadFromJsonAsync

                    // Get data about the user from the backend response
                    User.Account = Model_Member.From_Resp_Inst(member_info);

                    // Dispatch all used resources
                    _httpClient.Dispose();

                    // Clear fields firstly
                    UsernameEntry.Text = "";
                    PasswordEntry.Text = "";

                    // Navigate to the main page (If has access).
                    if (User.Account.AccountType == AccountType.Member)
                    {
                        Application.Current.MainPage = new MemberShell();
                    }
                    else
                    {
                        Application.Current.MainPage = new AppShell();
                        // await Shell.Current.GoToAsync("//Page_Profile");
                    }
                }
                else
                {
                    string header = "Problem during Login";
                    string message = string.Format("Status code: {0} - {1}\n{2}", (int)response.StatusCode, response.StatusCode, response.Content.ToString());
                    await DisplayAlert(header, message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }
    }
}
