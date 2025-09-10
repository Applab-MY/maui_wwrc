using AndroidX.Browser.Trusted.Sharing;
using Newtonsoft.Json;
using SQLite;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using wwrc_maui.Content.Model;
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
        HttpClient client = new();
        //string WSurl = "http://211.24.92.46:5040/wwrc2api"; //Dev
        //string WSurl = "http://app.wwrc.com:5015"; //Staging
        string WSurl = "http://app.wwrc.com:5018"; //hana
        string DbaseValue = "db_WWRC";
        static readonly int NOTIFICATION_ID = 100;

        Task<RequestResult<ObservableCollection<ChangePasswordModel>>> IRestService.ChangePassword(API_ChangePasswordModel model)
        {
            throw new NotImplementedException();
        }

        Task<RequestResult<ObservableCollection<ResponseHeader>>> IRestService.CreateCustomerVisit(API_CreateCustomerVisit model)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IRestService.ForgotPassword(MultipartFormDataContent content)
        {
            var uri = new Uri(string.Format("{0}/api/Login/ForgotPassword", WSurl));
            StringContent dBase = new(DbaseValue);
            content.Add(dBase, "dBase");
            var result = false;

            try
            {
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode) { result = true; }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error", ex.Message, "Login");
            }
            return result;
        }

        Task<RequestResult<ObservableCollection<CountryModel.CountryMainModel>>> IRestService.GetCountry()
        {
            throw new NotImplementedException();
        }

        async Task<RequestResult<CurrencyMainModel>> IRestService.GetCurrency(API_Currency model)
        {
            if (client.DefaultRequestHeaders.Authorization == null)
            {
                client.DefaultRequestHeaders.Clear();
                var token = Preferences.Default.Get("login_token", "");
                if (!string.IsNullOrEmpty(token))
                { client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token); }
            }

            var uri = new Uri(string.Format("{0}/api/CurrencyExchange/Get", WSurl));
            model.DBase = DbaseValue;
            var json = JsonConvert.SerializeObject(model);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var result = new RequestResult<CurrencyMainModel>();

            try
            {
                var response = await client.PostAsync(uri, contentJson);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<RequestResult<CurrencyMainModel>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error", ex.Message, "Login");
            }
            return result;
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

        async Task<RequestResult<ObservableCollection<DashboardMainModel>>> IRestService.GetDashBoard(API_DashBoard model)
        {
            if (client.DefaultRequestHeaders.Authorization == null)
            {
                client.DefaultRequestHeaders.Clear();
                var token = Preferences.Default.Get("login_token", "");
                if (!string.IsNullOrEmpty(token))
                { client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token); }
            }

            var uri = new Uri(string.Format("{0}/api/Dashboard/Get", WSurl));
            model.DBase = DbaseValue;
            var json = JsonConvert.SerializeObject(model);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var result = new RequestResult<ObservableCollection<DashboardMainModel>>();

            try
            {
                var response = await client.PostAsync(uri, contentJson);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var input = new DashboardDataModel()
                    {
                        Key = "DashBoard",
                        Val = content,
                        Country = model.Country,
                        Subsidiary = model.Company,
                        UserId = model.UserId
                    };
                    using (var conn = AppDatabase.Instance.SqlConnection)
                    {
                        var _exist = string.Format("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{0}'",
                            "DashboardDataModel");
                        var check = conn.ExecuteScalar<int>(_exist);
                        if (check == 0) conn.CreateTable<DashboardDataModel>();
                        else
                        {
                            conn.DropTable<DashboardDataModel>();
                            conn.CreateTable<DashboardDataModel>();
                        }
                        var exist = conn.Table<DashboardDataModel>().Where(s => s.Key == input.Key).FirstOrDefault();
                        if (exist != null)
                        {
                            exist = input;
                            conn.Update(exist);
                        }
                        else conn.Insert(input);
                    }
                    result = JsonConvert.DeserializeObject<RequestResult<ObservableCollection<DashboardMainModel>>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error", ex.Message, "Login");
            }
            return result;
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

        async Task<RequestResult<ObservableCollection<LoginMainModel>>> IRestService.Login(API_LoginModel model)
        {
            var uri = new Uri(string.Format("{0}/api/Login/login", WSurl));
            model.dBase = DbaseValue;
            var json = JsonConvert.SerializeObject(model);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var result = new RequestResult<ObservableCollection<LoginMainModel>>();

            try
            {
                var response = await client.PostAsync(uri, contentJson);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<RequestResult<ObservableCollection<LoginMainModel>>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error", ex.Message, "Login");
            }
            return result;
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
