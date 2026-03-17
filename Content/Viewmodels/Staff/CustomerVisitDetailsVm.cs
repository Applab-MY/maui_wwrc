using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.CustomerVisitModel;
using static wwrc_maui.Content.Model.StaffModel;
using Application = Microsoft.Maui.Controls.Application;

namespace wwrc_maui.Content.Viewmodels.Staff
{
    public class CustomerVisitDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _customer = "";
        DateTime _visitDt = new();
        TimeSpan _visitTime = new();
        string _visitDateTime = "";
        string _location = "";
        string _salesPerson = "";
        string _attendees = "";
        int _attdCount = 0;
        string _remarks = "";
        bool _ieEditable = false;
        double _entryWidth = 0.0;
        #endregion
        #region props
        public string Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
        }
        public DateTime VisitDt
        {
            get { return _visitDt; }
            set { SetProperty(ref _visitDt, value); }
        }
        public TimeSpan VisitTime
        {
            get { return _visitTime; }
            set { SetProperty(ref _visitTime, value); }
        }
        public string VisitDateTime
        {
            get { return _visitDateTime; }
            set { SetProperty(ref _visitDateTime, value); }
        }
        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }
        public string SalesPerson
        {
            get { return _salesPerson; }
            set { SetProperty(ref _salesPerson, value); }
        }
        public string Attendees
        {
            get { return _attendees; }
            set { SetProperty(ref _attendees, value); }
        }
        public int AttdCount
        {
            get { return _attdCount; }
            set { SetProperty(ref _attdCount, value); }
        }
        public string Remarks
        {
            get { return _remarks; }
            set { SetProperty(ref _remarks, value); }
        }
        public bool IsEditable
        {
            get { return _ieEditable; }
            set { SetProperty(ref _ieEditable, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public LoginMainModel? userLogin = null;
        public string visitId = "";
        public List<string> staffIdsCache = [];

        public CustomerVisitDetailsVm() { EntryWidth = App.ScreenWidth - 70; }

        public async Task GetVisitDetails()
        {
            try
            {
                string _qVisit = "SELECT * FROM DB_CustomerVisit WHERE Id = '" + visitId + "'";
                var _visits = AppDatabase.Instance.SqlConnection.Query<DB_CustomerVisit>(_qVisit);
                if (_visits.Count > 0)
                {
                    var timeAmPm = _visits[0].VisitDateTimeFull.ToString("HH:mm tt");
                    VisitDateTime = _visits[0].VisitDateDay + " at " + timeAmPm;
                    Customer = _visits[0].CustomerName;
                    Location = _visits[0].Location;
                    SalesPerson = _visits[0].EmployeeName;
                    Remarks = _visits[0].Remarks;

                    IsEditable = false;
                    string _qUser = "SELECT * FROM Userinfo WHERE Name = '" + _visits[0].EmployeeName + "'";
                    var _user = AppDatabase.Instance.SqlConnection.Query<Userinfo>(_qUser);
                    if (_user != null && _user.Count > 0)
                    { if (_visits[0].EmployeeName.Equals(_user[0].Name)) IsEditable = true; }
                }

                staffIdsCache = []; Attendees = "";
                AppDatabase.Instance.SqlConnection.DeleteAll<ChooseStaff>();
                string _qAttnd = "SELECT * FROM AttendeesItem WHERE CustomerVisitId = '" + visitId + "'";
                var _attendees = AppDatabase.Instance.SqlConnection.Query<AttendeesItem>(_qAttnd);
                AttdCount = _attendees.Count;
                if (_attendees.Count > 0)
                {
                    int i = 1;
                    string _txt = "";
                    foreach (var data in _attendees)
                    {
                        staffIdsCache.Add(data.AttendeeId);
                        var datadd = new ChooseStaff
                        {
                            Name = data.AttendeeName,
                            Id = data.AttendeeId,
                        };
                        AppDatabase.Instance.SqlConnection.Insert(datadd);

                        if (_attendees.Count == 1) _txt = data.AttendeeName;
                        else
                        {
                            if (i == 1) _txt = data.AttendeeName + ",";
                            else
                            {
                                if (i < _attendees.Count) _txt += " " + data.AttendeeName + ",";
                                else _txt += " " + data.AttendeeName;
                            }
                        }
                        i++;
                    }
                    Attendees = _txt;
                }

                //IsEditable = true; //for demo
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async Task InitializeCreate()
        {
            try
            {
                AppDatabase.Instance.SqlConnection.DeleteAll<ChooseStaff>();
                userLogin = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                    ("Select * from LoginMainModel").FirstOrDefault();
                if (userLogin != null && userLogin.UserData != null) { SalesPerson = userLogin.UserData.Name; }
                VisitDt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                VisitTime = DateTime.Now.TimeOfDay;
                Attendees = "Select Attendee(s)";
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public void SetAttendeesFromDB()
        {
            staffIdsCache = []; Attendees = "Select Attendee(s)";
            string _qStaff = "SELECT * FROM ChooseStaff";
            var _staff = AppDatabase.Instance.SqlConnection.Query<ChooseStaff>(_qStaff);
            var noduplicate = _staff.GroupBy(x => x.Name).Select(g => g.First()).ToList();
            AttdCount = noduplicate.Count;
            if (noduplicate.Count > 0)
            {
                int i = 1;
                string _txt = "";
                foreach (var data in noduplicate)
                {
                    if (noduplicate.Count == 1) _txt = data.Name;
                    else
                    {
                        if (i == 1) _txt = data.Name + ",";
                        else
                        {
                            if (i < noduplicate.Count) _txt += " " + data.Name + ",";
                            else _txt += " " + data.Name;
                        }
                    }
                    staffIdsCache.Add(data.Id);
                    i++;
                }
                Attendees = _txt;
            }
        }

        public async Task<bool> UpdateVisitDetails()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                #region validation
                if (string.IsNullOrEmpty(Customer))
                {
                    await App.DisplayAlert("Empty", "Please insert customer", null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
                else if (string.IsNullOrEmpty(Location))
                {
                    await App.DisplayAlert("Empty", "Please insert location", null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
                #endregion

                try
                {
                    if (string.IsNullOrEmpty(VisitDateTime))
                        VisitDateTime = DateTime.Today.ToString("yyyy-MM-dd");
                    var _dt = VisitDateTime + "T" + VisitTime.ToString();
                    var universalDateTime = DateTime.Parse(_dt).ToUniversalTime();
                    string sendDate = string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", universalDateTime);

                    var model = new API_UpdateCustomerVisit
                    {
                        Id = visitId,
                        CustomerName = Customer,
                        VisitDateTime = sendDate,
                        Location = Location,
                        Remarks = Remarks,
                        AttendeesId = staffIdsCache,
                    };
                    var _res = await App.AppClient.UpdateCustomerVisit(model);
                    if (_res.SystemCode == 401)
                    {
                        AppDatabase.Instance.DeleteAllData();
                        Preferences.Default.Clear();
                        await App.DisplayAlert("Relogin", "Please login again", null, "Okay");
                        Application.Current?.Dispatcher.Dispatch(() =>
                        { Application.Current.Windows[0].Page = new Login(); });
                    }
                    else if (_res.SystemCode == 200) { return true; }
                    else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                        + ". " + _res.SystemMessage, null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
            }
            else
            {
                await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
                IsBusy = false; IsRefreshing = false;
                return false;
            }
        }

        public async Task<bool> SaveNewVisit()
        {
            IsBusy = true; IsRefreshing = true;
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                #region validation
                if (string.IsNullOrEmpty(Customer))
                {
                    await App.DisplayAlert("Empty", "Please insert customer", null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
                else if (string.IsNullOrEmpty(Location))
                {
                    await App.DisplayAlert("Empty", "Please insert location", null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
                #endregion

                try
                {
                    var _dt = VisitDt.ToString("yyyy-MM-dd") + "T" + VisitTime.ToString();
                    var _uni = DateTime.Parse(_dt).ToUniversalTime();
                    string sendDate = string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", _uni);

                    var model = new API_CreateCustomerVisit
                    {
                        Id = "",
                        CustomerName = Customer,
                        VisitDateTime = sendDate,
                        Location = Location,
                        Remarks = Remarks,
                        AttendeesId = staffIdsCache,
                    };
                    var _res = await App.AppClient.CreateCustomerVisit(model);
                    if (_res.SystemCode == 401)
                    {
                        AppDatabase.Instance.DeleteAllData();
                        Preferences.Default.Clear();
                        await App.DisplayAlert("Relogin", "Please login again", null, "Okay");
                        Application.Current?.Dispatcher.Dispatch(() =>
                        { Application.Current.Windows[0].Page = new Login(); });
                    }
                    else if (_res.SystemCode == 200)
                    {
                        AppDatabase.Instance.SqlConnection.DeleteAll<ChooseStaff>();
                        return true;
                    }
                    else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                        + ". " + _res.SystemMessage, null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    await App.DisplayAlert("Exception", error, null, "Okay");
                    IsBusy = false; IsRefreshing = false;
                    return false;
                }
            }
            else
            {
                await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
                IsBusy = false; IsRefreshing = false;
                return false;
            }
        }
    }
}
