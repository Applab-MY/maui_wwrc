using wwrc_maui.Content.Viewmodels.Profile;

namespace wwrc_maui.Content.Views.Profile.Details;

public partial class ProfileDetailsMainPage : ContentPage
{
    ProfileDetailsVm viewmodel = new();

    public ProfileDetailsMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    private async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.SetupData();
    }

    private async void OnOpenCamera_Tapped(object sender, EventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.9, 100);
        view.Scale = 1;
        viewmodel.TakePhoto();
    }

    private async void OnBrowse_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.9, 100);
        view.Scale = 1;
        viewmodel.PickGallery();
    }

    private async void OnEdit_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button view) return;
        viewmodel.IsEditing = !viewmodel.IsEditing;
        if (viewmodel.IsEditing) //start editing
        {
            view.Text = "Save"; view.Style = default;
            Grid.SetColumnSpan(view, 1);
        }
        else //finish editing
        {
            var _style = Application.Current?.Resources["BtnTertiary"] as Style;
            btn_edit.Text = "Edit"; btn_edit.Style = _style;
            Grid.SetColumnSpan(btn_edit, 2);
            var _res = await viewmodel.SaveProfile();
            if (_res)
            {
                viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
                await viewmodel.GetStaffDetails();
                viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
            }
        }
    }

    private void OnCancel_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button view) return;
        viewmodel.IsEditing = false;
        var _style = Application.Current?.Resources["BtnTertiary"] as Style;
        btn_edit.Text = "Edit"; btn_edit.Style = _style;
        Grid.SetColumnSpan(btn_edit, 2);
        viewmodel.SetupData();
    }
}