using static wwrc_maui.Content.Model.ProductModel;

namespace wwrc_maui.Content.Views.Media.Product;

public partial class ProductCell : ViewCell
{
    public ProductCell()
    {
        InitializeComponent();
    }

    private async void OnStatus_Tapped(object sender, EventArgs e)
    {
        if (sender is not StackLayout view) return;
        await view.ScaleTo(0.9, 100);
        view.Scale = 1;

        var tap = (TapGestureRecognizer)view.GestureRecognizers[0];
        var model = tap.CommandParameter as ProductMainModel;
        var parent = (ProductMainPage)Parent.Parent.Parent.Parent.Parent;
        await parent.OpenDetailsView(model);
    }
}