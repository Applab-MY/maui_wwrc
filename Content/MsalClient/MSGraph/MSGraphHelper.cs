using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace wwrc_maui.Content.MsalClient.MSGraph
{
    public class MSGraphHelper
    {
        public MSALClientHelper? MSALClient = null;
        public readonly MSGraphApiConfig MSGraphApiConfig = new();
        public GraphServiceClient? _graphServiceClient = null;
        public string[] GraphScopes = [];

        public MSGraphHelper(MSALClientHelper? msalClientHelper)
        {
            ArgumentNullException.ThrowIfNull(msalClientHelper);
            MSALClient = msalClientHelper;
            GraphScopes = MSGraphApiConfig.ScopesArray;
        }

        public async Task<User?> GetMeAsync()
        {
            if (_graphServiceClient == null) { await SignInAndInitializeGraphServiceClient(); }
            User? graphUser = null;
            if (_graphServiceClient != null)
            {
                try
                {
                    // Call /me Api
                    graphUser = await _graphServiceClient.Me.GetAsync();
                }
                catch (ServiceException ex) when (ex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
                {
                    _graphServiceClient = await SignInAndInitializeGraphServiceClientPostCAE(ex);

                    // Call the /me endpoint of Graph again with a fresh token
                    graphUser = await _graphServiceClient.Me.GetAsync();
                }
            }
            return graphUser;
        }

        /// Resets the GraphClientService used by this class
        public void ResetGraphClientService() { _graphServiceClient = null; }

        private async Task<GraphServiceClient> SignInAndInitializeGraphServiceClient()
        {
            string? token = "";
            if (MSALClient != null)
                token = await MSALClient.SignInUserAndAcquireAccessToken(GraphScopes);
            return InitializeGraphServiceClientAsync(token);
        }

        private async Task<GraphServiceClient> SignInAndInitializeGraphServiceClientPostCAE(ServiceException ex)
        // Signs the in and initialize graph service client post a CAE event exception.
        // The Graph Service exception. Contains the header required to properly process a CAE event
        {
            // Get challenge from response of Graph API
            var claimChallenge = WwwAuthenticateParameters.GetClaimChallengeFromResponseHeaders(ex.ResponseHeaders);
            //for windows
            string? token = "";
            if (MSALClient != null)
                token = await MSALClient.SignInUserAndAcquireAccessToken(GraphScopes, claimChallenge);
            return InitializeGraphServiceClientAsync(token);
        }

        private GraphServiceClient InitializeGraphServiceClientAsync(string? token)
        // Bootstraps the MS Graph SDK with the provided token and returns it for use
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _graphServiceClient = new GraphServiceClient(client);
            return _graphServiceClient;
        }
    }
}
