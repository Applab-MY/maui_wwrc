using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.ProductModel;

namespace wwrc_maui.Content.Viewmodels.Media.Product
{
    public class ProductVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        List<ProductMainModel> _catalogs = [];
        List<string> _branches = [];
        string _selectedBranch = "";
        bool _noData = false;
        #endregion
        #region props
        public List<ProductMainModel> Catalogs
        {
            get { return _catalogs; }
            set
            {
                SetProperty(ref _catalogs, value);
                NoData = value.Count == 0;
            }
        }
        public List<string> Branches
        {
            get { return _branches; }
            set { SetProperty(ref _branches, value); }
        }
        public string SelectedBranch
        {
            get { return _selectedBranch; }
            set { SetProperty(ref _selectedBranch, value); }
        }
        public bool NoData
        {
            get { return _noData; }
            set { SetProperty(ref _noData, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public string branch = "";
        public string folderId = "";
        public LoginMainModel? LoginData = null;
        List<ProductMainModel> catalogsCache = [];

        public ProductVm()
        {
            RefreshCommand = new Command(GetProductCatalogList);
            LoginData = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                ("Select * from LoginMainModel").FirstOrDefault();
        }

        public async void GetProductCatalogList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    catalogsCache = []; Catalogs = [];
                    var model = new API_ProductCatalogModel { BranchId = branch, FolderId = folderId };
                    var _res = await App.AppClient.ProductCatalog(model);
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<ProductMainModel>();
                        foreach (var data in _res.items)
                        {
                            var productCatalog = new ProductMainModel()
                            {
                                Id = data.Id,
                                Title = data.Title,
                                Description = data.Description,
                                IsFile = data.IsFile,
                                File = data.File,
                                FileImage = data.FileImage,
                                FileType = data.FileType,
                                Filesize = data.Filesize,
                                CreateByName = data.CreateByName,
                                CreateDate = data.CreateDate.Substring(0, 10),
                                IsRead = data.IsRead
                            };
                            catalogsCache.Add(productCatalog);
                            AppDatabase.Instance.SqlConnection.Insert(productCatalog);
                        }
                        Catalogs = catalogsCache;
                    }
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

        public async void SetProductCatalogReadStatus(string itemId)
        {
            try
            {
                var catalogId = new StringContent(itemId);
                var content = new MultipartFormDataContent { { catalogId, "Id" } };
                if (App.AppClient != null) await App.AppClient.ReadProductCatalog(content);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }
    }
}
