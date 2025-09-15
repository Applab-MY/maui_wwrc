using wwrc_maui.Content.Viewmodels.Sales.SalesOrder;

namespace wwrc_maui.Content.Views.Sales.SalesOrder;

public partial class SalesOrderMainPage : ContentPage
{
    SalesOrderVm viewmodel = new();

    public SalesOrderMainPage()
	{
		InitializeComponent();
		navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
		BindingContext = viewmodel;
    }
}