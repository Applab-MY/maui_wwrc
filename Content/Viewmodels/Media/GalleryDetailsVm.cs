using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Viewmodels.Media
{
    public class GalleryDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        DB_Album? _album = null;
        List<ImageInfo> _photos = [];
        string _vidUrl = "";
        ImageInfo? _imgUrl = null;
        bool _noData = false;
        #endregion
        #region props
        public DB_Album? Album
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
        public ImageInfo? ImageUrl
        {
            get { return _imgUrl; }
            set { SetProperty(ref _imgUrl, value); }
        }
        public bool NoData
        {
            get { return _noData; }
            set { SetProperty(ref _noData, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public string albumId = "";
        public string mediaId = "";

        public GalleryDetailsVm() { RefreshCommand = new Command(GetAlbum); }

        async void GetAlbum() { await GetAlbumById(); }

        public async Task GetAlbumById()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                string _qAlbum = "SELECT * FROM DB_Album WHERE Id = '" + albumId + "'";
                var _data = AppDatabase.Instance.SqlConnection.Query<DB_Album>(_qAlbum);
                //var _data = DummyAlbum(); //for demo
                if (_data.Count > 0) { Album = _data[0]; }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = true; IsRefreshing = true;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async Task GetPhotosFromAlbum()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                string _qPhoto = "SELECT * FROM ImageInfo WHERE PhotoGalleryId = '" + albumId.ToUpper() + "'";
                AllPhotos = AppDatabase.Instance.SqlConnection.Query<ImageInfo>(_qPhoto);
                //AllPhotos = DummyPhotos(); //for demo
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = true; IsRefreshing = true;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async Task GetPhotoById()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                ImageUrl = null;
                string query = "SELECT * FROM ImageInfo WHERE Image = '" + mediaId + "'";
                ImageUrl = AppDatabase.Instance.SqlConnection.Query<ImageInfo>(query).FirstOrDefault();
                //ImageUrl = DummyPhoto(); //for demo
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = true; IsRefreshing = true;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async Task GetVideoById()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                string _qVideo = "SELECT * FROM VideoInfo WHERE Id = '" + mediaId + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<VideoInfo>(_qVideo);
                if (items.Count > 0)
                {
                    string s = items[0].YoutubeUrl;
                    string stringCutted = s.Split('=').Last();
                    VideoUrl = "https://www.youtube.com/embed/" + stringCutted + "";
                }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = true; IsRefreshing = true;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        #region dummy data
        List<DB_Album> DummyAlbum()
        {
            var _album = new List<DB_Album>{ new()
            {
                Title = "Album-Demo-1",
                AlbumDate = DateTime.Now,
                Description = "This is a demo album description.",
                CreateDate = DateTime.Now.AddDays(-5),
                CreatedBy = "Admin"
            }};
            return _album;
        }

        List<ImageInfo> DummyPhotos()
        {
            var _list = new List<ImageInfo>();
            for (int i = 0; i < 15; i++)
            {
                var _isOdd = (i % 2) > 0;
                var _dt = DateTime.Now;
                var model = new ImageInfo
                {
                    PhotoGalleryId = Guid.NewGuid().ToString(),
                    Image = _isOdd ? "test_img1.jpg" : "test_img2.jpg",
                    ImageSize = i * 10 + "Mb",
                };
                _list.Add(model);
            }
            return _list;
        }

        ImageInfo DummyPhoto()
        {
            return new ImageInfo
            {
                PhotoGalleryId = Guid.NewGuid().ToString(),
                Image = "http://i3.ytimg.com/vi/mBcfl7SUqjY/maxresdefault.jpg",
                ImageSize = "15Mb",
            };
        }
        #endregion

        public async Task SetPhotoReadStatus()
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

        public async Task SetVideoReadStatus()
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
