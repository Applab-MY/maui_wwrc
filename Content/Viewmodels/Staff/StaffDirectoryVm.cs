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
        List<StaffMainModel> _staffs = [];
        List<ObservableGroupCollection<string, StaffMainModel>> _groupstaffs = [];
        List<CountryMainModel> _countries = [];
        bool _nodata = false;
        bool _nocountry = false;
        private double _entryWidth = 0.0;
        #endregion
        #region props
        public bool IsSearchVisible
        {
            get { return _isSearch; }
            set { SetProperty(ref _isSearch, value); }
        }
        public List<StaffMainModel> StaffList
        {
            get { return _staffs; }
            set
            {
                SetProperty(ref _staffs, value);
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
        public List<CountryMainModel> Countries
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
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public List<string> tabList = [];
        public string selectedTab = "";

        public StaffDirectoryVm()
        {
            IsBusy = false;
            tabList = ["Office", "Others"];
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(RefreshContent);
        }

        public void RefreshContent()
        {
            if (selectedTab.Equals("Office")) GetStaffList();
            else if (selectedTab.Equals("Others")) GetCountryList();
        }

        public async void GetStaffList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    StaffList = []; GroupedStaffList = [];
                    var model = new API_StaffModel
                    { Country = Preferences.Default.Get("country", "") };
                    var _res = await App.AppClient.Staff(model);
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

                        StaffList = [.. _list.OrderBy(c => c.Name)];
                        //GroupedStaffList = [.. StaffList.OrderBy(p => p.Name).GroupBy(p => p.GetStaffGroup()).
                        //    Select(p => new ObservableGroupCollection<string, StaffMainModel>(p))];
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

        public async void GetCountryList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    Countries = [];
                    var _res = await App.AppClient.GetCountry();
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
                        var _list = new List<CountryMainModel>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<CountryMainModel>();
                        foreach (var data in _res.items)
                        {
                            if (string.IsNullOrEmpty(data.CountryImage))
                                data.CountryImage = "ic_calendar.png";

                            _list.Add(data);
                            var country_db = new CountryMainModel()
                            {
                                Id = data.Id,
                                CountryCode = data.CountryCode,
                                CountryName = data.CountryName,
                                CountryImage = data.CountryImage
                            };
                            AppDatabase.Instance.SqlConnection.Insert(country_db);
                        }
                        Countries = _list;
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
