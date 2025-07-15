using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
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