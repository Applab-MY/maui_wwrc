namespace wwrc_maui.Content.MsalClient
{
    public class AzureConfig
    {
        public string ClientId { get; set; } = "";
        public string RedirectUri { get; set; } = "";

        /// The tenant identifier (tenant Id/directory id) of the Azure AD tenant
        /// where the app registration exists
        /// you can use a name as obtained from the azure portal, e.g. kko365.onmicrosoft.com
        public string TenantId { get; set; } = "";

        /// <value>
        /// The Azure AD authority URL.
        /// </value>
        /// <remarks>
        ///   - For Work or School account in your org, use your tenant ID, or domain
        ///   - for any Work or School accounts, use organizations
        ///   - for any Work or School accounts, or Microsoft personal account, use common
        ///   - for Microsoft Personal account, use consumers
        /// </remarks>
        public string Authority { get; set; } = "";
    }
}
