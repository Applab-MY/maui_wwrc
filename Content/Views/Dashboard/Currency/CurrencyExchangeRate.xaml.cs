using wwrc_maui.Content.Viewmodels.Dashboard;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class CurrencyExchangeRate : ContentPage
{
    CurrencyExchangeVm viewmodel = new();

    public CurrencyExchangeRate()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }

    protected override async void OnAppearing()
    {
        viewmodel.IsBusy = true;
        base.OnAppearing();
        await Task.Delay(300);
        viewmodel.SetDateList();
        viewmodel.CurrencyList();
        viewmodel.IsBusy = false;
    }

    private async void OnDaysRate_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        var cell = new CurrencyExchangeDateList { Itemsource = viewmodel.DateList };
        cell.Initialize();
        cell.OnItemSelected = (data) =>
        {
            viewmodel.SelectedDate = data.ToString("yyyy-MM-dd");
            viewmodel.CurrencyList();
        };
        await App.DisplayAlert("Select Data", "", cell, "", "Cancel");
    }

    private void listview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}