using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using impakt_maui_app.Schemas;
using Newtonsoft.Json;

namespace impakt_maui_app
{
    public enum PassType: ushort
    {
        NO             = 0,
        LIMITED_1      = 1,
        LIMITED_4      = 4,
        LIMITED_8      = 8,
        LIMITED_12     = 12,
        UNLIMITED      = 20,
        MEDICOVER_1    = 21,
        PZU_1          = 41,
        MULTISPORT_1   = 61,
        OTHER_1        = 101,
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
        public static string? Token { get; set; }
        public static bool? Activated { get; set; }

        public static void Fill_FromLogInResp(string json)
        {
            Resp_LogIn? user = JsonConvert.DeserializeObject<Resp_LogIn>(json);

            Card_ID = user.card_id;
            Name = user.name;
            SurName = user.surname;
            Email = user.email;
            Phone = user.phone_number;
            DateOfBirth = user.date_of_birth;
            AccountType = (AccountType)user.account_type;
            Token = user.token;
            Activated = user.activated; 
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
        public static string Get_MemberInfo_Url(string member_id) => string.Format("{0}/members/get/info/{1}/{2}", URL, UserInfo.Card_ID, member_id);

        public static string Post_Member_UpdatePass
        {
            get => string.Format("{0}/membres/update/pass/{1}", URL, UserInfo.Card_ID);
        }
        public static string AddNewMemberUrl
        {
            get => string.Format("{0}/members/add/{1}", URL, UserInfo.Card_ID);
        }
        public static string CheckInUrl
        {
            get { return string.Format("{0}/checkin/{1}", URL, UserInfo.Card_ID); }
        }
        public static string LogInUrl
        {
            get { return URL + "/login/username"; }
        }
        public static string CheckInHistoryUrl
        {
            get { return string.Format("{0}/checkin/log/filtered", URL); }
        }
        public static string StatisticInstructorUrl
        {
            get { return string.Format("{0}/statistics/instructor/entries_amount/{1}", URL, UserInfo.Card_ID); }
        }
        public static string StatisticAllInstructorsUrl
        {
            get { return string.Format("{0}/statistics/all_instructors/entries_amount/{1}", URL, UserInfo.Card_ID); }
        }
    }
}
