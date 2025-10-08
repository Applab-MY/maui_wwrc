using wwrc_maui.Content.Viewmodels.Media;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class VideoDetailsPage : ContentPage
{
    GalleryDetailsVm viewmodel = new();

    public VideoDetailsPage(string id)
    {
        InitializeComponent();
        viewmodel.albumId = id;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await viewmodel.GetVideoById();
        await viewmodel.SetVideoReadStatus();
    }
}