using CommunityToolkit.Maui.Views;
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
    public class Popup_NewMember_VM
    {
        // [TBD] For future NVM pattern implementation
    }
    public partial class Popup_NewMember : Popup, INotifyPropertyChanged
    {
        // Bound properties
        private bool is_radio_button_visible = (UserInfo.AccountType == AccountType.Admin) ? true : false;
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

        public Popup_NewMember()
        {
            InitializeComponent();
            BindingContext = this;

            // Set size of the Popup form
            //double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.8;
            //double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density * 0.8;
            //double height = this.Size.Height;
            //this.Size = new Size(width, height);

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
            Req_Members_AddNewMember new_member = new Req_Members_AddNewMember
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
                send_welcome_email = SendEmailCheckBox.IsChecked,
                send_welcome_mms = false,
            };

            // Establish connection --> Send collected data --> react on response
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.NewMemberUrl, new_member);
                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("success", "member added!", "ok");
                }
                else
                {
                    // add some reaction later
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

        /* INotifyPropertyChanged Implemantation */
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class AccountTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AccountType selected && parameter is string param)
                return selected.ToString() == param;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string param &&
                Enum.TryParse(typeof(AccountType), param, out var result))
            {
                return result!;
            }

            return Binding.DoNothing;
        }
    }
}
