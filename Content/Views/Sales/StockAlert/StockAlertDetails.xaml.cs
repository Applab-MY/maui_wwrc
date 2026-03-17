using wwrc_maui.Content.Viewmodels.Sales.StockAlert;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.Views.Sales.StockAlert;

public partial class StockAlertDetails : ContentPage
{
    StockAlertDetailsVm viewmodel = new();

    public StockAlertDetails(string itemCode)
    {
        InitializeComponent();
        viewmodel.itemCode = itemCode;
        BindingContext = viewmodel;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetStockDetails();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (DB_WarehouseItem)e.Item;
        await Navigation.PushAsync(new StockAlertDetailsMore(data.ItemCode, data.Warehouse));
    }
}