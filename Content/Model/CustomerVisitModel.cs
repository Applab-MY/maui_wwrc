namespace wwrc_maui.Content.Model
{
    public class CustomerVisitModel
    {
        public class AttendeesItem
        {
            public string CustomerVisitId { get; set; } = "";
            public string AttendeeId { get; set; } = "";
            public string AttendeeName { get; set; } = "";
        }

        public class CustomerVisitMainModel
        {
            public string Id { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public DateTime VisitDateDayFull { get; set; } = new DateTime();
            public DateTime VisitDateTimeFull { get; set; } = new DateTime();
            public string VisitDateTime { get; set; } = "";
            public string VisitDateDay { get; set; } = "";
            public string Location { get; set; } = "";
            public string EmployeeId { get; set; } = "";
            public string EmployeeName { get; set; } = "";
            public string Remarks { get; set; } = "";
            public string Calendar { get; set; } = "";
            public List<AttendeesItem> Attendees { get; set; } = [];
        }

        public class DB_CustomerVisit
        {
            public string Id { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public DateTime VisitDateDayFull { get; set; } = new DateTime();
            public DateTime VisitDateTimeFull { get; set; } = new DateTime();
            public string VisitDateTime { get; set; } = "";
            public string VisitDateDay { get; set; } = "";
            public string Location { get; set; } = "";
            public string EmployeeId { get; set; } = "";
            public string EmployeeName { get; set; } = "";
            public string Remarks { get; set; } = "";
            public string Calendar { get; set; } = "";
        }

        public class API_CustomerVisitModel
        {
            public string DBase { get; set; } = "";
            public string Id { get; set; } = "";
            public string StaffId { get; set; } = "";
            public string FromDate { get; set; } = "";
        }

        public class API_CreateCustomerVisit
        {
            public string DBase { get; set; } = "";
            public string Id { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public string VisitDateTime { get; set; } = "";
            public string Location { get; set; } = "";
            public string Remarks { get; set; } = "";
            public List<string> AttendeesId { get; set; } = [];
        }

        public class API_UpdateCustomerVisit
        {
            public string DBase { get; set; } = "";
            public string Id { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public string VisitDateTime { get; set; } = "";
            public string Location { get; set; } = "";
            public string Remarks { get; set; } = "";
            public List<string> AttendeesId { get; set; } = [];
        }
    }
}
