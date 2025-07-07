using impakt_maui_app.Pages;

namespace impakt_maui_app
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            /* Register "invisible" route tabs. */
            Routing.RegisterRoute("Page_Statistics_AdminSummary", typeof(Page_Statistics_AdminSummary));

            Routing.RegisterRoute("Page_Scanner_QRScanner", typeof(Page_Scanner_QRScanner));

            Routing.RegisterRoute("Page_ExternalProvider", typeof(Page_ExternalProvider));
        }
    }
}
