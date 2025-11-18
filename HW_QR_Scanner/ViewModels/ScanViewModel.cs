using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW_QR_Scanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ImpactAPI;

namespace HW_QR_Scanner.ViewModels
{
    public partial class ScanViewModel : ViewModelBase
    {
        /* DEFINITIONS */
        private CancellationTokenSource? overlay_token_source;

        /* PRIVATE DATA */
        private HWScanner scanner;

        /* PROPERTIES */
        [ObservableProperty]
        private bool isOverlayVisible = false;

        [ObservableProperty]
        private Color overlayColor = Colors.Transparent;

        [ObservableProperty]
        private string overlayText = string.Empty;

        /* COMMANDS */
        // ...

        public ScanViewModel()
        {
            scanner = new HWScanner(port_name: "COM5", baudrate: 115200);
            scanner.DataReceived += scanner_data_received;
            scanner.StartScan();
        }

        /* PUBLIC METHODS */
        public void DismissOverlay() =>
            overlay_token_source?.Cancel();

        /* PRIVATE METHODS */
        private async void scanner_data_received(string card_id)
        {
            // Stop scanning
            scanner.StopScan();

            // Test: Show overlay --> start scanning again
            await show_overlay(color: Colors.Green, text: "Scanning complete", ms: 2200);
            scanner.StartScan();
        }

        private async Task check_in(string card_id)
        {
            Requests.Post_Scanner_CheckIn();
        }

        private async Task user_info(string card_id)
        {
            Requests.Post_Scanner_MemberInfo();
        }

        private async Task show_overlay(Color color, string text = "", int ms = 2200)
        {
            overlay_token_source?.Cancel();
            overlay_token_source = new CancellationTokenSource();
            CancellationToken cancel_token = overlay_token_source.Token;

            try
            {
                OverlayColor = color;
                OverlayText = text;
                IsOverlayVisible = true;

                await Task.Delay(ms, cancel_token);
            }
            catch (TaskCanceledException) { /* user tapped */ }
            finally
            {
                IsOverlayVisible = false;
                OverlayText = string.Empty;
                OverlayColor = Colors.Transparent;
            }
        }
    }
}
