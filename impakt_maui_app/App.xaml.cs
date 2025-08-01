using impakt_maui_app.Pages;

namespace impakt_maui_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // MainPage = new NavigationPage(new Page_Start());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new LogInShell());
        }

        private async Task LoadMainShellAsync()
        {
            if (User.Account.AccountType is Models.AccountType.Member)
            {
                Application.Current.MainPage = new MemberShell();
            }
        }
    }
}