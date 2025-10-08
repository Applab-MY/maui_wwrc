using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Views.Media.Gallery.Photos;

public partial class PhotoDetailsCell : ContentView
{
    public DB_Album? Album = null;
    public ImageInfo? Photo = null;
    CancellationTokenSource _cts = new();

    public PhotoDetailsCell() { InitializeComponent(); }

    public void Initialize()
    {
        if (Album != null && Photo != null)
        {
            string _file = Photo.Image.Split('/').Last();
            string _type = Photo.Image.Split('.').Last();

            img_file.Source = Photo.Image;
            lbl_filename.Text = _file;
            lbl_filesize.Text = Photo.ImageSize;

            lbl_type.Text = _type;
            lbl_createOn.Text = Album.CreateDate.ToString("dd/MM/yyyy");
            lbl_createBy.Text = Album.CreatedBy;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        try
        {
            await SaveFile(_cts.Token);
        }
        catch (OperationCanceledException ex)
        {
            var error = ex.Message;
            await App.DisplayAlert("Info", error, null, "Okay");
        }
    }

    async Task SaveFile(CancellationToken cancellationToken)
    {
        if (Photo != null)
        {
            string fileName = Photo.Image.Split('/').Last();
            var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(Photo.Image);
            var filePath = Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.LocalApplicationData), fileName);
            File.WriteAllBytes(filePath, imageBytes);

            ////Xamarin.Essentials Share API
            //await Share.RequestAsync(new ShareFileRequest(filePath, "image/jpeg", "Downloaded Image"));

            //string fileName = Photo.Image.Split('/').Last();
            //using var stream = new MemoryStream(Encoding.Default.GetBytes(Photo.Image));
            //var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
            //if (fileSaverResult.IsSuccessful)
            //    await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}")
            //        .Show(cancellationToken);
            //else
            //    await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}")
            //        .Show(cancellationToken);
        }
        else await App.DisplayAlert("Empty", "File not found.", null, "Okay");
    }
}