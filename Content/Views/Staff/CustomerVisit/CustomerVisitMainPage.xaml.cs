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
        navbar.OnRightIconTapped += async () => { await Navigation.PushAsync(new CreateCustomerVisit()); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetCustVisitList();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void OnSalesPerson_Tapped(object sender, EventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        var cell = new SalesPersonList { Itemsource = viewmodel.personListCache };
        cell.Initialize();
        cell.OnItemSelected = async (data) =>
        {
            viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
            viewmodel.SalesPerson = data;
            await viewmodel.GetSalesPersonVisits();
            viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
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

    private async void OnDateFrom_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (sender is not DatePicker view) return;
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        viewmodel.SelectedDate = view.Date;
        await viewmodel.GetCustVisitList();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (DB_CustomerVisit)e.Item;
        await Navigation.PushAsync(new CustomerVisitDetailsPage(data.Id));
    }
}