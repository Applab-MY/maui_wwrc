using wwrc_maui.Content.Viewmodels.Sales.CustomerAging;

namespace wwrc_maui.Content.Views.Sales.CustomerAging;

public partial class CustomerAgingMainPage : ContentPage
{
    CustomerAgingVm viewmodel = new();

    public CustomerAgingMainPage()
	{
		InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}