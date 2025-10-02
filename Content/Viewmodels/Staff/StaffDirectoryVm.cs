using System.Collections.ObjectModel;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.CountryModel;
using static wwrc_maui.Content.Model.StaffModel;
using Application = Microsoft.Maui.Controls.Application;

namespace wwrc_maui.Content.Viewmodels.Staff
{
    public class StaffDirectoryVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        bool _isSearch = false;
        bool _isSelectable = false;
        bool _showClear = false;
        int _selectedCount = 0;
        ObservableCollection<StaffMainModel> _stafflist = [];
        List<ObservableGroupCollection<string, StaffMainModel>> _groupstaffs = [];
        ObservableCollection<CountryMainModel> _countries = [];
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
        public bool IsSelectable
        {
            get { return _isSelectable; }
            set { SetProperty(ref _isSelectable, value); }
        }
        public bool ShowClearSelectable
        {
            get { return _showClear; }
            set { SetProperty(ref _showClear, value); }
        }
        public int SelectableCount
        {
            get { return _selectedCount; }
            set { SetProperty(ref _selectedCount, value); }
        }
        public ObservableCollection<StaffMainModel> StaffList
        {
            get { return _stafflist; }
            set
            {
                SetProperty(ref _stafflist, value);
                NoData = value.Count == 0;
            }
        }
        public List<ObservableGroupCollection<string, StaffMainModel>> GroupedStaffList
        {
            get { return _groupstaffs; }
            set
            {
                SetProperty(ref _groupstaffs, value);
                NoData = value.Count == 0;
            }
        }
        public ObservableCollection<CountryMainModel> Countries
        {
            get { return _countries; }
            set
            {
                SetProperty(ref _countries, value);
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

        public List<string> tabList = [];
        public IList<StaffMainModel> allStaffCache = [];
        public ObservableCollection<StaffMainModel> lastState = []; //for pagination
        public List<string> selectedStaff = [];
        public List<CountryMainModel> allCountryCache = [];
        public string selectedTab = "";
        public bool isOfficeLoad = false;
        public bool isOtherLoad = false;
        public int pageIndex = 0;
        public int pageTotal = 0;
        public int pageSize = 20;

        public StaffDirectoryVm()
        {
            IsBusy = false;
            tabList = ["Office", "Others"];
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(RefreshContent);
            SearchCommand = new Command(DoSearch);
        }

        public void RefreshContent()
        {
            if (selectedTab.Equals("Office")) GetOfficeStaffList();
            else if (selectedTab.Equals("Others")) GetCountryList();
        }

        #region get from API
        public async void GetOfficeStaffList()
        {
            isOfficeLoad = false;
            IsBusy = true; IsRefreshing = true;
            pageIndex = 0; pageTotal = 0;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    GroupedStaffList = []; StaffList = []; allStaffCache = []; lastState = [];
                    var model = new API_StaffModel { Country = Preferences.Default.Get("country", "") };
                    var _res = await App.AppClient.Staff(model);
                    //_res.items = []; //for demo
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
                        var _list = new List<StaffMainModel>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_StaffModel>();
                        foreach (var data in _res.items)
                        {
                            if (string.IsNullOrEmpty(data.ProfileImage)) data.ProfileImage = "ic_user_empty.png";
                            _list.Add(data);

                            var staff_db = new DB_StaffModel()
                            {
                                Image = data.Image,
                                Id = data.Id,
                                AccessCode = data.AccessCode,
                                UserID = data.UserID,
                                Email = data.Email,
                                EmployeeNo = data.EmployeeNo,
                                Name = data.Name,
                                Gender = data.Gender,
                                DOB = data.DOB,
                                DefaultBranchId = data.DefaultBranchName,
                                DefaultBranchName = data.DefaultBranchName,
                                CountryId = data.CountryId,
                                CountryCode = data.CountryCode,
                                CountryName = data.CountryName,
                                Department = data.Department,
                                Position = data.Position,
                                HODId = data.HODId,
                                HODName = data.HODName,
                                ContactNo = data.ContactNo,
                                OfficeNo = data.OfficeNo,
                                ProfileImage = data.ProfileImage,
                                DefaultSalesTeamId = data.DefaultSalesTeamId,
                                DefaultSalesTeamName = data.DefaultSalesTeamName,
                                IsOfficeCredential = data.IsOfficeCredential
                            };
                            AppDatabase.Instance.SqlConnection.Insert(staff_db);
                        }

                        allStaffCache = _list;
                        pageTotal = _list.Count / pageSize;
                        if (pageTotal > 0)
                        {
                            var _cache = new List<StaffMainModel>();
                            for (int a = 0; a < pageSize; a++) _cache.Add(_list[a]);
                            StaffList = new ObservableCollection<StaffMainModel>(_cache);
                        }
                        else StaffList = new ObservableCollection<StaffMainModel>(_list);
                        //GroupedStaffList = [.. allStaffCache.OrderBy(p => p.Name).GroupBy(p => p.GetStaffGroup())
                        //    .Select(p => new ObservableGroupCollection<string, StaffMainModel>(p))];
                        lastState = StaffList;
                        isOfficeLoad = true;
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

        public async void GetStaffListNextPage()
        {
            if (allStaffCache.Count > 0 && !IsSearchVisible)
            {
                IsBusy = true; IsRefreshing = true;
                await Task.Delay(300);
                pageIndex++;
                var _start = pageIndex * pageSize;
                var _end = _start + pageSize;
                for (int a = _start; a < _end; a++)
                {
                    if (a < allStaffCache.Count)
                        StaffList.Add(allStaffCache[a]);
                    else break;
                }
                lastState = StaffList;
                IsBusy = false; IsRefreshing = false;
            }
        }

        public async void GetCountryList()
        {
            isOtherLoad = false;
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    Countries = []; allCountryCache = [];
                    var _res = await App.AppClient.GetCountry();
                    //_res.items = []; //for demo
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<CountryMainModel>();
                        foreach (var data in _res.items)
                        {
                            if (string.IsNullOrEmpty(data.CountryImage))
                                data.CountryImage = "ic_calendar.png";

                            allCountryCache.Add(data);
                            var country_db = new CountryMainModel()
                            {
                                Id = data.Id,
                                CountryCode = data.CountryCode,
                                CountryName = data.CountryName,
                                CountryImage = data.CountryImage
                            };
                            AppDatabase.Instance.SqlConnection.Insert(country_db);
                        }
                        Countries = new ObservableCollection<CountryMainModel>(allCountryCache);
                        isOtherLoad = true;
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
        #endregion

        #region select staff
        public void CheckForSelected()
        {
            var query = "Select * From ChooseStaff";
            var _list = AppDatabase.Instance.SqlConnection.Query<ChooseStaff>(query).ToList();
            SelectableCount = _list.Count;
            ShowClearSelectable = _list.Count > 0;
        }

        public async Task<bool> SaveSelectedStaffToDb()
        {
            if (selectedStaff.Count > 0 && allStaffCache.Count > 0)
            {
                try
                {
                    var _list = new List<ChooseStaff>();
                    foreach (var item in selectedStaff)
                    {
                        var found = allStaffCache.Where(x => x.Id.Equals(item)).FirstOrDefault();
                        if (found != null)
                        {
                            var model = new ChooseStaff { Id = item, Name = found.Name };
                            _list.Add(model);
                        }
                    }

                    foreach (var item in _list)
                    {
                        var query = "Select * From ChooseStaff Where Id='" + item.Id + "'";
                        var _res = AppDatabase.Instance.SqlConnection.Query<ChooseStaff>(query).FirstOrDefault();
                        if (_res == null) AppDatabase.Instance.SqlConnection.Insert(item);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                    return false;
                }
            }
            else return false;
        }

        public void ClearSelectedStaffDb() { AppDatabase.Instance.SqlConnection.DeleteAll<ChooseStaff>(); }
        #endregion

        #region search
        public void DoSearch()
        {
            if (selectedTab.Equals(tabList[0])) SearchStaff();
            else if (selectedTab.Equals(tabList[1])) SearchCountry();
        }

        public void SearchStaff()
        {
            if (string.IsNullOrEmpty(SearchTxt)) { StaffList = lastState; }
            else
            {
                var result = allStaffCache.ToList().FindAll(item => item.Id.ToLower().Contains(SearchTxt)
                        || item.Name.ToLower().Contains(SearchTxt));
                StaffList = new ObservableCollection<StaffMainModel>(result);
            }
        }

        public void SearchCountry()
        {
            if (string.IsNullOrEmpty(SearchTxt))
            { Countries = new ObservableCollection<CountryMainModel>(allCountryCache); }
            else
            {
                var result = allCountryCache.ToList().FindAll(item => item.CountryCode.ToLower().Contains(SearchTxt)
                        || item.CountryName.ToLower().Contains(SearchTxt));
                Countries = new ObservableCollection<CountryMainModel>(result);
            }
        }
        #endregion
    }
}