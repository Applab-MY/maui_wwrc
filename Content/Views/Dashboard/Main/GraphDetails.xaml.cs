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
        viewmodel.OnFinishLoad += (data) => { salesView.Itemsource = viewmodel.filterPerson; };
        entry_search.OnTextCleared += () => { viewmodel.SearchTxt = ""; viewmodel.SearchSales(); };
        BindingContext = viewmodel;
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
            if (okay)
            {
                if (salesView.Selected != null)
                {
                    viewmodel.SalesPerson = salesView.Selected.Id;
                    viewmodel.isFilterSales = true;
                    viewmodel.Initialize();
                }
            }
            else
            {
                // for reset all checked item
                viewmodel.isFilterSales = false;
                if (salesView.Itemsource.Count > 0)
                    foreach (var item in salesView.Itemsource) { item.Checked = false; }
                if (salesView.MainSource.Count > 0)
                    foreach (var item in salesView.MainSource) { item.Checked = false; }
            }
        }
        await App.DisplayAlert("Sales Person", null, salesView, "Okay", "Cancel", closeAction);
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }
}