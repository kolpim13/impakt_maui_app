using CommunityToolkit.Maui.Views;
using impakt_maui_app.Models;
using impakt_maui_app.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM
{
    /* Page_Scanner_QRScanner */
    public class QRScanModeToIsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QRScanMode mode && parameter is string param)
                return mode.ToString() == param;

            return false;
        }

        // Convert Back not used at the moment --> not implemented.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    /* */
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

    public class Converter_EnumToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            // Default fallback: just return the enum name
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && targetType.IsEnum)
                return Enum.Parse(targetType, str, ignoreCase: true);

            return BindableProperty.UnsetValue;
        }
    }

    /* Show DisplayAllert from a VM */
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string cancel);
        Task<string> ShowActionSHeet(string title, string cancel, string destruction, params string[] buttons);
    }

    public class AlertService : IAlertService
    {
        [Obsolete]
        public async Task ShowAlertAsync(string title, string message, string cancel)
        {
            var currentPage = Application.Current?.MainPage;
            if (currentPage != null)
            {
                await currentPage.DisplayAlert(title, message, cancel);
            }
        }

        [Obsolete]
        public async Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel)
        {
            var currentPage = Application.Current?.MainPage;
            if (currentPage != null)
            {
                return await currentPage.DisplayAlert(title, message, accept, cancel);
            }
            return false;
        }

        public async Task<string> ShowActionSHeet(string title, string cancel, string destruction, params string[] buttons)
        {
            var currentPage = Application.Current?.MainPage;
            return await currentPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }
    }
}
