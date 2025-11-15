using CommunityToolkit.Mvvm.ComponentModel;

namespace HW_QR_Scanner.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        public INavigation _navigation;

        public void SetNavigation(INavigation navigation)
        {
            this._navigation = navigation;
        }
    }
}
