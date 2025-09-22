using Microsoft.Identity.Client;
using System.Diagnostics;

namespace wwrc_maui.Content.MsalClient
{
    public class MSALClientHelper
    {
        #region getter setter
        public IPublicClientApplication? PublicClientApplication { get; private set; } = null;
        public AuthenticationResult? AuthResult { get; private set; } = null;
        public bool IsBrokerInitialized { get; private set; } // this instance of PublicClientApp was initialized with a broker
        public bool UseEmbedded { get; set; } = false; // the Interactive Authentication should be Embedded or System view
        #endregion

        public AzureConfig? AzureConfig = null;
        private PublicClientApplicationBuilder? PublicClientApplicationBuilder = null;

        public MSALClientHelper(AzureConfig azureADConfig)
        {
            AzureConfig = azureADConfig;
            InitializePublicClientApplicationBuilder();
        }

        private void InitializePublicClientApplicationBuilder()
        {
            if (AzureConfig != null)
            {
                PublicClientApplicationBuilder = PublicClientApplicationBuilder.Create(AzureConfig.ClientId)
                    .WithAuthority(string.Format(AzureConfig.Authority, AzureConfig.TenantId))
                    //.WithExperimentalFeatures() // this is for upcoming logger
                    //.WithLogging(new IdentityLogger(EventLogLevel.Warning), enablePiiLogging: false) // This is the currently recommended way to log MSAL message. For more info refer to https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/logging. Set Identity Logging level to Warning which is a middle ground
                    //.WithClientCapabilities(new string[] { "cp1" }) // declare this client app capable of receiving CAE events- https://aka.ms/clientcae
                    .WithIosKeychainSecurityGroup("com.microsoft.adalcache");
            }
        }

        public async Task<IAccount?> InitializePublicClientAppAsync()
        // Initializes the public client application of MSAL.NET with the required information to correctly authenticate the user.
        {
            // Initialize the MSAL library by building a public client application
            if (PublicClientApplicationBuilder != null)
            {
                // redirect URI is set later in PlatformConfig when the platform has been decided
                PublicClientApplication = PublicClientApplicationBuilder.WithRedirectUri(PlatformConfig.Instance.RedirectUri).Build();
                //await AttachTokenCache();
                return await FetchSignedInUserFromCache().ConfigureAwait(false);
            }
            else return null;
        }

        private async Task<IEnumerable<IAccount>?> AttachTokenCache()
        //Attaches the token cache to the Public Client app. IAccount list of already signed-in users (if available)
        {
            if (DeviceInfo.Current.Platform != DevicePlatform.WinUI) return null;
            // Cache configuration and hook-up to public application. Refer to https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache#configuring-the-token-cache
            //var storageProperties = new StorageCreationPropertiesBuilder(AzureConfig.CacheFileName,
            //    AzureConfig.CacheDir).Build();
            //var msalcachehelper = await MsalCacheHelper.CreateAsync(storageProperties);
            //msalcachehelper.RegisterCache(PublicClientApplication.UserTokenCache);

            // If the cache file is being reused, we'd find some already-signed-in accounts
            return await PublicClientApplication.GetAccountsAsync().ConfigureAwait(false);
        }

        public async Task<AuthenticationResult> SignInUserInteractivelyAsync(string[]? scopes,
            IAccount? existingAccount = null)
        // Shows a pattern to sign-in a user interactively in applications that are input constrained
        // and would need to fall-back on device code flow.
        {
            if (PublicClientApplication == null)
                throw new NullReferenceException();

            // If the operating system has UI
            if (PublicClientApplication.IsUserInteractive())
            {
                if (PublicClientSingleton.Instance.UseEmbedded)
                {
                    return await PublicClientApplication.AcquireTokenInteractive(scopes)
                        .WithLoginHint(existingAccount?.Username ?? string.Empty)
                        .WithUseEmbeddedWebView(true)
                        .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                        .ExecuteAsync()
                        .ConfigureAwait(false);
                }
                else
                {
                    SystemWebViewOptions systemWebViewOptions = new();
#if IOS
                    // Hide the privacy prompt in iOS
                    systemWebViewOptions.iOSHidePrivacyPrompt = true;
#endif
                    return await PublicClientApplication.AcquireTokenInteractive(scopes)
                        .WithLoginHint(existingAccount?.Username ?? string.Empty)
                        .WithSystemWebViewOptions(systemWebViewOptions)
                        .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                        .ExecuteAsync()
                        .ConfigureAwait(false);
                }
            }

            // If the operating system does not have UI (e.g. SSH into Linux), you can fallback to device code,
            // however this flow will not satisfy the "device is managed" CA policy.
            return await PublicClientApplication.AcquireTokenWithDeviceCode(scopes, (dcr) =>
            {
                Console.WriteLine(dcr.Message);
                return Task.CompletedTask;
            }).ExecuteAsync().ConfigureAwait(false);
        }

