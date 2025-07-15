using CommunityToolkit.Maui.Views;
using impakt_maui_app.Popups;
using impakt_maui_app.Schemas;
using System.ComponentModel;
using System.Net.Http.Json;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using ZXing.QrCode;

namespace impakt_maui_app.Pages;

public enum ExternalPaymentType
{
    OneTimePass = 1,
    Medicover = 21,
    PZU = 41,
    Multisport = 61,    /* For the future */
}

[QueryProperty(nameof(Mode), "mode")]
public partial class Page_Scanner_QRScanner : ContentPage, INotifyPropertyChanged
{
    /* Input parameter for Shell */
    public string Mode { get; set; }

    private QRScanMode _scanMode;
    public QRScanMode ScanMode
    {
        get { return _scanMode; }
        set
        {
            if (_scanMode != value)
            {
                _scanMode = value;
                OnPropertyChanged(nameof(ScanMode));
            }
        }
    }

    /* Properties for CheckIn */
    private ExternalPaymentType _externalPaymentType = ExternalPaymentType.Medicover;
    public ExternalPaymentType ExternalPaymentType
    {
        get { return _externalPaymentType; }
        set 
        {
            if (_externalPaymentType != value)
            {
                _externalPaymentType = value;
                OnPropertyChanged(nameof(ExternalPaymentType));
            }
        }
    }

    /* Properties for Update Pass */
    public IEnumerable<PassType> PassTypes { get; } = Enum.GetValues(typeof(PassType))
        .Cast<PassType>()
        .Where(f => f == PassType.LIMITED_4 || f == PassType.LIMITED_8 || f == PassType.LIMITED_12 || f == PassType.UNLIMITED)
        .ToList();

    private PassType _selectedPassType = PassType.LIMITED_4;
    public PassType SelectedPassType
    {
        get => _selectedPassType;
        set
        {
            if (_selectedPassType != value)
            {
                _selectedPassType = value;
                OnPropertyChanged(nameof(SelectedPassType));
            }
        }
    }

    private void HandleScannedValue(string value)
    {
        // Assume scanned value is user ID (could be a GUID, int, etc.)
        switch (_scanMode)
        {
            case QRScanMode.CheckIn:
                ProceedQRScann_CheckIn(value);
                break;
            case QRScanMode.UpdatePass:
                ProceedQRScann_UpdatePass(value);
                break;
            case QRScanMode.MemberInfo:
                ProceedQRScann_MemberInfo(value);
                break;
            default:
                DisplayAlert("Error", "Unsupported scan mode.", "OK");
                break;
        }
    }
    private async void ProceedQRScann_CheckIn(string card_id)
    {
        // Assemble request
        string? hall = null;    // [TBD]
        bool? external_payment = CheckIn_ChB_ExternalPayment.IsChecked ? true : null;
        int? pass_type = CheckIn_ChB_ExternalPayment.IsChecked ? (int)ExternalPaymentType : null;
        Req_CheckIn req = new Req_CheckIn
        {
            card_id = card_id,
            hall = hall,
            external_payment = external_payment,
            pass_type = pass_type,
        };

        // Process http request
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(Network.CheckInUrl, req);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Member was checked in", card_id, "OK");
                await Navigation.PopAsync();
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
    private async void ProceedQRScann_UpdatePass(string card_id)
    {
        Req_Members_UpdatePassData req = new Req_Members_UpdatePassData()
        {
            card_id = card_id,
            pass_type = (int)_selectedPassType,
        }

        ;
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_Member_UpdatePass, req);
            if (response.IsSuccessStatusCode)
            {
                // For probable future usage
                Resp_Member_UpdatePass pass_info = await response.Content.ReadFromJsonAsync<Resp_Member_UpdatePass>();

                await DisplayAlert("Success", "Members pass updated", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                string negative_info = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", negative_info, "OK");
                await Navigation.PopAsync();
            }
        }
        catch
        {
            ;
        }
    }
    private async void ProceedQRScann_MemberInfo(string card_id)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(Network.Get_Member_Inst(card_id));
        if (response.IsSuccessStatusCode)
        {
            var member_info = await response.Content.ReadFromJsonAsync<Resp_Members_Inst>();
            await Shell.Current.ShowPopupAsync(new MemberInfoPopup(member_info));
        }
        else
        {
            ;
        }
    }
    public Page_Scanner_QRScanner()
	{
		InitializeComponent();
        BindingContext = this;

        /* Set Barcode Reader Options */
        QrReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode, // Set to recognize EAN-13 barcodes
            AutoRotate = true,
            Multiple = false,
        };

        OnPropertyChanged(nameof(PassTypes));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Parse the passed-in mode
        if (!Enum.TryParse<QRScanMode>(Mode, out _scanMode))
        {
            _scanMode = QRScanMode.None; // Default mode
        }

        OnPropertyChanged(nameof(ScanMode));
    }

    string checkin_scan_previous_result = "";
    DateTime checkin_scan_timestamp = DateTime.Now;
    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        /* Check there is scanned value */
        BarcodeResult? result = e.Results.FirstOrDefault();
        if (result == null) return;

        /* Check if same qr was scanned for last 5 seconds */
        string scanned_value = result.Value;
        if ((scanned_value == checkin_scan_previous_result) &&
            ((DateTime.Now - checkin_scan_timestamp).Seconds) <= 5) return;

        // Stop detection after first result
        QrReader.IsDetecting = false;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            HandleScannedValue(scanned_value);
        });
    }

    private void OnTestClicked(object sender, EventArgs e)
    {
        string scanned_value = "PAJuVz4xIV1X";
        MainThread.BeginInvokeOnMainThread(() =>
        {
            HandleScannedValue(scanned_value);
        });
    }

    /* INotifyPropertyChanged Implemantation */
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}