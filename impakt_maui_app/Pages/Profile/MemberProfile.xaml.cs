
using impakt_maui_app.Schemas;
using impakt_maui_app.VM.Profile;

namespace impakt_maui_app.Pages.Profile;

public partial class MemberProfile : ContentPage, IQueryAttributable
{
    private readonly VM_MemberProfile _viewModel;
    public MemberProfile(VM_MemberProfile vm)
	{
		InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    /* INTERFACES IMPLEMANTATION */
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Is error detection is needed?
        if (query.TryGetValue("Member", out var memberObj) && memberObj is Resp_Members_Inst m)
            _viewModel.Member = m;
    }
}