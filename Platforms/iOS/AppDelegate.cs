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
            string fileName = "db_wwrc.sqlite";
            string fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string DB_fullpath = Path.Combine(fileLocation, fileName);
            App.DatabasePath = DB_fullpath;
            return base.FinishedLaunching(app, options);
        }
    }
}
