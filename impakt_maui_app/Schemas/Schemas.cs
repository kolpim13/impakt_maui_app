using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.Schemas
{
    public struct PaginatedRequestDetails
    {
        private int total;
        private int page;
        private int page_size;
        private int remaining;
    }
}
