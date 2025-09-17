using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using wwrc_maui.Content.Views.Auth;
using static wwrc_maui.Content.Model.PasswordModel;

namespace wwrc_maui.Content.Viewmodels.Profile
{
    public class ChangePasswordVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private string _currentPwd = "";
        private string _newPwd = "";
        private string _confirmPwd = "";
        private double _entryWidth = 0.0;
        #endregion
        #region properties
        public string CurrentPwd
        {
            get { return _currentPwd; }
            set { SetProperty(ref _currentPwd, value); }
        }
        public string NewPwd
        {
            get { return _newPwd; }
            set { SetProperty(ref _newPwd, value); }
        }
        public string ConfirmPwd
        {
            get { return _confirmPwd; }
            set { SetProperty(ref _confirmPwd, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        public Command? SubmitCommand { get; set; } = null;
        public Action<bool>? SubmitAction { get; set; } = null;

        public ChangePasswordVm()
        {
            IsBusy = false;
            EntryWidth = App.ScreenWidth - 40;
            SubmitCommand = new Command(ChangePassword);
        }

        async void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(CurrentPwd))
                await App.DisplayAlert("Empty!", "Please insert current password", null, "Okay");
            else if (string.IsNullOrWhiteSpace(NewPwd))
                await App.DisplayAlert("Empty!", "Please insert new password", null, "Okay");
            else if (string.IsNullOrWhiteSpace(ConfirmPwd))
                await App.DisplayAlert("Empty!", "Please insert confirm password", null, "Okay");
            else
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                if (accessType == NetworkAccess.Internet && App.AppClient != null)
                {
                    var model = new API_ChangePasswordModel
                    {
                        CurrentPassword = CurrentPwd,
                        NewPassword = NewPwd,
                        ConfirmPassword = ConfirmPwd,
                    };
                    var _res = await App.AppClient.ChangePassword(model);
                    if (_res.SystemCode == 401)
                    {
                        AppDatabase.Instance.DeleteAllData();
                        Preferences.Default.Clear();
                        await App.DisplayAlert("Relogin", "Please login again", null, "Okay");
                        Application.Current?.Dispatcher.Dispatch(() =>
                        { Application.Current.Windows[0].Page = new Login(); });
                    }
                    else if (_res.SystemCode == 200)
                    {
                        await App.DisplayAlert("Success", "Changed Password Completed", null, "Okay");
                        SubmitAction?.Invoke(true);
                    }
                    else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                        + ". " + _res.SystemMessage, null, "Okay");
                }
            }
        }
    }
}
