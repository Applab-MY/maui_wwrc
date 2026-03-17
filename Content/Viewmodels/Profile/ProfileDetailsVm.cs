using CommunityToolkit.Mvvm.Messaging;
using Plugin.Media;
using Plugin.Media.Abstractions;
using wwrc_maui.Content.Helper;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Helper.ReferenceMessenger;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.StaffModel;

namespace wwrc_maui.Content.Viewmodels.Profile
{
    public class ProfileDetailsVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        string _primary = "";
        string _picture = "";
        ImageSource? _imgSource = null;
        string _dob = "";
        LoginMainModel? _login = null;
        bool _isEditing = false;
        string _contactNo = "";
        string _officeNo = "";
        private double _entryWidth = 0.0;
        #endregion
        #region properties
        public string PrimaryColor
        {
            get { return _primary; }
            set { SetProperty(ref _primary, value); }
        }
        public string Picture
        {
            get { return _picture; }
            set { SetProperty(ref _picture, value); }
        }
        public ImageSource? ImgSource
        {
            get { return _imgSource; }
            set { SetProperty(ref _imgSource, value); }
        }
        public string Dob
        {
            get { return _dob; }
            set { SetProperty(ref _dob, value); }
        }
        public LoginMainModel? LoginData
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }
        public bool IsEditing
        {
            get { return _isEditing; }
            set { SetProperty(ref _isEditing, value); }
        }
        public string ContactNoEdit
        {
            get { return _contactNo; }
            set { SetProperty(ref _contactNo, value); }
        }
        public string OfficeNoEdit
        {
            get { return _officeNo; }
            set { SetProperty(ref _officeNo, value); }
        }
        public double EntryWidth
        {
            get { return _entryWidth; }
            set { SetProperty(ref _entryWidth, value); }
        }
        #endregion
        #endregion

        string fileName = "";

        public ProfileDetailsVm()
        {
            IsBusy = false;
            var _clr = Application.Current?.Resources["Primary"] as Color;
            if (_clr != null) { PrimaryColor = _clr.ToArgbHex(); }
            EntryWidth = (App.ScreenWidth - 45) / 2;
        }

        public async void SetupData()
        {
            try
            {
                ContactNoEdit = ""; OfficeNoEdit = ""; ImgSource = null;
                LoginData = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                    ("Select * from LoginMainModel").FirstOrDefault();
                if (LoginData != null && LoginData.UserData != null)
                {
                    Picture = string.IsNullOrEmpty(LoginData.UserData.ProfileImage) ?
                        "ic_user_empty" : LoginData.UserData.ProfileImage;

                    if (!string.IsNullOrEmpty(LoginData.UserData.ContactNo))
                        ContactNoEdit = LoginData.UserData.ContactNo;
                    if (!string.IsNullOrEmpty(LoginData.UserData.OfficeNo))
                        OfficeNoEdit = LoginData.UserData.OfficeNo;

                    var _dt = Convert.ToDateTime(LoginData.UserData.DOB).ToLocalTime();
                    if (_dt.Year != 1 && _dt.Month != 1 && _dt.Day != 1)
                    {
                        var dtConvert = _dt.Add(new TimeSpan(-8, 0, 0));
                        Dob = dtConvert.ToString("yyyy-MM-dd");
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                await App.DisplayAlert("Exception", error, null, "Okay");
            }
        }

        public async Task GetStaffDetails()
        {
            await Task.Delay(300);
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                ImgSource = null;
                var model = new API_StaffModel
                {
                    Id = "", //Preferences.Default.Get("userId", ""),
                    Country = "", //Preferences.Default.Get("country", ""),
                };
                var _res = await App.AppClient.Staff(model);
                if (_res.SystemCode == 401) { }
                else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count > 0
                    && LoginData?.UserData != null)
                {
                    var found = _res.items.Where(x => x.Id.Equals(LoginData.UserData.Id)).FirstOrDefault();
                    if (found != null && LoginData?.UserData != null)
                    {
                        if (!string.IsNullOrEmpty(found.ProfileImage)) { } //mvvm
                        var query = "Update Userinfo Set ProfileImage='" + found.ProfileImage + "', ContactNo='"
                            + found.ContactNo + "', OfficeNo='" + found.OfficeNo + "' Where Id='"
                            + found.Id + "'";
                        AppDatabase.Instance.SqlConnection.Query<Userinfo>(query);
                        WeakReferenceMessenger.Default.Send(new StringNotify("RefreshProfilePicture"));
                        SetupData();
                    }
                }
                else if (_res.SystemCode == 200 && _res.items != null && _res.items.Count == 0)
                { } //bugfix :: sometimes api success but return null items
                else await App.DisplayAlert("Error: " + _res.SystemCode.ToString(), _res.SystemDebugMessage
                    + ". " + _res.SystemMessage, null, "Okay");
            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
        }

        public async void TakePhoto()
        {
            ImgSource = null;
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.DisplayAlert("No Camera", "This device have no camera.", null, "Okay");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                DefaultCamera = CameraDevice.Front,
                AllowCropping = true,
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 20
            });

            if (file == null) return;
            ImgSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            fileName = file.AlbumPath.Split('/').Last();
        }

        public async void PickGallery()
        {
            ImgSource = null;
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.DisplayAlert("Info", "Pick photo from gallery not supported.", null, "Okay");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            { PhotoSize = PhotoSize.Custom, CustomPhotoSize = 20 });

            if (file == null) return;
            ImgSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            fileName = file.Path.Split('/').Last();
        }

        public async Task<bool> SaveProfile()
        {
            IsBusy = true;
            await Task.Delay(300);
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                var content = new MultipartFormDataContent();
                if (ImgSource != null)
                {
                    var data = MediaConverter.ImageSourceToByteArray(ImgSource);
                    var toArray = new ByteArrayContent(data);
                    content.Add(toArray, "ProfileImage", fileName);
                }

                var PhoneNumber = new StringContent(ContactNoEdit);
                var OfficeNumber = new StringContent(OfficeNoEdit);
                content.Add(PhoneNumber, "ContactNo");
                content.Add(OfficeNumber, "OfficeNo");

                var _res = await App.AppClient.ProfileUpdate(content);
                IsBusy = false; return _res;
            }
            else
            {
                await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
                IsBusy = false; return false;
            }
        }
    }
}
