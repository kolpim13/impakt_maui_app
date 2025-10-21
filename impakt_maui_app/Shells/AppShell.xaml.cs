using impakt_maui_app.Pages;
using impakt_maui_app.Pages.Statistics;

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
            Routing.RegisterRoute("Page_AllMembers", typeof(Page_AllMembers));  // This name convention is more preferable

            Routing.RegisterRoute("Page_Scanner_QRScanner", typeof(Page_Scanner_QRScanner));

            Routing.RegisterRoute("Page_ExternalProvider", typeof(Page_ExternalProvider));
            Routing.RegisterRoute("Page_PassTypes", typeof(Page_PassTypes));

            /* Profile related pages. */
            Routing.RegisterRoute(nameof(Pages.Profile.QRCode), typeof(Pages.Profile.QRCode));
        }
    }
}
