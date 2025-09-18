using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.NewsModel;

namespace wwrc_maui.Content.Viewmodels.Media
{
    public class NewsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private List<NewsTable> _newsList = [];
        private bool _isSearchVisible = false;
        private bool _isNewsVisible = false;
        private double _entryWidth = 0.0;
        private NewsTable? _newsModel = null;
        private string? _imgSource = "";
        private string? _attachTitle = "";
        private bool _hasAttachment = false;
        #endregion
        #region properties
        public bool IsSearchVisible
        {
            get { return _isSearchVisible; }
            set { SetProperty(ref _isSearchVisible, value); }
        }
        public List<NewsTable> NewsList
        {
            get { return _newsList; }
            set
            {
                SetProperty(ref _newsList, value);
                IsNewsVisible = value.Count > 0;
            }
        }
        public bool IsNewsVisible
        {
            get { return _isNewsVisible; }
            set { SetProperty(ref _isNewsVisible, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        public NewsTable? NewsDetailsModel
        {
            get { return _newsModel; }
            set { SetProperty(ref _newsModel, value); }
        }
        public string? ImgSource
        {
            get { return _imgSource; }
            set { SetProperty(ref _imgSource, value); }
        }
        public string? AttachmentTitle
        {
            get { return _attachTitle; }
            set
            {
                SetProperty(ref _attachTitle, value);
                HasAttachment = !string.IsNullOrEmpty(value);
            }
        }
        public bool HasAttachment
        {
            get { return _hasAttachment; }
            set { SetProperty(ref _hasAttachment, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        List<NewsTable> newsCache = [];

        public NewsVm()
        {
            IsSearchVisible = false;
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(GetNewsList);
        }

        public async void GetNewsList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    NewsList = []; newsCache = [];
                    var _res = await App.AppClient.GetNews();
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<NewsTable>();
                        foreach (var data in _res.items)
                        {
                            var newsItem = new NewsTable()
                            {
                                Id = data.Id,
                                Title = data.Title,
                                Description = data.Description,
                                Date = data.Date.Substring(0, 10),
                                Attachment = data.Attachment,
                                CreatedBy = data.CreatedBy,
                                IsRead = data.IsRead
                            };
                            foreach (var img in data.Images)
                            {
                                var newsinfo = new NewsInfoTable()
                                {
                                    NewsId = img.NewsId,
                                    ImageUrl = img.ImageUrl,
                                    IsDefault = img.IsDefault
                                };
                                AppDatabase.Instance.SqlConnection.Insert(newsinfo);
                                if (img.IsDefault == "true") newsItem.DefaultImg = img.ImageUrl;
                                else if (img.IsDefault == "false") newsItem.DefaultImgFalse = img.ImageUrl;
                            }
                            AppDatabase.Instance.SqlConnection.Insert(newsItem);
                            newsCache.Add(newsItem);
                        }
                        NewsList = newsCache;
                    }
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count == 0)
                    { } //bugfix :: sometimes api success but return null items
                    else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                        + ". " + _res.SystemMessage, null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    IsBusy = false; IsRefreshing = false;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                }

            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
            IsBusy = false; IsRefreshing = false;
        }

        public async void GetNewsById(string? id = null)
        {
            if (string.IsNullOrEmpty(id)) return;
            try
            {
                IsBusy = true; IsRefreshing = true;
                await Task.Delay(300);
                string _qNewsDetail = "SELECT * FROM NewsTable WHERE Id = '" + id + "'";
                string _qNews = "SELECT * FROM NewsInfoTable WHERE NewsId = '" + id.ToUpper() + "'";
                var details = AppDatabase.Instance.SqlConnection.Query<NewsTable>(_qNewsDetail);
                var news = AppDatabase.Instance.SqlConnection.Query<NewsInfoTable>(_qNews);

                if (details.Count > 0)
                {
                    NewsDetailsModel = details.FirstOrDefault();
                    //label_createdBy.Text = "By " + details[0].CreatedBy + " on " + details[0].Date;

                    if (news.Count > 0)
                    {
                        if (news[0].IsDefault == "false")
                            ImgSource = details.FirstOrDefault()?.DefaultImgFalse;
                        else if (news[0].IsDefault == "true")
                            ImgSource = details.FirstOrDefault()?.DefaultImg;
                    }

                    AttachmentTitle = "";
                    if (!string.IsNullOrEmpty(details.FirstOrDefault()?.Attachment))
                    {
                        var stringpdf = details.FirstOrDefault()?.Attachment.Split('/').Last();
                        AttachmentTitle = stringpdf;
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
            IsBusy = false; IsRefreshing = false;
        }

        public async void UpdateNewsReadStatus(string? newsId = null)
        {
            if (string.IsNullOrEmpty(newsId)) return;
            try
            {
                IsBusy = true; IsRefreshing = true;
                if (App.AppClient != null)
                {
                    var news = new StringContent(newsId);
                    var content = new MultipartFormDataContent { { news, "Id" } };
                    await App.AppClient.UpdateNews(content); //update read status
                }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public void SearchNews(string? text = null)
        {
            if (!string.IsNullOrEmpty(text))
                NewsList = newsCache.FindAll(item => item.Title.ToLower().Contains(text.ToLower()));
            else NewsList = newsCache;
        }

        public async void DownloadAttachment(string? newsId = null)
        {
            if (string.IsNullOrEmpty(newsId)) return;
            string news = "SELECT * FROM NewsTable WHERE Id = '" + newsId + "'";
            var items = AppDatabase.Instance.SqlConnection.Query<NewsTable>(news);
            if (items.Count > 0)
            {
                var link = items[0].Attachment;
                await Launcher.OpenAsync(new Uri(link));
            }
        }
    }
}
