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
        viewmodel.WarehouseTappedCommand = new Command<DB_WarehouseItem>(OnWarehouseTapped);
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

    private async void OnWarehouseTapped(DB_WarehouseItem? data)
    {
        if (data == null) return;
        await Navigation.PushAsync(new StockAlertDetailsMore(data.ItemCode, data.Warehouse));
    }
}