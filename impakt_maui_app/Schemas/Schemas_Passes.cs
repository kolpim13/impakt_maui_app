using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    /* EXTERNAL PROVIDERS */
    public class Model_ExternalProvider
    {
        required public int Id { get; set; }
        required public string Name { get; set; }
        public string? Description { get; set; }
        required public bool IsPartialPayment { get; set; }
        public decimal? PartialPayment { get; set; }
        required public bool IsDeleted { get; set; }

        public static Model_ExternalProvider From_Resp_Inst(Resp_Instance_ExternalProviders inst) =>
           new Model_ExternalProvider
           {
               Id = inst.id,
               Name = inst.name,
               Description = inst.description,
               IsPartialPayment = inst.is_partial_payment,
               PartialPayment = inst.partial_payment,
               IsDeleted = inst.is_deleted,
           };
    }

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
    public class Model_PassType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? ValidityDays { get; set; }
        public int? MaximumEntries { get; set; }
        public bool RequiresExternalAuth { get; set; }
        public string? ExternalProviderName { get; set; }
        public int? ExternalProviderId { get; set; }
        public bool IsExtEventPass { get; set; }
        public string? ExtEventCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }

        public static Model_PassType From_Resp_Inst(Resp_PassTypes_Inst inst) =>
            new Model_PassType
            {
                Id = inst.id,
                Name = inst.name,
                Description = inst.description,
                Price = inst.price,
                ValidityDays = inst.validity_days,
                MaximumEntries = inst.maximum_entries,
                RequiresExternalAuth = inst.requires_external_auth,
                ExternalProviderName = inst.external_provider_name,
                ExternalProviderId = inst.external_provider_id,
                IsExtEventPass = inst.is_ext_event_pass,
                ExtEventCode = inst.ext_event_code,
                IsDeleted = inst.is_deleted,
                DeleteDate = inst.delete_date,
            };
    }

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

}
