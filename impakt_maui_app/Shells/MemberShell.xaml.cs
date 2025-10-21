using impakt_maui_app.Pages;

namespace impakt_maui_app
{
    public partial class MemberShell : Shell
    {
        public MemberShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Pages.Profile.QRCode), typeof(Pages.Profile.QRCode));
        }
    }
}
