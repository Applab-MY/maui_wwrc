using wwrc_maui.Content.Viewmodels.Sales.CustomerAging;
using static wwrc_maui.Content.Model.CustomerAgingModel;

namespace wwrc_maui.Content.Views.Sales.CustomerAging;

public partial class CustomerAgingDetailsMore : ContentPage
{
    CustomerAgingDetailVm viewmodel = new();

    public CustomerAgingDetailsMore(DB_MonthsModel? data = null)
    {
        InitializeComponent();
        viewmodel.selectedAging = data;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        if (viewmodel.selectedAging != null)
        {
            viewmodel.CardName = viewmodel.selectedAging.CardName;
            viewmodel.SelectedMonth = viewmodel.selectedAging.Month;
        }
        viewmodel.GetAgingMonthDetail();
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}