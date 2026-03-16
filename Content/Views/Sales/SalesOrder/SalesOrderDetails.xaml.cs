using wwrc_maui.Content.Viewmodels.Sales.SalesOrder;
using static wwrc_maui.Content.Model.SOModel;

namespace wwrc_maui.Content.Views.Sales.SalesOrder;

public partial class SalesOrderDetails : ContentPage
{
    SalesOrderDetailsVm viewmodel = new();

    public SalesOrderDetails(Db_SOList? model = null)
    {
        InitializeComponent();
        viewmodel.soData = model;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        viewmodel.SetSOData();
        await viewmodel.GetSalesOrderById();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private void SOlist_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }

    private async void DOlist_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var model = (Db_DOList)e.Item;
        await Navigation.PushAsync(new SODeliveryDetails(model));
    }
}