using wwrc_maui.Content.Viewmodels.Auth;

namespace wwrc_maui.Content.Views.Auth;

public partial class Login : ContentPage
{
    LoginVm viewmodel = new();

    public Login()
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }

    private async void OnForgotPwd_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label lbl) return;
        await lbl.ScaleTo(0.9, 100);
        lbl.Scale = 1;
    }
}