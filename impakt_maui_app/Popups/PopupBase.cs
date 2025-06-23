using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace impakt_maui_app.Popups
{
    public class PopupBase : Popup
    {
        public PopupBase() 
        {
            // Set size of the Popup form
            double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.8;
            double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density * 0.8;
            this.Size = new Size(width, height);
        }
    }
}
