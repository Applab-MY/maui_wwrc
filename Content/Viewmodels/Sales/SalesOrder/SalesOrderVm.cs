using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.DashboardModel;
using static wwrc_maui.Content.Model.SOModel;

namespace wwrc_maui.Content.Viewmodels.Sales.SalesOrder
{
    public class SalesOrderVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        bool _isSearch = false;
        string _country = "";
        string _subsidiary = "";
        string _salesperson = "";
        List<SalesOrderMainModel> _soMain = [];
        List<DB_SalesOrderModel> _soMonth = [];
        List<Db_SOList> _salesorder = [];
        List<Db_SOItemList> _soItems = [];
        bool _nodata = false;
        string _searchTxt = "";
        private double _entryWidth = 0.0;
        #endregion
        #region props
        public bool IsSearchVisible
        {
            get { return _isSearch; }
            set { SetProperty(ref _isSearch, value); }
        }
        public string Country
        {
            get { return _country; }
            set { SetProperty(ref _country, value); }
        }
        public string Subsidiary
        {
            get { return _subsidiary; }
            set { SetProperty(ref _subsidiary, value); }
        }
        public string SalesPerson
        {
            get { return _salesperson; }
            set { SetProperty(ref _salesperson, value); }
        }
        public List<SalesOrderMainModel> SoMain
        {
            get { return _soMain; }
            set
            {
                SetProperty(ref _soMain, value);
                NoData = value.Count == 0;
            }
        }
        public List<DB_SalesOrderModel> SoMonth
        {
            get { return _soMonth; }
            set { SetProperty(ref _soMonth, value); }
        }
        public List<Db_SOList> SalesOrder
        {
            get { return _salesorder; }
            set { SetProperty(ref _salesorder, value); }
        }
        public List<Db_SOItemList> SoItems
        {
            get { return _soItems; }
            set { SetProperty(ref _soItems, value); }
        }
        public bool NoData
        {
            get { return _nodata; }
            set { SetProperty(ref _nodata, value); }
        }
        public string SearchTxt
        {
            get { return _searchTxt; }
            set { SetProperty(ref _searchTxt, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public Command? SearchCommand { get; set; } = null;

        public DashboardMainModel? DashboardData = null;
        public List<SalesOrderMainModel> SoMainCache = [];
        public Action<bool>? OnFinishLoad = null;
        public List<SalesPersonList> SalesList = [];
        public bool isFilterSales = false;

        public SalesOrderVm()
        {
            Country = Preferences.Default.Get("country", "");
            Subsidiary = Preferences.Default.Get("subsidiary", "");
            SalesPerson = Preferences.Default.Get("userId", "");
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(OnRefreshCommand);
            SearchCommand = new Command(SearchSalesOrder);
        }

        public async void OnRefreshCommand()
        {
            IsBusy = true; IsRefreshing = true;
            await GetSalesOrderList();
            IsBusy = false; IsRefreshing = false;
        }

        public async Task GetSalesOrderList()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    SoMainCache = []; SoMain = [];
                    SoMonth = []; SalesOrder = []; SoItems = [];
                    var model = new API_SalesOrder
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        UserId = isFilterSales ? SalesPerson : Preferences.Default.Get("userId", ""),
                    };
                    var _res = await App.AppClient.GetSalesOrder(model);
                    if (_res.SystemCode == 401)
                    {
                        AppDatabase.Instance.DeleteAllData();
                        Preferences.Default.Clear();
                        await App.DisplayAlert("Relogin", "Please login again", null, "Okay");
                        Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(() =>
                        { Microsoft.Maui.Controls.Application.Current.Windows[0].Page = new Login(); });
                    }
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count > 0)
                    {
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_SalesOrderModel>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<Db_SOList>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<Db_SOItemList>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<Db_DOList>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<Db_DOItemsList>();
                        SoMainCache = [.. _res.items];
                        SoMain = SoMainCache;

                        foreach (var item in _res.items)
                        {
                            var dataList = item.Data;
                            var salesMonth_db = new DB_SalesOrderModel()
                            { Date = item.Date, Records = item.Records, };
                            SoMonth.Add(salesMonth_db);

                            foreach (var data in dataList)
                            {
                                var dbDataList = new Db_SOList()
                                {
                                    Id = data.Id,
                                    SalesDate = item.Date,
                                    Country = data.Country,
                                    Company = data.Company,
                                    UserID = data.UserID,
                                    CardCode = data.CardCode,
                                    CardName = data.CardName,
                                    SONO = data.SONO,
                                    PostingDate = data.PostingDate,
                                    DeliveryDate = data.DeliveryDate,
                                    ApprovedDate = data.ApprovedDate,
                                    DODate = data.DODate,
                                    DocStatus = data.DocStatus,
                                    DocTotal = data.DocTotal,
                                    Currency = data.Currency
                                };
                                SalesOrder.Add(dbDataList);

                                foreach (var SOItem in data.Items)
                                {
                                    var dbSOItemList = new Db_SOItemList()
                                    {
                                        Id = data.SONO,
                                        ItemCode = SOItem.ItemCode,
                                        ItemName = SOItem.ItemName,
                                        UnitPrice = SOItem.UnitPrice,
                                        Quantity = SOItem.Quantity,
                                        OpenQTY = SOItem.OpenQTY
                                    };
                                    SoItems.Add(dbSOItemList);
                                }
                            }
                        }
                        AppDatabase.Instance.SqlConnection.InsertAll(SoMonth);
                        AppDatabase.Instance.SqlConnection.InsertAll(SalesOrder);
                        AppDatabase.Instance.SqlConnection.InsertAll(SoItems);
                    }
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count == 0)
                    { } //bugfix :: sometimes api success but return null items
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

