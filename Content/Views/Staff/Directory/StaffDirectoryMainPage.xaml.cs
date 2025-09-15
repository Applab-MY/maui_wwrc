using wwrc_maui.Content.Viewmodels.Staff;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class StaffDirectoryMainPage : ContentPage
{
	StaffDirectoryVm viewmodel = new();

    public StaffDirectoryMainPage()
	{
		InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}