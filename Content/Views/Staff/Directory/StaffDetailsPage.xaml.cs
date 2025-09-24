using wwrc_maui.Content.Viewmodels.Staff;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class StaffDetailsPage : ContentPage
{
    StaffDetailsVm viewmodel = new();

    public StaffDetailsPage(string staffId)
    {
        InitializeComponent();
        viewmodel.staffId = staffId;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetStaffDetails();
    }

    private async void OnCall_Tapped(object sender, EventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.9, 100);
        view.Scale = 1;
    }

    private async void OnEmail_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.FadeTo(0.9, 100);
        view.Scale = 1;
    }
}