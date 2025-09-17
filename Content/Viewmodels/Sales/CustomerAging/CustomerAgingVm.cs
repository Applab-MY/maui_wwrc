using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.CustomerAgingModel;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Viewmodels.Sales.CustomerAging
{
    public class CustomerAgingVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        bool _isSearch = false;
        string _country = "";
        string _subsidiary = "";
        string _salesperson = "";
        string _totalOverdue = "";
        List<DB_CustAging> _custAgings = [];
        List<DB_CustAging> _custAgingList = [];
        List<DB_MonthsModel> _monthList = [];
        List<DB_DocListModel> _doclist = [];
        bool _nodata = false;
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
        public string TotalOverdue
        {
            get { return _totalOverdue; }
            set { SetProperty(ref _totalOverdue, value); }
        }
        public List<DB_CustAging> CustAgings
        {
            get { return _custAgings; }
            set
            {
                SetProperty(ref _custAgings, value);
                NoData = value.Count == 0;
            }
        }
        public List<DB_CustAging> CustAgingList
        {
            get { return _custAgingList; }
            set { SetProperty(ref _custAgingList, value); }
        }
        public List<DB_MonthsModel> MonthList
        {
            get { return _monthList; }
            set { SetProperty(ref _monthList, value); }
        }
        public List<DB_DocListModel> DocsList
        {
            get { return _doclist; }
            set { SetProperty(ref _doclist, value); }
        }
        public bool NoData
        {
            get { return _nodata; }
            set { SetProperty(ref _nodata, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public DashboardMainModel? DashboardData = null;
        public Action<bool>? OnFinishLoad = null;
        public List<SalesPersonList> SalesList = [];
        public bool isFilterSales = false;
        string currents = "";
        string overdays150s = "";

        public CustomerAgingVm()
        {
            IsBusy = false;
            Country = Preferences.Default.Get("country", "");
            Subsidiary = Preferences.Default.Get("subsidiary", "");
            SalesPerson = Preferences.Default.Get("userId", "");
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(GetCustomerAgingData);
        }

        public void GetPastMonth()
        {
            var today = DateTime.Today;
            var current = new DateTime(today.Year, today.Month, 1);
            currents = string.Format("{0:MMM yyyy}", current);

            var days30 = current.AddMonths(-1);
            var _day30 = string.Format("{0:MMM yyyy}", days30);

            var days60 = current.AddMonths(-2);
            var _day60 = string.Format("{0:MMM yyyy}", days60);

            var days90 = current.AddMonths(-3);
            var _day90 = string.Format("{0:MMM yyyy}", days90);

            var days120 = current.AddMonths(-4);
            var _day120 = string.Format("{0:MMM yyyy}", days120);

            var days150 = current.AddMonths(-5);
            var _day150 = string.Format("{0:MMM yyyy}", days150);

            var overdays150 = current.AddMonths(-6);
            overdays150s = string.Format("{0:MMM yyyy}", overdays150);
        }

        public async void GetCustomerAgingData()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    CustAgings = []; CustAgingList = []; MonthList = []; DocsList = [];
                    var model = new API_CustomerAging
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        UserId = isFilterSales ? SalesPerson : Preferences.Default.Get("userId", ""),
                    };
                    var _res = await App.AppClient.GetCustomerAging(model);
                    AppDatabase.Instance.SqlConnection.DeleteAll<DB_CustAging>();
                    AppDatabase.Instance.SqlConnection.DeleteAll<DB_MonthsModel>();
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
                        var _main = _res.items[0];
                        //MainCustomerAgingData = mainBody.Data;
                        //var totalOverdue = MainCustomerAgingData.Sum(a => Convert.ToDouble(a.UtilizedLimit));
                        //label_totaloverdue.Text = string.Format("{0:N2}", totalOverdue);
                        TotalOverdue = _main.TotalOverdue;

                        if (_main.Data != null && _main.Data.Count > 0)
                        {
                            foreach (var item in _main.Data)
                            {
                                var dbCustAging = new DB_CustAging()
                                {
                                    Id = item.Id,
                                    BPCurrency = item.BPCurrency,
                                    Country = item.Country,
                                    Company = item.Company,
                                    CardCode = item.CardCode,
                                    CardName = item.CardName,
                                    SlpCode = item.SlpCode,
                                    SlpName = item.SlpName,
                                    email = item.email,
                                    CreditLimit = item.CreditLimit,
                                    AvailableLimit = item.AvailableLimit,
                                    UtilizedLimit = item.UtilizedLimit,
                                    Terms = item.Terms,
                                    startDate = overdays150s,
                                    endDate = currents,
                                    ALERT = item.ALERT,
                                    Current = item.Current,
                                    Days30 = item.Days30,
                                    Days60 = item.Days60,
                                    Days90 = item.Days90,
                                    Days120 = item.Days120,
                                    Days150 = item.Days150,
                                    Over150Days = item.Over150Days,
                                    TotalOverdue = item.TotalOverdue,
                                };
                                if (item.ALERT == "0")
                                {
                                    dbCustAging.ALERT = "ic_status_grey";
                                    dbCustAging.status = "Outstanding Amount";
                                }
                                else
                                {
                                    dbCustAging.ALERT = "ic_status_red";
                                    dbCustAging.status = "Overdue Amount";
                                }
                                CustAgingList.Add(dbCustAging);
                            }
                            AppDatabase.Instance.SqlConnection.InsertAll(CustAgingList);
                            CustAgings = [.. CustAgingList.OrderByDescending(f => f.ALERT)];
                        }
                    }
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

        public void SearchCustomerAging(string? txt = null)
        {
            if (string.IsNullOrEmpty(txt)) CustAgings = CustAgingList;
            else
            {
                var result = CustAgingList.FindAll(item => item.Id.ToLower().Contains(txt) ||
                    item.CardName.ToLower().Contains(txt) || item.CardName.ToUpper().Contains(txt) ||
                    item.CardCode.ToLower().Contains(txt) || item.CardName.ToUpper().Contains(txt));
                CustAgings = result;
            }
            GetPastMonth();
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
