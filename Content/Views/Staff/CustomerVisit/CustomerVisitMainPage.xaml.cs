using wwrc_maui.Content.Viewmodels.Staff;
using static wwrc_maui.Content.Model.CustomerVisitModel;

namespace wwrc_maui.Content.Views.Staff.CustomerVisit;

public partial class CustomerVisitMainPage : ContentPage
{
    CustomerVisitVm viewmodel = new();

    public CustomerVisitMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetCustVisitList();
    }

    private async void OnSalesPerson_Tapped(object sender, EventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        var cell = new SalesPersonList { Itemsource = viewmodel.personListCache };
        cell.Initialize();
        cell.OnItemSelected = (data) =>
        {
            viewmodel.SalesPerson = data;
            viewmodel.GetSalesPersonVisits();
        };
        await App.DisplayAlert("Select Sales Person", "", cell, "", "Cancel");
    }

    private async void OnFromDate_Tapped(object sender, EventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        dt_from.Focus();
    }

    private void OnDateFrom_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (sender is not DatePicker view) return;
        viewmodel.SelectedDate = view.Date;
        viewmodel.GetCustVisitList();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (DB_CustomerVisit)e.Item;

        //if (!string.IsNullOrEmpty(data.Records))
        //{ await Navigation.PushAsync(new PurchaseOrderMonth(data.Date)); }
        //await App.DisplayAlert("Empty", "No record found for the selected month.", null, "Okay");
    }
}