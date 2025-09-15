using wwrc_maui.Content.Viewmodels.Sales.PurchaseOrder;

namespace wwrc_maui.Content.Views.Sales.PurchaseOrder;

public partial class PurchaseOrderMainPage : ContentPage
{
    PurchaseOrderVm viewmodel = new();

    public PurchaseOrderMainPage()
	{
		InitializeComponent();
		navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
		BindingContext = viewmodel;
    }
}