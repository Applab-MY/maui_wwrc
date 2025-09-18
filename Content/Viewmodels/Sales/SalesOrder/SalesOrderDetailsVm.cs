using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.SOModel;

namespace wwrc_maui.Content.Viewmodels.Sales.SalesOrder
{
    public class SalesOrderDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _sono = "";
        string _cardname = "";
        string _cardcode = "";
        string _postingdate = "";
        string _dopostingdate = "";
        string _deliveryDate = "";
        string _doCloseDate = "";
        string _doReturnDate = "";
        string _totalDoc = "0.00";
        string _currency = "";
        string _status = "";
        string _doStatus = "";
        List<Db_SOItemList> _soitem = [];
        List<Db_DOList> _doList = [];
        List<Db_DOItemsList> _doItems = [];
        bool _noitems = false;
        bool _nodelivery = false;
        bool _nodeliveryitem = false;
        double _btomiconsize = 0.0;
        #endregion
        #region props
        public string SoNo
        {
            get { return _sono; }
            set { SetProperty(ref _sono, value); }
        }
        public string CardName
        {
            get { return _cardname; }
            set { SetProperty(ref _cardname, value); }
        }
        public string CardCode
        {
            get { return _cardcode; }
            set { SetProperty(ref _cardcode, value); }
        }
        public string PostingDate
        {
            get { return _postingdate; }
            set { SetProperty(ref _postingdate, value); }
        }
        public string DoPostingDate
        {
            get { return _dopostingdate; }
            set { SetProperty(ref _dopostingdate, value); }
        }
        public string DeliveryDate
        {
            get { return _deliveryDate; }
            set { SetProperty(ref _deliveryDate, value); }
        }
        public string DoCloseDate
        {
            get { return _doCloseDate; }
            set { SetProperty(ref _doCloseDate, value); }
        }
        public string DoReturnDate
        {
            get { return _doReturnDate; }
            set { SetProperty(ref _doReturnDate, value); }
        }
        public string TotalDoc
        {
            get { return _totalDoc; }
            set { SetProperty(ref _totalDoc, value); }
        }
        public string Currency
        {
            get { return _currency; }
            set { SetProperty(ref _currency, value); }
        }
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        public string DoStatus
        {
            get { return _doStatus; }
            set { SetProperty(ref _doStatus, value); }
        }
        public List<Db_SOItemList> SoItems
        {
            get { return _soitem; }
            set
            {
                SetProperty(ref _soitem, value);
                NoItems = value.Count == 0;
            }
        }
        public List<Db_DOList> DoList
        {
            get { return _doList; }
            set
            {
                SetProperty(ref _doList, value);
                NoDelivery = value.Count == 0;
            }
        }
        public List<Db_DOItemsList> DoItems
        {
            get { return _doItems; }
            set
            {
                SetProperty(ref _doItems, value);
                NoDeliveryItem = value.Count == 0;
            }
        }
        public bool NoItems
        {
            get { return _noitems; }
            set { SetProperty(ref _noitems, value); }
        }
        public bool NoDelivery
        {
            get { return _nodelivery; }
            set { SetProperty(ref _nodelivery, value); }
        }
        public bool NoDeliveryItem
        {
            get { return _nodeliveryitem; }
            set { SetProperty(ref _nodeliveryitem, value); }
        }
        public double BtomIconSize
        {
            get { return _btomiconsize; }
            set { SetProperty(ref _btomiconsize, value); }
        }
        #endregion
        #endregion

        public Db_SOList? soData = null;
        public Db_DOList? doData = null;
        List<Db_DOList> doListCache = [];

        public SalesOrderDetailsVm()
        { BtomIconSize = Math.Floor((App.ScreenWidth - 110)/3); }

