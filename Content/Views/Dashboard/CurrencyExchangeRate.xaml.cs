using wwrc_maui.Content.Viewmodels.Dashboard;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class CurrencyExchangeRate : ContentPage
{
    CurrencyExchangeVm viewmodel = new();

    public CurrencyExchangeRate()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        picker_.OnItemSelected += (value) =>
        {
            if (string.IsNullOrEmpty(value)) return;
            viewmodel.SelectedDate = value;
            viewmodel.CurrencyList();
        };
        BindingContext = viewmodel;
    }

    protected override async void OnAppearing()
    {
        viewmodel.IsBusy = true;
        base.OnAppearing();
        await Task.Delay(300);
        viewmodel.SetDateList();
        viewmodel.CurrencyList();
        picker_.ItemsSource = viewmodel.DateList;
        viewmodel.IsBusy = false;
    }

    private async void OnDaysRate_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        picker_.OpenPicker?.Invoke();
    }

    private void listview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}