namespace wwrc_maui.Content.MsalClient.MSGraph
{
    public class MSGraphApiConfig
    {
        public string MSGraphBaseUrl { get; set; } = "https://graph.microsoft.com/v1.0";
        public string Scopes { get; set; } = "user.read";
        public string[] ScopesArray { get { return Scopes.Split(' '); } }
    }
}
