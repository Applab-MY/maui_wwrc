using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.CustomerVisitModel;

namespace wwrc_maui.Content.Viewmodels.Staff
{
    public class CustomerVisitDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _customer = "";
        string _visitDateTime = "";
        string _location = "";
        string _salesPerson = "";
        string _attendees = "";
        string _remarks = "";
        #endregion
        #region props
        public string Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
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
        public string Remarks
        {
            get { return _remarks; }
            set { SetProperty(ref _remarks, value); }
        }
        #endregion
        #endregion

        public string visitId = "";

        public CustomerVisitDetailsVm() { }

        public async void SetupData()
        {
            try
            {
                string _qVisit = "SELECT * FROM DB_CustomerVisit WHERE Id = '" + visitId + "'";
                string _qAttnd = "SELECT * FROM AttendeesItem WHERE CustomerVisitId = '" + visitId + "'";
                var _visits = AppDatabase.Instance.SqlConnection.Query<DB_CustomerVisit>(_qVisit);
                var _attendees = AppDatabase.Instance.SqlConnection.Query<AttendeesItem>(_qAttnd);

                if (_visits.Count > 0)
                {
                    VisitDateTime = _visits[0].VisitDateDay;
                    string[] _dt = _visits[0].VisitDateDay.Split('-');
                    DateTime DateTimeFormat = new(Convert.ToInt32(_dt[0]), Convert.ToInt32(_dt[1]), Convert.ToInt32(_dt[2]));
                    //Date.Date = DateTimeFormat;

                    var _time = _visits[0].VisitDateTimeFull.ToString("HH:mm:ss");
                    var SpliteTime = _time.Split(':');
                    TimeSpan TimeSpan = new(Convert.ToInt32(SpliteTime[0]), Convert.ToInt32(SpliteTime[1]), Convert.ToInt32(SpliteTime[2]));
                    //Time.Time = TimeSpan;

                    Customer = _visits[0].CustomerName;
                    Location = _visits[0].Location;
                    SalesPerson = _visits[0].EmployeeName;
                    Remarks = _visits[0].Remarks;
                    //idname = _visits[0].EmployeeName;

                    //string masteruser = "SELECT * FROM userinfo WHERE Name = '" + _visits[0].EmployeeName + "'";
                    //var Name = AppDatabase.Instance.SqlConnection.Query<userinfo>(masteruser);
                    //if (Name != null && Name.Count > 0)
                    //{
                    //    if (idname == Name[0].Name) Button.IsVisible = true;
                    //    else Button.IsVisible = false;
                    //}
                    //else Button.IsVisible = false;
                }

                if (_attendees.Count > 0)
                {
                    int i = 1;
                    string attendees = "";
                    foreach (var data in _attendees)
                    {
                        //AttendeesId.Add(data.AttendeeId);
                        //var datadd = new ChooseStaff
                        //{
                        //    Name = data.AttendeeName,
                        //    Id = data.AttendeeId,
                        //};
                        //AppDatabase.Instance.SqlConnection.Insert(datadd);

                        if (_attendees.Count == 1) attendees = data.AttendeeName;
                        else
                        {
                            if (i == 1) attendees = data.AttendeeName + ",";
                            else
                            {
                                if (i < _attendees.Count)
                                    attendees = attendees + " " + data.AttendeeName + ",";
                                else
                                    attendees = attendees + " " + data.AttendeeName;
                            }
                        }
                        i++;
                    }
                    Attendees = attendees;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }
    }
}
