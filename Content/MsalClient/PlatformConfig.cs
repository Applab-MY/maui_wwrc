namespace wwrc_maui.Content.MsalClient
{
    public class PlatformConfig
    {
        /// The singleton instance to hold data per platform
        public static PlatformConfig Instance { get; } = new PlatformConfig();

        /// Platform specific Redirect URI
        public string RedirectUri { get; set; } = "";

        /// Platform specific parent window
        public object? ParentWindow { get; set; } = null;

        // private constructor to ensure singleton
        private PlatformConfig() { }
    }
}
