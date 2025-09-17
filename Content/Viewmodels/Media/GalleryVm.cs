using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Viewmodels.Media.Gallery
{
    public class GalleryVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        List<DB_Album> _albums = [];
        List<VideoInfo> _videos = [];
        List<ImageInfo> _photos = [];
        bool _noData = false;
        #endregion
        #region props
        public List<DB_Album> AllAlbums
        {
            get { return _albums; }
            set { SetProperty(ref _albums, value); }
        }
        public List<VideoInfo> VideoAlbums
        {
            get { return _videos; }
            set { SetProperty(ref _videos, value); }
        }
        public List<ImageInfo> PhotoAlbums
        {
            get { return _photos; }
            set { SetProperty(ref _photos, value); }
        }
        public bool NoData
        {
            get { return _noData; }
            set { SetProperty(ref _noData, value); }
        }
        #endregion
        #endregion

        public List<string> tabList = [];

        public GalleryVm()
        {
            IsBusy = false;
            tabList = ["Photo", "Video"];
        }

        public async void GetAllAlbums()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    var _res = await App.AppClient.GetPhoto();
                    if (_res.SystemCode == 401)
                    {
                        AppDatabase.Instance.DeleteAllData();
                        Preferences.Default.Clear();
                        await App.DisplayAlert("Relogin", "Please login again", null, "Okay");
                        Application.Current?.Dispatcher.Dispatch(() =>
                        { Application.Current.Windows[0].Page = new Login(); });
                    }
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count > 0)
                    {
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_Album>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<ImageInfo>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<VideoInfo>();

                        foreach (var data in _res.items)
                        {
                            foreach (var item in data.PhotoGallery)
                            {
                                var img_db = new DB_Album()
                                {
                                    Id = item.Id,
                                    Title = item.Title,
                                    Description = item.Description,
                                    AlbumDate = item.AlbumDate,
                                    CreatedBy = item.CreatedBy,
                                    CreateDate = item.CreateDate,
                                    IsRead = item.IsRead,
                                };
                                AppDatabase.Instance.SqlConnection.Insert(img_db);

                                foreach (var inside in item.Images)
                                {
                                    var img_inside = new ImageInfo()
                                    {
                                        PhotoGalleryId = inside.PhotoGalleryId,
                                        Image = inside.Image,
                                        ImageSize = inside.ImageSize,
                                        IsDefault = inside.IsDefault,
                                    };
                                    AppDatabase.Instance.SqlConnection.Insert(img_inside);
                                }
                            }
                            foreach (var vid in data.VideoGallery)
                            {
                                var vid_db = new VideoInfo()
                                {
                                    Id = vid.Id,
                                    Title = vid.Title,
                                    Description = vid.Description,
                                    YoutubeUrl = vid.YoutubeUrl,
                                    Image = vid.Image,
                                    CreatedBy = vid.CreatedBy,
                                    CreateDate = vid.CreateDate,
                                    IsRead = vid.IsRead
                                };
                                AppDatabase.Instance.SqlConnection.Insert(vid_db);
                            }
                        }
                        GetPhotoVideo();
                    }
                    else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                        + ". " + _res.SystemMessage, null, "Okay");
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                }
            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
        }

        public void GetPhotoVideo()
        {
            string _qMedia = "SELECT * FROM DB_Album";
            string _qPhoto = "SELECT * FROM ImageInfo";
            string _qVideo = "SELECT * FROM VideoInfo";
            AllAlbums = AppDatabase.Instance.SqlConnection.Query<DB_Album>(_qMedia);
            PhotoAlbums = AppDatabase.Instance.SqlConnection.Query<ImageInfo>(_qPhoto);
            VideoAlbums = AppDatabase.Instance.SqlConnection.Query<VideoInfo>(_qVideo);
        }
    }
}
