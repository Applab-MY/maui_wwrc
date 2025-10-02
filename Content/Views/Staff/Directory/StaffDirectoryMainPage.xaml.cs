using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Viewmodels.Staff;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static wwrc_maui.Content.Helper.ReferenceMessenger;
using static wwrc_maui.Content.Model.CountryModel;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class StaffDirectoryMainPage : ContentPage
{
    public StaffDirectoryVm viewmodel = new();

    public StaffDirectoryMainPage(bool isSelectable = false)
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.DoSearch();
        };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.DoSearch(); };
        viewmodel.IsSelectable = isSelectable;
        BindingContext = viewmodel;
        Initialize();

        #region messenger
        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, (receiver, message) =>
        {
            if (message.Value.Key.Equals("StaffTabSelected"))
            { SetTabContent(Convert.ToInt32(message.Value.Value)); }
        });
        WeakReferenceMessenger.Default.Register<StringNotify>(this, async (receiver, message) =>
        {
            if (message.Value != null && message.Value.Equals("CustomerVisitSelectAttendees"))
            { await Navigation.PopAsync(); }
        });
        #endregion
    }

    public void Initialize()
    {
        tabbar.Children.Clear();
        tabbar.ColumnDefinitions.Clear();
        if (viewmodel.tabList.Count > 0)
        {
            viewmodel.selectedStaff = [];
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
            if (viewmodel.IsSelectable) { viewmodel.CheckForSelected(); }
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
            btn_select.IsVisible = viewmodel.IsSelectable;
            btn_clear.IsVisible = viewmodel.IsSelectable;
        }
        else if (tabId == 2)
        {
            content_tab1.IsVisible = false;
            content_tab2.IsVisible = true;
            entry_search.Placeholder = "search country...";
            if (!viewmodel.isOtherLoad) viewmodel.GetCountryList();
            btn_select.IsVisible = false;
            btn_clear.IsVisible = false;
        }
    }

    private void OfficeList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        //var data = (StaffMainModel)e.Item;
        //navigation move to viewcell level
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

    private async void OtherList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null; //OthersTabDetailsPage
        var data = (CountryMainModel)e.Item;
        await Navigation.PushAsync(new OthersTabDetailsPage(data.CountryCode, viewmodel.IsSelectable));
    }

    private async void OnSelect_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button view) return;
        if (viewmodel.selectedStaff.Count > 0)
        {
            var _res = await viewmodel.SaveSelectedStaffToDb();
            if (_res)
            {
                await Navigation.PopAsync();
                WeakReferenceMessenger.Default.Send(new StringNotify("CustomerVisitSelectAttendees"));
            }
        }
        else await App.DisplayAlert("Empty", "No staff selected as attendee", null, "Okay");
    }

    private async void OnClear_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button view) return;
        viewmodel.ClearSelectedStaffDb();
        await Navigation.PopAsync();
        WeakReferenceMessenger.Default.Send(new StringNotify("CustomerVisitSelectAttendees"));
    }
}