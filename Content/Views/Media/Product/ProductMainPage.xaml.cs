using wwrc_maui.Content.Viewmodels.Media.Product;

namespace wwrc_maui.Content.Views.Media.Product;

public partial class ProductMainPage : ContentPage
{
	ProductVm viewmodel = new();

    public ProductMainPage()
	{
		InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
    }
}