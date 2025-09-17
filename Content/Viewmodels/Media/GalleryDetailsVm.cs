using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Viewmodels.Media
{
    public class GalleryDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        List<DB_Album> _album = [];
        List<ImageInfo> _photos = [];
        string _vidUrl = "";
        bool _noData = false;
        string _title = "";
        #endregion
        #region props
        public List<DB_Album> Album
        {
            get { return _album; }
            set { SetProperty(ref _album, value); }
        }
        public List<ImageInfo> AllPhotos
        {
            get { return _photos; }
            set
            {
                SetProperty(ref _photos, value);
                NoData = value.Count == 0;
            }
        }
        public string VideoUrl
        {
            get { return _vidUrl; }
            set { SetProperty(ref _vidUrl, value); }
        }
        public bool NoData
        {
            get { return _noData; }
            set { SetProperty(ref _noData, value); }
        }
        public string PageTitle
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public string albumId = "";
        public Action? OnFinishLoad = null;

        public GalleryDetailsVm() { RefreshCommand = new Command(GetAlbumById); }

        public async void GetAlbumById()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                string _qAlbum = "SELECT * FROM DB_Album WHERE Id = '" + albumId + "'";
                string _qPhoto = "SELECT * FROM ImageInfo WHERE PhotoGalleryId = '" + albumId.ToUpper() + "'";
                Album = AppDatabase.Instance.SqlConnection.Query<DB_Album>(_qAlbum);
                AllPhotos = AppDatabase.Instance.SqlConnection.Query<ImageInfo>(_qPhoto);
                if (Album.Count > 0) { PageTitle = Album[0].Title; }
                IsBusy = false; IsRefreshing = false;
                OnFinishLoad?.Invoke();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = true; IsRefreshing = true;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async void GetVideoById()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                string _qVideo = "SELECT * FROM VideoInfo WHERE Id = '" + albumId + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<VideoInfo>(_qVideo);
                if (items.Count > 0)
                {
                    string s = items[0].YoutubeUrl;
                    string stringCutted = s.Split('=').Last();
                    VideoUrl = "https://www.youtube.com/embed/" + stringCutted + "";
                }
                IsBusy = false; IsRefreshing = false;
                OnFinishLoad?.Invoke();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = true; IsRefreshing = true;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async void SetPhotoReadStatus()
        {
            IsBusy = true; IsRefreshing = true;
            try
            {
                var photoId = new StringContent(albumId);
                var content = new MultipartFormDataContent { { photoId, "Id" } };
                if (App.AppClient != null) await App.AppClient.ReadPhoto(content);
            }
            catch (Exception ex) { var error = ex.Message; }
            IsBusy = false; IsRefreshing = false;
        }

        public async void SetVideoReadStatus()
        {
            IsBusy = true; IsRefreshing = true;
            try
            {
                var videoId = new StringContent(albumId);
                var content = new MultipartFormDataContent { { videoId, "Id" } };
                if (App.AppClient != null) await App.AppClient.ReadVideo(content);
            }
            catch (Exception ex) { var error = ex.Message; }
            IsBusy = false; IsRefreshing = false;
        }
    }
}
