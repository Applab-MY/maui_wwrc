using wwrc_maui.Content.Viewmodels.Auth;

namespace wwrc_maui.Content.Views.Auth;

public partial class Walkthrough : ContentPage
{
    WalkthroughVm viewmodel = new();

    public Walkthrough()
    {
        InitializeComponent();
        BindingContext = viewmodel;
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