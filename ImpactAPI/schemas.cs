using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Schemas
{
    public class Req_Scanner_Register
    {
        required public string admin_login { get; set; }
        required public string admin_password { get; set; }
        required public string scanner_login { get; set; }
        required public string scanner_password { get; set; }
        required public string name { get; set; }
        required public string location { get; set; }
        public string? rsa_public_key { get; set; }
    }
    public class Resp_Scanner_Register
    {
        required public string details { get; set; }
    }

    public class Req_Scanner_LogIn
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class Resp_Scanner_LogIn
    {
        required public string key { get; set; }
    }
}
