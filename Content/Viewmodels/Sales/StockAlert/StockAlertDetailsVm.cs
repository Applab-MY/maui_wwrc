using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.Viewmodels.Sales.StockAlert
{
    public class StockAlertDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _itemCode = "";
        string _itemName = "";
        string _whsname = "";
        string _manufacture = "";
        string _itemGroup = "";
        string _totalOnhand = "";
        string _totalCommitted = "";
        string _totalOrder = "";
        string _totalAvailable = "";
        string _minLevel = "";
        string _lastPrhsPrice = "";
        string _lastPrhsCurrency = "";
        string _lastPrhsDate = "";
        string _lastSellPrice = "";
        string _lastSellCurrency = "";
        string _lastSellDate = "";
        string _committed = "";
        List<DB_WarehouseItem> _listDetails = [];
        List<DB_IsCommitedPW_Customer> _listCommitted = [];
        bool _nodata = false;
        bool _nodatacomited = false;
        #endregion
        #region props
        public string ItemCode
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
        }
        public string ItemName
        {
            get { return _itemName; }
            set { SetProperty(ref _itemName, value); }
        }
        public string WhsName
        {
            get { return _whsname; }
            set { SetProperty(ref _whsname, value); }
        }
        public string Manufacture
        {
            get { return _manufacture; }
            set { SetProperty(ref _manufacture, value); }
        }
        public string ItemGroup
        {
            get { return _itemGroup; }
            set { SetProperty(ref _itemGroup, value); }
        }
        public string TotalOnhand
        {
            get { return _totalOnhand; }
            set { SetProperty(ref _totalOnhand, value); }
        }
        public string TotalCommitted
        {
            get { return _totalCommitted; }
            set { SetProperty(ref _totalCommitted, value); }
        }
        public string TotalOrder
        {
            get { return _totalOrder; }
            set { SetProperty(ref _totalOrder, value); }
        }
        public string TotalAvailable
        {
            get { return _totalAvailable; }
            set { SetProperty(ref _totalAvailable, value); }
        }
        public string MinLevel
        {
            get { return _minLevel; }
            set { SetProperty(ref _minLevel, value); }
        }
        public string LastPrhsPrice
        {
            get { return _lastPrhsPrice; }
            set { SetProperty(ref _lastPrhsPrice, value); }
        }
        public string LastPrhsCurrency
        {
            get { return _lastPrhsCurrency; }
            set { SetProperty(ref _lastPrhsCurrency, value); }
        }
        public string LastPrhsDate
        {
            get { return _lastPrhsDate; }
            set { SetProperty(ref _lastPrhsDate, value); }
        }
        public string LastSellPrice
        {
            get { return _lastSellPrice; }
            set { SetProperty(ref _lastSellPrice, value); }
        }
        public string LastSellCurrency
        {
            get { return _lastSellCurrency; }
            set { SetProperty(ref _lastSellCurrency, value); }
        }
        public string LastSellDate
        {
            get { return _lastSellDate; }
            set { SetProperty(ref _lastSellDate, value); }
        }
        public List<DB_WarehouseItem> ListDetails
        {
            get { return _listDetails; }
            set
            {
                SetProperty(ref _listDetails, value);
                NoData = value.Count == 0;
            }
        }
        public List<DB_IsCommitedPW_Customer> ListCommitted
        {
            get { return _listCommitted; }
            set
            {
                SetProperty(ref _listCommitted, value);
                NoDataCommited = value.Count == 0;
            }
        }
        public string Committed
        {
            get { return _committed; }
            set { SetProperty(ref _committed, value); }
        }
        public bool NoData
        {
            get { return _nodata; }
            set { SetProperty(ref _nodata, value); }
        }
        public bool NoDataCommited
        {
            get { return _nodatacomited; }
            set { SetProperty(ref _nodatacomited, value); }
        }
        #endregion
        #endregion

        public string itemCode = "";
        public string whsName = "";

        public StockAlertDetailsVm() { }

        public async void GetStockDetails()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            try
            {
                string _qItem = "SELECT * FROM DB_StockAlert WHERE ItemCode = '" + itemCode + "'";
                var data = AppDatabase.Instance.SqlConnection.Query<DB_StockAlert>(_qItem);
                if (data.Count > 0)
                {
                    ItemCode = data[0].ItemCode;
                    ItemName = data[0].ItemName;
                    Manufacture = data[0].Manufacturer;
                    ItemGroup = data[0].ItmsGrpName;
                    TotalOnhand = data[0].OnHand.ToString();
                    TotalCommitted = data[0].IsCommited.ToString();
                    TotalOrder = data[0].OnOrder.ToString();
                    TotalAvailable = data[0].Available.ToString();
                    MinLevel = data[0].OnHand.ToString();
                    LastPrhsPrice = data[0].IsCommited.ToString();
                    LastPrhsCurrency = data[0].OnOrder.ToString();
                    LastPrhsDate = data[0].Available.ToString();
                    MinLevel = data[0].MinLevel.ToString();
                    LastPrhsPrice = data[0].LastPurchPrice;
                    LastPrhsCurrency = data[0].LastPurchCurr;
                    LastPrhsDate = data[0].LastPurchDate;
                    LastSellPrice = data[0].LastSalesPrice;
                    LastSellCurrency = data[0].LastSalesCur;
                    LastSellDate = data[0].LastSalesDate;
                }

                ListDetails = [];
                string query = "SELECT * FROM DB_WarehouseItem WHERE ItemCode = '" + itemCode + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<DB_WarehouseItem>(query);
                if (items.Count > 0)
                {
                    var _list = new List<DB_WarehouseItem>();
                    foreach (var _item in items)
                    {
                        _item.TotalCommited = double.Parse(_item.TotalCommited).ToString();
                        _list.Add(_item);
                    }
                    ListDetails = _list;
                }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = false; IsRefreshing = false;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
            IsBusy = false; IsRefreshing = false;
        }

        public async void GetCommittedPw()
        {
            IsBusy = true; IsRefreshing = true;
            try
            {
                string _cmtd = "SELECT * FROM DB_IsCommitedPW WHERE ItemCode = '" + itemCode
                    + "' AND Warehouse = '" + whsName + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<DB_WarehouseItem>(_cmtd);
                if (items.Count > 0) { WhsName = items[0].Warehouse; }

                ListCommitted = [];
                string _qCmtd = "SELECT * FROM DB_IsCommitedPW WHERE ItemCode = '" + itemCode
                    + "' AND Warehouse = '" + whsName + "'";
                var data = AppDatabase.Instance.SqlConnection.Query<DB_IsCommitedPW>(_qCmtd);
                if (data.Count > 0) { Committed = data[0].TotalCommited; }

                string _qCmtDetail = "SELECT * FROM DB_IsCommitedPW_Customer WHERE ItemCode = '"
                    + itemCode + "' AND Warehouse = '" + whsName + "'";
                var details = AppDatabase.Instance.SqlConnection.Query<DB_IsCommitedPW_Customer>(_qCmtDetail);
                if (details.Count > 0)
                {
                    var _list = new List<DB_IsCommitedPW_Customer>();
                    foreach (var item in details)
                    {
                        if (item.Commited != null)
                        { item.Commited = double.Parse(item.Commited).ToString(); }
                        if (item.TotalCommited != null)
                        { item.TotalCommited = double.Parse(item.TotalCommited).ToString(); }
                        _list.Add(item);
                    }
                    ListCommitted = _list;
                }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = false; IsRefreshing = false;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
            IsBusy = false; IsRefreshing = false;
        }
    }
}
