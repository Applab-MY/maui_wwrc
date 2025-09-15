using wwrc_maui.Content.Viewmodels.Media.News;

namespace wwrc_maui.Content.Views.Media.News;

public partial class NewsMainPage : ContentPage
{
	NewsVm viewmodel = new();

    public NewsMainPage()
	{
		InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}