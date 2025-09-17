using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Viewmodels.Media.Gallery;
using static wwrc_maui.Content.Helper.ReferenceMessenger;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class GalleryMainPage : ContentPage
{
    GalleryVm viewmodel = new();

    public GalleryMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;

        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, (receiver, message) =>
        {
            if (message.Value.Key.Equals("MediaTabSelected"))
            { SetTabContent(Convert.ToInt32(message.Value.Value)); }
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        tabbar.Children.Clear();
        tabbar.ColumnDefinitions.Clear();
        viewmodel.GetAllAlbums();
        if (viewmodel.tabList.Count > 0)
        {
            int index = 1; bool selected = false;
            foreach (var item in viewmodel.tabList)
            {
                tabbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                if (index == 1) selected = true;
                else selected = false;
                var _view = new TabCell
                { TabId = index, Title = item, IsSelected = selected };
                tabbar.Add(_view, index-1, 0);
                index++;
            }
            SetTabContent(1);
        }
    }

    void SetTabContent(int tabId)
    {
        view_content.Content = null;
        if (tabId == 1) view_content.Content = new GalleryPhotoTab(viewmodel);
        else if (tabId == 2) view_content.Content = new GalleryVideoTab(viewmodel);
    }
}