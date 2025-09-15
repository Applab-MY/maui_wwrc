using wwrc_maui.Content.Viewmodels.Media.EmpHandbook;

namespace wwrc_maui.Content.Views.Media.EmpHandbook;

public partial class EmpHandbookMainPage : ContentPage
{
    EmpHandbookVm viewmodel = new();

    public EmpHandbookMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}