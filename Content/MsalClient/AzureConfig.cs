namespace wwrc_maui.Content.MsalClient
{
    public class AzureConfig
    {
        public string ClientId { get; set; } = "9be06a28-806c-430f-93d5-49cdb62fdc50";
        public string RedirectUri { get; set; } = "msal9be06a28-806c-430f-93d5-49cdb62fdc50";

        /// The tenant identifier (tenant Id/directory id) of the Azure AD tenant
        /// where the app registration exists
        /// you can use a name as obtained from the azure portal, e.g. kko365.onmicrosoft.com
        public string TenantId { get; set; } = "65656888-a7c0-4613-9dee-29d8556683b0";

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
