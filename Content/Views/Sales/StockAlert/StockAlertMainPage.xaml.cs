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
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.SearchStockList();
        };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchStockList(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetStockAlertList();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (DB_StockAlert)e.Item;
        await Navigation.PushAsync(new StockAlertDetails(data.ItemCode));
    }
}