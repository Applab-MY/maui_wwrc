using wwrc_maui.Content.Model;
using wwrc_maui.Content.Views.Media.EmpHandbook;
using wwrc_maui.Content.Views.Media.Gallery;
using wwrc_maui.Content.Views.Media.News;
using wwrc_maui.Content.Views.Media.Product;
using static wwrc_maui.Content.Model.Auth.LoginModel;
using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Views.Media;

public partial class MediaOptionPage : ContentView
{
    public int NewsCount { get; set; } = 0;
    public int MediaCount { get; set; } = 0;
    public int EmpHandbookCount { get; set; } = 0;
    public int ProductCount { get; set; } = 0;
    List<OptionPageModel> Items = [];

    public MediaOptionPage()
	{
		InitializeComponent();
        BindingContext = this;
        var _login = AppDatabase.Instance.SqlConnection.Query<LoginMainModel>
                ("Select * from LoginMainModel").FirstOrDefault();
        if (_login != null && _login.UserData != null)
        {
            var index = 0;
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.News)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "News",
                    Desc = "What's new from WWRC",
                    Image = "menu_news_update",
                    AlertCount = NewsCount
                });
                index++;
            }
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.MediaGallery)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Media Gallery",
                    Desc = "Photo and video gallery",
                    Image = "menu_media_gallery",
                    AlertCount = MediaCount,
                });
                index++;
            }
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.EmployeeHandbook)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Employee Handbook",
                    Desc = "New employee must read",
                    Image = "menu_handbook",
                    AlertCount = EmpHandbookCount,
                });
                index++;
            }
            if (_login.UserData.UserModules != null && _login.UserData.UserModules.ProductCatalogue)
            {
                Items.Add(new OptionPageModel()
                {
                    Index = index,
                    Name = "Product Catalogue",
                    Desc = "Download of full product here",
                    Image = "menu_user_guide",
                    AlertCount = ProductCount,
                });
            }
        }
        listview.ItemsSource = Items;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView list) return;
        list.SelectedItem = null;
        var item = e.Item as OptionPageModel;
        if (item != null)
        {
            if (item.Name.Equals("News")) await Navigation.PushAsync(new NewsMainPage());
            if (item.Name.Equals("Media Gallery")) await Navigation.PushAsync(new GalleryMainPage());
            if (item.Name.Equals("Employee Handbook")) await Navigation.PushAsync(new EmpHandbookMainPage("*", ""));
            if (item.Name.Equals("Product Catalogue")) await Navigation.PushAsync(new ProductMainPage(""));
        }
    }
}