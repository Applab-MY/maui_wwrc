using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.DashboardModel;
using static wwrc_maui.Content.Model.POModel;

namespace wwrc_maui.Content.Viewmodels.Sales.PurchaseOrder
{
    public class PurchaseOrderVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        bool _isSearch = false;
        string _country = "";
        string _subsidiary = "";
        string _salesperson = "";
        List<POMainModel> _poMain = [];
        List<DB_PurchaseMonth> _poMonth = [];
        List<DB_Purchase> _purchase = [];
        List<DB_POItem> _poItems = [];
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
        public List<POMainModel> PoMain
        {
            get { return _poMain; }
            set
            {
                SetProperty(ref _poMain, value);
                NoData = value.Count == 0;
            }
        }
        public List<DB_PurchaseMonth> PoMonth
        {
            get { return _poMonth; }
            set { SetProperty(ref _poMonth, value); }
        }
        public List<DB_Purchase> Purchases
        {
            get { return _purchase; }
            set { SetProperty(ref _purchase, value); }
        }
        public List<DB_POItem> PoItems
        {
            get { return _poItems; }
            set { SetProperty(ref _poItems, value); }
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
        public List<POMainModel> PoMainCache = [];
        public Action<bool>? OnFinishLoad = null;
        public List<SalesPersonList> SalesList = [];
        public bool isFilterSales = false;

        public PurchaseOrderVm()
        {
            Country = Preferences.Default.Get("country", "");
            Subsidiary = Preferences.Default.Get("subsidiary", "");
            SalesPerson = Preferences.Default.Get("userId", "");
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(GetPurchaseOrderList);
            SearchCommand = new Command(SearchPurchaseOrder);
        }

        public async void GetPurchaseOrderList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    PoMainCache = []; PoMain = [];
                    PoMonth = []; Purchases = []; PoItems = [];
                    var model = new API_PurchaseModel
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        UserId = isFilterSales ? SalesPerson : Preferences.Default.Get("userId", ""),
                    };
                    var _res = await App.AppClient.GetPurchase(model);
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
                        PoMainCache = [.. _res.items];
                        PoMain = [.. _res.items];
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_PurchaseMonth>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_Purchase>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_POItem>();

                        foreach (var data in _res.items)
                        {
                            List<PurchaseItem> purchaseorder = data.Data;
                            var purchaseMonth_db = new DB_PurchaseMonth()
                            {
                                Date = data.Date,
                                Records = data.Records,
                            };
                            PoMonth.Add(purchaseMonth_db);

                            foreach (var purchase in purchaseorder)
                            {
                                List<POItem> poitem = purchase.Items;
                                var purchase_db = new DB_Purchase()
                                {
                                    Date = data.Date,
                                    Country = purchase.Country,
                                    Company = purchase.Company,
                                    UserID = purchase.UserID,
                                    CardCode = purchase.CardCode,
                                    CardName = purchase.CardName,
                                    PONO = purchase.PONO,
                                    PostingDate = purchase.PostingDate,
                                    CurrencyCode = purchase.CurrencyCode,
                                    DocStatus = purchase.DocStatus,
                                    DocTotal = purchase.DocTotal,
                                };
                                Purchases.Add(purchase_db);

                                foreach (var purchasedetail in poitem)
                                {
                                    var poitem_db = new DB_POItem()
                                    {
                                        PONO = purchase.PONO,
                                        ItemCode = purchasedetail.ItemCode,
                                        ItemName = purchasedetail.ItemName,
                                        UnitPrice = purchasedetail.UnitPrice,
                                        POOrder = purchasedetail.POOrder,
                                        Quantity = purchasedetail.Quantity,
                                        ETD = purchasedetail.ETD,
                                        ETA = purchasedetail.ETA,
                                        OpenQty = purchasedetail.OpenQty,
                                    };
                                    PoItems.Add(poitem_db);
                                }
                            }
                        }
                        AppDatabase.Instance.SqlConnection.InsertAll(PoMonth);
                        AppDatabase.Instance.SqlConnection.InsertAll(Purchases);
                        AppDatabase.Instance.SqlConnection.InsertAll(PoItems);
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

        public void SearchPurchaseOrder()
        {
            var _cache = new List<POMainModel>();
            if (string.IsNullOrEmpty(SearchTxt)) { PoMain = PoMainCache; }
            else
            {
                var numberList = new List<int>();
                var result = new List<PurchaseItem>();
                var monthHash = new Dictionary<string, int>();

                if (PoMainCache.Count > 0)
                {
                    for (int i = 0; i < PoMainCache.Count; i++)
                    {
                        if (PoMainCache[i].Data != null)
                        {
                            result = PoMainCache[i].Data.FindAll(item => item.CardName.ToLower().Contains(SearchTxt) ||
                                item.CardName.ToUpper().Contains(SearchTxt) || item.PONO.ToLower().Contains(SearchTxt));
                            if (result.Count > 0) numberList.Add(i);
                            if (monthHash.ContainsKey(PoMainCache[i].Date))
                            {
                                monthHash.Remove(PoMainCache[i].Date);
                                monthHash.Add(PoMainCache[i].Date, result.Count);
                            }
                            else monthHash.Add(PoMainCache[i].Date, result.Count);
                        }
                    }

                    foreach (var number in numberList)
                    {
                        var current = new POMainModel();
                        string date = PoMainCache[number].Date;
                        int count = monthHash[date];
                        current.Date = date;
                        current.Records = count + " record(s) found";
                        _cache.Add(current);
                    }
                    PoMain = _cache;
                }
            }
        }

        public async void GetDashboardData()
        {
            IsBusy = true; IsRefreshing = true;
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
