using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Auth;
using static wwrc_maui.Content.Helper.ReferenceMessenger;

namespace wwrc_maui.Content.Views.Auth;

public partial class Login : ContentPage
{
    LoginVm viewmodel = new();

    public Login()
    {
        InitializeComponent();
        BindingContext = viewmodel;

        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, async (receiver, message) =>
        {
            if (message.Value != null && message.Value.Key.Equals("O365login"))
            {
                //2001:usercancel, 2003:success
                if (message.Value.Value.Equals("2001"))
                    await App.DisplayAlert("Cancel", "Login cancelled.", null, "Close");
                else if (message.Value.Value.Equals("2003"))
                    await App.DisplayAlert(message.Value.Value, "login success", null, "Okay");
                viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
            }
        });
    }

    private async void OnForgotPwd_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label lbl) return;
        await lbl.ScaleTo(0.9, 100);
        lbl.Scale = 1;
        await Navigation.PushAsync(new ForgotPassword());
    }
}