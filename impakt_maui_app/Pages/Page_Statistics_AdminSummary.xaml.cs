using impakt_maui_app.Schemas;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;

namespace impakt_maui_app.Pages;

public partial class Page_Statistics_AdminSummary : ContentPage, INotifyPropertyChanged
{
    //public ObservableCollection<Resp_CheckIns_AmountInstructor>  { get; set; }

    public ObservableCollection<Resp_Statistics_Admin_CheckInsByType> TableContent { get; set; } =
        new ObservableCollection<Resp_Statistics_Admin_CheckInsByType>();

    DateTime _date_start = DateTime.Now;
    public DateTime SelectedDateStart
    {
        get => _date_start;
        set
        {
            if (_date_start != value)
            {
                _date_start = value;
                OnPropertyChanged(nameof(SelectedDateStart));
            }
        }
    }

    DateTime _date_end = DateTime.Now;
    public DateTime SelectedDateEnd
    {
        get => _date_end;
        set
        {
            if (_date_end != value)
            {
                _date_end = value;
                OnPropertyChanged(nameof(SelectedDateEnd));
            }
        }
    }

    public Page_Statistics_AdminSummary()
	{
		InitializeComponent();
        BindingContext = this;
	}
    private async void OnClicked_GetData(object? sender, EventArgs e)
    {
        try
        {
            Req_Statistics_Admin_CheckInsByType_All req = new Req_Statistics_Admin_CheckInsByType_All
            {
                date_time_min = _date_start,
                date_time_max = _date_end,
            };

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(Network.StatisticAllInstructorsUrl, req);
            if (response.IsSuccessStatusCode)
            {
                List<Resp_Statistics_Admin_CheckInsByType> data = await response.Content.ReadFromJsonAsync<List<Resp_Statistics_Admin_CheckInsByType>>();
                if (data != null)
                {
                    TableContent.Clear();
                    foreach (Resp_Statistics_Admin_CheckInsByType row in data)
                    {
                        TableContent.Add(row);
                    }
                    OnPropertyChanged(nameof(TableContent));
                }
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
    }

    /* INotifyPropertyChanged Implemantation */
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}