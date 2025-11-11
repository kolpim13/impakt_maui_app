using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http.Json;
using ZXing.Net.Maui;


namespace impakt_maui_app.VM.Scanner
{
    public partial class VM_Scanner_QR : ObservableObject, IQueryAttributable
    {
        /* DEFINITIONS */
        // ...

        /* PRIVATE DATA */
        private QRScanMode scan_type = QRScanMode.None;
        private ExternalProvider provider;
        private EntryPass pass;

        private CancellationTokenSource? overlay_token_source; 

        /* PROPERTIES 
         * Scanner related */

        [ObservableProperty]
        private bool isDetecting = true;

        /* PROPERTIES 
         * Overlay */

        [ObservableProperty]
        private bool isOverlayVisible = false;

        [ObservableProperty]
        private Color overlayColor = Colors.Transparent;

        [ObservableProperty]
        private double overlayOpacity = 0.85;

        [ObservableProperty]
        private string overlayText = string.Empty;

        /* COMMANDS */
        [RelayCommand]
        public void DismissOverlay() =>
            overlay_token_source?.Cancel();

        public VM_Scanner_QR() 
        {
            ;
        }

        /* PUBLIC METHODS (TO BE USED OUTSIDE OF VM) */
        public async Task InitializeAsync()
        {
            ;
        }

        public async Task BarcodeDetected(BarcodeDetectionEventArgs args)
        {
            // Check prerequisites
            BarcodeResult? result = args.Results.FirstOrDefault();
            if (result == null) return;

            string scanned_value = result.Value;

            // Disable detection
            IsDetecting = false;

            // Validate QR
            if (validate_scanned_value(scanned_value) == false)
            {
                await show_overlay(color: Colors.Red, 
                    text: $"QR Code: {scanned_value}\n Does not match valid pattern",
                    ms: 5000);

                IsDetecting = true;
                return;
            }

            // Perform work depens on scan type
            switch (scan_type)
            { 
                case QRScanMode.MemberInfo:
                    {
                        await member_profile(scanned_value);
                        break;
                    }
                case QRScanMode.UpdatePass:
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            await update_entry_pass(scanned_value);
                        });
                        break;
                    }
                case QRScanMode.CheckIn:
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            await check_in(scanned_value);
                        });
                        break;
                    }
                default:
                    break;
            }

            // If still on this page --> Restore detection
            IsDetecting = true;
        }

        /* PRIVATE METHODS */
        private async Task member_profile(string card_id)
        {
            // Transit to page with Member`s info to be added (After merging with Statistic branch).
            Resp_Members_Inst member = await fetch_member(card_id);
            if (member is null)
            {
                await show_overlay(color: Colors.Red, 
                    text: "Member with given ID was not found in DB",
                    ms: 5000);
            }
            else
            {
                await show_overlay(color: Colors.Green,
                    ms: 1000);
                
                // Go to the Member`s profile [TBD - wait for statistics branch merge]
                // ...
            }
        }

        private async Task update_entry_pass(string card_id)
        {
            Req_MemberPass_Add req = new Req_MemberPass_Add()
            {
                member_card_id = card_id,
                pass_type_id = pass.Id,
            };

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_MemberPass_Add, req);
                if (response.IsSuccessStatusCode)
                {
                    var pass_info = await response.Content.ReadFromJsonAsync<Resp_MemberPass_Inst>();
                    await show_overlay(color: Colors.Green);
                }
                else
                {
                    string negative_info = await response.Content.ReadAsStringAsync();
                    await show_overlay(color: Colors.Red,
                        text: negative_info,
                        ms: 5000);
                }
            }
            catch (Exception ex)
            {
                await show_overlay(color: Colors.Red,
                    text: ex.Message,
                    ms: 5000);
            }
            finally
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task check_in(string card_id)
        {
            Req_CheckIn_Add req = new Req_CheckIn_Add
            {
                validated_by_card_id = User.Account.CardId,
                external_provider_id = provider?.Id,
                member_card_id = card_id,
            };

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_CheckIn_Add, req);
                if (response.IsSuccessStatusCode)
                {
                    Resp_ChecIn_Inst inst = await response.Content.ReadFromJsonAsync<Resp_ChecIn_Inst>();
                    Model_Checkin checkin = Model_Checkin.From_resp_Inst(inst);

                    if (checkin.IsSuccessful)
                    {
                        await show_overlay(color: Colors.Green);
                    }
                    else
                    {
                        await show_overlay(color: Colors.Red,
                            text: $"Checkin failed:\n{checkin.RejectedReason}",
                            ms: 5000);
                    }
                }
                else
                {
                    string message = await Network.ParseResponse_AsString_FullInfo(response);
                    await show_overlay(color: Colors.Red,
                        text: message,
                        ms: 5000);
                }
            }
            catch (Exception ex)
            {
                await show_overlay(color: Colors.Red,
                    text: ex.Message,
                    ms: 5000);
            }
        
            finally
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task show_overlay(Color color, string text = "", int ms = 2200)
        {
            overlay_token_source?.Cancel();
            overlay_token_source = new CancellationTokenSource();
            CancellationToken cancel_token = overlay_token_source.Token;

            try
            {
                OverlayColor = color;
                OverlayOpacity = 1;
                OverlayText = text;
                IsOverlayVisible = true;

                await Task.Delay(ms, cancel_token);
            }
            catch (TaskCanceledException) { /* user tapped */ }
            finally
            {
                IsOverlayVisible = false;
                OverlayOpacity = 0;
                OverlayText = string.Empty;
                OverlayColor = Colors.Transparent;
            }
        }

        private async Task<Resp_Members_Inst> fetch_member(string card_id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Network.Get_Member_Inst(card_id));
                if (response.IsSuccessStatusCode)
                {
                    Resp_Members_Inst member = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();
                    return member;
                }
                else
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                ;
            }
            return null;
        }
        
        private bool validate_scanned_value(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.Length != 12)
                return false;

            return value.All(char.IsLetterOrDigit);
        }

        /* INTERFACE IMPLEMENTATION */
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            /* There are couple different arguments that can be passed
             * Required: "ScanType": QRScanMode
             * Optional: "ExternalProvider": ExternalProvider; "EntryPass": EntryPass */

            // Required argument
            if (query.TryGetValue("ScanType", out var ScanTypeObj) && ScanTypeObj is QRScanMode ScanType)
                scan_type = ScanType;

            // Get optional arguments if needed - depends on scan type
            switch (scan_type)
            {
                case QRScanMode.UpdatePass:
                    {
                        if (query.TryGetValue("EntryPass", out var EntryPassObj) && EntryPassObj is EntryPass EntryPass)
                            pass = EntryPass;
                        break;
                    }
                case QRScanMode.CheckIn:
                    {
                        if (query.TryGetValue("ExternalProvider", out var ExternalProviderObj) && ExternalProviderObj is ExternalProvider ExternalProvider)
                            provider = ExternalProvider;
                        break;
                        
                    }
                default:
                    break;
            }
        }
    }
}
