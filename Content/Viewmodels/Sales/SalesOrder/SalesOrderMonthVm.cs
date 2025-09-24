using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.DashboardModel;
using static wwrc_maui.Content.Model.SOModel;

namespace wwrc_maui.Content.Viewmodels.Sales.SalesOrder
{
    public class SalesOrderMonthVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        bool _isSearch = false;
        string _country = "";
        string _subsidiary = "";
        string _salesperson = "";
        List<Db_SOList> _soMain = [];
        bool _nodata = false;
        string _searchTxt = "";
        private double _entryWidth = 0.0;
        string _pagetitle = "";
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
        public List<Db_SOList> SoMain
        {
            get { return _soMain; }
            set
            {
                SetProperty(ref _soMain, value);
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
        public string PageTitle
        {
            get { return _pagetitle; }
            set { SetProperty(ref _pagetitle, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public Command? SearchCommand { get; set; } = null;

        public DashboardMainModel? DashboardData = null;
        public List<Db_SOList> SoMainCache = [];
        public Action<bool>? OnFinishLoad = null;
        public List<SalesPersonList> SalesList = [];
        public bool isFilterSales = false;
        public string selectedDate = "";

        public SalesOrderMonthVm()
        {
            Country = Preferences.Default.Get("country", "");
            Subsidiary = Preferences.Default.Get("subsidiary", "");
            SalesPerson = Preferences.Default.Get("userId", "");
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(GetSalesOrderByMonth);
            SearchCommand = new Command(SearchSalesOrder);
        }

        public async void GetSalesOrderByMonth()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(500);
            try
            {
                SoMainCache = []; SoMain = [];
                string _query = "SELECT * FROM Db_SOList WHERE SalesDate = '" + selectedDate + "'";
                var items = AppDatabase.Instance.SqlConnection.Query<Db_SOList>(_query);
                if (items.Count > 0)
                {
                    foreach (var data in items)
                    {
                        string cardCode = "(" + data.CardCode + ")";
                        var dbData = new Db_SOList()
                        {
                            Id = data.Id,
                            SalesDate = selectedDate,
                            Country = data.Country,
                            Company = data.Company,
                            UserID = data.UserID,
                            CardCode = cardCode,
                            CardName = data.CardName,
                            SONO = data.SONO,
                            PostingDate = data.PostingDate,
                            DeliveryDate = data.DeliveryDate,
                            ApprovedDate = data.ApprovedDate,
                            DODate = data.DODate,
                            DocStatus = data.DocStatus,
                            DocTotal = data.DocTotal,
                            Currency = data.Currency,

                        };
                        SoMainCache.Add(dbData);
                    }
                    SoMain = SoMainCache;
                }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = false; IsRefreshing = false;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public void SearchSalesOrder()
        {
            if (string.IsNullOrEmpty(SearchTxt))
            { SoMain = SoMainCache; }
            else
            {
                var result = SoMainCache.FindAll(item => item.CardName.ToLower().Contains(SearchTxt)
                    || item.CardName.ToUpper().Contains(SearchTxt) || item.SONO.ToLower().Contains(SearchTxt)
                    || item.SONO.ToUpper().Contains(SearchTxt) || item.CardCode.ToUpper().Contains(SearchTxt)
                    || item.CardCode.ToLower().Contains(SearchTxt));
                SoMain = result;
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
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count > 0)
                    {
                        DashboardData = _res.items[0];
                        SetupSalesList();
                    }
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count == 0)
                    { } //bugfix :: sometimes api success but return null items
                    else await App.DisplayAlert("Error", "API error : " + _res.SystemCode.ToString()
                        + ", " + _res.SystemMessage + "\r" + _res.SystemDebugMessage, null, "Okay");
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
