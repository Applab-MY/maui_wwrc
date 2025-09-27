using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
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
        public bool UseEmbedded { get; set; } = false; //the Interactive Authentication should be Embedded or System view

        public MSALClientHelper? MSALClientHelper = null;
        public MSGraphHelper? MSGraphHelper = null;
        public MSGraphApiConfig? msGraph = null;
        public GraphServiceClient? _graphServiceClient = null;
        #endregion

        public PublicClientSingleton()
        {
            MSALClientHelper = new MSALClientHelper();
            MSGraphHelper = new MSGraphHelper(MSALClientHelper);
            msGraph = new MSGraphApiConfig();
        }

        public async Task<string?> AcquireTokenSilentAsync()
        { return await AcquireTokenSilentAsync(GetScopes()).ConfigureAwait(false); }

        public async Task<string> AcquireTokenSilentAsync(string[]? scopes)
        {
            if (MSALClientHelper != null)
                return await MSALClientHelper.SignInUserAndAcquireAccessToken(scopes).ConfigureAwait(false);
            else return "";
        }

        public async Task<AuthenticationResult?> AcquireTokenInteractiveAsync()
        // Perform the interactive acquisition of the token for the given scope
        {
            if (MSALClientHelper != null)
            {
                MSALClientHelper.UseEmbedded = UseEmbedded;
                return await MSALClientHelper.SignInUserInteractivelyAsync(msGraph?.ScopesArray).ConfigureAwait(false);
            }
            else return null;
        }

        public async Task<User?> GetMeAsync()
        {
            if (_graphServiceClient == null)
                await SignInAndInitializeGraphServiceClient();

            User? graphUser = null;
            if (_graphServiceClient != null)
            {
                try
                {
                    graphUser = await _graphServiceClient.Me.GetAsync();
                }
                catch (ServiceException ex)
                {
                    var error = ex.Message;
                }
            }
            return graphUser;
        }

        private async Task<GraphServiceClient> SignInAndInitializeGraphServiceClient()
        {
            string token = "";
            if (MSALClientHelper != null && msGraph != null)
                token = await MSALClientHelper.SignInUserAndAcquireAccessToken(msGraph.ScopesArray);

            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _graphServiceClient = new GraphServiceClient(client);
            return _graphServiceClient;
        }

        internal async Task SignOutAsync()
        // Signout the user and delete old GraphServiceClient
        {
            MSGraphHelper?.ResetGraphClientService();
            if (MSALClientHelper != null)
                await MSALClientHelper.SignOutUserAsync().ConfigureAwait(false);
        }

        internal string[]? GetScopes() { return MSGraphHelper?.GraphScopes; }
    }
}