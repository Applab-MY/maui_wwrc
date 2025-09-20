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
        string _manufacture = "";
        string _itemGroup = "";
        string _minLevel = "";
        string _lastPrhsPrice = "";
        string _lastPrhsCurrency = "";
        string _lastPrhsDate = "";
        string _lastSellPrice = "";
        string _lastSellCurrency = "";
        string _lastSellDate = "";
        List<DB_WarehouseItem> _listDetails = [];
        bool _nodata = false;
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
        public bool NoData
        {
            get { return _nodata; }
            set { SetProperty(ref _nodata, value); }
        }
        #endregion
        #endregion

        public string itemCode = "";

        public StockAlertDetailsVm() { }

        public async void GetStockDetails()
        {
            IsBusy = true; IsRefreshing = true;
            try
            {
                string _qItem = "SELECT * FROM DB_StockAlert WHERE ItemCode = '" + ItemCode + "'";
                var data = AppDatabase.Instance.SqlConnection.Query<DB_StockAlert>(_qItem);
                if (data.Count > 0)
                {
                    ItemCode = data[0].ItemCode;
                    ItemName = data[0].ItemName;
                    Manufacture = data[0].Manufacturer;
                    ItemGroup = data[0].ItmsGrpName;

                    //MinLevel = data[0].OnHand.ToString();
                    //LastPrhsPrice = data[0].IsCommited.ToString();
                    //LastPrhsCurrency = data[0].OnOrder.ToString();
                    //LastPrhsDate = data[0].Available.ToString();

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
                    foreach (var _item in items)
                    {
                        _item.TotalCommited = double.Parse(_item.TotalCommited).ToString();
                        ListDetails.Add(_item);
                    }
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
