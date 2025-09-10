using wwrc_maui.Content.Viewmodels.Auth;

namespace wwrc_maui.Content.Views.Auth;

public partial class ForgotPassword : ContentPage
{
    ForgotPwdVm viewmodel = new();

    public ForgotPassword()
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }

    private async void OnBack_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label lbl) return;
        await lbl.ScaleTo(0.9, 100);
        lbl.Scale = 1;
        await Navigation.PopAsync();
    }
}