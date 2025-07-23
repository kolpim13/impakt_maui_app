using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    public class Req_Statistics_InstructorsCheckIns
    {
        required public DateOnly date_from { get; set; }
        required public DateOnly date_to { get; set; }
    }
    public class Resp_Statistics_InstructorsCheckIns
    {
        required public string validated_by_card_id { get; set; }
        required public string validated_by_name { get; set; }
        required public string validated_by_surnamename { get; set; }
        required public int count { get; set; }
    }
    public class Req_Statistics_InstructorCheckInsDetailed
    {
        required public string validated_by_card_id { get; set; }
        required public DateOnly date_from { get; set; }
        required public DateOnly date_to { get; set; }
        public int? page { get; set; }
        public int? page_size { get; set; }
    }
    public class Resp_Statistics_InstructorCheckInsDetailed
    {
        required public string name { get; set; }
        required public string surname { get; set; }
        required public DateTime date_time { get; set; }
        required public bool is_successful { get; set; }
        public string? rejected_reason { get; set; }
    }
    public class Resp_Paginated_Statistics_InstructorCheckInsDetailed
    {
        public int total { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
        public int remaining { get; set; }
        public List<Resp_Statistics_InstructorCheckInsDetailed> items { get; set; }
    }
}
