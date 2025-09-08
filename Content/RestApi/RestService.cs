using System.Collections.ObjectModel;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Model.Auth;
using wwrc_maui.Content.Model.Common;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.CurrencyModel;
using static wwrc_maui.Content.Model.CustomerAgingModel;
using static wwrc_maui.Content.Model.CustomerVisitModel;
using static wwrc_maui.Content.Model.DashboardModel;
using static wwrc_maui.Content.Model.EmpHandbookModel;
using static wwrc_maui.Content.Model.FcmTokenModel;
using static wwrc_maui.Content.Model.PasswordModel;
using static wwrc_maui.Content.Model.POModel;
using static wwrc_maui.Content.Model.ProductModel;
using static wwrc_maui.Content.Model.SOModel;
using static wwrc_maui.Content.Model.StaffModel;
using static wwrc_maui.Content.Model.StockModel;

namespace wwrc_maui.Content.RestApi
{
    public class RestService : IRestService
    {
        Task<RequestResult<ObservableCollection<ChangePasswordModel>>> IRestService.ChangePassword(API_ChangePasswordModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<ResponseHeader>>> IRestService.CreateCustomerVisit(API_CreateCustomerVisit model)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.ForgotPassword(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<CountryModel.CountryMainModel>>> IRestService.GetCountry()
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<CurrencyMainModel>> IRestService.GetCurrency(API_Currency model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<CustomerAgingMainModel>>> IRestService.GetCustomerAging(API_CustomerAging model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<AgingDetailMainModel>>> IRestService.GetCustomerAgingDetail(API_CustomerAging model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<CustomerVisitMainModel>>> IRestService.GetCustomerVisit(API_CustomerVisitModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<DashboardMainModel>>> IRestService.GetDashBoard(API_DashBoard model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<DashboardMainModel>>> IRestService.GetDashBoardOffLine(API_DashBoard model, bool isrefresh)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<DOList>>> IRestService.GetDOBySalesOrder(API_DObySO model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<EmployeeHandbookMainModel>>> IRestService.GetEmployeeHandbook(API_EmployeeHandbook model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<NewsModel.NewsMainModel>>> IRestService.GetNews()
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<MediaModel.MediaMainModel>>> IRestService.GetPhoto()
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<POMainModel>>> IRestService.GetPurchase(API_PurchaseModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<SalesOrderMainModel>>> IRestService.GetSalesOrder(API_SalesOrder model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<SalesModel.SalesMainModel>>> IRestService.GetSalesResult(API_DashBoard model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<StockAlertMainModel>>> IRestService.GetStockAlert(API_StockAlert model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<LoginMainModel>>> IRestService.Login(API_LoginModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<LoginMainModel>>> IRestService.MicrosoftLogin(API_MicrosoftLoginModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<ProductMainModel>>> IRestService.ProductCatalog(API_ProductCatalogModel model)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.ProfileUpdate(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.ReadEmployeeHandbook(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.ReadPhoto(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.ReadProductCatalog(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.ReadVideo(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<StaffMainModel>>> IRestService.Staff(API_StaffModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<ResponseHeader>>> IRestService.UpdateCustomerVisit(API_UpdateCustomerVisit model)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.UpdateFCMToken(API_FcmTokenModel model)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRestService.UpdateNews(MultipartFormDataContent content)
        {
            throw new NotImplementedException();
        }
    }
}
