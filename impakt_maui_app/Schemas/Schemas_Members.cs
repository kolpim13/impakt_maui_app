using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    /* New member - registration || adding by the administrator. */
    public class Req_Members_AddNewMember
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }

        // Optional
        public string? phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public int? account_type { get; set; }

        // Optional[Options]
        public bool? send_welcome_email { get; set; } = true;
        public bool? send_welcome_mms { get; set; } = false;
    }

    public class Resp_Members_AddNewMembers
    {

    }

    /* Information about the member */
    public class Resp_Members_MemberInfo
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public int account_type { get; set; }
        public int pass_type { get; set; }
        public int entrances_left { get; set; }
        public DateOnly? expiration_date { get; set; }
        public DateTime? last_check_in { get; set; }
    }

    /* Update member`s data written in DB */
    public class Req_Members_UpdatePassData
    {
        public string card_id { get; set; }
        public int pass_type { get; set; }
    }
    public class Resp_Member_UpdatePass
    {
        public int pass_type { get; set; }
        public int entrances_left { get; set; }
        public DateOnly expiration_date { get; set; }
    }
}