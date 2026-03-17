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
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetVisitDetails();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void OnUpdate_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button view) return;
        var _res = await viewmodel.UpdateVisitDetails();
        if (_res)
        {
            await App.DisplayAlert("Success", "Customer visit details updated.", null, "Okay");
            await Navigation.PopAsync();
        }
    }
}