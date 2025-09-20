using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using CommunityToolkit.Mvvm.Messaging;
using RGPopup.Maui.Droid;
using static wwrc_maui.Content.Helper.ReferenceMessenger;

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
        }

        public override void OnBackPressed()
        { Popup.SendBackPressed(base.OnBackPressed); }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            //2001:usercancel, 2003:success => send back to Login.cs
            WeakReferenceMessenger.Default.Send(new KeyValueNotify(new KeyValue
            { Key = "O365login", Value = resultCode.ToString() }));
        }
    }
}
