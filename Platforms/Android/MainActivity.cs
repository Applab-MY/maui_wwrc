using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Identity.Client;
using RGPopup.Maui.Droid;
using wwrc_maui.Content.MsalClient;

namespace wwrc_maui
{
    [Activity(Label = "WWRC", Icon = "@drawable/launcher", Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
        ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
        ScreenOrientation = ScreenOrientation.Portrait | ScreenOrientation.Landscape, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? bundle)
        {
            Popup.Init(this);
            string fileName = "db_wwrc.db";
            string fileLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(fileLocation); // Ensure directory exists
            string DB_fullpath = Path.Combine(fileLocation, fileName);
            App.DatabasePath = DB_fullpath;
            base.OnCreate(bundle);

            if (PublicClientSingleton.Instance.MSALClientHelper != null)
            {
                PlatformConfig.Instance.RedirectUri = $"msal{PublicClientSingleton.Instance.MSALClientHelper.AzureConfig?.ClientId}://auth";
                PlatformConfig.Instance.ParentWindow = this;
                // Initialize MSAL and platformConfig is set
                IAccount? existinguser = Task.Run(PublicClientSingleton.Instance.MSALClientHelper.InitializePublicClientAppAsync).Result;
            }
        }

        public override void OnBackPressed()
        { Popup.SendBackPressed(base.OnBackPressed); }

        /// <summary>
        /// This is a callback to continue with the authentication
        /// Info about redirect URI: https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-client-application-configuration#redirect-uri
        /// </summary>
        /// <param name="requestCode">request code </param>
        /// <param name="resultCode">result code</param>
        /// <param name="data">intent of the activity</param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }
    }
}
