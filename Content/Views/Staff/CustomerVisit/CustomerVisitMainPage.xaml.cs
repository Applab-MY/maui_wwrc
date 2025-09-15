using wwrc_maui.Content.Viewmodels.Staff;

namespace wwrc_maui.Content.Views.Staff.CustomerVisit;

public partial class CustomerVisitMainPage : ContentPage
{
    CustomerVisitVm viewmodel = new();

    public CustomerVisitMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}