using Microsoft.Maui.Controls;
using wwrc_maui.Content.Viewmodels.Staff;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class OthersTabDetailsPage : ContentPage
{
    StaffDetailsVm viewmodel = new();

    public OthersTabDetailsPage(string countryCode)
	{
		InitializeComponent();
        viewmodel.countryCode = countryCode;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () => { viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible; };
        //entry_search.OnTextChanged += viewmodel.SearchSalesOrder;
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.GetStaffList();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
        //var data = (StaffMainModel)e.Item;
        //await Navigation.PushAsync(new StaffDetailsPage(data.Id));
    }
}