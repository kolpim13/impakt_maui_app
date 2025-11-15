using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_QR_Scanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        private readonly INavigation _navigation;

        /* PROPERTIES */
        // ...

        /* COMMANDS */
        // ...

        public MainViewModel(INavigation navigation)
        {
            this._navigation = navigation;
        }

        /* PRIVATE METHODS */
        // ...
    }
}
