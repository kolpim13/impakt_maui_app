using CommunityToolkit.Mvvm.ComponentModel;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM.Profile
{
    public partial class VM_MemberProfile : ObservableObject
    {
        /* PROPERTIES */
        [ObservableProperty]
        private Resp_Members_Inst member;

        public VM_MemberProfile()
        {
            ;
        }

        /* COMMANDS */
        // ...

        /* COMMANDS CONDITIONS */
        // ...

        /* PRIVATE METHODS */ 
    }
}

namespace impakt_maui_app.VM
{
    public class Converter_AcoountTypeIntToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            AccountType temp = (AccountType)Enum.Parse(typeof(AccountType), value.ToString());
            return temp.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && targetType.IsEnum)
                return Enum.Parse(targetType, str, ignoreCase: true);

            return BindableProperty.UnsetValue;
        }
    }
}
