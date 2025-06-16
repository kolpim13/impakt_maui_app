namespace impakt_maui_app.Schemas
{
    public class Req_LogIn
    {
        public string username { get; set; } = "";
        public string password { get; set; } = "";
    }

    public class Resp_LogIn
    {
        public string card_id { get; set; } = "";
        public string name { get; set; } = "";
        public string surname { get; set; } = "";
        public string email { get; set; } = "";
        public int account_type { get; set; }

        public string? phone_number { get; set; }
        public DateOnly? date_of_birth { get; set; }
        public string? token { get; set; }
        public bool? activated { get; set; }

    }

}
