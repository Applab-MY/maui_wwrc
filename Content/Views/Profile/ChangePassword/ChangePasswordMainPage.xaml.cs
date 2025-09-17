using wwrc_maui.Content.Viewmodels.Profile;

namespace wwrc_maui.Content.Views.Profile.ChangePassword;

public partial class ChangePasswordMainPage : ContentPage
{
    ChangePasswordVm viewmodel = new();

    public ChangePasswordMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        viewmodel.SubmitAction += async (data) => { if (data) await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}