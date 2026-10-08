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
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetCommittedPw();
        //viewmodel.DemoCommittedList(); //for demo
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }
}