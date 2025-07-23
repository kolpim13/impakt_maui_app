using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Popups
{
    public partial class Popup_NewMember : Popup, INotifyPropertyChanged
    {
        // Bound properties
        private bool is_radio_button_visible = (
            User.Account.AccountType == AccountType.Root ||
            User.Account.AccountType == AccountType.Admin) ? true : false;
        public bool IsRadioButtonVisible { get => is_radio_button_visible; }

        private AccountType selected_account_type = AccountType.Member;
        public AccountType SelectedAccountType
        {
            get => selected_account_type;
            set
            {
                if (selected_account_type != value)
                {
                    selected_account_type = (AccountType)value;
                    OnPropertyChanged(nameof(SelectedAccountType));
                }
            }
        }

        private bool isDateOfBirthVisible = false;
        public bool IsDateOfBirthVisible
        {
            get => isDateOfBirthVisible;
            set
            {
                if (isDateOfBirthVisible != value)
                {
                    isDateOfBirthVisible = value; 
                    OnPropertyChanged(nameof(IsDateOfBirthVisible));
                }
            }
        }

        private DateTime? _date_of_birth = null;
        public DateTime SelectedDateOfBirth
        {
            get => (_date_of_birth == null) ? DateTime.Now : (DateTime)_date_of_birth;
            set
            {
                if (_date_of_birth != value)
                {
                    _date_of_birth = value;
                    OnPropertyChanged(nameof(SelectedDateOfBirth));
                }
            }
        }
     
        private bool sendWelcomeMail = true;
        public bool SendWelcomeMail
        {
            get => sendWelcomeMail;
            set
            {
                if (sendWelcomeMail != value)
                {
                    sendWelcomeMail = value;
                    OnPropertyChanged(nameof(SendWelcomeMail));
                }
            }
        }

        private bool sendWelcomeMms = false;
        public bool SendWelcomeMms
        {
            get => sendWelcomeMms;
            set
            {
                if (sendWelcomeMms != value)
                {
                    sendWelcomeMms = value;
                    OnPropertyChanged(nameof(SendWelcomeMms));
                }
            }
        }

        public Popup_NewMember()
        {
            InitializeComponent();
            BindingContext = this;

            // Set size of the Popup form
            double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.8;
            double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density * 0.8;
            this.Size = new Size(width, height);

            // Initialize variables
            DateOfBirthPicker.Date = DateTime.Now;
        }

        private async void OnAddMemberlicked(object sender, EventArgs e)
        {
            // Make sure all necessary fields are filled
            if ((string.IsNullOrEmpty(NameEntry.Text)) ||
                string.IsNullOrEmpty(SurnameEntry.Text) ||
                string.IsNullOrEmpty(EmailEntry.Text))
            {
                return;
            }

            // Make button inactive during operation
            ButtonAddMember.IsEnabled = false;

            // Coolect data --> validate them --> prepare to be sent
            Req_Member_Add new_member = new Req_Member_Add
            {
                // Properties
                name = NameEntry.Text,
                surname = SurnameEntry.Text,
                email = EmailEntry.Text,

                // Optional properties
                phone_number = string.IsNullOrEmpty(PhoneEntry.Text) ? null : PhoneEntry.Text,
                date_of_birth = _date_of_birth == null ? null : DateOnly.FromDateTime((DateTime)_date_of_birth),
                account_type = (int)selected_account_type,

                // Options
                send_welcome_email = SendWelcomeMail,
                send_welcome_mms = SendWelcomeMms, // Temporlarly
            };

            // Establish connection --> Send collected data --> react on response
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_Member_Add, new_member);
                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("success", "member added!", "ok");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await Shell.Current.DisplayAlert("Error", error, "OK");
                }
            }
            catch (Exception ex)
            {
                ;
            }
            
            // Restore button disregardless result
            ButtonAddMember.IsEnabled = true;
        }

        /* INotifyPropertyChanged Implemantation */
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
