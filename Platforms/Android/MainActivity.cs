using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Core.View;
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

            PlatformConfig.Instance.RedirectUri = $"msal{PublicClientSingleton.Instance.MSALClientHelper?.AzureConfig?.ClientId}://auth";
            PlatformConfig.Instance.ParentWindow = this;

            ApplySystemBarInsets();
        }

        /// <summary>
        /// Android 15 (API 35) enforces edge-to-edge and Android 16 (API 36) removes the
        /// opt-out, so window content draws under the status and navigation bars by default.
        /// Pad the root content view by the system-bar + display-cutout insets so no UI
        /// (top nav bar, bottom toolbars) is ever hidden behind them.
        /// </summary>
        void ApplySystemBarInsets()
        {
            var content = FindViewById(Android.Resource.Id.Content);
            if (content == null) return;

            ViewCompat.SetOnApplyWindowInsetsListener(content, new SystemBarInsetsListener());
            ViewCompat.RequestApplyInsets(content);
        }

        sealed class SystemBarInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
        {
            public WindowInsetsCompat OnApplyWindowInsets(Android.Views.View? v, WindowInsetsCompat? insets)
            {
                if (v == null || insets == null) return insets ?? WindowInsetsCompat.Consumed;

                var bars = insets.GetInsets(
                    WindowInsetsCompat.Type.SystemBars() | WindowInsetsCompat.Type.DisplayCutout());
                v.SetPadding(bars.Left, bars.Top, bars.Right, bars.Bottom);
                return WindowInsetsCompat.Consumed;
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
