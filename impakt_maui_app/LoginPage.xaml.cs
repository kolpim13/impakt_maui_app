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

namespace impakt_maui_app
{
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            // Temporarly --> jump on main page
            //await Shell.Current.GoToAsync("//MainPage");
            //return;

            // Collect data from forms
            string? username = UsernameEntry.Text?.Trim();
            string? password = PasswordEntry.Text;

            // Validate both username && password were entered
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Fill both: username and password", "OK");
                await Navigation.PopAsync();
                return;
            }

            try
            {
                // Save URL
#if ANDROID
                string? ip = IpEntry.Text?.Trim();
                if (string.IsNullOrEmpty(ip) == false)
                {
                    Network.URL = "http://" + ip + ":8000";
                }
#endif
                // Assemble post request --> send it.
                Req_LogIn req = new Req_LogIn
                {
                    username = username,
                    password = password,
                };

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(Network.LogInUrl, req);
                string response_body = await response.Content.ReadAsStringAsync();

                // Validate the response
                if (response.IsSuccessStatusCode)
                {
                    // response.Content.ReadFromJsonAsync

                    // Get data about the user from the backend response
                    UserInfo.Fill_FromLogInResp(response_body);

                    // Dispatch all used resources
                    _httpClient.Dispose();

                    // Free all resources --> navigate to the main page
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    string header = "Problem during Login";
                    string message = string.Format("Status code: {0} - {1}\n{2}", (int)response.StatusCode, response.StatusCode, response_body);
                    await DisplayAlert(header, message, "OK");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", "random error", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}
