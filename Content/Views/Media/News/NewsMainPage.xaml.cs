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
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.SearchNews();
        };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchNews(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.GetNewsList();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var model = (NewsTable)e.Item;
        await Navigation.PushAsync(new NewsDetailsPage(model.Id));
    }
}