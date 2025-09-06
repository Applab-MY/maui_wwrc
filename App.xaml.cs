using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Handlers;
using RGPopup.Maui.Extensions;
using System.Diagnostics;
using wwrc_maui.Content.CustomControls;
using wwrc_maui.Content.CustomControls.Base;
using wwrc_maui.Content.Views.Auth;

namespace wwrc_maui
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public static double ScreenWidth;
        public static double ScreenHeight;
        public static Window? AppPage;

        public App()
        {
            InitializeComponent();
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
                if (view is BorderlessDatePicker)
                {
#if __ANDROID__
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
                }
            });

            AppPage = CreateWindow(null);
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