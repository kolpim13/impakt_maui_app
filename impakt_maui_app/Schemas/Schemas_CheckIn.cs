using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    /* Checkin related */
    public class Req_CheckIn
    {
        public string card_id { get; set; }
        public string? hall { get; set; }
        public bool? external_payment { get; set; }
        public int? pass_type { get; set; }
    }

    /* To get amount of members were  */
    public class Req_Statistics_AmountInstructor
    {
        public string card_id { get; set; } = "";
        public DateTime date_time_min { get; set; }
        public DateTime date_time_max { get; set; }
    }

    public class Req_Statistics_AmountAllInstructors
    {
        public DateTime date_time_min { get; set; }
        public DateTime date_time_max { get; set; }
    }

    public class Resp_Statistics_AmountInstructor
    {
        public string name { get; set; }
        public string surname { get; set; }
        public int entries_pass { get; set; }
        public int entries_pzu { get; set; }
        public int entries_medicover { get; set; }
        public int entries_total { get; set; }
    }
}

