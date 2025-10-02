using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Viewmodels.Staff;
using static wwrc_maui.Content.Helper.ReferenceMessenger;
using static wwrc_maui.Content.Model.StaffModel;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class OthersTabDetailsPage : ContentPage
{
    public StaffDetailsVm viewmodel = new();
    CountryListCell countryView = new();

    public OthersTabDetailsPage(string countryCode, bool isSelectable = false)
    {
        InitializeComponent();
        viewmodel.IsSelectable = isSelectable;
        viewmodel.countryCode = countryCode;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.SearchStaff();
        };
        viewmodel.OnFinishLoad += (data) => { countryView.Itemsource = viewmodel.countryList; };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchStaff(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetCountries();
        viewmodel.GetStaffList();
        if (viewmodel.IsSelectable) { viewmodel.CheckForSelected(); }
    }

    private async void OnPickCountry_Tapped(object sender, EventArgs e)
    {
        if (sender is not HorizontalStackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        void closeAction(bool okay)
        {
            if (okay)
            {
                if (countryView.Selected != null)
                {
                    viewmodel.SelectedCountry = countryView.Selected;
                    viewmodel.countryCode = countryView.Selected.CountryCode;
                    viewmodel.GetStaffList();
                }
            }
        }
        await App.DisplayAlert("Country", null, countryView, "Okay", "Cancel", closeAction);
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        var data = (StaffMainModel)e.Item;
        await Navigation.PushAsync(new StaffDetailsPage(data.Id));
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