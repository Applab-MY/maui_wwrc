using wwrc_maui.Content.Viewmodels.Sales.StockAlert;

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
        await Task.Delay(300);
        viewmodel.GetStockDetails();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        //var data = (SalesOrderMainModel)e.Item;
        //await Navigation.PushAsync(new SalesOrderMonth(data.Date));
    }
}