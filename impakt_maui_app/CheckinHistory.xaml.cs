using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app
{
    public partial class CheckinHistory : ContentPage
    {
        public ObservableCollection<BackendResp_CheckIn> CheckIns { get; set; }
        public CheckinHistory()
        {
            InitializeComponent();

            /* Temporary to see how table will look */
            CheckIns = new ObservableCollection<BackendResp_CheckIn>
            {
                new BackendResp_CheckIn{ control_name = "Name", control_surname = "Surname", hall = "impakt", name = "Member", surname = "Important", card_id = "CARD ID", date_time = DateTime.MinValue },
            };

            BindingContext = this;
            CheckInDataGrid.ItemsSource = CheckIns;
        }

        private async void OnRequestDataClicked(object? sender, EventArgs e)
        {
            try
            {
                // Assemble request [TBD add more filters]
                BackendReq_CheckInFilters req = new BackendReq_CheckInFilters();
                req.limit = 50;

                // Send request --> process it
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.CheckInHistoryUrl, req);
                if (response.IsSuccessStatusCode)
                {
                    BindingContext = null;
                    CheckInDataGrid.ItemsSource = null;

                    // Get new list of the filtered entries
                    List<BackendResp_CheckIn> check_ins = await response.Content.ReadFromJsonAsync<List<BackendResp_CheckIn>>();
                    //CheckIns = new ObservableCollection<BackendResp_CheckIn>(check_ins);

                    // Update Table values
                    CheckIns.Clear();
                    foreach (BackendResp_CheckIn row in check_ins)
                    {
                        CheckIns.Add(row);
                    }

                    // Update Item Source for grid                    
                    BindingContext = this;
                    CheckInDataGrid.ItemsSource = CheckIns;

                    
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
            
            return;
        }
    }
}
