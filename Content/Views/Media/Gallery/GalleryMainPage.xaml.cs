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
        viewmodel.isFinishLoad += (data) => { SetTabContent(1); };
        BindingContext = viewmodel;
        tab_photos.SetParentBinding(viewmodel);
        tab_videos.SetParentBinding(viewmodel);
        Initialize();

        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, (receiver, message) =>
        {
            if (message.Value.Key.Equals("MediaTabSelected"))
            { SetTabContent(Convert.ToInt32(message.Value.Value)); }
        });
    }

    public async void Initialize()
    {
        await Task.Delay(300);
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
        }
    }

    void SetTabContent(int tabId)
    {
        stack_tab1.IsVisible = false;
        stack_tab2.IsVisible = false;
        if (tabId == 1)
        {
            stack_tab1.IsVisible = true;
            tab_photos.BuildView();
        }
        else if (tabId == 2)
        {
            stack_tab2.IsVisible = true;
            tab_videos.BuildView();
        }
    }
}