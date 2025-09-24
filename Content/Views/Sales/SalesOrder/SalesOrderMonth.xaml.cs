using wwrc_maui.Content.Viewmodels.Sales.SalesOrder;
using wwrc_maui.Content.Views.Dashboard;
using static wwrc_maui.Content.Model.SOModel;

namespace wwrc_maui.Content.Views.Sales.SalesOrder;

public partial class SalesOrderMonth : ContentPage
{
    SalesOrderMonthVm viewmodel = new();
    FilterSalesPersonView salesView = new();

    public SalesOrderMonth(string selectedDate)
    {
        InitializeComponent();
        viewmodel.selectedDate = selectedDate;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.SearchSalesOrder();
        };
        viewmodel.OnFinishLoad += (data) => { salesView.Itemsource = viewmodel.SalesList; };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchSalesOrder(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.PageTitle = viewmodel.selectedDate;
        viewmodel.GetDashboardData();
        viewmodel.GetSalesOrderByMonth();
    }

    private async void OnSales_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not VerticalStackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        void closeAction(bool okay)
        {
            if (okay)
            {
                if (salesView.Selected != null)
                {
                    viewmodel.SalesPerson = salesView.Selected.Id;
                    viewmodel.isFilterSales = true;
                    viewmodel.GetSalesOrderByMonth();
                }
            }
            else
            {
                // for reset all checked item
                viewmodel.isFilterSales = false;
                foreach (var item in salesView.Itemsource)
                { item.Checked = false; }
            }
        }
        await App.DisplayAlert("Sales Person", null, salesView, "Okay", "Cancel", closeAction);
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (Db_SOList)e.Item;
        await Navigation.PushAsync(new SalesOrderDetails(data));
    }
}