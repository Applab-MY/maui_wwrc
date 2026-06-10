using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.POModel;

namespace wwrc_maui.Content.Viewmodels.Sales.PurchaseOrder
{
    public class PurchaseOrderDetailVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _pono = "";
        string _cardname = "";
        string _cardcode = "";
        string _postingdate = "";
        string _totalDoc = "0.00";
        string _currency = "";
        string _status = "";
        List<POItem> _poItems = [];
        bool _nodata = false;
        #endregion
        #region props
        public string PoNo
        {
            get { return _pono; }
            set { SetProperty(ref _pono, value); }
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
        public List<POItem> PoItems
        {
            get { return _poItems; }
            set
            {
                SetProperty(ref _poItems, value);
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

        public string poNo = "";
        List<POItem> poItemsCache = [];

        public PurchaseOrderDetailVm() { }

        public async Task GetPurchaseOrderDetails()
        {
            try
            {
                string _qPurchase = "SELECT * FROM DB_Purchase WHERE PONO ='" + poNo + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<DB_Purchase>(_qPurchase);
                if (items.Count > 0)
                {
                    PoNo = items[0].PONO;
                    CardName = items[0].CardName;
                    CardCode = items[0].CardCode;
                    PostingDate = items[0].PostingDate;
                    TotalDoc = string.Format("{0:N2}", items[0].DocTotal);
                    if (!string.IsNullOrEmpty(items[0].CurrencyCode))
                        Currency = items[0].CurrencyCode;
                    if (items[0].DocStatus == "O") Status = "Open";
                    else Status = "Close";
                }

                string _qDetail = "SELECT * FROM DB_POItem WHERE PONO ='" + poNo + "'";
                var purchasedetail = AppDatabase.Instance.SqlConnection.Query<POItem>(_qDetail);
                if (purchasedetail.Count > 0)
                {
                    //foreach (var data in purchasedetail)
                    //{ poItemsCache.Add(data); }
                    for (int a=0; a<10; a++) //for demo
                    {
                        poItemsCache.Add(new POItem
                        {
                            ItemCode = Faker.Identification.BulgarianPin(),
                            ItemName = Faker.Identification.UkNhsNumber(),
                            UnitPrice = Faker.RandomNumber.Next(1000).ToString(),
                            POOrder = Faker.RandomNumber.Next(1000).ToString(),
                            Quantity = Faker.RandomNumber.Next(1000).ToString(),
                            ETD = Faker.RandomNumber.Next(1000).ToString(),
                            ETA = Faker.RandomNumber.Next(1000).ToString(),
                            OpenQty = Faker.RandomNumber.Next(1000).ToString(),
                        });
                    }

                    //PoItems = []; //for demo
                    PoItems = poItemsCache;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }
    }
}