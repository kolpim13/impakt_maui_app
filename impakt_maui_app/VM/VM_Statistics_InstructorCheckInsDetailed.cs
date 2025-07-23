using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Schemas;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace impakt_maui_app.VM
{
    public class Group_InstructorCheckInsDetailed : ObservableCollection<Resp_Statistics_InstructorCheckInsDetailed>
    {
        public string Header { get; private set; }
        public Group_InstructorCheckInsDetailed(string header, ObservableCollection<Resp_Statistics_InstructorCheckInsDetailed> items) : base(items)
        {
            Header = header;
        }
    }
    public partial class VM_Statistics_InstructorCheckInsDetailed : ObservableObject
    {
        /* PRIVATE */
        private int page = 0;
        private readonly int page_size = 50;
        private int total = 0;
        private int remaining = 0;
        private bool is_loading = false;

        /* SHELL ARGUMENTS */
        [ObservableProperty] private string? cardId = null;
        [ObservableProperty] private DateOnly? dateFrom = null;
        [ObservableProperty] private DateOnly? dateTo = null;

        public ObservableCollection<Group_InstructorCheckInsDetailed> Table { get; private set; } = new ObservableCollection<Group_InstructorCheckInsDetailed>();
        private ObservableCollection<Group_InstructorCheckInsDetailed> last_data { get; set; } = new();

        [RelayCommand]
        private async Task LoadMoreDataInTable()
        {
            // Data is loading right now || no more data in DB --> do nothing.
            if (total == 0)
                return;

            if (is_loading || remaining <= 0)
                return;

            is_loading = true;

            // Fetch data for DB
            Resp_Paginated_Statistics_InstructorCheckInsDetailed db_data = await fetch_data_from_DB();

            // Sort fetched data
            var grouped = db_data.items
            .GroupBy(entry => new DateTime(
                entry.date_time.Year,
                entry.date_time.Month,
                entry.date_time.Day,
                entry.date_time.Hour,   // <--- groups by the hour
                0, 0))                  // minutes/seconds set to 0
            .OrderBy(g => g.Key)
            .ToList();

            // protect main table --> create reference for the all data storage
            ObservableCollection<Group_InstructorCheckInsDetailed> dummy = new();
            ObservableCollection<Group_InstructorCheckInsDetailed> all_data = new();
            all_data = Table;
            Table = dummy;
            OnPropertyChanged(nameof(Table));

            // Collect new data
            ObservableCollection<Group_InstructorCheckInsDetailed> new_data = new();
            foreach (var group in grouped)
            {
                new_data.Add(new Group_InstructorCheckInsDetailed(group.Key.ToString(), group.ToObservableCollection()));
            }

            // If last data and new have same Key --> merge its data into one
            if (new_data.Any(item => item.Header == all_data.Last().Header))
            {
                // Since the List<> are ordered by data --> this should be fine.
                foreach (var item in new_data.First())
                    all_data.Last().Add(item);
                new_data.RemoveAt(0);
            }

            // Add new data to all data
            foreach (var item in new_data)
                all_data.Add(item);

            // Store last data reference
            last_data = new_data;

            // Update main table --> scroll it to previous position
            Table = all_data;
            OnPropertyChanged(nameof(Table));

            // Update variables
            total = db_data.total;
            remaining = db_data.remaining;
            page++;

            // Increment page to load
            is_loading = false;
        }

        public VM_Statistics_InstructorCheckInsDetailed()
        {
            ;
        }

        public async Task InitializeAsync()
        {
            Resp_Paginated_Statistics_InstructorCheckInsDetailed db_data = await fetch_data_from_DB();

            var grouped = db_data.items
                .GroupBy(entry => new DateTime(
                    entry.date_time.Year,
                    entry.date_time.Month,
                    entry.date_time.Day,
                    entry.date_time.Hour,   // <--- groups by the hour
                    0, 0))                  // minutes/seconds set to 0
                .OrderBy(g => g.Key)
                .ToList();

            // Create temporar table --> and write everything into it. {due to a bug}
            foreach (var group in grouped)
            {
                last_data.Add(new Group_InstructorCheckInsDetailed(group.Key.ToString(), group.ToObservableCollection()));
            }

            ObservableCollection<Group_InstructorCheckInsDetailed> temp = new();
            foreach (var item in last_data)
                temp.Add(item);

                // Update main table
            Table = temp;
            OnPropertyChanged(nameof(Table));

            // Update variables
            total = db_data.total;
            remaining = db_data.remaining;
            page++;

            // Increment page to load
            is_loading = false;
        }

        private async Task<Resp_Paginated_Statistics_InstructorCheckInsDetailed?> fetch_data_from_DB()
        {
            Req_Statistics_InstructorCheckInsDetailed req = new()
            {
                validated_by_card_id = CardId,
                date_from = (DateOnly)DateFrom,
                date_to = (DateOnly)DateTo,
                page = page,
                page_size = page_size,
            };

            Resp_Paginated_Statistics_InstructorCheckInsDetailed result = null;
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_Statistics_InstructorCheckInsDetailed, req);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadFromJsonAsync<Resp_Paginated_Statistics_InstructorCheckInsDetailed>();
                }
            }
            catch (Exception ex)
            {
                ;
            }
            return result;
        }

        /* PROPERTIES NOTIFICATIONS */
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            /* Update Command: Btn OK */
            if (e.PropertyName == nameof(CardId) ||
                e.PropertyName == nameof(DateFrom) ||
                e.PropertyName == nameof(DateTo))
            {
                if (CardId is null ||
                    DateFrom is null ||
                    DateTo is null)
                {
                    return;
                }
                else
                {
                    // LoadMoreDataInTableCommand.Execute(null);
                    ;
                }
            }
        }
    }

    public class RejectReasonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is string s ? s : "NO";
            
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b ? !b : value;
    }
}


//MainThread.BeginInvokeOnMainThread(() =>
//{

//});

//var header = "GOWNO!";
//var entries = new ObservableCollection<Resp_Statistics_InstructorCheckInsDetailed>()
//{
//    new Resp_Statistics_InstructorCheckInsDetailed() { name = "A", surname = "B", date_time = DateTime.Now, is_successful = false },
//    new Resp_Statistics_InstructorCheckInsDetailed() { name = "A", surname = "B", date_time = DateTime.Now, is_successful = false },
//    new Resp_Statistics_InstructorCheckInsDetailed() { name = "A", surname = "B", date_time = DateTime.Now, is_successful = false },
//    new Resp_Statistics_InstructorCheckInsDetailed() { name = "A", surname = "B", date_time = DateTime.Now, is_successful = false },
//};
//Table.Add(new Group_InstructorCheckInsDetailed(header, entries));