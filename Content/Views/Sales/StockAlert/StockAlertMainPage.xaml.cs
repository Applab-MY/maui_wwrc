using wwrc_maui.Content.Viewmodels.Sales.StockAlert;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.Views.Sales.StockAlert;

public partial class StockAlertMainPage : ContentPage
{
    StockAlertVm viewmodel = new();

    public StockAlertMainPage()
    {
        InitializeComponent();
        BindingContext = viewmodel;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () => { viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible; };
        entry_search.OnTextChanged += viewmodel.SearchStockList;
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetStockAlertList();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (DB_StockAlert)e.Item;
        await Navigation.PushAsync(new StockAlertDetails(data.ItemCode));
    }
}