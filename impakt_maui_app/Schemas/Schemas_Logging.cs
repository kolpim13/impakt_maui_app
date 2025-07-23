using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    public class Req_CheckIn_Add
    {
        public string? validated_by_card_id { get; set; }
        public int? external_provider_id { get; set; }
        required public string member_card_id { get; set; }
    }
    public class Resp_ChecIn_Inst
    {
        required public int id { get; set; }
        public string? validated_by_card_id { get; set; }
        public string? validated_by_name { get; set; }
        public string? validated_by_surnamename { get; set; }
        public string? hall { get; set; }
        public int? member_pass_id { get; set; }
        public int? pass_id { get; set; }
        public string? pass_name { get; set; }
        public bool? is_ext_event_pass { get; set; }
        public string? ext_event_code { get; set; }
        public int? external_provider_id { get; set; }
        public string? external_provider_name { get; set; }
        required public string member_card_id { get; set; }
        required public string member_name { get; set; }
        required public string member_surname { get; set; }
        required public DateTime date_time { get; set; }
        required public bool is_successful { get; set; }
        public string? rejected_reason { get; set; }
    }
}
