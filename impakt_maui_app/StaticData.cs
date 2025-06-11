using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace impakt_maui_app
{
    public enum PassType: ushort
    {
        No = 0,
        Limited = 1,
        Unlimited = 2,
    }

    public enum AccountType: ushort
    {
        Admin = 0,
        Instructor = 1,
        Member = 2,
    }

    public class BackendReq_RegisterNewMember
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string? phone_number { get; set; } // Optional
        public DateTime? date_of_birth { get; set; } // Optional
        public string? account_type { get; set; } // Optional
    }

    public class BackendReq_CheckInMember
    {
        public string card_id { get; set; }
    }

    public class BackendReq_UpdatePassDetails
    {
        public string card_id { get; set; }
        public int pass_type { get; set; }
        public int entrances_left { get; set; }
        public DateOnly expiration_date { get; set; }
    }

    /* The data will be returned from backend when user is logged in */
    public class BackendResp_LogIn
    {
        public string card_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public int? account_type { get; set; }
    }

    public class BackendReq_MemberInfo
    {
        public string card_id { get; set; }
    }
    public class BackendResp_MemberInfo
    {
        public string card_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string? phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public int account_type { get; set; }
    }

    public class BackendReq_CheckInFilters
    {
        public int limit { get; set; }
        public string? control_name { get; set; }
        public string? control_surname { get; set; }
        public string? hall { get; set; }
        public string? card_id { get; set; }
        public string? name { get; set; }
        public string? surname { get; set; }
        public DateTime? date_time_min { get; set; }
        public DateTime? date_time_max { get; set; }
    }
    public class BackendResp_CheckIn
    {
        public string control_name { get; set; }
        public string control_surname { get; set; }
        public string hall { get; set; }
        public string card_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime date_time { get; set; }
    }
    public static class UserInfo
    {
        //public static int? ID { get; set; } = int.MaxValue;

        public static string? Card_ID { get; set; }
        public static string? Name { get; set; } 
        public static string? SurName { get; set; }
        public static string? Email { get; set; }
        public static string? Phone { get; set; }
        public static DateOnly? DateOfBirth{ get; set; }
        public static AccountType? AccountType { get; set; }

        public static void Fill_FromLogInResp(string json)
        {
            BackendResp_LogIn? user = JsonConvert.DeserializeObject<BackendResp_LogIn>(json);

            Card_ID = user.card_id;
            Name = user.name;
            SurName = user.surname;
            Email = user.email;
            Phone = user.phone;
            DateOfBirth = user.date_of_birth;
            AccountType = (AccountType)user.account_type;
        }
    }

    public static class Network
    {
#if ANDROID
        // public static string URL { get; set; } = "http://192.168.0.1:8000";  // ZF
        // public static string URL { get; set; } = "http://192.168.0.199:8000";  // Personal
        public static string URL { get; set; } = "http://192.168.0.6:8000";  // Grzegosz
#else
        public static string URL { get; set; } = "http://localhost:8000";
#endif

        public static string NewMemberUrl
        {
            get { return string.Format("{0}/members/{1}/add", URL, UserInfo.Card_ID); }
        }

        public static string LogInUrl
        {
            get { return URL + "/login/username"; }
        }
        public static string CheckInHistoryUrl
        {
            get { return string.Format("{0}/checkin/log/filtered", URL); }
        }
    }
}
