using static wwrc_maui.Content.Model.ProductModel;

namespace wwrc_maui.Content.Views.Media.Product;

public partial class ProductDetailsCell : ContentView
{
	public ProductMainModel? model { get; set; } = null;

    public ProductDetailsCell()
	{
		InitializeComponent();
		BindingContext = this;
	}

	public void Initialize()
	{
		if (model != null)
		{
			img_file.Source = model.FileImage;
			lbl_title.Text = model.Title;
			lbl_size.Text = model.Filesize;
			lbl_type.Text = model.FileType;
			lbl_createdt.Text = model.CreateDate;
			lbl_createby.Text = model.CreateByName;
        }
	}
}