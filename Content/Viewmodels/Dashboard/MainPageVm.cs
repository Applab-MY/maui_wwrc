using AndroidX.Browser.Trusted.Sharing;
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
        private string _countStockAlert = "";
        private string _countNewsAlert = "";
        private string _countEmpHandbookAlert = "";
        private string _countProductAlert = "";
        private string _countMediaAlert = "";
        private List<string> _carouselItems = [];
        private DataTemplateSelector? _templateSelector = null;
        private List<DashboardCarouselTemplate> _menuList = [];
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
        public string CountStockAlert
        {
            get { return _countStockAlert; }
            set { SetProperty(ref _countStockAlert, value); }
        }
        public string CountNewsAlert
        {
            get { return _countNewsAlert; }
            set { SetProperty(ref _countNewsAlert, value); }
        }
        public string CountEmpHandbookAlert
        {
            get { return _countEmpHandbookAlert; }
            set { SetProperty(ref _countEmpHandbookAlert, value); }
        }
        public string CountProductAlert
        {
            get { return _countProductAlert; }
            set { SetProperty(ref _countProductAlert, value); }
        }
        public string CountMediaAlert
        {
            get { return _countMediaAlert; }
            set { SetProperty(ref _countMediaAlert, value); }
        }
        public List<string> CarouselItems
        {
            get { return _carouselItems; }
            set { SetProperty(ref _carouselItems, value); }
        }
        public DataTemplateSelector? TemplateSelector
        {
            get { return _templateSelector; }
            set { SetProperty(ref _templateSelector, value); }
        }
        public List<DashboardCarouselTemplate> MenuList
        {
            get { return _menuList; }
            set { SetProperty(ref _menuList, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
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

                    CountStockAlert = items.StockAlertCount.ToString();
                    CountNewsAlert = items.NewsCount.ToString();
                    CountEmpHandbookAlert = items.HandbookCount.ToString();
                    CountProductAlert = items.CatalogCount.ToString();
                    var total = items.PhotoCount + items.VideoCount;
                    CountMediaAlert = total.ToString();
                    items.TotalMediaCount = total.ToString();

                    //RenderIconItems(country, subsidiary, ShareData.selectedUser); //Preferences.Default.Get("userId", "");
                }
                if (_res.SystemCode == 0)
                { await App.DisplayAlert("API Error", "Please contact system admin", null, "Okay"); }
            }
        }

        private async Task GetCurrencyData()
        {
            Subsidiary = Preferences.Default.Get("subsidiary", "");
            IsSubsidiaryVisible = !string.IsNullOrEmpty(Subsidiary);
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
            MenuList =
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
                },
            ];
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
            var firstTemplate = new DataTemplate();
            var secondTemplate = new DataTemplate();
            CarouselItems = [];
            CarouselItems.Add("1");

            if (filtered.Count <= 8)
            {
                firstTemplate = new DataTemplate(() =>
                {
                    var cell = new CarouselCell { MenuFilter = filtered, MenuList = MenuList };
                    cell.BuildView();
                    return cell;
                });
            }
            else
            {
                CarouselItems.Add("2");
                var _page1 = new List<string>();
                var _page2 = new List<string>();
                for (int a = 0; a < 8; a++) _page1.Add(filtered[a]);
                for (int a = 8; a < filtered.Count; a++) _page2.Add(filtered[a]);

                firstTemplate = new DataTemplate(() =>
                {
                    var cell = new CarouselCell { MenuFilter = _page1, MenuList = MenuList };
                    cell.BuildView();
                    return cell;
                });
                secondTemplate = new DataTemplate(() =>
                {
                    var cell = new CarouselCell { MenuFilter = _page2, MenuList = MenuList };
                    cell.BuildView();
                    return cell;
                });
            }

            TemplateSelector = new DashBoardDataTemplate
            { FirstTemplate = firstTemplate, SecondTemplate = secondTemplate };
            #endregion
        }
    }
}