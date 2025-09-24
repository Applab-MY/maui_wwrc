using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Viewmodels.Staff;
using static wwrc_maui.Content.Helper.ReferenceMessenger;
using static wwrc_maui.Content.Model.StaffModel;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class StaffDirectoryMainPage : ContentPage
{
    StaffDirectoryVm viewmodel = new();

    public StaffDirectoryMainPage()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.DoSearch();
        };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.DoSearch(); };
        BindingContext = viewmodel;
        Initialize();

        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, (receiver, message) =>
        {
            if (message.Value.Key.Equals("StaffTabSelected"))
            { SetTabContent(Convert.ToInt32(message.Value.Value)); }
        });
    }

    public void Initialize()
    {
        tabbar.Children.Clear();
        tabbar.ColumnDefinitions.Clear();
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
                tabbar.Add(_view, index - 1, 0);
                index++;
            }
            SetTabContent(1);
        }
    }

    void SetTabContent(int tabId)
    {
        viewmodel.selectedTab = viewmodel.tabList[tabId - 1];
        if (tabId == 1)
        {
            content_tab1.IsVisible = true;
            content_tab2.IsVisible = false;
            entry_search.Placeholder = "search staff...";
            if (!viewmodel.isOfficeLoad) viewmodel.GetOfficeStaffList();
        }
        else if (tabId == 2)
        {
            content_tab1.IsVisible = false;
            content_tab2.IsVisible = true;
            entry_search.Placeholder = "search country...";
            if (!viewmodel.isOtherLoad) viewmodel.GetCountryList();
        }
    }

    private async void OfficeList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (StaffMainModel)e.Item;
        await Navigation.PushAsync(new StaffDetailsPage(data.Id));
    }

    private void OfficeList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
        if (sender is not ListView lv) return;
        if (viewmodel.StaffList.Count > 0)
        {
            if (e.Item == viewmodel.StaffList.Last())
            { viewmodel.GetStaffListNextPage(); }
        }
    }

    private void OtherList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}