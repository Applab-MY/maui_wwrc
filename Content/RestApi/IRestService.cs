using System.Collections.ObjectModel;
using wwrc_maui.Content.Model.Auth;
using wwrc_maui.Content.Model.Common;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.CountryModel;
using static wwrc_maui.Content.Model.CurrencyModel;
using static wwrc_maui.Content.Model.CustomerAgingModel;
using static wwrc_maui.Content.Model.CustomerVisitModel;
using static wwrc_maui.Content.Model.DashboardModel;
using static wwrc_maui.Content.Model.EmpHandbookModel;
using static wwrc_maui.Content.Model.MediaModel;
using static wwrc_maui.Content.Model.NewsModel;
using static wwrc_maui.Content.Model.PasswordModel;
using static wwrc_maui.Content.Model.POModel;
using static wwrc_maui.Content.Model.ProductModel;
using static wwrc_maui.Content.Model.SalesModel;
using static wwrc_maui.Content.Model.SOModel;
using static wwrc_maui.Content.Model.StaffModel;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.RestApi
{
    public interface IRestService
    {
        Task<RequestResult<ObservableCollection<LoginModel>>> Login(API_LoginModel model);
        Task<RequestResult<ObservableCollection<LoginModel>>> MicrosoftLogin(API_MicrosoftLoginModel model);
        Task<RequestResult<ObservableCollection<StaffMainModel>>> Staff(API_StaffModel model);
        Task<RequestResult<ObservableCollection<MediaMainModel>>> GetPhoto();
        Task<RequestResult<ObservableCollection<NewsMainModel>>> GetNews();
        Task<RequestResult<ObservableCollection<CountryMainModel>>> GetCountry();
        Task<RequestResult<ObservableCollection<ProductMainModel>>> ProductCatalog(API_ProductCatalogModel model);
        Task<RequestResult<ObservableCollection<EmployeeHandbookMainModel>>> GetEmployeeHandbook(API_EmployeeHandbook model);
        Task<RequestResult<ObservableCollection<CustomerVisitMainModel>>> GetCustomerVisit(API_CustomerVisitModel model);
        Task<RequestResult<CurrencyMainModel>> GetCurrency(API_Currency model);
        Task<RequestResult<ObservableCollection<ResponseHeader>>> CreateCustomerVisit(API_CreateCustomerVisit model);
        Task<RequestResult<ObservableCollection<ResponseHeader>>> UpdateCustomerVisit(API_UpdateCustomerVisit model);
        Task<RequestResult<ObservableCollection<DashboardMainModel>>> GetDashBoard(API_DashBoard model);
        Task<RequestResult<ObservableCollection<DashboardMainModel>>> GetDashBoardOffLine(API_DashBoard model, bool isrefresh = false);
        Task<RequestResult<ObservableCollection<SalesMainModel>>> GetSalesResult(API_DashBoard model);
        Task<RequestResult<ObservableCollection<StockAlertMainModel>>> GetStockAlert(API_StockAlert model);
        Task<RequestResult<ObservableCollection<ChangePasswordModel>>> ChangePassword(API_ChangePasswordModel model);
        Task<RequestResult<ObservableCollection<SalesOrderMainModel>>> GetSalesOrder(API_SalesOrder model);
        Task<RequestResult<ObservableCollection<DOList>>> GetDOBySalesOrder(API_DObySO model);
        Task<RequestResult<ObservableCollection<POMainModel>>> GetPurchase(API_PurchaseModel model);
        Task<RequestResult<ObservableCollection<CustomerAgingMainModel>>> GetCustomerAging(API_CustomerAging model);
        Task<RequestResult<ObservableCollection<AgingDetailMainModel>>> GetCustomerAgingDetail(API_CustomerAging model);
        Task<bool> ProfileUpdate(MultipartFormDataContent content);
        Task<bool> UpdateNews(MultipartFormDataContent content);
        Task<bool> UpdateFCMToken(string platform, string imei, string token, string userId);
        Task<bool> ForgotPassword(MultipartFormDataContent content);
        Task<bool> ReadPhoto(MultipartFormDataContent content);
        Task<bool> ReadVideo(MultipartFormDataContent content);
        Task<bool> ReadEmployeeHandbook(MultipartFormDataContent content);
        Task<bool> ReadProductCatalog(MultipartFormDataContent content);
    }
}
