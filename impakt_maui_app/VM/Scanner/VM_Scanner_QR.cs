using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Models;
using impakt_maui_app.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

        /* PROPERTIES */
        // ...

        /* COMMANDS */
        [RelayCommand]
        private async Task ExecuteOperationAfterScan()
        {
            switch (scan_type)
            {
                case QRScanMode.MemberInfo:
                    {
                        await open_member_profile();
                        break;
                    }
                case QRScanMode.UpdatePass:
                    {
                        await update_entry_pass();
                        break;
                    }
                case QRScanMode.CheckIn:
                    {
                        await check_in();
                        break;
                    }
                default:
                    break;
            }
        }

        public VM_Scanner_QR() 
        {
            ;
        }

        /* PUBLIC METHODS (TO BE USED OUTSIDE OF VM) */
        public async Task InitializeAsync()
        {
            ;
        }

        /* PRIVATE METHODS */
        private async Task open_member_profile()
        {
            ;
            // Transit to page with Member`s info to be added (After merging with Statistic branch).
        }

        private async Task update_entry_pass()
        {
            // Place code from 
        }

        private async Task check_in()
        {

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
                        if (query.TryGetValue("ExternalProvider", out var ExternalProviderObj) && ExternalProviderObj is ExternalProvider ExternalProvider)
                            provider = ExternalProvider;
                        break;
                    }
                case QRScanMode.CheckIn:
                    {
                        if (query.TryGetValue("EntryPass", out var EntryPassObj) && EntryPassObj is EntryPass EntryPass)
                            pass = EntryPass;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