        public void SearchSalesOrder()
        {
            var _cache = new List<SalesOrderMainModel>();
            if (string.IsNullOrEmpty(SearchTxt)) { SoMain = SoMainCache; }
            else
            {
                var numberList = new List<int>();
                var result = new List<SOList>();
                var monthHash = new Dictionary<string, int>();

                if (SoMainCache.Count > 0)
                {
                    for (int i = 0; i < SoMainCache.Count; i++)
                    {
                        if (SoMainCache[i].Data != null)
                        {
                            var Data = SoMainCache[i].Data;
                            result = Data.ToList().FindAll(item => item.CardName.ToUpper().Contains(SearchTxt)
                                || item.SONO.ToLower().Contains(SearchTxt));
                            if (result.Count > 0) numberList.Add(i);
                            if (monthHash.ContainsKey(SoMainCache[i].Date))
                            {
                                monthHash.Remove(SoMainCache[i].Date);
                                monthHash.Add(SoMainCache[i].Date, result.Count);
                            }
                            else monthHash.Add(SoMainCache[i].Date, result.Count);
                        }
                    }

                    foreach (var number in numberList)
                    {
                        var current = new SalesOrderMainModel();
                        string date = SoMainCache[number].Date;
                        int count = monthHash[date];
                        current.Date = date;
                        current.Records = count + " record(s) found";
                        _cache.Add(current);
                    }
                    SoMain = _cache;
                }
            }
        }

        public async Task GetDashboardData()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    DashboardData = null;
                    var model = new API_DashBoard
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        UserId = Preferences.Default.Get("userId", "")
                    };
                    var _res = await App.AppClient.GetDashBoard(model);
                    if (_res.SystemCode == 401) { }
                    else if (_res.SystemCode == 200 && _res.items != null)
                    {
                        DashboardData = _res.items[0];
                        SetupSalesList();
                    }
                    //else await App.DisplayAlert("Error", "API error : " + _res.SystemCode.ToString()
                    //    + ", " + _res.SystemMessage + "\r" + _res.SystemDebugMessage, null, "Okay");
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                }
            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
        }

        public void SetupSalesList()
        {
            SalesList = [];
            if (DashboardData != null)
            {
                var found = DashboardData.Filter.Where(x => x.Country.Equals(Country)).FirstOrDefault();
                if (found != null)
                {
                    if (found.SubsidiaryList != null && found.SubsidiaryList.Count > 0)
                    {
                        var _subs = found.SubsidiaryList.Where(x => x.Subsidiary.Equals(Subsidiary)).FirstOrDefault();
                        if (_subs != null)
                        {
                            foreach (var sp in _subs.SalesPersonList)
                            { if (sp.Id.Equals(SalesPerson)) { sp.Checked = true; } }
                            SalesList = [.. _subs.SalesPersonList];
                            OnFinishLoad?.Invoke(true);
                        }
                    }
                }
            }
        }
    }
}
