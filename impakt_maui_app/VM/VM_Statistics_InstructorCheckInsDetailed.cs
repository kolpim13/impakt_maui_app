using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using impakt_maui_app.Schemas;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace impakt_maui_app.VM
{
    public class Group_InstructorCheckInsDetailed : ObservableCollection<Resp_Statistics_InstructorCheckInsDetailed>
    {
        public string Header { get; private set; }
        public string Footer { get; private set; }

        public Group_InstructorCheckInsDetailed(string header, string footer,
            ObservableCollection<Resp_Statistics_InstructorCheckInsDetailed> items) : base(items)
        {
            Header = header;
            Footer = footer;
        }
    }
    public partial class VM_Statistics_InstructorCheckInsDetailed : ObservableObject
    {
        public enum GroupEntriesBy : ushort
        {
            Hours,
            Days,
            Month,
        } 

        /* PRIVATE */
        private int page = 0;
        private readonly int page_size = 200;
        private int total = 0;
        private int remaining = 0;
        private bool is_loading = false;

        /* SHELL ARGUMENTS */
        [ObservableProperty] private string? cardId = null;
        [ObservableProperty] private DateOnly? dateFrom = null;
        [ObservableProperty] private DateOnly? dateTo = null;

        [NotifyCanExecuteChangedFor(nameof(GouppingEntriesChangedCommand))]
        [ObservableProperty] private GroupEntriesBy groupBy = GroupEntriesBy.Days;

        public ObservableCollection<Group_InstructorCheckInsDetailed> Table { get; private set; } = new ObservableCollection<Group_InstructorCheckInsDetailed>();
        private ObservableCollection<Group_InstructorCheckInsDetailed> last_data { get; set; } = new();

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task LoadMoreDataInTable()
        {
            // Data is loading right now || no more data in DB --> do nothing.
            if (total == 0)
                return;

            if (is_loading || remaining <= 0)
                return;

            is_loading = true;

            // Fetch data for DB --> Sort it
            Resp_Paginated_Statistics_InstructorCheckInsDetailed db_data = await fetch_data_from_DB();
            var grouped = group_entries_by(db_data.items);

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
                new_data.Add(TableGroup_FromGrouppedData(group));
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

        [RelayCommand(CanExecute = nameof(can_execute_group_entries_changed))]
        void GouppingEntriesChanged()
        {
            if (is_loading)
                return;

            is_loading = true;

            // Ungroup data and froup it again
            var ungrouped_data = Table
                .SelectMany(g => g)
                .ToList();
            var grouped_data = group_entries_by(ungrouped_data);
            
            // Free resources
            Table.Clear();

            // Use temporary collection
            ObservableCollection<Group_InstructorCheckInsDetailed> temp = new();
            foreach (var group in grouped_data)
            {
                temp.Add(TableGroup_FromGrouppedData(group));
            }

            // Assign "Table" to a new collection
            Table = temp;
            OnPropertyChanged(nameof(Table));

            // Repeat same with "last_data" collection
            ungrouped_data = last_data
                .SelectMany(g => g)
                .ToList();
            grouped_data = group_entries_by(ungrouped_data);

            last_data.Clear();
            temp = new();

            foreach (var group in grouped_data)
            {
                temp.Add(TableGroup_FromGrouppedData(group));
            }
            last_data = temp;

            is_loading = false;
        }

        public VM_Statistics_InstructorCheckInsDetailed()
        {
            ;
        }

        public async Task InitializeAsync()
        {
            Resp_Paginated_Statistics_InstructorCheckInsDetailed db_data = await fetch_data_from_DB();
            var grouped = group_entries_by(db_data.items);

            // Create temporar table --> and write everything into it. {due to a bug}
            foreach (var group in grouped)
            {
                last_data.Add(TableGroup_FromGrouppedData(group));
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

        private List<IGrouping<DateTime, Resp_Statistics_InstructorCheckInsDetailed>> group_entries_by(List<Resp_Statistics_InstructorCheckInsDetailed> data)
        {
            switch (GroupBy)
            {
                case GroupEntriesBy.Hours:
                    return data
                        .GroupBy(entry => new DateTime(
                            entry.date_time.Year,
                            entry.date_time.Month,
                            entry.date_time.Day,
                            entry.date_time.Hour,
                            0, 0))                 
                        .OrderBy(g => g.Key)
                        .ToList();

                case GroupEntriesBy.Days:
                    return data
                        .GroupBy(entry => entry.date_time.Date)                
                        .OrderBy(g => g.Key)
                        .ToList();

                case GroupEntriesBy.Month:
                    return data
                        .GroupBy(entry => new DateTime(
                            entry.date_time.Year,
                            entry.date_time.Month,  
                            1))                 
                        .OrderBy(g => g.Key)
                        .ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(GroupBy), GroupBy, "It is allowed to group data by [Hours, Days, Weeks] only");
            }
        }

        private Group_InstructorCheckInsDetailed TableGroup_FromGrouppedData(IGrouping<DateTime, Resp_Statistics_InstructorCheckInsDetailed> group)
        {
            string header = GroupBy switch
                {
                    GroupEntriesBy.Hours => group.Key.ToString("dd.MM.yyyy HH:mm [dddd]"),
                    GroupEntriesBy.Days => group.Key.ToString("dd.MM.yyyy [dddd]"),
                    GroupEntriesBy.Month => group.Key.ToString("MMMM yyyy"),
                };
            string footer = $"Entries Positive / Total: [{group.Count(i => i.is_successful)}/{group.Count()}]";
            return new(header, footer, group.ToObservableCollection());
        }

        private bool can_execute_group_entries_changed() =>
            Table.Count > 0 && is_loading == false;
    }

    public class RejectReasonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is string s ? s : "NO";
            
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b ? !b : value;
    }

    public class GroupEntriesByConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            var enumValue = value.ToString().Trim();
            var targetValue = parameter.ToString().Trim();
            var test = enumValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
            return enumValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue) || !boolValue || parameter == null)
                return null;
            var temp = Enum.Parse(targetType, parameter.ToString());
            return Enum.Parse(targetType, parameter.ToString());
        }
    }
}
