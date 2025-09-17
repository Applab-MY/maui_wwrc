using System.ComponentModel;
using System.Runtime.CompilerServices;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Dashboard;
using wwrc_maui.Content.Views.Auth;
using wwrc_maui.Content.Views.Profile.ChangePassword;
using wwrc_maui.Content.Views.Profile.Details;
using static wwrc_maui.Content.Model.ProfileModel;

namespace wwrc_maui.Content.Views.Profile;

public partial class ProfileOptionPage : ContentView, INotifyPropertyChanged
{
    #region bindables props
    #region beans
    string _name = "";
    string _position = "";
    string _picture = "";
    bool _isOfficeCreds = false;
    #endregion
    #region props
    public string Name
    {
        get { return _name; }
        set { SetProperty(ref _name, value); }
    }
    public string Position
    {
        get { return _position; }
        set { SetProperty(ref _position, value); }
    }
    public string Picture
    {
        get { return _picture; }
        set { SetProperty(ref _picture, value); }
    }
    public bool IsOfficeCredential
    {
        get { return _isOfficeCreds; }
        set
        {
            SetProperty(ref _isOfficeCreds, value);
            SetMenuList(value);
        }
    }
    #endregion
    protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action? onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null) return;
        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    List<OptionPageModel> Items = [];
    MainPageVm? ParentPage = null;

    public ProfileOptionPage(MainPageVm? parent = null)
    {
        InitializeComponent();
        ParentPage = parent;
        BindingContext = this;
    }

    void SetMenuList(bool isOffice)
    {
        Items.Add(new() { Index = 0, Name = "My Profile", Desc = "Information of user", Image = "menu_profile" });
        if (!isOffice)
            Items.Add(new() { Index = 1, Name = "Change Password", Desc = "Update password here", Image = "menu_reset_password" });
        listview.ItemsSource = Items;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView list) return;
        list.SelectedItem = null;
        var item = e.Item as OptionPageModel;
        if (item != null)
        {
            if (item.Index == 0) await Navigation.PushAsync(new ProfileDetailsMainPage());
            if (item.Index == 1) await Navigation.PushAsync(new ChangePasswordMainPage());
        }
    }

    private async void OnLogout_Tapped(object sender, EventArgs e)
    {
        if (ParentPage != null)
        {
            ParentPage.IsBusy = true;
            try
            {
                await Task.Delay(300);
                AppDatabase.Instance.DeleteAllData();
                Preferences.Default.Clear();
                Application.Current?.Dispatcher.Dispatch(() =>
                { Application.Current.Windows[0].Page = new Login(); });
            }
            catch (Exception ex)
            { await App.DisplayAlert("Error", ex.Message, null, "Okay"); }
            ParentPage.IsBusy = false;
        }
    }
}