        public void SetSOData()
        {
            if (soData != null)
            {
                var charsToRemove = new string[] { "(", ")" };
                foreach (var c in charsToRemove)
                    soData.CardCode = soData.CardCode.Replace(c, string.Empty);

                SoNo = soData.SONO;
                CardName = soData.CardName;
                CardCode = soData.CardCode;
                PostingDate = soData.PostingDate;
                Currency = soData.Currency;
                Status = soData.DocStatus;

                if (soData.DocStatus == "C") Status = "Closed";
                else Status = "Open";

                //SOItem
                string query = "SELECT * FROM Db_SOItemList WHERE Id = '" + soData.SONO + "'";
                SoItems = AppDatabase.Instance.SqlConnection.Query<Db_SOItemList>(query);
            }
        }

        public void SetDOData()
        {
            if (doData != null)
            {
                string query = "SELECT * FROM Db_DOItemsList WHERE Id = '" + doData.DONO + "'";
                DoItems = AppDatabase.Instance.SqlConnection.Query<Db_DOItemsList>(query);

                CardName = doData.CardName;
                CardCode = doData.CardCode;

                DoPostingDate = doData.DODate;
                if (!string.IsNullOrEmpty(doData.DODate))
                {
                    string date = doData.DODate[..10];
                    if (date.Equals("0001-01-01")) DoPostingDate = "";
                }
                DeliveryDate = doData.DeliveryDate;
                if (!string.IsNullOrEmpty(doData.DeliveryDate))
                {
                    string date = doData.DeliveryDate[..10];
                    if (date.Equals("0001-01-01")) DeliveryDate = "";
                }
                DoCloseDate = doData.CloseDate;
                if (!string.IsNullOrEmpty(doData.CloseDate))
                {
                    string date = doData.CloseDate[..10];
                    if (date.Equals("0001-01-01")) DoCloseDate = "";
                }
                DoReturnDate = doData.ReturnDate;
                if (!string.IsNullOrEmpty(doData.ReturnDate))
                {
                    string date = doData.ReturnDate[..10];
                    if (date.Equals("0001-01-01")) DoReturnDate = "";
                }

                if (doData.DoStatus == "D") DoStatus = "Out from Warehouse";
                else if (doData.DoStatus == "R") DoStatus = "Return";
                else if (doData.DoStatus == "WD") DoStatus = "Waiting Delivery";
            }
        }

        public async void GetSalesOrderById()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    DoList = []; doListCache = [];
                    var model = new API_DObySO
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        DocNum = soData != null ? soData.SONO : "",
                    };
                    var _res = await App.AppClient.GetDOBySalesOrder(model);
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<Db_DOItemsList>();
                        foreach (var item in _res.items)
                        {
                            var dbModel = new Db_DOList()
                            {
                                Id = item.Id,
                                DOId = item.Id,
                                Country = item.Country,
                                Company = item.Company,
                                CardCode = item.CardCode,
                                CardName = item.CardName,
                                DONO = item.DONO,
                                DocNum = item.DocNum,
                                DocEntry = item.DocEntry,
                                DODate = item.DODate,
                                DeliveryDate = item.DeliveryDate,
                                ReturnDate = item.ReturnDate,
                                CloseDate = item.CloseDate,
                                DoStatus = item.DoStatus,
                                Currency = item.Currency,
                            };

                            //TODO Add all image here with add condition
                            if (item.DoStatus == "D") dbModel.ImageDotSource = "ic_status_lightgreen";
                            else if (item.DoStatus == "R") dbModel.ImageDotSource = "ic_status_blue";
                            else if (item.DoStatus == "WD") dbModel.ImageDotSource = "ic_status_yellow";
                            if (dbModel.DODate != "") dbModel.DODate = dbModel.DODate.Substring(0, 10);
                            AppDatabase.Instance.SqlConnection.Insert(dbModel);
                            doListCache.Add(dbModel);

                            foreach (var _item in item.Items)
                            {
                                var doItem = new Db_DOItemsList()
                                {
                                    Id = item.DONO,
                                    //SOId = DbDataList.SONO,
                                    ItemCode = _item.ItemCode,
                                    ItemName = _item.ItemName,
                                    Quantity = _item.Quantity
                                };
                                AppDatabase.Instance.SqlConnection.Insert(doItem);
                            }
                        }
                        DoList = doListCache;
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
    }
}
