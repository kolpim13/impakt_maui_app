using CommunityToolkit.Mvvm.ComponentModel;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
    
    public static class GeneralResources
    {
        /* EXTERNAL PROVIDERS */
        public static bool IsExternalProvidersObtained = false;
        private static List<Model_ExternalProvider> ExternalProviders = new List<Model_ExternalProvider>();
        public static ObservableCollection<Model_ExternalProvider> Get_ExternalProviders_AsCollection() =>
            new ObservableCollection<Model_ExternalProvider>(ExternalProviders);
        public static void Get_ExternalProviders_AsCollection(ObservableCollection<Model_ExternalProvider> collection)
        {
            foreach (Model_ExternalProvider provider in ExternalProviders)
            {
                collection.Add(provider);
            }
        }
            
        public static void Set_ExernalProviders_FromCollection(ObservableCollection<Model_ExternalProvider> collection) =>
            ExternalProviders = [.. collection];
        public static async Task ExternalProviders_FromDataBase()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_ExternalProviders);
                if (response.IsSuccessStatusCode)
                {
                    ExternalProviders.Clear();
                    var all_providers = await response.Content.ReadFromJsonAsync<List<Resp_Instance_ExternalProviders>>();
                    foreach (Resp_Instance_ExternalProviders provider in all_providers)
                    {
                        ExternalProviders.Add(Model_ExternalProvider.From_Resp_Inst(provider));
                    }

                    IsExternalProvidersObtained = true;
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        /* PASS TYPES */
        public static bool IsPassTypesObtained = false;
        private static List<Model_PassType> PassTypes = new List<Model_PassType>();
        public static ObservableCollection<Model_PassType> Get_PassTypes_AsCollection() =>
            new ObservableCollection<Model_PassType>(PassTypes);
        public static void Get_PassTypes_AsCollection(ObservableCollection<Model_PassType> collection)
        {
            foreach (Model_PassType pass_type in PassTypes) 
            { 
                collection.Add(pass_type); 
            }
        }
        public static void Set_PassTypes_FromCollection(ObservableCollection<Model_PassType> collection) =>
            PassTypes = [.. collection];
        public static async Task PassTypes_FromDataBase()
        { 
            try
            {
                HttpClient _httpClient = new HttpClient();
                HttpResponseMessage response = await _httpClient.GetAsync(Network.Get_PassTypes);
                if (response.IsSuccessStatusCode)
                {
                    PassTypes.Clear();
                    var pass_types = await response.Content.ReadFromJsonAsync<List<Resp_PassTypes_Inst>>();
                    foreach(Resp_PassTypes_Inst pass_type in pass_types)
                    {
                        PassTypes.Add(Model_PassType.From_Resp_Inst(pass_type));
                    }

                    IsPassTypesObtained = true;
                }
            }
            catch
            {
                ;
            }
        }
    }

    public static class User
    {
        public static Model_Member Account { get; set; } = Model_Member.GetDefaultInst();
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

        /* Links: ExternalProviders */
        public static string Post_ExternalProviders_Create
        {
            /* Token should be used */
            get => string.Format("{0}/external_providers", URL);
        }
        public static string Get_ExternalProviders
        {
            /* Token should be used */
            get => string.Format("{0}/external_providers", URL);
        }
        public static string Put_ExternalProviders_Update
        {
            /* Token should be used */
            get => string.Format("{0}/external_providers", URL);
        }

        /* Links: PassTypes */
        public static string Post_PassTypes_Create
        {
            get => string.Format("{0}/pass_types", URL);
        }
        public static string Get_PassTypes
        {
            /* Token should be used */
            get => string.Format("{0}/pass_types", URL);
        }
        public static string Put_PassTypes_Update
        {
            /* Token should be used */
            get => string.Format("{0}/pass_types", URL);
        }

        /* Links: MemberPass */
        public static string Post_MemberPass_Add
        {
            get => string.Format("{0}/member_pass", URL);
        }

        public static string Get_MemberPass_Active(string member_card_id) =>
            /* Token should be used */
            string.Format("{0}/member_pass/active/{1}", URL, member_card_id);

        /* Links: LogIn */
        public static string Post_LogIn_Username =>
            string.Format("{0}/login/username", URL);

        /* Links: Members */
        public static string Post_Member_Add =>
            string.Format("{0}/members/add", URL);
        public static string Get_Member_Inst(string member_id) =>
            string.Format("{0}/members/{1}", URL, member_id);

        /* Links: Statistics */


        /* Links: ... */

        public static string Post_Member_UpdatePass
        {
            get => string.Format("{0}/membres/update/pass/{1}", URL, User.Account.CardId);
        }
        public static string CheckInUrl
        {
            get { return string.Format("{0}/checkin/{1}", URL, User.Account.CardId); }
        }

        public static string CheckInHistoryUrl
        {
            get { return string.Format("{0}/checkin/log/filtered", URL); }
        }
        public static string StatisticInstructorUrl
        {
            get { return string.Format("{0}/statistics/instructor/entries_amount/{1}", URL, User.Account.CardId); }
        }
        public static string StatisticAllInstructorsUrl
        {
            get { return string.Format("{0}/statistics/all_instructors/entries_amount/{1}", URL, User.Account.CardId); }
        }
    }
}
