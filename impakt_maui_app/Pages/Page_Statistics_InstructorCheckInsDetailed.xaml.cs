using impakt_maui_app.VM;

namespace impakt_maui_app.Pages;

[QueryProperty(nameof(CardId), "CardId")]
[QueryProperty(nameof(DateFrom), "DateFrom")]
[QueryProperty(nameof(DateTo), "DateTo")]
public partial class Page_Statistics_InstructorCheckInsDetailed : ContentPage
{
    public Page_Statistics_InstructorCheckInsDetailed()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // All navigation QueryProperty values should be set at this point!
        // Safe to trigger ViewModel commands or update collections
        if (BindingContext is VM_Statistics_InstructorCheckInsDetailed vm)
        {
            await vm.InitializeAsync();
        }

    }

    public string CardId
    {
        get => BindingContext is VM_Statistics_InstructorCheckInsDetailed vm ? vm.CardId : null;
        set { if (BindingContext is VM_Statistics_InstructorCheckInsDetailed vm) vm.CardId = value; }
    }
    public string DateFrom
    {
        get => null;
        set { if (BindingContext is VM_Statistics_InstructorCheckInsDetailed vm) vm.DateFrom = DateOnly.Parse(value); }
    }
    public string DateTo
    {
        get => null;
        set { if (BindingContext is VM_Statistics_InstructorCheckInsDetailed vm) vm.DateTo = DateOnly.Parse(value); }
    }
}