using wwrc_maui.Content.Model;
using wwrc_maui.Content.Views.Sales.CustomerAging;
using wwrc_maui.Content.Views.Sales.PurchaseOrder;
using wwrc_maui.Content.Views.Sales.SalesOrder;
using wwrc_maui.Content.Views.Sales.StockAlert;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.SalesModel;

namespace wwrc_maui.Content.Views.Sales;

public partial class SalesOptionPage : ContentView
{
    public int StockAlertCount { get; set; } = 0;
    List<OptionPageModel> Items = [];

    public SalesOptionPage()
    {
        InitializeComponent();
        BindingContext = this;
        var _login = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                ("Select * from LoginMainModel").FirstOrDefault();
        if (_login != null && _login.UserData != null)
        {
            var index = 0;
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.StockAlert)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Stock Alert",
                    Desc = "Real-time stock level alert",
                    Image = "menu_stock_alert",
                    AlertCount = StockAlertCount
                });
                index++;
            }
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.CustomerAging)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Customer Aging",
                    Desc = "List of account receivable aging",
                    Image = "menu_customer_aging",
                });
                index++;
            }
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.SalesOrder)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Sales Order",
                    Desc = "List of sales order",
                    Image = "menu_so",
                });
                index++;
            }
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.PurchaseOrder)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Purchase Order",
                    Desc = "List of purchase order",
                    Image = "menu_po",
                });
            }
        }
        listview.ItemsSource = Items;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView list) return;
        list.SelectedItem = null;
        var item = e.Item as OptionPageModel;
        if (item != null)
        {
            if (item.Name.Equals("Stock Alert")) await Navigation.PushAsync(new StockAlertMainPage());
            if (item.Name.Equals("Customer Aging")) await Navigation.PushAsync(new CustomerAgingMainPage());
            if (item.Name.Equals("Sales Order")) await Navigation.PushAsync(new SalesOrderMainPage());
            if (item.Name.Equals("Purchase Order")) await Navigation.PushAsync(new PurchaseOrderMainPage());
        }
    }
}