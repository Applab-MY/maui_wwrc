using Foundation;
using RGPopup.Maui.IOS;
using UIKit;

namespace wwrc_maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Popup.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}
