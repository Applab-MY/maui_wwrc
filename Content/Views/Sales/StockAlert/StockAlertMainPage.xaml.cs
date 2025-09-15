using wwrc_maui.Content.Viewmodels.Sales.StockAlert;

namespace wwrc_maui.Content.Views.Sales.StockAlert;

public partial class StockAlertMainPage : ContentPage
{
    StockAlertVm viewmodel = new();

    public StockAlertMainPage()
	{
		InitializeComponent();
		BindingContext = viewmodel;
		navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
    }
}