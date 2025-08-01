using impakt_maui_app.Pages;

namespace impakt_maui_app
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            /* Register "invisible" route tabs. */
            Routing.RegisterRoute("Page_Statistics_Admin_InstructorsStatistics", typeof(Page_Statistics_Admin_InstructorsStatistics));
            Routing.RegisterRoute("Page_Statistics_InstructorCheckInsDetailed", typeof(Page_Statistics_InstructorCheckInsDetailed));

            Routing.RegisterRoute("Page_Scanner_QRScanner", typeof(Page_Scanner_QRScanner));

            Routing.RegisterRoute("Page_ExternalProvider", typeof(Page_ExternalProvider));
            Routing.RegisterRoute("Page_PassTypes", typeof(Page_PassTypes));
            // Routing.RegisterRoute("Page_QRCode", typeof(Page_QRCode));
        }
    }
}
