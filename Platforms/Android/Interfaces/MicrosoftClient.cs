using Microsoft.Identity.Client;
using wwrc_maui.Content.Interfaces;

//[assembly: Dependency(typeof(IMicrosoftClient))]
namespace wwrc_maui.Platforms.Android.Interfaces
{
    public class MicrosoftClient : IMicrosoftClient
    {
        IPublicClientApplication? androidClient;
        string clientId = "aeb29272-0767-4ba2-ab45-4e231cc40d3a";
        string[] scopes = { "User.Read" };

        void IMicrosoftClient.Initialize()
        {
            androidClient = PublicClientApplicationBuilder.Create(clientId).WithRedirectUri($"msal{clientId}://auth")
                .WithParentActivityOrWindow(() => Platform.CurrentActivity)
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache").Build();
        }

        public async void Authenticate()
        {
            if (androidClient != null)
            {
                AuthenticationResult? authResult = null;
                var accounts = await androidClient.GetAccountsAsync();
                while (accounts.Any())
                {
                    await androidClient.RemoveAsync(accounts.FirstOrDefault());
                    accounts = await androidClient.GetAccountsAsync();
                }

                var firstAccount = accounts.FirstOrDefault();
                if (firstAccount != null)
                    authResult = await androidClient.AcquireTokenSilent(scopes, firstAccount).ExecuteAsync();
                else
                {
                    var systemWebViewOptions = new SystemWebViewOptions() { iOSHidePrivacyPrompt = true, };
                    authResult = await androidClient.AcquireTokenInteractive(scopes)
                        .WithPrompt(Prompt.ForceLogin).WithParentActivityOrWindow(App.AppPage)
                        .WithSystemWebViewOptions(systemWebViewOptions).ExecuteAsync();
                    await App.DisplayAlert("LoginVm", "success return to callback function", null, "Okay");
                }
            }
        }
    }
}
