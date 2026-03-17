using wwrc_maui.Content.Viewmodels.Media;

namespace wwrc_maui.Content.Views.Media.News;

public partial class NewsDetailsPage : ContentPage
{
    NewsVm viewmodel = new();
    string? newsId = "";

    public NewsDetailsPage(string? newsId = null)
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        this.newsId = newsId;
        BindingContext = viewmodel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await viewmodel.GetNewsById(newsId);
        await viewmodel.UpdateNewsReadStatus(newsId);
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    private void OnDownload_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        viewmodel.DownloadAttachment(newsId);
    }
}