using wwrc_maui.Content.Viewmodels.Media;
using wwrc_maui.Content.Views.Media.Gallery.Photos;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class PhotoDetailsPage : ContentPage
{
    GalleryDetailsVm viewmodel = new();
    PhotoDetailsCell detailsCell = new();

    public PhotoDetailsPage(string albumId, string photoId)
    {
        InitializeComponent();
        viewmodel.albumId = albumId;
        viewmodel.mediaId = photoId;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    async void Initialize()
    {
        await Task.Delay(300);
        await viewmodel.GetAlbumById();
        await viewmodel.GetPhotoById();
        detailsCell.Album = viewmodel.Album;
        detailsCell.Photo = viewmodel.ImageUrl;
        detailsCell.Initialize();
        await viewmodel.SetPhotoReadStatus();
    }

    private async void OnDownload_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        if (viewmodel.ImageUrl != null)
            await Launcher.OpenAsync(new Uri(viewmodel.ImageUrl.Image));
    }

    private async void OnInfo_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        await App.DisplayAlert("Photo Details", null, detailsCell, "Close");
    }
}