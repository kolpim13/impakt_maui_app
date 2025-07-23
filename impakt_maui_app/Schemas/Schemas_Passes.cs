using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    /* EXTERNAL PROVIDERS */
    public class Req_Create_ExternalProviders
    {
        required public string name { get; set; }
        public string? description { get; set; }
        required public bool is_partial_payment { get; set; }
        public decimal? partial_payment { get; set; }
    }
    public class Resp_Instance_ExternalProviders
    {
        required public int id { get; set; }
        required public string name { get; set; }
        public string? description { get; set; }
        required public bool is_partial_payment { get; set; }
        public decimal? partial_payment { get; set; } = null;
        required public bool is_deleted { get; set; }
    }
    public class Req_Update_ExternalProviders
    {
        required public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        required public bool is_partial_payment { get; set; }
        public decimal? partial_payment { get; set; }
    }

    /* PASS TYPES */
    public class Req_PassTypes_Create
    {
        required public string name { get; set; }
        public string? description { get; set; }
        required public decimal price { get; set; }
        public int? validity_days { get; set; }
        public int? maximum_entries { get; set; }
        required public bool requires_external_auth { get; set; }
        public string? external_provider_name { get; set; }
        public int? external_provider_id { get; set; }
        required public bool is_ext_event_pass { get; set; }
        public string? ext_event_code { get; set; }
    }
    public class Resp_PassTypes_Inst
    {
        required public int id { get; set; }
        required public string name { get; set; }
        public string? description { get; set; }
        required public decimal price { get; set; }
        public int? validity_days { get; set; }
        public int? maximum_entries { get; set; }
        required public bool requires_external_auth { get; set; }
        public string? external_provider_name { get; set; }
        public int? external_provider_id { get; set; }
        required public bool is_ext_event_pass { get; set; }
        public string? ext_event_code { get; set; }
        required public bool is_deleted { get; set; }
        public DateTime? delete_date { get; set; }
    }
    public class Req_PassTypes_Update
    {
        required public int id { get; set; }
        required public string name { get; set; }
        public string? description { get; set; }
        required public decimal price { get; set; }
        public int? validity_days { get; set; }
        public int? maximum_entries { get; set; }
        required public bool requires_external_auth { get; set; }
        public string? external_provider_name { get; set; }
        public int? external_provider_id { get; set; }
        required public bool is_ext_event_pass { get; set; }
        public string? ext_event_code { get; set; }
    }

    /* MEMBER PASS */
    public class Req_MemberPass_Add
    {
        required public string member_card_id { get; set; }
        required public int pass_type_id { get; set; }
    }
    public class Resp_MemberPass_Inst
    {
        required public int id {  set; get; }
        required public string member_card_id { get; set; }
        required public int pass_type_id { get; set; }
        required public string pass_type_name { get; set; }
        required public DateOnly purchase_date { get; set; }
        public DateOnly? expiration_date { get; set; }
        public int? entries_left { get; set; }
        required public bool requires_external_auth { get; set; }
        public int? external_provider_id { get; set; }
        public string? external_provider_name { get; set; }
        required public bool is_ext_event_pass { get; set; }
        public string? ext_event_code { get; set; }
        public string? status { get; set; }
        required public bool is_closed { get; set; }
    }

}
