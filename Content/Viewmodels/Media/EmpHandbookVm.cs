using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.EmpHandbookModel;

namespace wwrc_maui.Content.Viewmodels.Media.EmpHandbook
{
    public class EmpHandbookVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        List<EmployeeHandbookMainModel> _handbooks = [];
        List<string> _branches = [];
        string _selectedBranch = "";
        bool _noData = false;
        #endregion
        #region props
        public List<EmployeeHandbookMainModel> Handbooks
        {
            get { return _handbooks; }
            set
            {
                SetProperty(ref _handbooks, value);
                NoData = value.Count == 0;
            }
        }
        public List<string> Branches
        {
            get { return _branches; }
            set { SetProperty(ref _branches, value); }
        }
        public string SelectedBranch
        {
            get { return _selectedBranch; }
            set { SetProperty(ref _selectedBranch, value); }
        }
        public bool NoData
        {
            get { return _noData; }
            set { SetProperty(ref _noData, value); }
        }
        #endregion
        #endregion

        public Command? RefreshCommand { get; set; } = null;
        public string branch = "";
        public string folderId = "";
        public LoginMainModel? LoginData = null;
        List<EmployeeHandbookMainModel> handbooksCache = [];

        public EmpHandbookVm()
        {
            RefreshCommand = new Command(SetupData);
            LoginData = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                ("Select * from LoginMainModel").FirstOrDefault();
        }

        public void SetupData()
        {
            GetEmployeeHandbook();
            GetBranchList();
        }

        public async void GetEmployeeHandbook()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    Handbooks = []; handbooksCache = [];
                    var model = new API_EmployeeHandbook { BranchId = branch, FolderId = folderId, };
                    var _res = await App.AppClient.GetEmployeeHandbook(model);
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<EmployeeHandbookMainModel>();
                        foreach (var data in _res.items)
                        {
                            var timeInString = data.CreateDate.ToString("dd/MM/yyyy");
                            var employeeHandbook = new EmployeeHandbookMainModel()
                            {
                                Id = data.Id,
                                Title = data.Title,
                                Description = data.Description,
                                IsFile = data.IsFile,
                                File = data.File,
                                FileImage = data.FileImage,
                                FileType = data.FileType,
                                Filesize = data.Filesize,
                                CreateByName = data.CreateByName,
                                CreateDateString = timeInString,
                                IsRead = data.IsRead
                            };
                            handbooksCache.Add(employeeHandbook);
                            AppDatabase.Instance.SqlConnection.Insert(employeeHandbook);
                        }
                        Handbooks = handbooksCache;
                    }
                    else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count == 0)
                    { } //bugfix :: sometimes api success but return null items
                    else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                        + ". " + _res.SystemMessage, null, "Okay");
                    IsBusy = true; IsRefreshing = true;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    IsBusy = true; IsRefreshing = true;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                }
                IsBusy = false; IsRefreshing = false;
            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
            IsBusy = false; IsRefreshing = false;
        }

        public void GetBranchList()
        {
            Branches = [];
            if (LoginData != null && LoginData.UserData != null)
            {
                Branches.Add("All");
                if (LoginData.UserData.BranchesFromDB.Count > 0)
                {
                    foreach (var item in LoginData.UserData.BranchesFromDB)
                    { Branches.Add(item); }
                }
                SelectedBranch = Branches[0];
            }
        }
    }
}
