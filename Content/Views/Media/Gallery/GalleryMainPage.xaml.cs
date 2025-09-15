using wwrc_maui.Content.Viewmodels.Media.Gallery;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class GalleryMainPage : ContentPage
{
	GalleryVm viewmodel = new();

    public GalleryMainPage()
	{
		InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}