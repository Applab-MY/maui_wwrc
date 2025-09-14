using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Dashboard;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Helper.ReferenceMessenger;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class MainPage : ContentPage
{
    MainPageVm viewmodel = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = viewmodel;
        navbar.OnLeftIconTapped += OnFilter_Tapped;
        navbar.OnRightIconTapped += OnLogout_Tapped;
        viewmodel.SetupData();

        WeakReferenceMessenger.Default.Register<StringNotify>(this, (receiver, message) =>
        {
            if (message.Value != null && message.Value.Equals("RefreshDashboardFilterModel"))
            { viewmodel.SetupData(); }
        });
    }

    private async void OnFilter_Tapped()
    { await Navigation.PushAsync(new DashboardFilter(viewmodel.DashboardData, viewmodel.FilterData)); }

    private async void OnLogout_Tapped()
    {
        viewmodel.IsBusy = true;
        try
        {
            await Task.Delay(300);
            AppDatabase.Instance.DeleteAllData();
            Preferences.Default.Clear();
            Application.Current?.Dispatcher.Dispatch(() =>
            { Application.Current.Windows[0].Page = new Login(); });
        }
        catch (Exception ex)
        { await App.DisplayAlert("Error", ex.Message, null, "Okay"); }
        viewmodel.IsBusy = false;
    }

    private async void OnDateCurrency_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        await Navigation.PushAsync(new CurrencyExchangeRate());
    }
}