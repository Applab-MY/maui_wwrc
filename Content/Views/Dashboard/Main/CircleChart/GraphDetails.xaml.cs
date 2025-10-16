using wwrc_maui.Content.Viewmodels.Dashboard;

namespace wwrc_maui.Content.Views.Dashboard.Main;

public partial class GraphDetails : ContentPage
{
    GraphDetailsVm viewmodel = new();
    FilterSalesPersonView salesView = new();

    public GraphDetails()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += () =>
        {
            viewmodel.IsSearchVisible = !viewmodel.IsSearchVisible;
            viewmodel.SearchTxt = ""; viewmodel.SearchSales();
        };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchSales(); };
        BindingContext = viewmodel;
        salesView.SetParentBinding(viewmodel);
        viewmodel.Initialize();
    }

    private async void OnSales_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not VerticalStackLayout view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        void closeAction(bool okay)
        {
            salesView.Reset();
            if (okay) { viewmodel.Initialize(); }
        }
        await App.DisplayAlert("Sales Person", null, salesView, "Okay", "Cancel", closeAction);
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}