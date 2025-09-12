using System.Collections.ObjectModel;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using wwrc_maui.Content.Views.Dashboard;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.CurrencyModel;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Viewmodels.Dashboard
{
    public class MainPageVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private string _version = "";
        private string _subsidiary = "";
        private bool _isSubsVisible = false;
        private DateTime _excDate = new();
        private string _flagIcon = "";
        private string _usdRate = "";
        private string _centerPercentYtd = "";
        private string _centerPercentMtd = "";
        private double _ytdProgress = 0;
        private double _mtdProgress = 0;
        private string _ytdSales = "";
        private string _mtdSales = "";
        private string _ytdDetails = "";
        private string _mtdDetails = "";
        private double _entryWidth = 0.0;
        private ObservableCollection<string> _carouselItems = [];
        private DataTemplateSelector? _templateSelector = null;
        #endregion
        #region properties
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }
        public string Subsidiary
        {
            get { return _subsidiary; }
            set { SetProperty(ref _subsidiary, value); }
        }
        public bool IsSubsidiaryVisible
        {
            get { return _isSubsVisible; }
            set { SetProperty(ref _isSubsVisible, value); }
        }
        public DateTime ExchangeDate
        {
            get { return _excDate; }
            set { SetProperty(ref _excDate, value); }
        }
        public string FlagIcon
        {
            get { return _flagIcon; }
            set { SetProperty(ref _flagIcon, value); }
        }
        public string UsdRate
        {
            get { return _usdRate; }
            set { SetProperty(ref _usdRate, value); }
        }
        public string CenterPercentYtd
        {
            get { return _centerPercentYtd; }
            set { SetProperty(ref _centerPercentYtd, value); }
        }
        public string CenterPercentMtd
        {
            get { return _centerPercentMtd; }
            set { SetProperty(ref _centerPercentMtd, value); }
        }
        public double YtdProgress
        {
            get { return _ytdProgress; }
            set { SetProperty(ref _ytdProgress, value); }
        }
        public double MtdProgress
        {
            get { return _mtdProgress; }
            set { SetProperty(ref _mtdProgress, value); }
        }
        public string YtdSales
        {
            get { return _ytdSales; }
            set { SetProperty(ref _ytdSales, value); }
        }
        public string MtdSales
        {
            get { return _mtdSales; }
            set { SetProperty(ref _mtdSales, value); }
        }
        public string YtdDetails
        {
            get { return _ytdDetails; }
            set { SetProperty(ref _ytdDetails, value); }
        }
        public string MtdDetails
        {
            get { return _mtdDetails; }
            set { SetProperty(ref _mtdDetails, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        public ObservableCollection<string> CarouselItems
        {
            get { return _carouselItems; }
            set { SetProperty(ref _carouselItems, value); }
        }
        public DataTemplateSelector? TemplateSelector
        {
            get { return _templateSelector; }
            set { SetProperty(ref _templateSelector, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public DashboardMainModel? DashboardData = null;
        public FilterDataModel? FilterData = null;
        public bool IsDisplayGraph = false;
        public bool IsDisplayCurrency = false;

        public MainPageVm()
        {
            IsBusy = false;
            Version += "Ver." + AppInfo.VersionString;
            ExchangeDate = DateTime.Now;
            UsdRate = "-";
            RefreshCommand = new Command(SetupData);
        }

        public async void SetupData()
        {
            IsBusy = true; IsRefreshing = true;
            FlagIcon = "ic_uncheck";
            var _login = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                ("Select * from LoginMainModel").FirstOrDefault();
            if (_login != null && _login.UserData != null)
            {
                var _mods = _login.UserData.UserModules;
                if (_mods != null)
                {
                    if (_mods.SalesTargetChart) { IsDisplayGraph = true; }
                    if (_mods.CurrencyExchangeRate) { IsDisplayCurrency = true; }
                }
            }
            await GetDashboardData();
            await GetCurrencyData();
            SetupCarouselMenu();

            var _dbSalesTarget = AppDatabase.Instance.SqlConnection.Query<SalesTargetModule>
                ("Select * from SalesTargetModule").FirstOrDefault();
            var _dbBranch = AppDatabase.Instance.SqlConnection.Query<Branch>
                ("Select * from Branch").FirstOrDefault();
            var _userid = Preferences.Default.Get("userId", "");
            var _country = Preferences.Default.Get("country", "");
            var _subs = Preferences.Default.Get("subsidiary", "");
            if (string.IsNullOrWhiteSpace(_userid)) { _userid = _dbSalesTarget?.Type; }
            if (string.IsNullOrWhiteSpace(_country)) { _country = _dbBranch?.branch; }
            if (string.IsNullOrWhiteSpace(_subs)) { _subs = _dbSalesTarget?.Subsidiary; }

            Subsidiary = _subs ?? "";
            IsSubsidiaryVisible = !string.IsNullOrEmpty(_subs);
            FilterData = new FilterDataModel
            {
                Country = _country ?? "",
                Subsidiary = _subs ?? "",
                UserId = _userid ?? "",
                UserName = _userid ?? "",
            };
            IsBusy = false; IsRefreshing = false;
        }

        private async Task GetDashboardData()
        {
            if (App.AppClient != null)
            {
                var model = new API_DashBoard
                {
                    Country = Preferences.Default.Get("country", ""),
                    Company = Preferences.Default.Get("subsidiary", ""),
                    UserId = Preferences.Default.Get("userId", "")
                };
                var _res = await App.AppClient.GetDashBoard(model);
                if (_res.SystemCode == 401)
                {
                    AppDatabase.Instance.DeleteAllData();
                    Preferences.Default.Clear();
                    await App.DisplayAlert("Relogin", "Please login again", null, "Okay");
                    Application.Current?.Dispatcher.Dispatch(() =>
                    { Application.Current.Windows[0].Page = new Login(); });
                }
                if (_res.SystemCode == 200 && _res.items != null)
                {
                    var items = _res.items[0];
                    if (IsDisplayGraph)
                    {
                        CenterPercentYtd = items.GPPYTD + "%";
                        CenterPercentMtd = items.GPPMTD + "%";
                        YtdProgress = Convert.ToDouble(items.YTDP) / 100;
                        MtdProgress = Convert.ToDouble(items.MTDP) / 100;
                        YtdSales = items.YTD;
                        MtdSales = items.MTD;
                        YtdDetails = items.YTDP;
                        MtdDetails = items.MTDP;

                        Preferences.Default.Set("dashb-mtd", items.MTD);
                        Preferences.Default.Set("dashb-mtdTarget", items.MTDTarget);
                        Preferences.Default.Set("dashb-mtdP", items.MTDP);
                        Preferences.Default.Set("dashb-mtdGP", items.GPPMTD);
                        Preferences.Default.Set("dashb-ytd", items.YTD);
                        Preferences.Default.Set("dashb-ytdTarget", items.YTDTarget);
                        Preferences.Default.Set("dashb-ytdP", items.YTDP);
                        Preferences.Default.Set("dashb-ytdGP", items.GPPYTD);
                    }

                    var total = items.PhotoCount + items.VideoCount;
                    items.TotalMediaCount = total.ToString();
                    DashboardData = items;
                }
                if (_res.SystemCode == 0)
                { await App.DisplayAlert("API Error", "Please contact system admin", null, "Okay"); }
            }
        }

        private async Task GetCurrencyData()
        {
            if (IsDisplayCurrency && App.AppClient != null)
            {
                UsdRate = "-";
                var model = new API_Currency
                {
                    Country = Preferences.Default.Get("country", ""),
                    Company = Preferences.Default.Get("subsidiary", ""),
                    Date = DateTime.Now.ToString("yyyy-MM-dd")
                };
                var _res = await App.AppClient.GetCurrency(model);
                if (_res.SystemCode == 401) { }
                if (_res.SystemCode == 200 && _res.items != null)
                {
                    foreach (var item in _res.items.ExchangeRate)
                    {
                        if (item.CurrencyCode == "USD")
                        {
                            UsdRate = string.Format("{0:0.000}", item.ExchangeRate);
                            FlagIcon = item.CurrencyImage;
                            //USDRate.Text = "1,000,000,000,00";
                        }
                    }
                }
            }
        }

        private void SetupCarouselMenu()
        {
            var _login = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                ("Select * from LoginMainModel").FirstOrDefault();
            string[] menuList = [
                "StockAlert",
                "CustomerAging",
                "SalesOrder",
                "PurchaseOrder",
                "CustomerVisit",
                "StaffDirectory",
                "News",
                "MediaGallery",
                "EmployeeHandbook",
                "ProductCatalogue"
            ];

            #region filter menu by user module
            var filtered = new List<string>();
            var properties = typeof(UserModules).GetProperties();
            var _userModules = new UserModules();
            if (_login != null) _userModules = _login.UserData?.UserModules;
            foreach (var item in menuList)
            {
                foreach (var property in properties)
                {
                    if (item.Equals(property.Name))
                    {
                        var exist = Convert.ToBoolean(property.GetValue(_userModules, null));
                        if (exist) filtered.Add(property.Name);
                    }
                }
            }
            filtered.Add("MyProfile");
            if (_login != null)
            {
                var myProfile = _login?.UserData;
                if (myProfile != null && !myProfile.IsOfficeCredential)
                    filtered.Add("ResetPassword");
            }
            #endregion
            #region set view item template
            DataTemplate? cell1, cell2 = null;
            CarouselItems.Add("1");
            if (filtered.Count <= 8)
            {
                cell1 = new DataTemplate(() =>
                {
                    var _view = new CarouselCell
                    { MenuFilter = filtered, MenuList = CarouselDataSource(), DashboardData = DashboardData };
                    _view.BuildView();
                    return _view;
                });
            }
            else
            {
                var _page1 = new List<string>();
                var _page2 = new List<string>();
                for (int a = 0; a < 8; a++) _page1.Add(filtered[a]);
                for (int a = 8; a < filtered.Count; a++) _page2.Add(filtered[a]);

                cell1 = new DataTemplate(() =>
                {
                    var _view = new CarouselCell
                    { MenuFilter = _page1, MenuList = CarouselDataSource(), DashboardData = DashboardData };
                    _view.BuildView();
                    return _view;
                });
                cell2 = new DataTemplate(() =>
                {
                    var _view = new CarouselCell
                    { MenuFilter = _page2, MenuList = CarouselDataSource(), DashboardData = DashboardData };
                    _view.BuildView();
                    return _view;
                });
                CarouselItems.Add("2");
            }
            TemplateSelector = new DashBoardDataTemplate { FirstTemplate = cell1, SecondTemplate = cell2 };
            #endregion
        }

        private static List<DashboardCarouselTemplate> CarouselDataSource()
        {
            return
            [
                new DashboardCarouselTemplate()
                {
                    Image = "menu_stock_alert",
                    FirstLabel = "Stock",
                    SecondLabel = "Alert",
                    Type = "StockAlert",
                    CountType = "StockAlertCount"
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_customer_aging",
                    FirstLabel = "Customer",
                    SecondLabel = "Aging",
                    Type = "CustomerAging",
                    CountType = ""
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_so",
                    FirstLabel = "Sales",
                    SecondLabel = "Order",
                    Type = "SalesOrder",
                    CountType = ""
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_po",
                    FirstLabel = "Purchase",
                    SecondLabel = "Order",
                    Type = "PurchaseOrder",
                    CountType = ""
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_customer_visit",
                    FirstLabel = "Customer",
                    SecondLabel = "Visit",
                    Type = "CustomerVisit",
                    CountType = ""
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_staff_dir",
                    FirstLabel = "Staff",
                    SecondLabel = "Directory",
                    Type = "StaffDirectory",
                    CountType = ""
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_news_update",
                    FirstLabel = "News",
                    SecondLabel = "Update",
                    Type = "News",
                    CountType = "NewsCount"
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_media_gallery",
                    FirstLabel = "Media",
                    SecondLabel = "Gallery",
                    Type = "MediaGallery",
                    CountType = "TotalMediaCount"
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_handbook",
                    FirstLabel = "Employee",
                    SecondLabel = "Handbook",
                    Type = "EmployeeHandbook",
                    CountType = "HandbookCount"
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_user_guide",
                    FirstLabel = "Product",
                    SecondLabel = "Catalog",
                    Type = "ProductCatalogue",
                    CountType = "CatalogCount"
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_profile",
                    FirstLabel = "My",
                    SecondLabel = "Profile",
                    Type = "MyProfile",
                    CountType = ""
                },
                new DashboardCarouselTemplate()
                {
                    Image = "menu_reset_password",
                    FirstLabel = "Reset",
                    SecondLabel = "Password",
                    Type = "ResetPassword",
                    CountType = ""
                }
            ];
        }
    }
}