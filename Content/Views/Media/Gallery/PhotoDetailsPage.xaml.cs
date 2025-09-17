namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class PhotoDetailsPage : ContentPage
{
	public PhotoDetailsPage()
	{
		InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
    }
}