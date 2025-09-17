using System.Globalization;
using System.Text.RegularExpressions;
using wwrc_maui.Content.Viewmodels.Common;

namespace wwrc_maui.Content.Viewmodels.Auth
{
    public class ForgotPwdVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private string _email = "";
        private string _version = "";
        private double _entryWidth = 0.0;
        #endregion
        #region properties
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public Command? SubmitCommand { get; set; } = null;

        public ForgotPwdVm()
        {
            IsBusy = false;
            Version += "Ver." + AppInfo.VersionString;
            SubmitCommand = new Command(RequestPassword);
            EntryWidth = App.ScreenWidth - 40;
        }

        public async void RequestPassword()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                try
                {
                    MultipartFormDataContent content = [];
                    StringContent _email = new(Email);
                    content.Add(_email, "Email");
                    var res = await App.AppClient.ForgotPassword(content);
                    if (res) await App.DisplayAlert("Success", "Forgot password requested", null, "Okay");
                    else await App.DisplayAlert("Error", "Request password reset failed.", null, "Okay");
                }
                catch (Exception ex)
                {
                    await App.DisplayAlert("Error", ex.Message, null, "Okay");
                }
            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection", null, "Okay");
        }

        public async Task<bool> IsValidEmail()
        {
            try
            {
                var _email = Regex.Replace(Email, @"(@)(.+)$", DomainMapper, RegexOptions.None,
                    TimeSpan.FromMilliseconds(200));
                static string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
                return Regex.IsMatch(_email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException e)
            {
                await App.DisplayAlert("Error", e.Message, null, "Okay");
                return false;
            }
            catch (ArgumentException e)
            {
                await App.DisplayAlert("Error", e.Message, null, "Okay");
                return false;
            }
        }
    }
}
