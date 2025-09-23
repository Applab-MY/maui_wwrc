using wwrc_maui.Content.Viewmodels.Sales.StockAlert;

namespace wwrc_maui.Content.Views.Sales.StockAlert;

public partial class StockAlertDetailsMore : ContentPage
{
    StockAlertDetailsVm viewmodel = new();

    public StockAlertDetailsMore(string itemCode, string whsName)
    {
        InitializeComponent();
        viewmodel.itemCode = itemCode;
        viewmodel.whsName = whsName;
        BindingContext = viewmodel;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetCommittedPw();
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}