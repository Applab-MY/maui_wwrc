using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Reflection;
using System.Runtime.CompilerServices;
using wwrc_maui.Content.MsalClient.MSGraph;

namespace wwrc_maui.Content.MsalClient
{
    public class PublicClientSingleton
    {
        #region getter setter
        /// This is the singleton used by Ux. Since PublicClientWrapper constructor does not have
        /// perf or memory issue, it is instantiated directly.

        private static readonly Lazy<PublicClientSingleton> singleton = new(() => new PublicClientSingleton());
        public static PublicClientSingleton Instance { get { return singleton.Value; } }

        /// This is the configuration for the application found within the 'appsettings.json' file.
        private static IConfiguration? AppConfiguration = null;

        /// Gets the instance of MSALClientHelper.
        public MSALClientHelper? MSALClientHelper { get; } = null;

        /// Gets the MSGraphHelper instance.
        public MSGraphHelper? MSGraphHelper { get; } = null;

        /// This will determine if the Interactive Authentication should be Embedded or System view
        public bool UseEmbedded { get; set; } = false;
        #endregion

        /// Prevents a default instance of the <see cref="PublicClientSingleton"/> class
        /// from being created. or a private constructor for singleton
        [MethodImpl(MethodImplOptions.NoInlining)]
        private PublicClientSingleton()
        {
            // Load config
            var assembly = Assembly.GetExecutingAssembly();
            string _filename = $"{Assembly.GetCallingAssembly().GetName().Name}.appsettings.json";
            using var stream = assembly.GetManifestResourceStream(_filename);
            if (stream != null)
            {
                AppConfiguration = new ConfigurationBuilder().AddJsonStream(stream).Build();
                if (AppConfiguration != null)
                {
                    var azureADConfig = AppConfiguration.GetSection("AzureAD").Get<AzureConfig>();
                    if (azureADConfig != null)
                    {
                        MSALClientHelper = new MSALClientHelper(azureADConfig);
                        var graphApiConfig = AppConfiguration.GetSection("MSGraphApi").Get<MSGraphApiConfig>();
                        if (graphApiConfig != null)
                            MSGraphHelper = new MSGraphHelper(graphApiConfig, MSALClientHelper);
                    }
                }
            }
        }

        public async Task<string?> AcquireTokenSilentAsync()
        { return await AcquireTokenSilentAsync(GetScopes()).ConfigureAwait(false); }

        public async Task<string> AcquireTokenSilentAsync(string[]? scopes)
        {
            if (MSALClientHelper != null)
                return await MSALClientHelper.SignInUserAndAcquireAccessToken(scopes).ConfigureAwait(false);
            else return "";
        }

        internal async Task<AuthenticationResult?> AcquireTokenInteractiveAsync(string[] scopes)
        // Perform the interactive acquisition of the token for the given scope
        {
            if (MSALClientHelper != null)
            {
                MSALClientHelper.UseEmbedded = UseEmbedded;
                return await MSALClientHelper.SignInUserInteractivelyAsync(scopes).ConfigureAwait(false);
            }
            else return null;
        }

        internal async Task SignOutAsync()
        // Signout the user and delete old GraphServiceClient
        {
            MSGraphHelper?.ResetGraphClientService();
            if (MSALClientHelper != null)
                await MSALClientHelper.SignOutUserAsync().ConfigureAwait(false);
        }

        internal string[]? GetScopes() { return MSGraphHelper?.MSGraphApiConfig.ScopesArray; }
    }
}