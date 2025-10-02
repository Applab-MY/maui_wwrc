using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Handlers;
using RGPopup.Maui.Extensions;
using System.Diagnostics;
using wwrc_maui.Content.CustomControls;
using wwrc_maui.Content.CustomControls.Base;
using wwrc_maui.Content.MsalClient;
using wwrc_maui.Content.RestApi;
using wwrc_maui.Content.Views.Auth;

namespace wwrc_maui
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public static int DatabaseVersion => 7;
        public static string? DatabasePath;
        public static double ScreenWidth;
        public static double ScreenHeight;
        public static Window? AppPage;
        public static IRestService? AppClient;
        //public static string ClientId = "aeb29272-0767-4ba2-ab45-4e231cc40d3a"; //old id
        public static PublicClientSingleton? MsalClient = null;

        public App()
        {
            InitializeComponent();
            MsalClient = new PublicClientSingleton();

            #region custom controls handler
            EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if __ANDROID__
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
                }
            });
            DatePickerHandler.Mapper.AppendToMapping(nameof(BorderlessDatePicker), (handler, view) =>
            {
                if (view is BorderlessDatePicker)
                {
#if __ANDROID__
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
                }
            });
            TimePickerHandler.Mapper.AppendToMapping(nameof(BorderlessTimePicker), (handler, view) =>
            {
                if (view is BorderlessTimePicker)
                {
#if __ANDROID__
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
                }
            });
            #endregion

            AppPage = CreateWindow(null);
            AppClient = new RestService();

            //ServiceCollection services = new();
            //services.AddSingleton<IMicrosoftClient, Platform.Android.AndroidMicrosoftClient>();
            //using ServiceProvider provider = services.BuildServiceProvider();

            var width = DeviceDisplay.Current.MainDisplayInfo.Width;
            var height = DeviceDisplay.Current.MainDisplayInfo.Height;
            var dense = DeviceDisplay.Current.MainDisplayInfo.Density;
            ScreenWidth = width / dense;
            ScreenHeight = height / dense;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Current?.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            return new Window(new NavigationPage(new Walkthrough()));
        }

        public async static Task DisplayAlert(string? title = "", string? content = "",
            ContentView? view = null, string? btnOk = "", string? btnCancel = "",
            Action<bool>? alertAction = null)
        {
            var popup = new ShowAlert
            {
                Title = title,
                Description = content,
                Viewcell = view,
                BtnOkay = btnOk,
                BtnCancel = btnCancel,
                ClosePopupAction = alertAction
            };

            try
            {
                if (AppPage?.Page is Page page)
                { await page.Navigation.PushPopupAsync(popup, true); }
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                Debug.WriteLine($" - Error displaying alert: {ex.Message}");
            }
        }
    }
}