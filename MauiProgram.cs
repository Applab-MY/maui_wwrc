using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using wwrc_maui.Content.MsalClient;

namespace wwrc_maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Ubuntu-Bold.ttf", "UbuntuBold");
                    fonts.AddFont("Quicksand-Light.ttf", "QuickSandLight");
                }).UseMauiCommunityToolkit(options => options.SetShouldEnableSnackbarOnWindows(true));
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<PublicClientSingleton>();
            return builder.Build();
        }
    }
}
