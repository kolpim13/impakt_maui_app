using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
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

}
