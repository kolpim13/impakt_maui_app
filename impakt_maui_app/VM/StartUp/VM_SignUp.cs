using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM.StartUp
{
    public partial class VM_SignUp : ObservableObject
    {
        private readonly IAlertService _alertService;

        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        [ObservableProperty] private string name;

        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        [ObservableProperty] private string surname;

        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        [ObservableProperty] private string email;

        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        [ObservableProperty] private string username;

        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        [ObservableProperty] private string password;

        [NotifyCanExecuteChangedFor(nameof(SignUpCommand))]
        [ObservableProperty] private string repeatedPassword;

        [ObservableProperty] private string phoneNumber;

        [RelayCommand(CanExecute = nameof(can_execute_sign_up))]
        private async void SignUp()
        {
            var req = new Req_SignUp
            {
                name = Name,
                surname = Surname,
                email = Email,
                username = Username,
                password = Password,
                phone_number = PhoneNumber,
                date_of_birth = null,
            };

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_SignUp, req);

                var message = await response.Content.ReadAsStringAsync();
                await _alertService.ShowAlertAsync("Info", message, "OK");

                // Operation was successful --> Clear All forms.
                if (response.IsSuccessStatusCode)
                {
                    Name = "";
                    Surname = "";
                    Email = "";
                    Username = "";
                    Password = "";
                    RepeatedPassword = "";
                    PhoneNumber = "";
                    await Shell.Current.GoToAsync("..");
                }
                
            }
            catch (Exception ex)
            {
                ;
            }
        }

        [RelayCommand]
        private async void Test()
        {
            try
            {
                string token = "eyJlbWFpbCI6InEiLCJwd2QiOiIkYXJnb24yaWQkdj0xOSRtPTY1NTM2LHQ9MyxwPTQkVGtPcE56dFBqU3duYTYrRTA5d094QSRyZUdBQzc2L2JTTjN4WGQrbmdhK0pucTJJMVhkUEgvbHhjWFEyZ3FxS3dZIn0.aIuQkQ.rddFz7XpGuhEyP-j96jrm-Uz8zc";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_ConfimEmail(token));

                // Operation was successful --> Clear All forms.
                var message = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    await _alertService.ShowAlertAsync("OK", message, "OK");
                }
                else
                {
                    await _alertService.ShowAlertAsync("NOK", message, "OK");
                }

            }
            catch (Exception ex)
            {
                ;
            }
        }

        public VM_SignUp()
        {
            _alertService = new AlertService();
        }

        private bool can_execute_sign_up() =>
            !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Surname) &&
            !string.IsNullOrEmpty(Email) &&
            !string.IsNullOrEmpty(Username) &&
            !string.IsNullOrEmpty(Password) &&
            string.Equals(Password, RepeatedPassword);
    }
}
