using wwrc_maui.Content.Viewmodels.Media.Product;
using static wwrc_maui.Content.Model.ProductModel;

namespace wwrc_maui.Content.Views.Media.Product;

public partial class ProductMainPage : ContentPage
{
    ProductVm viewmodel = new();

    public ProductMainPage(string folderId)
    {
        viewmodel.folderId = folderId;
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetProductCatalogList();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var model = e.Item as ProductMainModel;
        if (model != null)
        {
            if (model.IsFile) await Launcher.OpenAsync(new Uri(model.File));
            viewmodel.SetProductCatalogReadStatus(model.Id);
        }
    }

    public async Task OpenDetailsView(ProductMainModel? model)
    {
        var cell = new ProductDetailsCell { model = model };
        cell.Initialize();
        await App.DisplayAlert("Catalog Details", null, cell, "Close");
    }
}