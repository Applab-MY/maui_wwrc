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
        viewmodel.OnFinishLoad += viewmodel.SetVideoReadStatus;
        BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewmodel.GetVideoById();
    }
}