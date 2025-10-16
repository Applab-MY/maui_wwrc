using System.Collections.ObjectModel;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.DashboardModel;
using static wwrc_maui.Content.Model.SalesModel;
using Application = Microsoft.Maui.Controls.Application;

namespace wwrc_maui.Content.Viewmodels.Dashboard
{
    public class GraphDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        DashboardMainModel _dashboardData = new();
        bool _isSearch = false;
        string _country = "";
        string _subsidiary = "";
        string _salesperson = "";
        ObservableCollection<SalesPersonList> _allSalesPerson = [];
        ObservableCollection<SalesMainModel> _salesList = [];
        bool _nodata = false;
        bool _nosalesperson = false;
        string _searchTxt = "";
        private double _entryWidth = 0.0;
        #endregion
        #region props
        public DashboardMainModel DashboardData
        {
            get { return _dashboardData; }
            set { SetProperty(ref _dashboardData, value); }
        }
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
        public ObservableCollection<SalesPersonList> AllSalesPerson
        {
            get { return _allSalesPerson; }
            set
            {
                NoSalesPerson = value.Count == 0;
                SetProperty(ref _allSalesPerson, value);
            }
        }
        public ObservableCollection<SalesMainModel> SalesList
        {
            get { return _salesList; }
            set
            {
                SetProperty(ref _salesList, value);
                NoData = value.Count == 0;
            }
        }
        public bool NoData
        {
            get { return _nodata; }
            set { SetProperty(ref _nodata, value); }
        }
        public bool NoSalesPerson
        {
            get { return _nosalesperson; }
            set { SetProperty(ref _nosalesperson, value); }
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

        List<SalesMainModel> salesCache = [];
        public List<SalesPersonList> filterPerson = [];
        public bool isFilterSales = false;
        //for sales person list
        public int MaxListSize = 15;
        public int IndexAt = 0;
        public bool IsFinish = false;

        public GraphDetailsVm()
        {
            Country = Preferences.Default.Get("country", "");
            Subsidiary = Preferences.Default.Get("subsidiary", "");
            SalesPerson = Preferences.Default.Get("userId", "");
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(Initialize);
            SearchCommand = new Command(SearchSales);
        }

        public async void Initialize()
        {
            IsBusy = true; IsRefreshing = true;
            await Task.Delay(300);
            GetDashboardData();
            GetSalesResultData();
            IsBusy = false; IsRefreshing = false;
        }

        public async void GetSalesResultData()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    SalesList = []; salesCache = [];
                    var model = new API_DashBoard
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        UserId = isFilterSales ? SalesPerson : Preferences.Default.Get("userId", ""),
                    };
                    var _res = await App.AppClient.GetSalesResult(model);
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
                        salesCache = [.. _res.items];
                        SalesList = _res.items;
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

        public void SearchSales()
        {
            if (string.IsNullOrEmpty(SearchTxt))
                SalesList = new ObservableCollection<SalesMainModel>(salesCache);
            else
            {
                var result = salesCache.FindAll(item => item.CARDCODE.ToLower().Contains(SearchTxt)
                    || item.CARDNAME.ToLower().Contains(SearchTxt));
                SalesList = new ObservableCollection<SalesMainModel>(result);
            }
        }

        public async void GetDashboardData()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    DashboardData = new();
                    var model = new API_DashBoard
                    {
                        Country = Preferences.Default.Get("country", ""),
                        Company = Preferences.Default.Get("subsidiary", ""),
                        UserId = isFilterSales ? SalesPerson : Preferences.Default.Get("userId", ""),
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
            filterPerson = []; AllSalesPerson = [];
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
                            {
                                if (sp.Id.Equals(SalesPerson)) { sp.Checked = true; }
                                else { sp.Checked = false; }
                            }
                            filterPerson = [.. _subs.SalesPersonList];

                            if (filterPerson.Count > MaxListSize)
                            {
                                IndexAt = MaxListSize;
                                AllSalesPerson = new ObservableCollection<SalesPersonList>(filterPerson.GetRange(0, MaxListSize));
                            }
                            else AllSalesPerson = new ObservableCollection<SalesPersonList>(filterPerson);
                        }
                    }
                }
            }
        }
    }
}
