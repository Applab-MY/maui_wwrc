using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Dashboard;
using static wwrc_maui.Content.Model.Auth.LoginModel;

namespace wwrc_maui.Content.Viewmodels.Auth
{
    public class LoginVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private string _email = "";
        private string _password = "";
        private string _version = "";
        private string _platform = "";
        private double _entryWidth = 0.0;
        #endregion
        #region properties
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }
        public string Platform
        {
            get { return _platform; }
            set { SetProperty(ref _platform, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public Command? LoginCommand { get; set; } = null;
        public Command? Login365Command { get; set; } = null;

        public LoginVm()
        {
            IsBusy = false;
            Email = Preferences.Default.Get("email", string.Empty);
            Platform = DeviceInfo.Platform.ToString();
            Version += "Ver." + AppInfo.VersionString;
            EntryWidth = App.ScreenWidth - 40;
            LoginCommand = new Command(ExecuteLogin);
        }

        public async void ExecuteLogin()
        {
            IsBusy = true;
            await Task.Delay(300);
            if (string.IsNullOrEmpty(Email))
                await App.DisplayAlert("Empty", "Please insert your email", null, "Okay");
            else if (string.IsNullOrEmpty(Password))
                await App.DisplayAlert("Empty", "Please insert your password", null, "Okay");
            else
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                if (accessType == NetworkAccess.Internet && App.AppClient != null)
                {
                    try
                    {
                        var model = new API_LoginModel
                        {
                            dBase = "",
                            Email = Email,
                            Password = Password,
                            Version = Version,
                            Platform = Platform,
                        };
                        var _res = await App.AppClient.Login(model);
                        if (_res.SystemCode == 200 && _res.items != null)
                        {
                            if (_res.items.Count > 0)
                            {
                                AppDatabase.Instance.SqlConnection.DeleteAll<LoginMainModel>();
                                AppDatabase.Instance.SqlConnection.DeleteAll<Userinfo>();
                                AppDatabase.Instance.SqlConnection.DeleteAll<UserModules>();
                                AppDatabase.Instance.SqlConnection.DeleteAll<ItemGroup>();
                                AppDatabase.Instance.SqlConnection.DeleteAll<SalesTarget>();
                                AppDatabase.Instance.SqlConnection.DeleteAll<Branch>();
                                if (_res.items[0].Data != null)
                                {
                                    AppDatabase.Instance.SqlConnection.Insert(_res.items[0].Data);
                                    AppDatabase.Instance.SqlConnection.Insert(_res.items[0].Data?.Modules);
                                    _res.items[0].Data?.SalesTarget.ForEach(b =>
                                    {
                                        var model = new SalesTargetModule
                                        {
                                            StaffId = b.StaffId,
                                            Subsidiary = b.Subsidiary,
                                            Type = b.Type,
                                            YTD = b.YTD,
                                            MTD = b.MTD,
                                            Default = b.Default,
                                            Country = b.Country,
                                        };
                                        AppDatabase.Instance.SqlConnection.Insert(model);
                                        Preferences.Default.Set("subsidiary", b.Subsidiary);
                                        Preferences.Default.Set("userId", b.Type);
                                        Preferences.Default.Set("userTitle", b.Type);
                                        Preferences.Default.Set("country", b.Country);
                                    });
                                    _res.items[0].Data?.ItemGroup.ForEach(b =>
                                    { AppDatabase.Instance.SqlConnection.Insert(new ItemGroup { item = b }); });

                                    var _is365 = Convert.ToBoolean(_res.items[0].Data?.IsOfficeCredential);
                                    if (!_is365) { Preferences.Default.Set("email", Email); }
                                }

                                var login = new LoginMainModel { Token = _res.items[0].Token };
                                AppDatabase.Instance.SqlConnection.Insert(login);
                                Preferences.Default.Set("login_token", _res.items[0].Token);

                                //Post Token
                                //await SendRegistrationToServer(LoginResult.items[0].Data.Id);
                                //AppTheme appTheme = AppInfo.RequestedTheme;

                                Application.Current?.Dispatcher.Dispatch(() =>
                                { Application.Current.Windows[0].Page = new NavigationPage(new MainPage()); });
                            }
                        }
                        else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                            + ". " + _res.SystemMessage, null, "Okay");
                    }
                    catch (Exception ex)
                    { await App.DisplayAlert("Error", ex.Message, null, "Okay"); }
                }
                else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
            }
            IsBusy = false;
        }

        async void ExecuteLogin365()
        {
            //AuthenticationResult authResult = null;
            try
            {
                //IEnumerable<IAccount> accounts = await App.PCA.GetAccountsAsync();
                //while (accounts.Any())
                //{
                //    await App.PCA.RemoveAsync(accounts.FirstOrDefault());
                //    accounts = await App.PCA.GetAccountsAsync();
                //}

                //IAccount firstAccount = accounts.FirstOrDefault();
                //if (firstAccount != null)
                //{
                //    authResult = await App.PCA.AcquireTokenSilent(App.Scopes, firstAccount)
                //    .ExecuteAsync();
                //}
                //else
                //{
                //    SystemWebViewOptions systemWebViewOptions = new SystemWebViewOptions()
                //    { iOSHidePrivacyPrompt = true, };
                //    authResult = await App.PCA.AcquireTokenInteractive(App.Scopes)
                //        .WithPrompt(Prompt.ForceLogin).WithParentActivityOrWindow(App.ParentWindow)
                //        .WithSystemWebViewOptions(systemWebViewOptions).ExecuteAsync();
                //}

                //if (authResult != null)
                //{
                //    var content = await GetHttpContentWithTokenAsync(authResult.AccessToken);
                //    UpdateUserFromMicrosoft(content, authResult.AccessToken);
                //}
                //else DummyData(); //mark for testing
            }
            catch (Exception ex)
            {
                await App.DisplayAlert("Authentication failed: ", ex.Message, null, "Dismiss");
            }
        }

        public async Task<string> GetHttpContentWithTokenAsync(string token)
        {
            try
            {
                //get data from API
                HttpClient client = new();
                HttpRequestMessage message = new(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.SendAsync(message);
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch (Exception ex)
            {
                //await DisplayAlert("API call to graph failed: ", ex.Message, "Dismiss");
                return ex.ToString();
            }
        }

        public async Task SendRegistrationToServer(string userId)
        {
            string fcmToken = null;
            //if (Application.Current.Properties.ContainsKey("fcm_token"))
            //{ fcmToken = Application.Current.Properties["fcm_token"] as string; }
            //else fcmToken = RequestFcmToken();
            //string deviceId = null;
            //if (Application.Current.Properties.ContainsKey("device_imei"))
            //{ deviceId = Application.Current.Properties["device_imei"] as string; }
            //else deviceId = "";

            //await DisplayAlert("Devs Check", "FCM: " + fcmToken + "\r\nDevice ID: " + deviceId, "Dismiss");
            //var emailMessenger = CrossMessaging.Current.EmailMessenger;
            //if (emailMessenger.CanSendEmail)
            //{
            //    var builder = fcmToken + "\r\n" + deviceId;
            //    emailMessenger.SendEmail("iOS Testing", "iOS FCM Push Notification", builder);
            //    emailMessenger.SendEmail("muhdrashid@applab.com.my");
            //}

            //if (CrossConnectivity.Current.IsConnected)
            //{
            //    SQLiteConnection conn = new SQLiteConnection(App.DB_PATH);
            //    var result = await App.WSHelper.UpdateFCMToken(platform, deviceId, fcmToken, userId);
            //}
        }

        public string RequestFcmToken()
        {
            string token = "";

            //try
            //{
            //    var instanceid = FirebaseInstanceId.Instance;
            //    instanceid.DeleteInstanceId();
            //    instanceid.GetToken(this.GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope)
            //    var instanceID = Firebase.Iid.FirebaseInstanceId.Instance;
            //    token = instanceID.Token;
            //    token = instanceID.GetToken("689683368216", FirebaseMessaging.InstanceIdScope);
            //    Application.Current.Properties.Add("fcm_token", token);
            //    Application.Current.SavePropertiesAsync();
            //}
            //catch (Exception ex)
            //{ System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace); }
            return token;
        }
    }
}
