using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.CustomerAgingModel;

namespace wwrc_maui.Content.Viewmodels.Sales.CustomerAging
{
    public class CustomerAgingDetailVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        List<DB_MonthsModel> _mthlist = [];
        List<CustomerDetailDetailModel> _agingdetails = [];
        string _startdate = "";
        string _enddate = "";
        string _cardname = "";
        string _currency = "";
        string _term = "";
        string _utilized = "";
        string _credit = "";
        string _available = "";
        string _outstanding = "0.00";
        string _selectedMonth = "";
        bool _nodata = false;
        bool _nodataAging = false;
        #endregion
        #region properties
        public List<DB_MonthsModel> MonthList
        {
            get { return _mthlist; }
            set
            {
                SetProperty(ref _mthlist, value);
                NoData = value.Count == 0;
            }
        }
        public List<CustomerDetailDetailModel> AgingDetailList
        {
            get { return _agingdetails; }
            set
            {
                SetProperty(ref _agingdetails, value);
                NoDataAging = value.Count == 0;
            }
        }
        public string StartDate
        {
            get { return _startdate; }
            set { SetProperty(ref _startdate, value); }
        }
        public string EndDate
        {
            get { return _enddate; }
            set { SetProperty(ref _enddate, value); }
        }
        public string CardName
        {
            get { return _cardname; }
            set { SetProperty(ref _cardname, value); }
        }
        public string Currency
        {
            get { return _currency; }
            set { SetProperty(ref _currency, value); }
        }
        public string Term
        {
            get { return _term; }
            set { SetProperty(ref _term, value); }
        }
        public string UtilizedLimit
        {
            get { return _utilized; }
            set { SetProperty(ref _utilized, value); }
        }
        public string CreditLimit
        {
            get { return _credit; }
            set { SetProperty(ref _credit, value); }
        }
        public string AvailableLimit
        {
            get { return _available; }
            set { SetProperty(ref _available, value); }
        }
        public string TotalOutstanding
        {
            get { return _outstanding; }
            set { SetProperty(ref _outstanding, value); }
        }
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set { SetProperty(ref _selectedMonth, value); }
        }
        public bool NoData
        {
            get { return _nodata; }
            set { SetProperty(ref _nodata, value); }
        }
        public bool NoDataAging
        {
            get { return _nodataAging; }
            set { SetProperty(ref _nodataAging, value); }
        }
        #endregion
        #endregion

        public string agingId = "";
        public DB_MonthsModel? selectedAging = null;
        string currents = "";
        string _day30 = "";
        string _day60 = "";
        string _day90 = "";
        string _day120 = "";
        string _day150 = "";
        string overdays150s = "";

        public CustomerAgingDetailVm() { }

        public void GetPastMonth()
        {
            var today = DateTime.Today;
            var current = new DateTime(today.Year, today.Month, 1);
            currents = string.Format("{0:MMM yyyy}", current);

            var days30 = current.AddMonths(-1);
            _day30 = string.Format("{0:MMM yyyy}", days30);

            var days60 = current.AddMonths(-2);
            _day60 = string.Format("{0:MMM yyyy}", days60);

            var days90 = current.AddMonths(-3);
            _day90 = string.Format("{0:MMM yyyy}", days90);

            var days120 = current.AddMonths(-4);
            _day120 = string.Format("{0:MMM yyyy}", days120);

            var days150 = current.AddMonths(-5);
            _day150 = string.Format("{0:MMM yyyy}", days150);

            var overdays150 = current.AddMonths(-6);
            overdays150s = string.Format("{0:MMM yyyy}", overdays150);

            StartDate = overdays150s;
            EndDate = currents;
        }

        public async Task GetAgingDetails()
        {
            try
            {
                string _qAging = "SELECT * FROM DB_CustAging WHERE Id = '" + agingId + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<DB_CustAging>(_qAging);
                if (items.Count > 0)
                {
                    var _list = new List<DB_MonthsModel>();
                    CardName = items[0].CardName;
                    Currency = items[0].BPCurrency;
                    Term = items[0].Terms;
                    UtilizedLimit = string.Format("{0:N2}", items[0].UtilizedLimit);
                    CreditLimit = string.Format("{0:N2}", items[0].CreditLimit);
                    AvailableLimit = string.Format("{0:N2}", items[0].AvailableLimit);

                    var currentMonth = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = currents,
                        MonthString = "Current",
                        TotalOutstanding = items[0].Current
                    };
                    var day30Month = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = _day30,
                        MonthString = "30Days",
                        TotalOutstanding = items[0].Days30
                    };
                    var day60Month = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = _day60,
                        MonthString = "60Days",
                        TotalOutstanding = items[0].Days60
                    };
                    var day90Month = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = _day90,
                        MonthString = "90Days",
                        TotalOutstanding = items[0].Days90
                    };
                    var day120Month = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = _day120,
                        MonthString = "120Days",
                        TotalOutstanding = items[0].Days120
                    };
                    var day150Month = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = _day150,
                        MonthString = "150Days",
                        TotalOutstanding = items[0].Days150
                    };
                    var over150Month = new DB_MonthsModel()
                    {
                        AgingId = items[0].Id,
                        CardCode = items[0].CardCode,
                        CardName = items[0].CardName,
                        Month = "Over 6 months",
                        MonthString = "Over150Days",
                        TotalOutstanding = items[0].Over150Days
                    };

                    _list.Add(currentMonth);
                    _list.Add(day30Month);
                    _list.Add(day60Month);
                    _list.Add(day90Month);
                    _list.Add(day120Month);
                    _list.Add(day150Month);
                    _list.Add(over150Month);
                    MonthList = _list;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async Task GetAgingMonthDetail()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    AgingDetailList = [];
                    var model = new API_CustomerAging
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        CardCode = selectedAging != null ? selectedAging.CardCode : "",
                        Age = selectedAging != null ? selectedAging.MonthString : "",
                        UserId = Preferences.Default.Get("userId", ""),
                    };
                    var _res = await App.AppClient.GetCustomerAgingDetail(model);
                    AppDatabase.Instance.SqlConnection.DeleteAll<DB_DocListModel>();
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
                        if (_res.items[0] != null && _res.items[0].DocList != null &&
                            _res.items[0].DocList.Count > 0)
                        {
                            TotalOutstanding = string.Format("{0:N2}", _res.items[0].TotalOutstanding);
                            var _list = new List<CustomerDetailDetailModel>();
                            foreach (var docModel in _res.items[0].DocList)
                            {
                                var agDetailModel = new CustomerDetailDetailModel()
                                {
                                    CardCode = docModel.CardCode,
                                    Doc = docModel.DocNum,
                                    //Date = Convert.ToDateTime(docModel.DocDate).ToLocalTime().ToString("yyyy-MM-dd"),
                                    Date = docModel.DocDate.Substring(0, 10),
                                    Outstanding = string.Format("{0:N2}", docModel.Outstanding)
                                };
                                _list.Add(agDetailModel);
                            }
                            AgingDetailList = _list;
                        }
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
    }
}
