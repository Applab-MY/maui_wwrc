namespace wwrc_maui.Content.MsalClient
{
    public class AzureConfig
    {
        public string ClientId { get; set; } = "aeb29272-0767-4ba2-ab45-4e231cc40d3a";
        public string RedirectUri { get; set; } = "msalaeb29272-0767-4ba2-ab45-4e231cc40d3a";

        /// The tenant identifier (tenant Id/directory id) of the Azure AD tenant
        /// where the app registration exists
        /// you can use a name as obtained from the azure portal, e.g. kko365.onmicrosoft.com
        public string TenantId { get; set; } = "0539903d-170b-4b82-a144-e9057d68117c";

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
