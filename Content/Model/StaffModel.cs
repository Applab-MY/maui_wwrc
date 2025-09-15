using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wwrc_maui.Content.Model
{
    public class StaffModel
    {
        public class StaffModules
        {
            public string StaffId { get; set; } = "";
            public string SalesTargetChart { get; set; } = "";
            public string GrossProfitChart { get; set; } = "";
            public string StaffDirectory { get; set; } = "";
            public string CustomerVisit { get; set; } = "";
            public string StockAlert { get; set; } = "";
            public string CustomerAging { get; set; } = "";
            public string SalesOrder { get; set; } = "";
            public string PurchaseOrder { get; set; } = "";
            public string News { get; set; } = "";
            public string MediaGallery { get; set; } = "";
            public string EmployeeHandbook { get; set; } = "";
            public string ProductCatalogue { get; set; } = "";
            public string CurrencyExchangeRate { get; set; } = "";
        }

        public class StaffMainModel
        {
            public string SelectedImage { get; set; } = "";
            public bool IsSelected { get; set; } = false;
            public string Image { get; set; } = "";
            public string Id { get; set; } = "";
            public string AccessCode { get; set; } = "";
            public string UserID { get; set; } = "";
            public string Email { get; set; } = "";
            public string EmployeeNo { get; set; } = "";
            public string Name { get; set; } = "";
            public string Gender { get; set; } = "";
            public DateTime DOB { get; set; } = new DateTime();
            public string DefaultBranchId { get; set; } = "";
            public string DefaultBranchName { get; set; } = "";
            public string CountryId { get; set; } = "";
            public string CountryCode { get; set; } = "";
            public string CountryName { get; set; } = "";
            public string Department { get; set; } = "";
            public string Position { get; set; } = "";
            public string HODId { get; set; } = "";
            public string HODName { get; set; } = "";
            public string ContactNo { get; set; } = "";
            public string OfficeNo { get; set; } = "";
            public string ProfileImage { get; set; } = "";
            public string DefaultSalesTeamId { get; set; } = "";
            public string DefaultSalesTeamName { get; set; } = "";
            public string IsOfficeCredential { get; set; } = "";
            public StaffModules Modules { get; set; } = new();
            public List<string> ItemGroup { get; set; } = [];
            public List<string> SalesTarget { get; set; } = [];
            public List<string> Branch { get; set; } = [];
            public string GetStaffGroup() { return Name[0].ToString().ToUpper(); }
        }

        public class API_StaffModel
        {
            public string DBase { get; set; } = "";
            public string Id { get; set; } = "";
            public string Country { get; set; } = "";
        }

        public class DB_StaffModel
        {
            public string SelectedImage { get; set; } = "";
            public bool IsSelected { get; set; }
            public string Image { get; set; } = "";
            public string Id { get; set; } = "";
            public string AccessCode { get; set; } = "";
            public string UserID { get; set; } = "";
            public string Email { get; set; } = "";
            public string EmployeeNo { get; set; } = "";
            public string Name { get; set; } = "";
            public string Gender { get; set; } = "";
            public DateTime DOB { get; set; } = new DateTime();
            public string DefaultBranchId { get; set; } = "";
            public string DefaultBranchName { get; set; } = "";
            public string CountryId { get; set; } = "";
            public string CountryCode { get; set; } = "";
            public string CountryName { get; set; } = "";
            public string Department { get; set; } = "";
            public string Position { get; set; } = "";
            public string HODId { get; set; } = "";
            public string HODName { get; set; } = "";
            public string ContactNo { get; set; } = "";
            public string OfficeNo { get; set; } = "";
            public string ProfileImage { get; set; } = "";
            public string DefaultSalesTeamId { get; set; } = "";
            public string DefaultSalesTeamName { get; set; } = "";
            public string IsOfficeCredential { get; set; } = "";

            //public string getstaffgroup()
            //{ return this.name[0].tostring().toupper(); }
            //public string GetStaffGroup()
            //{ return this.Name[0].ToString().ToUpper(); }
        }

        public class ChooseStaff
        {
            [PrimaryKey]
            public string Id { get; set; } = "";
            public bool IsSelected { get; set; } = false;
            public string Image { get; set; } = "";
            public string AccessCode { get; set; } = "";
            public string UserID { get; set; } = "";
            public string Email { get; set; } = "";
            public string EmployeeNo { get; set; } = "";
            public string Name { get; set; } = "";
            public string Gender { get; set; } = "";
            public DateTime DOB { get; set; } = new DateTime();
            public string DefaultBranchId { get; set; } = "";
            public string DefaultBranchName { get; set; } = "";
            public string CountryId { get; set; } = "";
            public string CountryCode { get; set; } = "";
            public string CountryName { get; set; } = "";
            public string Department { get; set; } = "";
            public string Position { get; set; } = "";
            public string HODId { get; set; } = "";
            public string HODName { get; set; } = "";
            public string ContactNo { get; set; } = "";
            public string OfficeNo { get; set; } = "";
            public string ProfileImage { get; set; } = "";
            public string DefaultSalesTeamId { get; set; } = "";
            public string DefaultSalesTeamName { get; set; } = "";
            public string IsOfficeCredential { get; set; } = "";
        }

        public class StaffModelSelected : INotifyPropertyChanged
        {
            private bool _checked;
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Position { get; set; } = "";
            public string Image { get; set; } = "";
            public bool Checked
            {
                get { return _checked; }
                set { _checked = value; NotifyPropertyChanged(); }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public class OptionPageModel()
        {
            public int Index { get; set; }
            public string Name { get; set; } = "";
            public string Desc { get; set; } = "";
            public string Image { get; set; } = "";
        }
    }
}
