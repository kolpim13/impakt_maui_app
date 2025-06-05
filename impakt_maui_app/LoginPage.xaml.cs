using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace impakt_maui_app
{
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string APIUrl = "http://localhost:8000";          // Use LAN IP or 10.0.2.2 in emulator
        private const string LogInUrl = APIUrl + "/login/username";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            // Temporarly --> jump on main page
            await Shell.Current.GoToAsync("//MainPage");
            return;

            // Collect data from forms
            string? username = UsernameEntry.Text?.Trim();
            string? password = PasswordEntry.Text;

            // Validate: both username && password were entered
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return;
            }

            // Prepare data to be sent on a backend
            string post_json = JsonConvert.SerializeObject(new
            {
                username = username,
                password = password,
            });
            var post_content = new StringContent(post_json, Encoding.UTF8, "application/json");

            // Send prepared data on backend side
            try
            {
                var response = await _httpClient.PostAsync(LogInUrl, post_content);
                var body = await response.Content.ReadAsStringAsync();

                // Validate the response
                if (response.IsSuccessStatusCode)
                {
                    // Get data about the user from the backend response
                    UserInfo.LoadData(body);

                    // Dispatch all used resources
                    _httpClient.Dispose();

                    // Free all resources --> navigate to the main page
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
            catch (Exception ex)
            {
                // Add smth here later
                ButtonLogin.BackgroundColor = Colors.Red;
            }
        }
    }
}
