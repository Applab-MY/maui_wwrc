using SQLite;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.CountryModel;
using static wwrc_maui.Content.Model.CurrencyModel;
using static wwrc_maui.Content.Model.CustomerAgingModel;
using static wwrc_maui.Content.Model.CustomerVisitModel;
using static wwrc_maui.Content.Model.EmpHandbookModel;
using static wwrc_maui.Content.Model.MediaModel;
using static wwrc_maui.Content.Model.NewsModel;
using static wwrc_maui.Content.Model.POModel;
using static wwrc_maui.Content.Model.ProductModel;
using static wwrc_maui.Content.Model.SOModel;
using static wwrc_maui.Content.Model.StaffModel;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.Model
{
    public class AppDatabase
    {
        private static readonly Lazy<AppDatabase> singleton = new(() => new AppDatabase());

        public static AppDatabase Instance
        { get { return singleton.Value; } }

        public string DbPath
        { get { return string.IsNullOrEmpty(App.DatabasePath) ? "" : App.DatabasePath; } }

        public SQLiteConnection SqlConnection
        {
            get
            {
                return new SQLiteConnection(DbPath, SQLiteOpenFlags.Create |
                    SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.PrivateCache);
            }
        }

        private AppDatabase()
        {
            int dbv = Preferences.Get("DB_VERSION", 0);
            if (dbv < App.DatabaseVersion)
            {
                DropAllTable();
                Preferences.Set("DB_VERSION", App.DatabaseVersion);
            }
            CreateTables();
        }

        public void CreateTables()
        {
            // Staff & Country
            SqlConnection.CreateTable<DB_StaffModel>();
            SqlConnection.CreateTable<CountryMainModel>();
            // News & Album
            SqlConnection.CreateTable<NewsInfoTable>();
            SqlConnection.CreateTable<NewsTable>();
            SqlConnection.CreateTable<DB_Album>();
            SqlConnection.CreateTable<ImageInfo>();
            SqlConnection.CreateTable<VideoInfo>();
            // Product & EmpHandbook
            SqlConnection.CreateTable<ProductMainModel>();
            SqlConnection.CreateTable<EmployeeHandbookMainModel>();
            // Customer Visit
            SqlConnection.CreateTable<DB_CustomerVisit>();
            SqlConnection.CreateTable<AttendeesItem>();
            // Currency
            SqlConnection.CreateTable<DB_Currency>();
            SqlConnection.CreateTable<ChooseStaff>();
            // Stock Alert
            SqlConnection.CreateTable<DB_StockAlert>();
            SqlConnection.CreateTable<DB_WarehouseItem>();
            SqlConnection.CreateTable<DB_CustomerItem>();
            SqlConnection.CreateTable<DB_IsCommitedPW>();
            SqlConnection.CreateTable<DB_IsCommitedPW_Customer>();
            SqlConnection.CreateTable<DB_WarehouseTable>();
            // Purchase Order
            SqlConnection.CreateTable<DB_PurchaseMonth>();
            SqlConnection.CreateTable<DB_Purchase>();
            SqlConnection.CreateTable<DB_POItem>();
            // Sales Order
            SqlConnection.CreateTable<DB_SalesOrderModel>();
            SqlConnection.CreateTable<Db_SOList>();
            SqlConnection.CreateTable<Db_SOItemList>();
            SqlConnection.CreateTable<Db_DOList>();
            SqlConnection.CreateTable<Db_DOItemsList>();
            // Customer Aging
            SqlConnection.CreateTable<DB_CustAging>();
            SqlConnection.CreateTable<DB_MonthsModel>();
            SqlConnection.CreateTable<DB_DocListModel>();
            // User Info
            SqlConnection.CreateTable<LoginMainModel>();
            SqlConnection.CreateTable<Userinfo>();
            SqlConnection.CreateTable<UserModules>();
            SqlConnection.CreateTable<SalesTargetModule>();
            SqlConnection.CreateTable<ItemGroup>();
            SqlConnection.CreateTable<SalesTarget>();
            SqlConnection.CreateTable<Branch>();
        }

        public void DropAllTable()
        {
            // Staff & Country
            SqlConnection.DropTable<DB_StaffModel>();
            SqlConnection.DropTable<CountryMainModel>();
            // News & Album
            SqlConnection.DropTable<NewsInfoTable>();
            SqlConnection.DropTable<NewsTable>();
            SqlConnection.DropTable<DB_Album>();
            SqlConnection.DropTable<ImageInfo>();
            SqlConnection.DropTable<VideoInfo>();
            // Product & EmpHandbook
            SqlConnection.DropTable<ProductMainModel>();
            SqlConnection.DropTable<EmployeeHandbookMainModel>();
            // Customer Visit
            SqlConnection.DropTable<DB_CustomerVisit>();
            SqlConnection.DropTable<AttendeesItem>();
            // Currency
            SqlConnection.DropTable<DB_Currency>();
            SqlConnection.DropTable<ChooseStaff>();
            // Stock Alert
            SqlConnection.DropTable<DB_StockAlert>();
            SqlConnection.DropTable<DB_WarehouseItem>();
            SqlConnection.DropTable<DB_CustomerItem>();
            SqlConnection.DropTable<DB_IsCommitedPW>();
            SqlConnection.DropTable<DB_IsCommitedPW_Customer>();
            SqlConnection.DropTable<DB_WarehouseTable>();
            // Purchase Order
            SqlConnection.DropTable<DB_PurchaseMonth>();
            SqlConnection.DropTable<DB_Purchase>();
            SqlConnection.DropTable<DB_POItem>();
            // Sales Order
            SqlConnection.DropTable<DB_SalesOrderModel>();
            SqlConnection.DropTable<Db_SOList>();
            SqlConnection.DropTable<Db_SOItemList>();
            SqlConnection.DropTable<Db_DOList>();
            SqlConnection.DropTable<Db_DOItemsList>();
            // Customer Aging
            SqlConnection.DropTable<DB_CustAging>();
            SqlConnection.DropTable<DB_MonthsModel>();
            SqlConnection.DropTable<DB_DocListModel>();
            // User Info
            SqlConnection.DropTable<LoginMainModel>();
            SqlConnection.DropTable<Userinfo>();
            SqlConnection.DropTable<UserModules>();
            SqlConnection.DropTable<SalesTargetModule>();
            SqlConnection.DropTable<ItemGroup>();
            SqlConnection.DropTable<SalesTarget>();
            SqlConnection.DropTable<Branch>();
        }

        public void DeleteAllData()
        {
            // Staff & Country
            SqlConnection.DeleteAll<DB_StaffModel>();
            SqlConnection.DeleteAll<CountryMainModel>();
            // News & Album
            SqlConnection.DeleteAll<NewsInfoTable>();
            SqlConnection.DeleteAll<NewsTable>();
            SqlConnection.DeleteAll<DB_Album>();
            SqlConnection.DeleteAll<ImageInfo>();
            SqlConnection.DeleteAll<VideoInfo>();
            // Product & EmpHandbook
            SqlConnection.DeleteAll<ProductMainModel>();
            SqlConnection.DeleteAll<EmployeeHandbookMainModel>();
            // Customer Visit
            SqlConnection.DeleteAll<DB_CustomerVisit>();
            SqlConnection.DeleteAll<AttendeesItem>();
            // Currency
            SqlConnection.DeleteAll<DB_Currency>();
            SqlConnection.DeleteAll<ChooseStaff>();
            // Stock Alert
            SqlConnection.DeleteAll<DB_StockAlert>();
            SqlConnection.DeleteAll<DB_WarehouseItem>();
            SqlConnection.DeleteAll<DB_CustomerItem>();
            SqlConnection.DeleteAll<DB_IsCommitedPW>();
            SqlConnection.DeleteAll<DB_IsCommitedPW_Customer>();
            SqlConnection.DeleteAll<DB_WarehouseTable>();
            // Purchase Order
            SqlConnection.DeleteAll<DB_PurchaseMonth>();
            SqlConnection.DeleteAll<DB_Purchase>();
            SqlConnection.DeleteAll<DB_POItem>();
            // Sales Order
            SqlConnection.DeleteAll<DB_SalesOrderModel>();
            SqlConnection.DeleteAll<Db_SOList>();
            SqlConnection.DeleteAll<Db_SOItemList>();
            SqlConnection.DeleteAll<Db_DOList>();
            SqlConnection.DeleteAll<Db_DOItemsList>();
            // Customer Aging
            SqlConnection.DeleteAll<DB_CustAging>();
            SqlConnection.DeleteAll<DB_MonthsModel>();
            SqlConnection.DeleteAll<DB_DocListModel>();
            // User Info
            SqlConnection.DeleteAll<LoginMainModel>();
            SqlConnection.DeleteAll<Userinfo>();
            SqlConnection.DeleteAll<UserModules>();
            SqlConnection.DeleteAll<SalesTargetModule>();
            SqlConnection.DeleteAll<ItemGroup>();
            SqlConnection.DeleteAll<SalesTarget>();
            SqlConnection.DeleteAll<Branch>();
        }
    }
}
