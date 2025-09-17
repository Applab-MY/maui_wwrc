using wwrc_maui.Content.Viewmodels.Media;
using static wwrc_maui.Content.Model.NewsModel;

namespace wwrc_maui.Content.Views.Media.News;

public partial class NewsMainPage : ContentPage
{
    NewsVm viewmodel = new();

    public NewsMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () => { viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible; };
        entry_search.OnTextChanged += viewmodel.SearchNews;
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetNewsList();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var model = (NewsTable)e.Item;
        await Navigation.PushAsync(new NewsDetailsPage(model.Id));
    }
}