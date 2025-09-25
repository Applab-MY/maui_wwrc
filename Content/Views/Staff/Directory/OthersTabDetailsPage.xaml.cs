using wwrc_maui.Content.Viewmodels.Staff;
using static wwrc_maui.Content.Model.StaffModel;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class OthersTabDetailsPage : ContentPage
{
    StaffDetailsVm viewmodel = new();
    CountryListCell countryView = new();

    public OthersTabDetailsPage(string countryCode)
    {
        InitializeComponent();
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
}