using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.CountryModel;
using static wwrc_maui.Content.Model.StaffModel;
using Application = Microsoft.Maui.Controls.Application;

namespace wwrc_maui.Content.Viewmodels.Staff
{
    public class StaffDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        CountryMainModel? _selectedCountry = null;
        bool _isSelectable = false;
        bool _showClear = false;
        int _selectedCount = 0;
        string _primary = "";
        bool _isSearch = false;
        string _picture = "";
        string _name = "";
        string _position = "";
        string _empno = "";
        string _department = "";
        string _hpno = "";
        string _officeNo = "";
        string _email = "";
        string _dob = "";
        List<StaffMainModel> _staffListMain = [];
        List<string> _countries = [];
        bool _noData = false;
        string _searchTxt = "";
        double _entryWidth = 0.0;
        #endregion
        #region props
        public CountryMainModel? SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry, value); }
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
        public string PrimaryColor
        {
            get { return _primary; }
            set { SetProperty(ref _primary, value); }
        }
        public bool IsSearchVisible
        {
            get { return _isSearch; }
            set { SetProperty(ref _isSearch, value); }
        }
        public string Picture
        {
            get { return _picture; }
            set { SetProperty(ref _picture, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public string Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }
        public string EmpNo
        {
            get { return _empno; }
            set { SetProperty(ref _empno, value); }
        }
        public string Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }
        public string HpNo
        {
            get { return _hpno; }
            set { SetProperty(ref _hpno, value); }
        }
        public string OfficeNo
        {
            get { return _officeNo; }
            set { SetProperty(ref _officeNo, value); }
        }
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public string Dob
        {
            get { return _dob; }
            set { SetProperty(ref _dob, value); }
        }
        public List<string> Countries
        {
            get { return _countries; }
            set { SetProperty(ref _countries, value); }
        }
        public List<StaffMainModel> StaffListMain
        {
            get { return _staffListMain; }
            set
            {
                SetProperty(ref _staffListMain, value);
                NoData = value.Count == 0;
            }
        }
        public bool NoData
        {
            get { return _noData; }
            set { SetProperty(ref _noData, value); }
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

        public string staffId = "";
        public string countryCode = "";
        public Action<bool>? OnFinishLoad = null;
        public List<CountryMainModel> countryList = [];
        public List<StaffMainModel> staffListCache = [];
        public List<DB_StaffModel> staffListDb = [];
        public List<string> selectedStaff = [];

        public StaffDetailsVm()
        {
            var _clr = Application.Current?.Resources["Primary"] as Color;
            if (_clr != null) { PrimaryColor = _clr.ToArgbHex(); }
            EntryWidth = App.ScreenWidth - 40;
            RefreshCommand = new Command(GetStaffList);
            SearchCommand = new Command(SearchStaff);
        }

        public async void GetStaffDetails()
        {
            try
            {
                string _qStaff = "SELECT * FROM DB_StaffModel WHERE Id = '" + staffId + "'";
                var staffDb = AppDatabase.Instance.SqlConnection.Query<DB_StaffModel>(_qStaff);
                var staffList = AppDatabase.Instance.SqlConnection.Query<StaffMainModel>(_qStaff);
                if (staffDb.Count > 0)
                {
                    if (string.IsNullOrEmpty(staffDb[0].HODName)) staffDb[0].HODName = "-";
                    if (staffDb[0].ProfileImage != null) Picture = staffDb[0].ProfileImage;
                    else Picture = "ic_user_empty";

                    Name = staffDb[0].Name;
                    Position = staffDb[0].Position;
                    EmpNo = staffDb[0].EmployeeNo;
                    Department = staffDb[0].Department;
                    HpNo = staffDb[0].ContactNo;
                    OfficeNo = staffDb[0].OfficeNo;
                    Email = staffDb[0].Email;
                    Dob = staffDb[0].DOB.ToLocalTime().ToString("yyyy-MM-dd");
                }
                else
                {
                    if (string.IsNullOrEmpty(staffList[0].HODName)) staffList[0].HODName = "-";
                    if (staffList[0].ProfileImage != null) Picture = staffList[0].ProfileImage;
                    else Picture = "ic_user_empty";

                    Name = staffList[0].Name;
                    Position = staffList[0].Position;
                    EmpNo = staffList[0].EmployeeNo;
                    Department = staffList[0].Department;
                    HpNo = staffList[0].ContactNo;
                    OfficeNo = staffList[0].OfficeNo;
                    Email = staffList[0].Email;
                    Dob = staffList[0].DOB.ToLocalTime().ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async void GetCountries()
        {
            try
            {
                IsBusy = true; IsRefreshing = true;
                await Task.Delay(300);
                Countries = []; countryList = [];
                string _qCountry = "SELECT * FROM CountryMainModel";
                countryList = AppDatabase.Instance.SqlConnection.Query<CountryMainModel>(_qCountry);
                if (countryList.Count > 0)
                {
                    SelectedCountry = countryList.Where(x => x.CountryCode.Equals(countryCode)).FirstOrDefault();
                    var _data = new List<string>();
                    foreach (var item in countryList) _data.Add(item.CountryName);
                    Countries = _data;
                    OnFinishLoad?.Invoke(true);
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

        public async void GetStaffList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    StaffListMain = []; staffListCache = []; staffListDb = [];
                    var model = new API_StaffModel { Country = countryCode };
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_StaffModel>();
                        foreach (var data in _res.items)
                        {
                            if (string.IsNullOrEmpty(data.ProfileImage))
                            { data.ProfileImage = "ic_user_empty.png"; }
                            staffListCache.Add(data);

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
                            staffListDb.Add(staff_db);
                        }
                        AppDatabase.Instance.SqlConnection.InsertAll(staffListDb);
                        StaffListMain = [.. staffListCache.OrderBy(c => c.Name)];
                        //StaffListMain = [.. result.OrderBy(p => p.Name).GroupBy(p => p.GetStaffGroup())
                        //    .Select(p => new ObservableGroupCollection<string, StaffMainModel>(p))];
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
        }

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
            if (selectedStaff.Count > 0 && staffListCache.Count > 0)
            {
                try
                {
                    var _list = new List<ChooseStaff>();
                    foreach (var item in selectedStaff)
                    {
                        var found = staffListCache.Where(x => x.Id.Equals(item)).FirstOrDefault();
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

        public void SearchStaff()
        {
            if (string.IsNullOrEmpty(SearchTxt)) { StaffListMain = staffListCache; }
            else { StaffListMain = staffListCache.FindAll(item => item.Name.ToLower().Contains(SearchTxt)); }
        }
    }
}