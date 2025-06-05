using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace impakt_maui_app
{
    public class BackendResp_UserInfo
    {
        public string status { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }

    public static class UserInfo
    {
        public static string? Name { get; set; } = "Name";
        public static string? SurName { get; set; } = "SurName";

        public static void LoadData(string json)
        {
            BackendResp_UserInfo? user = JsonConvert.DeserializeObject<BackendResp_UserInfo>(json);

            Name = user.name;
            SurName = user.surname;
        }
    }
}
