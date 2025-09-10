using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Dashboard;
using wwrc_maui.Content.Views.Auth;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class MainPage : ContentPage
{
    MainPageVm viewmodel = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = viewmodel;
        navbar.OnRightIconTapped += OnLogout_Tapped;
        viewmodel.SetupData();
    }

    private async void OnLogout_Tapped()
    {
        IsBusy = true;
        await Task.Delay(500);
        try
        {
            AppDatabase.Instance.DeleteAllData();
            Preferences.Default.Clear();
            Application.Current?.Dispatcher.Dispatch(() =>
            { Application.Current.Windows[0].Page = new Login(); });
        }
        catch (Exception ex)
        { await App.DisplayAlert("Error", ex.Message, null, "Okay"); }
    }

    private async void OnDateCurrency_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
    }
}