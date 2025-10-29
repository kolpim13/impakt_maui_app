using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    public class Req_LogIn_Username
    {
        required public string username { get; set; }
        required public string password { get; set; }
    }
    public class Resp_Members_Inst
    {
        required public string card_id { get; set; }
        required public string name { get; set; }
        required public string surname { get; set; }
        required public string email { get; set; }
        public string? phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        required public DateOnly registration_date { get; set; }
        required public int account_type { get; set; }
        public string? privileges { get; set; }
        public bool? last_checkin_success { get; set; }
        public DateTime? last_checkin_datetime { get; set; }
        public string? token { get; set; }
        required public bool activated { get; set; }
    }
    public class Resp_Paginated_Members_Instances
    {
        public int total { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
        public int remaining { get; set; }
        public List<Resp_Members_Inst> items { get; set; }
    }
    public class Req_Member_Add
    {
        required public string name { get; set; }
        required public string surname { get; set; }
        required public string email { get; set; }
        public string? phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        required public int account_type { get; set; }
        public bool? send_welcome_email { get; set; }
        public bool? send_welcome_mms { get; set; }
    }
    public class Req_SignUp
    {
        required public string name { get; set; }
        required public string surname { get; set; }
        required public string email { get; set; }
        public string? phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        required public string username { get; set; }
        required public string password { get; set; }
    }
    public class Resp_Generic
    {
        required public string details { get; set; }
    }

}
