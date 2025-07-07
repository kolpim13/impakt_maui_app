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
        required public bool Active { get; set; }
        required public bool IsPartialPayment { get; set; }
        public decimal? PartialPayment { get; set; }
    }

    public class Req_Create_ExternalProviders
    {
        public string name { get; set; }
        public string? description { get; set; }
        public bool active { get; set; }
        public bool is_partial_payment { get; set; }
        public decimal? partial_payment { get; set; }
    }
    public class Resp_Instance_ExternalProviders
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public bool active { get; set; }
        public bool is_partial_payment { get; set; }
        public decimal? partial_payment { get; set; }
    }
    public class Req_Update_ExternalProviders
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public bool? active { get; set; }
        public bool is_partial_payment { get; set; }
        public decimal? partial_payment { get; set; }
    }

    /* PASS TYPES */
    // ...
}
