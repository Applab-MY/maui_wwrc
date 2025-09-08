namespace wwrc_maui.Content.Model
{
    public class PasswordModel
    {
        public class ChangePasswordModel { }

        public class API_ChangePasswordModel
        {
            public string DBase { get; set; } = "";
            public string CurrentPassword { get; set; } = "";
            public string NewPassword { get; set; } = "";
            public string ConfirmPassword { get; set; } = "";
        }
    }
}