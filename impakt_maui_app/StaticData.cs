using CommunityToolkit.Mvvm.ComponentModel;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using impakt_maui_app.VM;
using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
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
    public static class GeneralResources
    {
        /* MEMBERS */
        public static async Task<Model_Member> Get_Member_FromDB(string member_id)
        {
            Model_Member member = null;
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_Member_Inst(member_id));

                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();
                    member = Model_Member.From_Resp_Inst(resp);
                }
            }
            catch (Exception ex) 
            {
                ;
            }
            return member;
        }

        /* EXTERNAL PROVIDERS */
        public static bool IsExternalProvidersObtained = false;
        public static readonly ExternalProvider dummy_provider = new ExternalProvider
        {
            Id = -1,
            Name = "No Provider",
            IsPartialPayment = false,
            IsDeleted = false,
        };
        private static List<ExternalProvider> ExternalProviders = new List<ExternalProvider>();
        public static ObservableCollection<ExternalProvider> Get_ExternalProviders_AsCollection() =>
            new ObservableCollection<ExternalProvider>(ExternalProviders);
        public static void Get_ExternalProviders_AsCollection(ObservableCollection<ExternalProvider> collection)
        {
            foreach (ExternalProvider provider in ExternalProviders)
            {
                collection.Add(provider);
            }
        }
            
        public static void Set_ExernalProviders_FromCollection(ObservableCollection<ExternalProvider> collection) =>
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
                        ExternalProviders.Add(ExternalProvider.From_Resp_Inst(provider));
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
        private static List<EntryPass> PassTypes = new List<EntryPass>();
        public static ObservableCollection<EntryPass> Get_PassTypes_AsCollection() =>
            new ObservableCollection<EntryPass>(PassTypes);
        public static void Get_PassTypes_AsCollection(ObservableCollection<EntryPass> collection)
        {
            foreach (EntryPass pass_type in PassTypes) 
            { 
                collection.Add(pass_type); 
            }
        }
        public static void Set_PassTypes_FromCollection(ObservableCollection<EntryPass> collection) =>
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
                        PassTypes.Add(EntryPass.From_Resp_Inst(pass_type));
                    }

                    IsPassTypesObtained = true;
                }
            }
            catch
            {
                ;
            }
        }
    
        /* MEMBER PASSES */
        public static async Task Get_MemberPass_AsCollection_FromDB(ObservableCollection<Model_MemberPass> collection, string member_id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_MemberPass_Active(member_id));

                if (response.IsSuccessStatusCode)
                {
                    var member_passes = await response.Content.ReadFromJsonAsync<List<Resp_MemberPass_Inst>>();
                    foreach (Resp_MemberPass_Inst member_pass in member_passes)
                    {
                        collection.Add(Model_MemberPass.From_Resp_Inst(member_pass));
                    }
                }
            }
            catch (Exception ex)
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
        /* General */
        public static string ParseResponse_AsString(HttpResponseMessage response) =>
            string.Format("Status code: {0} - {1}", (int)response.StatusCode, response.StatusCode);

        public static async Task<string> ParseResponse_AsString_FullInfo(HttpResponseMessage response)
        {
            string reason = await response.Content.ReadAsStringAsync();
            return string.Format("Status code: {0} - {1}\n{2}", (int)response.StatusCode, response.StatusCode, reason);
        }

#if ANDROID
        // public static string URL { get; set; } = "http://192.168.0.1:8000";  // ZF
        // public static string URL { get; set; } = "http://192.168.0.199:8000";  // Personal
        // public static string URL { get; set; } = "http://192.168.0.6:8000";  // Grzegosz
        // public static string URL { get; set; } = "http://192.168.0.1:8080"; // Local Host HTTP
        // public static string URL { get; set; } = "https://bda238d32ade.ngrok-free.app"; // Local Host through grok
        // public static string URL { get; set; } = "http://209.38.198.242:8000"; // HTTP hosting
        public static string URL { get; set; } = "https://lmapkt.com"; // HTTPS hosting
#else
        // public static string URL { get; set; } = "http://localhost:8000";
        public static string URL { get; set; } = "https://lmapkt.com"; // HTTPS hosting
#endif

        /* Links: Members */
        public static string Post_Member_Add =>
            string.Format("{0}/members/add", URL);
        public static string Get_Member_Inst(string member_id) =>
            string.Format("{0}/members/{1}", URL, member_id);
        public static string Get_Members_Instances(int page, int page_size) =>
            $"{URL}/members?page={page}&page_size={page_size}";

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

        /* Links: User Management */
        public static string Post_LogIn =>
            string.Format("{0}/login/username", URL);

        public static string Post_SignUp =>
            $"{URL}/signup";

        public static string Get_ConfimEmail(string token) =>
            $"{URL}/confirm/{token}";

        /* Links: Logging */
        public static string Post_CheckIn_Add =>
            string.Format("{0}/logging/checkin", URL);

        /* Links: Statistics */
        public static string Post_Statistics_InstructorsCheckIns =>
            string.Format("{0}/statistics/instructors_checkins", URL);

        public static string Post_Statistics_InstructorCheckInsDetailed =>
            $"{URL}/statistics/instructor_checkins/detailed";

        /* Links: Combined */
        // ...
    }

    /* Brightness regulation */
    public interface IScreenBrightness
    {
        void SetValue(float level);
        void SetMaximum();
        void RestorePreviousValue();
    }

    public sealed class ScreenBrightnessService : IScreenBrightness
    {
        float _previous = -1f;

        public void SetValue(float value)
        {
#if ANDROID
            var activity = Platform.CurrentActivity!;
            var win = activity.Window!;
            var attrs = win.Attributes!;
            _previous = attrs.ScreenBrightness;

            attrs.ScreenBrightness = Math.Clamp(value, 0f, 1f);
            win.Attributes = attrs;
#else
            return;
#endif

        }

        public void SetMaximum()
        {
            this.SetValue(1.0f);
        }
        public void RestorePreviousValue()
        {
#if ANDROID
            var activity = Platform.CurrentActivity!;
            var win = activity.Window!;
            var attrs = win.Attributes!;
            attrs.ScreenBrightness = _previous;
            win.Attributes = attrs;

            _previous = -1f;
#else
            return;
#endif
        }
    }
}
