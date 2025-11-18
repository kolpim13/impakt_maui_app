using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HW_QR_Scanner.ViewModels;

namespace HW_QR_Scanner.Views;

public partial class ScanView : UserControl
{
    public ScanView()
    {
        InitializeComponent();
    }
    public void OnOverlayTapped(object? obj, Avalonia.Input.TappedEventArgs args)
    {
        if (this.DataContext is ScanViewModel vm)
        {
            vm.DismissOverlay();
        }
    }
}