        public async Task<string> SignInUserAndAcquireAccessToken(string[]? scopes)
        // Signs in the user and obtains an Access token for a provided set of scopes
        {
            if (PublicClientApplication == null)
                throw new NullReferenceException();

            var existingUser = await FetchSignedInUserFromCache().ConfigureAwait(false);
            try
            {
                // 1. Try to sign-in the previously signed-in account
                if (existingUser != null)
                {
                    AuthResult = await PublicClientApplication.AcquireTokenSilent(scopes, existingUser)
                        .ExecuteAsync().ConfigureAwait(false);
                }
                else
                {
                    if (IsBrokerInitialized)
                    {
                        Console.WriteLine("No accounts found in the cache. Trying Window's default account.");
                        AuthResult = await PublicClientApplication.AcquireTokenSilent(scopes,
                            Microsoft.Identity.Client.PublicClientApplication.OperatingSystemAccount)
                            .ExecuteAsync().ConfigureAwait(false);
                    }
                    else AuthResult = await SignInUserInteractivelyAsync(scopes);
                }
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync.
                // This indicates you need to call AcquireTokenInteractive to acquire a token interactively
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                AuthResult = await PublicClientApplication.AcquireTokenInteractive(scopes)
                    .WithLoginHint(existingUser?.Username ?? string.Empty)
                    .ExecuteAsync().ConfigureAwait(false);
            }
            catch (MsalException msalEx)
            {
                Debug.WriteLine($"Error Acquiring Token interactively:{Environment.NewLine}{msalEx}");
                throw;
            }

            return AuthResult.AccessToken;
        }

        public async Task<string?> SignInUserAndAcquireAccessToken(string[] scopes, string extraclaims)
        //The extra claims, usually from CAE. We basically handle CAE by sending the user back to Azure AD for
        //additional processing and requesting a new access token for Graph
        {
            if (PublicClientApplication == null)
                throw new NullReferenceException();

            try
            {
                // Send the user to Azure AD for re-authentication as a silent acquisition wont resolve any
                // CAE scenarios like an extra claims request
                AuthResult = await PublicClientApplication.AcquireTokenInteractive(scopes)
                        .WithClaims(extraclaims)
                        .ExecuteAsync()
                        .ConfigureAwait(false);
            }
            catch (MsalException msalEx)
            {
                Debug.WriteLine($"Error Acquiring Token:{Environment.NewLine}{msalEx}");
            }

            return AuthResult?.AccessToken;
        }

        public async Task<IAccount?> FetchSignedInUserFromCache()
        // Fetches the signed in user from MSAL's token cache (if available).
        {
            if (PublicClientApplication == null)
                throw new NullReferenceException();

            // get accounts from cache
            IEnumerable<IAccount> accounts = await PublicClientApplication.GetAccountsAsync().ConfigureAwait(false);

            // Error corner case: we should always have 0 or 1 accounts, not expecting > 1
            // This is just an example of how to resolve this ambiguity, which can arise if more apps share a token cache.
            // Note that some apps prefer to use a random account from the cache.
            if (accounts.Count() > 1)
            {
                foreach (var acc in accounts)
                { await PublicClientApplication.RemoveAsync(acc); }
                return null;
            }
            return accounts.SingleOrDefault();
        }

        public async Task SignOutUserAsync()
        // Removes the first signed-in user's record from token cache
        {
            var existingUser = await FetchSignedInUserFromCache().ConfigureAwait(false);
            if (existingUser != null)
                await SignOutUserAsync(existingUser).ConfigureAwait(false);
        }

        public async Task SignOutUserAsync(IAccount user)
        // Removes a given user's record from token cache
        {
            if (PublicClientApplication == null) return;
            await PublicClientApplication.RemoveAsync(user).ConfigureAwait(false);
        }
    }
}
