using wwrc_maui.Content.Viewmodels.Sales.CustomerAging;
using static wwrc_maui.Content.Model.CustomerAgingModel;

namespace wwrc_maui.Content.Views.Sales.CustomerAging;

public partial class CustomerAgingDetails : ContentPage
{
    CustomerAgingDetailVm viewmodel = new();

    public CustomerAgingDetails(string id)
    {
        InitializeComponent();
        viewmodel.agingId = id;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        viewmodel.GetPastMonth();
        await viewmodel.GetAgingDetails();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var model = (DB_MonthsModel)e.Item;
        await Navigation.PushAsync(new CustomerAgingDetailsMore(model));
    }
}