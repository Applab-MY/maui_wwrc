using wwrc_maui.Content.Viewmodels.Staff;

namespace wwrc_maui.Content.Views.Staff.CustomerVisit;

public partial class CustomerVisitDetailsPage : ContentPage
{
	CustomerVisitDetailsVm viewmodel = new();

    public CustomerVisitDetailsPage(string visitId)
	{
		InitializeComponent();
		viewmodel.visitId = visitId;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
		Initialize();
    }

	public async void Initialize()
	{
		await Task.Delay(300);
		viewmodel.SetupData();
	}
}