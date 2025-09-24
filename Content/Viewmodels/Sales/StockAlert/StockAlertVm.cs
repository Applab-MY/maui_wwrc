using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.Viewmodels.Sales.StockAlert
{
    public class StockAlertVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        bool _isSearch = false;
        List<DB_StockAlert> _stocklist = [];
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
        public List<DB_StockAlert> StockList
        {
            get { return _stocklist; }
            set
            {
                SetProperty(ref _stocklist, value);
                NoData = value.Count == 0;
            }
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

        List<DB_StockAlert> stockListCache = [];
        List<DB_IsCommitedPW> committedList = [];
        List<DB_IsCommitedPW_Customer> committedCustomer = [];
        List<DB_WarehouseItem> whsItems = [];

        public StockAlertVm()
        {
            IsBusy = false;
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(GetStockAlertList);
            SearchCommand = new Command(SearchStockList);
        }

        public async void GetStockAlertList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    StockList = []; stockListCache = [];
                    committedList = []; committedCustomer = [];
                    var model = new API_StockAlert
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                    };
                    var _res = await App.AppClient.GetStockAlert(model);
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_StockAlert>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_WarehouseItem>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_IsCommitedPW>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_IsCommitedPW_Customer>();
                        foreach (var data in _res.items)
                        {
                            if (data.ALERT == "0") data.ALERT = "ic_status_grey";
                            else data.ALERT = "ic_status_red";
                            List<WarehouseItem> wareHouseList = data.Warehouse;

                            var stockalert_db = new DB_StockAlert()
                            {
                                Country = data.Country,
                                Company = data.Company,
                                Buyer = data.Buyer,
                                ItemCode = data.ItemCode,
                                ItemName = data.ItemName,
                                ItmsGrpName = data.ItmsGrpName,
                                OnHand = data.OnHand,
                                IsCommited = data.IsCommited,
                                OnOrder = data.OnOrder,
                                Available = data.Available,
                                MinLevel = data.MinLevel,
                                ItemCost = data.ItemCost,
                                LastPurchPrice = data.LastPurchPrice,
                                LastPurchCurr = data.LastPurchCurr,
                                LastPurchDate = data.LastPurchDate.Substring(0, 10),
                                LastSalesCur = data.LastSalesCur,
                                LastSalesDate = data.LastSalesDate.Substring(0, 10),
                                LastSalesPrice = data.LastSalesPrice,
                                Manufacturer = data.Manufacturer,
                                ALERT = data.ALERT,
                            };
                            stockListCache.Add(stockalert_db);

                            // Inside Warehouse -> Committed -> Committed Detail
                            foreach (var warehouse in wareHouseList)
                            {
                                if (!string.IsNullOrWhiteSpace(warehouse.Warehouse))
                                {
                                    // Committed pass ItemCode
                                    var isCommittedPw = new IsCommitedPW();
                                    isCommittedPw = warehouse.IsCommitedPW;

                                    List<CustomerItem> customerlist = isCommittedPw.Customer;
                                    var commited_db = new DB_IsCommitedPW()
                                    {
                                        ItemCode = warehouse.ItemCode,
                                        TotalCommited = isCommittedPw.TotalCommited,
                                        Warehouse = warehouse.Warehouse,
                                        OnHandPW = warehouse.OnHandPW,
                                        OnOrderPW = warehouse.OnOrderPW,
                                        AvailablePW = warehouse.AvailablePW,
                                        ItemCostPW = warehouse.ItemCostPW,
                                    };
                                    committedList.Add(commited_db);

                                    foreach (var customer in customerlist)
                                    {
                                        var isCommittedCustomer = new DB_IsCommitedPW_Customer()
                                        {
                                            CardCode = customer.CardCode,
                                            CardName = customer.CardName,
                                            Commited = customer.Commited,
                                            TotalCommited = isCommittedPw.TotalCommited,
                                            ItemCode = warehouse.ItemCode,
                                            Warehouse = warehouse.Warehouse
                                        };
                                        committedCustomer.Add(isCommittedCustomer);
                                    }

                                    var warehouse_db = new DB_WarehouseItem()
                                    {
                                        ItemCode = warehouse.ItemCode,
                                        Warehouse = warehouse.Warehouse,
                                        OnHandPW = warehouse.OnHandPW,
                                        OnOrderPW = warehouse.OnOrderPW,
                                        AvailablePW = warehouse.AvailablePW,
                                        ItemCostPW = warehouse.ItemCostPW,
                                        TotalCommited = isCommittedPw.TotalCommited,
                                    };
                                    whsItems.Add(warehouse_db);
                                }
                            }
                        }
                        AppDatabase.Instance.SqlConnection.InsertAll(stockListCache);
                        AppDatabase.Instance.SqlConnection.InsertAll(committedList);
                        AppDatabase.Instance.SqlConnection.InsertAll(committedCustomer);
                        AppDatabase.Instance.SqlConnection.InsertAll(whsItems);
                        StockList = [.. stockListCache.OrderByDescending(f => f.ALERT)];
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
            IsBusy = false; IsRefreshing = false;
        }

        public void SearchStockList()
        {
            if (string.IsNullOrEmpty(SearchTxt))
            { StockList = stockListCache; }
            else
            {
                StockList = stockListCache.FindAll(item => item.ItemCode.ToLower().Contains(SearchTxt) ||
                    item.ItemCode.ToUpper().Contains(SearchTxt) || item.ItemName.ToLower().Contains(SearchTxt) ||
                    item.ItemName.ToUpper().Contains(SearchTxt));
            }
        }
    }
}
