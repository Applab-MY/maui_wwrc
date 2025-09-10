using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Auth;
using wwrc_maui.Content.Views.Dashboard;
using static wwrc_maui.Content.Model.Auth.LoginModel;

namespace wwrc_maui.Content.Views.Auth;

public partial class Walkthrough : ContentPage
{
    WalkthroughVm viewmodel = new();

    public Walkthrough()
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }

    protected override async void OnAppearing()
    {
        IsBusy = true;
        base.OnAppearing();
        var user = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
            ("select * from LoginMainModel").FirstOrDefault();
        if (user != null)
        {
            await Task.Delay(500);
            Application.Current?.Dispatcher.Dispatch(() =>
            { Application.Current.Windows[0].Page = new MainPage(); });
        }
        IsBusy = false;
    }

    private async void OnSkip_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label lbl) return;
        await lbl.ScaleTo(0.8, 100, Easing.Linear);
        lbl.Scale = 1;

        await Navigation.PushAsync(new Login());
    }

    private async void OnNext_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label lbl) return;
        await lbl.ScaleTo(0.8, 100, Easing.Linear);
        lbl.Scale = 1;

        if (carouselView.Position == 2)
        { await Navigation.PushAsync(new Login()); }
        else if (carouselView.Position != 2)
        { carouselView.Position += 1; }
    }
}