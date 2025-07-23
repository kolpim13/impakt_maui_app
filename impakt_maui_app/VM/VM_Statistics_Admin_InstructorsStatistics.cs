using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Schemas;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM
{
    public partial class VM_Statistics_Admin_InstructorsStatistics : ObservableObject
    {
        [ObservableProperty] private DateTime selectedDateFrom;
        [ObservableProperty] private DateTime selectedDateTo;
        public ObservableCollection<Resp_Statistics_InstructorsCheckIns> TableContent { get; set; } = new();
        [ObservableProperty] private Resp_Statistics_InstructorsCheckIns? selectedRow;

        [RelayCommand]
        private async void RequestData()
        {
            /* Assemble request */
            Req_Statistics_InstructorsCheckIns req = new Req_Statistics_InstructorsCheckIns
            {
                date_from = DateOnly.FromDateTime(SelectedDateFrom),
                date_to = DateOnly.FromDateTime(SelectedDateTo),
            };

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_Statistics_InstructorsCheckIns, req);
                if (response.IsSuccessStatusCode)
                {
                    TableContent.Clear();

                    var results = await response.Content.ReadFromJsonAsync<List<Resp_Statistics_InstructorsCheckIns>>();
                    foreach (Resp_Statistics_InstructorsCheckIns instructor in results)
                    {
                        TableContent.Add(instructor);
                    }
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        [RelayCommand(CanExecute = nameof(can_execute_request_detailed_data))]
        private async void RequestDetailedData()
        {
            await Shell.Current.GoToAsync($"Page_Statistics_InstructorCheckInsDetailed?CardId={SelectedRow.validated_by_card_id}&DateFrom={DateOnly.FromDateTime(SelectedDateFrom).ToString()}&DateTo={DateOnly.FromDateTime(SelectedDateTo).ToString()}");
            // await Shell.Current.GoToAsync("Page_Statistics_InstructorCheckInsDetailed");
        }

        public VM_Statistics_Admin_InstructorsStatistics() 
        {
            ; 
        }

        /* FUNCTIONALITY */

        /* CONDITIONS */
        private bool can_execute_request_detailed_data() =>
            selectedRow is not null;

        /* PROPERTIES NOTIFICATIONS */
        partial void OnSelectedRowChanged(Resp_Statistics_InstructorsCheckIns? value) =>
            RequestDetailedDataCommand.NotifyCanExecuteChanged();
    }
}
