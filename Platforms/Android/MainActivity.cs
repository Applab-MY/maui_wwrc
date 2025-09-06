using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using RGPopup.Maui.Droid;

namespace wwrc_maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
        ScreenOrientation = ScreenOrientation.Portrait | ScreenOrientation.Landscape,
        WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? bundle)
        {
            base.OnCreate(bundle);
            Popup.Init(this);
        }

        public override void OnBackPressed()
        { Popup.SendBackPressed(base.OnBackPressed); }
    }
}
