using wwrc_maui.Content.Viewmodels.Sales.PurchaseOrder;

namespace wwrc_maui.Content.Views.Sales.PurchaseOrder;

public partial class PurchaseOrderDetails : ContentPage
{
    PurchaseOrderDetailVm viewmodel = new();

    public PurchaseOrderDetails(string poNo)
    {
        InitializeComponent();
        viewmodel.poNo = poNo;
        BindingContext = viewmodel;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetPurchaseOrderDetails();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}