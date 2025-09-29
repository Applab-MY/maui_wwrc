using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.CustomerVisitModel;
using Application = Microsoft.Maui.Controls.Application;

namespace wwrc_maui.Content.Viewmodels.Staff
{
    public class CustomerVisitVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _salesperson = "";
        DateTime _selectedDate = DateTime.Now;
        List<DB_CustomerVisit> _mainVisitList = [];
        bool _nodata = false;
        #endregion
        #region props
        public string SalesPerson
        {
            get { return _salesperson; }
            set { SetProperty(ref _salesperson, value); }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public List<DB_CustomerVisit> MainVisitList
        {
            get { return _mainVisitList; }
            set
            {
                SetProperty(ref _mainVisitList, value);
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

        public Command? RefreshCommand { get; set; } = null;

        public List<DB_CustomerVisit> visitListCache = [];
        public List<string> personListCache = [];

        public CustomerVisitVm()
        {
            IsBusy = false;
            RefreshCommand = new Command(GetCustVisitList);
        }

        public async void GetCustVisitList()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    MainVisitList = []; visitListCache = []; personListCache = [];
                    var model = new API_CustomerVisitModel
                    {
                        Id = "",
                        StaffId = "",
                        FromDate = SelectedDate.ToString("yyyy-MM-dd")
                    };
                    var _res = await App.AppClient.GetCustomerVisit(model);
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
                        AppDatabase.Instance.SqlConnection.DeleteAll<DB_CustomerVisit>();
                        AppDatabase.Instance.SqlConnection.DeleteAll<AttendeesItem>();

                        string query = "SELECT * FROM Userinfo";
                        var user = AppDatabase.Instance.SqlConnection.Query<Userinfo>(query);

                        personListCache.Add("All");
                        foreach (var data in _res.items)
                        {
                            string calImg = "ic_calendar";
                            if (data.EmployeeName == user[0].Name) calImg = "ic_edit";
                            var _dt = Convert.ToDateTime(data.VisitDateTime);
                            //var resultDt = _dt.Add(new TimeSpan(-8, 0, 0)); //for utc+8 issue

                            var dbModel = new DB_CustomerVisit()
                            {
                                Id = data.Id,
                                CustomerName = data.CustomerName,
                                VisitDateDayFull = _dt,
                                VisitDateTimeFull = _dt,
                                VisitDateTime = _dt.ToString("HH:mm"),//data.VisitDateTime.Substring(14, 5),
                                VisitDateDay = _dt.ToString("yyyy-MM-dd"), //data.VisitDateTime.Substring(0, 10),
                                Location = data.Location,
                                EmployeeId = data.EmployeeId,
                                EmployeeName = data.EmployeeName,
                                Remarks = data.Remarks,
                                Calendar = calImg
                            };
                            AppDatabase.Instance.SqlConnection.Insert(dbModel);

                            foreach (var inside in data.Attendees)
                            {
                                var cusvisit_inside = new AttendeesItem()
                                {
                                    CustomerVisitId = inside.CustomerVisitId,
                                    AttendeeId = inside.AttendeeId,
                                    AttendeeName = inside.AttendeeName
                                };
                                AppDatabase.Instance.SqlConnection.Insert(cusvisit_inside);
                            }

                            var date1 = new DateTime(dbModel.VisitDateDayFull.Year, dbModel.VisitDateDayFull.Month,
                                dbModel.VisitDateDayFull.Day);
                            var date2 = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day);

                            int result = DateTime.Compare(date1, date2);
                            if (result >= 0) visitListCache.Add(dbModel);

                            if (!personListCache.Contains(dbModel.EmployeeName))
                                personListCache.Add(dbModel.EmployeeName);
                        }
                        MainVisitList = visitListCache.OrderByDescending(x => x.VisitDateDayFull).ToList();
                        SalesPerson = "All";
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

        public async void GetSalesPersonVisits()
        {
            IsBusy = true; IsRefreshing = true;
            string query = "SELECT * FROM Userinfo";
            var user = AppDatabase.Instance.SqlConnection.Query<Userinfo>(query);

            MainVisitList = [];
            string _qGet = "";
            if (SalesPerson.Equals("All"))
            { _qGet = "SELECT * FROM DB_CustomerVisit WHERE EmployeeName = '" + SalesPerson + "'"; }
            else { _qGet = "SELECT * FROM DB_CustomerVisit"; }

            try
            {
                await Task.Delay(300);
                var items = AppDatabase.Instance.SqlConnection.Query<DB_CustomerVisit>(_qGet);
                if (items.Count > 0)
                {
                    var _list = new List<DB_CustomerVisit>();
                    foreach (var data in items)
                    {
                        string calImg = "ic_calendar";
                        if (data.EmployeeName == user[0].Name) { calImg = "ic_edit"; }
                        data.Calendar = calImg;
                        _list.Add(data);
                    }
                    MainVisitList = _list;
                }
                IsBusy = false; IsRefreshing = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                IsBusy = false; IsRefreshing = false;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
            IsBusy = false; IsRefreshing = false;
        }
    }
}
