using wwrc_maui.Content.Viewmodels.Sales.SalesOrder;
using static wwrc_maui.Content.Model.SOModel;

namespace wwrc_maui.Content.Views.Sales.SalesOrder;

public partial class SODeliveryDetails : ContentPage
{
    SalesOrderDetailsVm viewmodel = new();

    public SODeliveryDetails(Db_DOList? data = null)
    {
        InitializeComponent();
        viewmodel.doData = data;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.SetDOData();
    }

    private void DoItemlist_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}