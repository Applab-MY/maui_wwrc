using wwrc_maui.Content.Viewmodels.Media;
using wwrc_maui.Content.Views.Media.Gallery.Photos;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class PhotoDetailsPage : ContentPage
{
    GalleryDetailsVm viewmodel = new();
    PhotoDetailsCell detailsCell = new();

    public PhotoDetailsPage(string id)
    {
        InitializeComponent();
        viewmodel.mediaId = id;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        viewmodel.OnFinishLoad += viewmodel.SetPhotoReadStatus;
        BindingContext = viewmodel;
        Initialize();
    }

    async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetPhotoById();
    }

    private async void OnDownload_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
    }

    private async void OnInfo_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
    }
}