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

    private void HandleScannedValue(string value)
    {
        // Assume scanned value is user ID (could be a GUID, int, etc.)
        switch (_scanMode)
        {
            case QRScanMode.CheckIn:
                ProceedQRScann_CheckIn(value);
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