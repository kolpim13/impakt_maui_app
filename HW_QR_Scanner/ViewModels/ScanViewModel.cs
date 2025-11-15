using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_QR_Scanner.ViewModels
{
    public partial class ScanViewModel : ViewModelBase
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        // ...

        /* PROPERTIES */
        // ...

        /* COMMANDS */
        public void Test()
        {
            this._navigation.ShowRegistration();
        }

        //public ScanViewModel(INavigation navigation)
        //{
        //    if (Design.IsDesignMode) 
        //        return;

        //    this._navigation = navigation;
        //}

        /* PUBLIC METHODS */
        // ...

        /* PRIVATE METHODS */
        // ...
    }
}
