using SQLite;

namespace wwrc_maui.Content.Model.Auth
{
    public class LoginModel
    {
        public class LoginMainModel
        {
            [PrimaryKey]
            public string Token { get; set; } = "";

            [Ignore]
            public Userinfo? Data { get; set; } = null;

            [Ignore]
            public Userinfo? UserData
            { get { return AppDatabase.Instance.SqlConnection.Query<Userinfo>("Select * from Userinfo").FirstOrDefault(); } }
        }

        public class Userinfo
        {
            [PrimaryKey]
            public string Id { get; set; } = "";
            public string AccessCode { get; set; } = "";
            public string UserID { get; set; } = "";
            public string Email { get; set; } = "";
            public int EmployeeNo { get; set; }
            public string Name { get; set; } = "";
            public string Gender { get; set; } = "";
            public string DOB { get; set; } = "";
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
            public bool IsOfficeCredential { get; set; }

            [Ignore]
            public UserModules? Modules { get; set; } = null;

            [Ignore]
            public UserModules? UserModules => AppDatabase.Instance.SqlConnection.Query<UserModules>
                ("Select * from UserModules").FirstOrDefault();

            [Ignore]
            public List<string> ItemGroup { get; set; } = [];

            [Ignore]
            public List<string> ItemGroupFromDb => [.. AppDatabase.Instance.SqlConnection.Query<ItemGroup>
                ("select * from ItemGroup").Select(x => x.item)];

            [Ignore]
            public List<SalesTargetModule> SalesTarget { get; set; } = [];

            [Ignore]
            public List<string> Branch { get; set; } = [];

            [Ignore]
            public List<string> BranchesFromDB => [.. AppDatabase.Instance.SqlConnection.Query<Branch>
                ("select * from Branch").Select(x => x.branch)];
        }

        public class UserModules
        {
            [PrimaryKey]
            public string StaffId { get; set; } = "";
            public bool SalesTargetChart { get; set; }
            public bool GrossProfitChart { get; set; }
            public bool StaffDirectory { get; set; }
            public bool CustomerVisit { get; set; }
            public bool StockAlert { get; set; }
            public bool CustomerAging { get; set; }
            public bool SalesOrder { get; set; }
            public bool PurchaseOrder { get; set; }
            public bool News { get; set; }
            public bool MediaGallery { get; set; }
            public bool EmployeeHandbook { get; set; }
            public bool ProductCatalogue { get; set; }
            public bool CurrencyExchangeRate { get; set; }
        }

        public class SalesTargetModule
        {
            [PrimaryKey]
            public string StaffId { get; set; } = "";
            public string Subsidiary { get; set; } = "";
            public string Type { get; set; } = "";
            public double YTD { get; set; }
            public double MTD { get; set; }
            public string Default { get; set; } = "";
            public string Country { get; set; } = "";
        }

        public class UserTable
        {
            public string Id { get; set; } = "";
            public string AccessCode { get; set; } = "";
            public string UserID { get; set; } = "";
            public string Email { get; set; } = "";
            public string EmployeeNo { get; set; } = "";
            public string Name { get; set; } = "";
            public string Gender { get; set; } = "";
            public string DOB { get; set; } = "";
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
            public bool IsOfficeCredential { get; set; }
        }

        public class GroupModel { }

        public class TargetModel { }

        public class Branch
        {
            public string branch { get; set; } = "";
        }

        public class SalesTarget
        {
            public string targets { get; set; } = "";
        }

        public class ItemGroup
        {
            public string item { get; set; } = "";
        }

        public class API_LoginModel
        {
            public string dBase { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public string Version { get; set; } = "";
            public string Platform { get; set; } = "";
        }

        public class API_MicrosoftLoginModel
        {
            public string dBase { get; set; } = "";
            public string Email { get; set; } = "";
            public string MicrosoftId { get; set; } = "";
            public string Version { get; set; } = "";
            public string Platform { get; set; } = "";
        }
    }
}