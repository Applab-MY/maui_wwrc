using CloudKit;
using CoreData;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.PlatformConfiguration;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using wwrc_maui.Content.Model.Common;
using wwrc_maui.Content.Viewmodels.Common;

namespace wwrc_maui.Content.Viewmodels.Auth
{
    public class LoginVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private string _email = "";
        private string _password = "";
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
        #endregion
        #endregion

        public Command? LoginCommand { get; set; } = null;
        public string Platform { get; set; } = "";
        public string Version { get; set; } = "Ver.";

        public LoginVm()
        {
            IsBusy = false;
            Email = Preferences.Default.Get("email", string.Empty);
            Platform = DeviceInfo.Platform.ToString();
            Version += AppInfo.VersionString;
        }

        async void ExecuteLogin()
        {
            if (string.IsNullOrEmpty(Email))
                await App.DisplayAlert("Empty", "Please insert your email", null, "Okay");
            else if (string.IsNullOrEmpty(Email))
                await App.DisplayAlert("Empty", "Please insert your password", null, "Okay");
            else
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                if (accessType == NetworkAccess.Internet)
                {
                    RequestResult<ObservableCollection<LoginModel>> LoginResult = null;
                    using (await MaterialDialog.Instance.LoadingDialogAsync("Signing you in"))
                    {
                        LoginResult = await App.WSHelper.Login(email, password, version, platform);
                    }

                    list.Clear();
                    dash.Clear();
                    if (LoginResult.SystemCode == 200)
                    {
                        if (LoginResult.items.Count > 0)
                        {
                            Database.Instance.SqlConnection.DeleteAll<LoginModel>();
                            Database.Instance.SqlConnection.DeleteAll<userinfo>();
                            Database.Instance.SqlConnection.DeleteAll<userModules>();
                            Database.Instance.SqlConnection.DeleteAll<ItemGroup>();
                            Database.Instance.SqlConnection.DeleteAll<SalesTarget>();
                            Database.Instance.SqlConnection.DeleteAll<Branch>();

                            Database.Instance.SqlConnection.Insert(LoginResult.items[0].Data);
                            Database.Instance.SqlConnection.Insert(LoginResult.items[0].Data.Modules);
                            //if (LoginResult.items[0].Data.Branch.Count > 0)
                            //   {
                            //   string cty = LoginResult.items[0].Data.Branch[0];
                            //   Database.Instance.SqlConnection.Insert(new Branch { branch = cty });
                            //      Barrel.Current.Empty("country");
                            //      ShareData.selectedCountry = cty;
                            //      Barrel.Current.Add<string>("country", cty, new TimeSpan(10000, 0, 0, 0));
                            // }

                            if (LoginResult.items[0].Data.SalesTarget.Count > 0)
                            {
                                LoginResult.items[0].Data.SalesTarget.ForEach(b =>
                                {
                                    Database.Instance.SqlConnection.Insert(
                                        new salesTargetModule
                                        {
                                            StaffId = b.StaffId,
                                            Subsidiary = b.Subsidiary,
                                            Type = b.Type,
                                            YTD = b.YTD,
                                            MTD = b.MTD,
                                            Default = b.Default,
                                            Country = b.Country,
                                        });

                                    ShareData.selectedSubsidiary = b.Subsidiary;
                                    ShareData.selectedUser = b.Type;
                                    ShareData.selectedCountry = b.Country;
                                    ShareData.selectedTitle = b.Type;

                                    Barrel.Current.Empty("subsidiary");
                                    Barrel.Current.Empty("userId");
                                    Barrel.Current.Empty("userTitle");
                                    Barrel.Current.Empty("country");

                                    Barrel.Current.Add<string>("subsidiary", b.Subsidiary, new TimeSpan(10000, 0, 0, 0));
                                    Barrel.Current.Add<string>("userId", b.Type, new TimeSpan(10000, 0, 0, 0));
                                    Barrel.Current.Add<string>("userTitle", b.Type, new TimeSpan(10000, 0, 0, 0));
                                    Barrel.Current.Add<string>("country", b.Country, new TimeSpan(10000, 0, 0, 0));
                                });
                            }

                            if (LoginResult.items[0].Data.ItemGroup.Count > 0)
                            {
                                LoginResult.items[0].Data.ItemGroup.ForEach(b =>
                                { Database.Instance.SqlConnection.Insert(new ItemGroup { item = b }); });
                            }

                            var token = new LoginModel { Token = LoginResult.items[0].Token };
                            Database.Instance.SqlConnection.Insert(token);

                            if (!LoginResult.items[0].Data.IsOfficeCredential)
                            { Barrel.Current.Add<string>("email", email, new TimeSpan(10000, 0, 0, 0)); }

                            //Post Token
                            //await SendRegistrationToServer(LoginResult.items[0].Data.Id);
                            //AppTheme appTheme = AppInfo.RequestedTheme;

                            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new NavigationPage(new BtmNavigationBar()));
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", LoginResult.SystemMessage, "OK");
                        if (Device.RuntimePlatform == Device.Android) Email_android.Focus();
                        else if (Device.RuntimePlatform == Device.iOS) Email_ios.Focus();
                    }
                }
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
