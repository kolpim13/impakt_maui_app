using impakt_maui_app.Pages;
using impakt_maui_app.Pages.StratUp;

namespace impakt_maui_app
{
    public partial class LogInShell : Shell
    {
        public LogInShell()
        {
            InitializeComponent();

            /* Register "invisible" route tabs. */
            Routing.RegisterRoute("Page_Login", typeof(Page_Login));
            Routing.RegisterRoute("Page_SignUp", typeof(Page_SignUp));
        }
    }
}
