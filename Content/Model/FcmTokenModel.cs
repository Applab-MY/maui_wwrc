namespace wwrc_maui.Content.Model
{
    public class FcmTokenModel
    {
        public FcmTokenModel() { }

        public class API_FcmTokenModel
        {
            public string Platform { get; set; } = "";
            public string IMEI { get; set; } = "";
            public string Token { get; set; } = "";
            public string NotificationChannel { get; set; } = "";
            public string Internal_Company { get; set; } = "";
            public string UserId { get; set; } = "";
        }
    }
}