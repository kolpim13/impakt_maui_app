using CommunityToolkit.Maui.Views;
using impakt_maui_app.Models;
using impakt_maui_app.Popups;
using impakt_maui_app.Schemas;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using ZXing.QrCode;

namespace impakt_maui_app.Pages;

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
    private ObservableCollection<Model_ExternalProvider> externalProviders;
    public ObservableCollection<Model_ExternalProvider> ExternalProviders
    {
        get { return externalProviders; }
        set 
        {
            if (externalProviders != value)
            {
                externalProviders = value;
                OnPropertyChanged(nameof(externalProviders));
            }
        }
    }

    private Model_ExternalProvider selectedExternalProvider;
    public Model_ExternalProvider SelectedExternalProvider
    {
        get => selectedExternalProvider;
        set
        {
            if (selectedExternalProvider != value)
            {
                selectedExternalProvider = value;
                OnPropertyChanged(nameof(selectedExternalProvider));
            }
        }
    }

    /* Properties: Update Pass */
    private readonly Model_PassType dummy_pass = new Model_PassType
    {
        Id = -1,
        Name = "No Pass",
        Price = 0,
        RequiresExternalAuth = false,
        IsExtEventPass = false,
        IsDeleted = true,
    };

    private ObservableCollection<Model_PassType> passTypes;
    public ObservableCollection<Model_PassType> PassTypes
    {
        get => passTypes;
        set
        {
            if (passTypes != value)
            {
                passTypes = value;
                OnPropertyChanged(nameof(PassTypes));
            }
        }
    }

    private Model_PassType selectedPassType;
    public Model_PassType SelectedPassType
    {
        get => selectedPassType;
        set
        {
            if (selectedPassType != value)
            {
                selectedPassType = value;
                OnPropertyChanged(nameof(SelectedPassType));
            }
        }
    }

    /* Properties: MemberInfo */
    // ....

    private async Task HandleScannedValue(string value)
    {
        // Assume scanned value is user ID (could be a GUID, int, etc.)
        switch (_scanMode)
        {
            case QRScanMode.CheckIn:
                await ProceedQRScann_CheckIn(value);
                break;
            case QRScanMode.UpdatePass:
                await ProceedQRScann_PassAdd(value);
                break;
            case QRScanMode.MemberInfo:
                await ProceedQRScann_MemberInfo(value);
                break;
            default:
                await DisplayAlert("Error", "Unsupported scan mode.", "OK");
                break;
        }
    }
    private async Task ProceedQRScann_CheckIn(string card_id)
    {
        // Assemble request
        Req_CheckIn_Add req = new Req_CheckIn_Add
        {
            validated_by_card_id = User.Account.CardId,
            external_provider_id = SelectedExternalProvider.Id,
            member_card_id = card_id,
        };

        // Process http request
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
                    await DisplayAlert("Success", string.Format("Checkin was successful"), "OK");
                }
                else
                {
                    await DisplayAlert("Fail", string.Format("Checkin failed due to:\n{0}", checkin.RejectedReason), "OK");
                }
            }
            else
            {
                string message = await Network.ParseResponse_AsString_FullInfo(response);
                await DisplayAlert("Request failed", message, "OK");
            }
        }
        catch (Exception ex)
        {
            ;
        }
        return;
    }
    private async Task ProceedQRScann_PassAdd(string card_id)
    {
        Req_MemberPass_Add req = new Req_MemberPass_Add()
        {
            member_card_id = card_id,
            pass_type_id = selectedPassType.Id,
        };

        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(Network.Post_MemberPass_Add, req);
            if (response.IsSuccessStatusCode)
            {
                // For probable future usage
                var pass_info = await response.Content.ReadFromJsonAsync<Resp_MemberPass_Inst>();

                await DisplayAlert("Success", "Members pass updated", "OK");
            }
            else
            {
                string negative_info = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", negative_info, "OK");
            }
        }
        catch(Exception ex)
        {
            ;
        }
    }
    private async Task ProceedQRScann_MemberInfo(string card_id)
    {
        /* All request will be done inside Popup itaelf */
        Popup_MemberInfo popup = await Popup_MemberInfo.CreateAsync(card_id);
        await this.ShowPopupAsync(popup);
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

        /* Option: Add Pass */
        PassTypes = GeneralResources.Get_PassTypes_AsCollection();
        PassTypes.Add(dummy_pass);
        SelectedPassType = PassTypes.First(pass => pass.Id == dummy_pass.Id);
        OnPropertyChanged(nameof(PassTypes));

        /* Option: CheckIn */
        ExternalProviders = GeneralResources.Get_ExternalProviders_AsCollection();
        ExternalProviders.Add(GeneralResources.dummy_provider);
        SelectedExternalProvider = ExternalProviders.First(provider => provider.Id == GeneralResources.dummy_provider.Id);
        OnPropertyChanged(nameof(ExternalProviders));
    }

    string checkin_scan_previous_result = "";
    DateTime checkin_scan_timestamp = DateTime.Now;
    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
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

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await HandleScannedValue(scanned_value);
        });

        // After job is done --> restore scanning
        QrReader.IsDetecting = true;
    }

    private void OnTestClicked(object sender, EventArgs e)
    {
        string scanned_value = "RQosADrP8Rzq";
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