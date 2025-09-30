using wwrc_maui.Content.Viewmodels.Sales.CustomerAging;
using static wwrc_maui.Content.Model.CustomerAgingModel;

namespace wwrc_maui.Content.Views.Sales.CustomerAging;

public partial class CustomerAgingMainPage : ContentPage
{
    CustomerAgingVm viewmodel = new();
    FilterSalesPersonView salesView = new();

    public CustomerAgingMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.SearchCustomerAging();
        };
        viewmodel.OnFinishLoad += (data) => { salesView.Itemsource = viewmodel.SalesList; };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchCustomerAging(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetDashboardData();
        viewmodel.GetPastMonth();
        viewmodel.GetCustomerAgingData();
    }

    private async void OnSales_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not VerticalStackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        void closeAction(bool okay)
        {
            salesView.Reset();
            if (okay)
            {
                if (salesView.Selected != null)
                {
                    viewmodel.SalesPerson = salesView.Selected.Id;
                    viewmodel.isFilterSales = true;
                    viewmodel.GetCustomerAgingData();
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
        var data = (DB_CustAging)e.Item;
        await Navigation.PushAsync(new CustomerAgingDetails(data.Id));
    }
}