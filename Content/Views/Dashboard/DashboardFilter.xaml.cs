using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using wwrc_maui.Content.Viewmodels.Dashboard;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class DashboardFilter : ContentPage
{
    DashboardFilterVm viewmodel = new();

    public DashboardFilter(DashboardMainModel? model = null, FilterDataModel? filterModel = null)
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        viewmodel.MainModel = model;
        viewmodel.FilterModel = filterModel;
        BindingContext = viewmodel;

        viewmodel.OnClearFilterTap += async () =>
        { SetupView(); await Task.Delay(500); await Navigation.PopAsync(); };
        viewmodel.OnRefreshTap += async () => { await Navigation.PopAsync(); };
    }

    protected override void OnAppearing()
    {
        viewmodel.IsBusy = true;
        base.OnAppearing();
        SetupView();
        viewmodel.IsBusy = false;
    }

    public void SetupView()
    {
        viewmodel.IsSubsVisible = false;
        viewmodel.SetupCountry();
        if (viewmodel.FilterModel != null && !string.IsNullOrEmpty(viewmodel.FilterModel.Country))
        {
            BuildCountryList(viewmodel.FilterModel.Country);
            viewmodel.SetupSubsidiary(viewmodel.FilterModel.Country);
            if (viewmodel.FilterModel != null && !string.IsNullOrEmpty(viewmodel.FilterModel.Subsidiary))
            { BuildSubsidiaryList(viewmodel.FilterModel.Subsidiary); }
            else BuildSubsidiaryList();
        }
        else BuildCountryList();

        if (viewmodel.FilterModel != null && !string.IsNullOrEmpty(viewmodel.FilterModel.UserId))
        { viewmodel.IsSalesVisible = !string.IsNullOrEmpty(viewmodel.FilterModel.UserId); }
    }

    #region UI elements
    public void BuildCountryList(string? country = null)
    {
        grid_country.Children.Clear();
        grid_country.RowDefinitions.Clear();
        grid_country.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        if (viewmodel.CountryList.Count > 0)
        {
            int row = 0, col = 0;
            foreach (var item in viewmodel.CountryList)
            {
                #region UI elements
                var _clr = Application.Current?.Resources["Gray600"] as Color;
                var _style = Application.Current?.Resources["PopupPromptButton"] as Style;
                var _theme = Application.Current?.RequestedTheme;
                var container = new Border
                {
                    BackgroundColor = _theme == AppTheme.Light ? Colors.White : _clr,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Stroke = Colors.LightGray,
                    Padding = new Thickness(15, 10),
                };
                var txt = new Label
                {
                    Text = item,
                    FontSize = 12,
                    TextColor = _theme == AppTheme.Light ? Colors.Black : Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Style = _style
                };
                container.Content = txt;
                if (country != null && item.Equals(country))
                { container.BackgroundColor = Application.Current?.Resources["Primary"] as Color; }

                var tap = new TapGestureRecognizer();
                tap.Tapped += OnCountry_Tapped;
                container.GestureRecognizers.Add(tap);
                #endregion

                if (col < 4)
                {
                    grid_country.Add(container, col, row);
                    col++;
                }
                else
                {
                    row++; col = 0;
                    grid_country.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid_country.Add(container, col, row);
                    col++;
                }
            }
        }
    }

    public void BuildSubsidiaryList(string? subs = null)
    {
        grid_subs.Children.Clear();
        grid_subs.RowDefinitions.Clear();
        grid_subs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        if (viewmodel.FilterModel != null)
            viewmodel.IsSubsVisible = !string.IsNullOrEmpty(viewmodel.FilterModel.Country);

        if (viewmodel.SubsList.Count > 0)
        {
            int row = 0, col = 0;
            foreach (var item in viewmodel.SubsList)
            {
                #region UI elements
                var _clr = Application.Current?.Resources["Gray600"] as Color;
                var _style = Application.Current?.Resources["PopupPromptButton"] as Style;
                var _theme = Application.Current?.RequestedTheme;
                var container = new Border
                {
                    BackgroundColor = _theme == AppTheme.Light ? Colors.White : _clr,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Stroke = Colors.LightGray,
                    Padding = new Thickness(15, 10),
                };
                var txt = new Label
                {
                    Text = item,
                    FontSize = 12,
                    MaxLines = 1,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    TextColor = _theme == AppTheme.Light ? Colors.Black : Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Style = _style
                };
                container.Content = txt;
                if (subs != null && item.Equals(subs))
                { container.BackgroundColor = Application.Current?.Resources["Primary"] as Color; }

                var tap = new TapGestureRecognizer();
                tap.Tapped += OnSubsidiary_Tapped;
                container.GestureRecognizers.Add(tap);
                #endregion

                if (col < 2)
                {
                    grid_subs.Add(container, col, row);
                    col++;
                }
                else
                {
                    row++; col = 0;
                    grid_subs.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid_subs.Add(container, col, row);
                    col++;
                }
            }
        }
    }

    public void ResetViewCountry(string country)
    {
        if (grid_country.Children.Count > 0)
        {
            foreach (var child in grid_country.Children)
            {
                if (child is Border view)
                {
                    view.BackgroundColor = Application.Current?.RequestedTheme == AppTheme.Light ?
                        Colors.White : Application.Current?.Resources["Gray600"] as Color;
                    var txt = view.Content as Label;
                    if (txt != null && txt.Text.Equals(country))
                    { view.BackgroundColor = Application.Current?.Resources["Primary"] as Color; }
                }
            }
        }
    }

    public void ResetViewSubsidiary(string subs)
    {
        if (grid_subs.Children.Count > 0)
        {
            foreach (var child in grid_subs.Children)
            {
                if (child is Border view)
                {
                    view.BackgroundColor = Application.Current?.RequestedTheme == AppTheme.Light ?
                        Colors.White : Application.Current?.Resources["Gray600"] as Color;
                    var txt = view.Content as Label;
                    if (txt != null && txt.Text.Equals(subs))
                    { view.BackgroundColor = Application.Current?.Resources["Primary"] as Color; }
                }
            }
        }
    }
    #endregion

    #region tap events
    private async void OnCountry_Tapped(object? sender, TappedEventArgs e)
    {
        if (sender != null)
        {
            if (sender is not Border view) return;
            var txt = view.Content as Label;
            await view.FadeTo(0.3, 200);
            view.Opacity = 1;

            if (txt != null)
            {
                ResetViewCountry(txt.Text);
                if (viewmodel.FilterModel != null)
                    viewmodel.FilterModel.Country = txt.Text;
                viewmodel.SetupSubsidiary(txt.Text);
                BuildSubsidiaryList();
            }
        }
    }

    private async void OnSubsidiary_Tapped(object? sender, TappedEventArgs e)
    {
        if (sender != null)
        {
            if (sender is not Border view) return;
            var txt = view.Content as Label;
            await view.FadeTo(0.3, 200);
            view.Opacity = 1;

            if (txt != null)
            {
                ResetViewSubsidiary(txt.Text);
                if (viewmodel.FilterModel != null)
                {
                    viewmodel.FilterModel.Subsidiary = txt.Text;
                    viewmodel.SetupSalesList(viewmodel.FilterModel.Country,
                        viewmodel.FilterModel.Subsidiary);
                }
            }
        }
    }

    private async void OnSalesPerson_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;

        if (!viewmodel.IsSubsVisible)
            await App.DisplayAlert("Info", "Please select country and subsidiary first.", null, "Okay");
        else if (string.IsNullOrEmpty(viewmodel.FilterModel?.Subsidiary))
            await App.DisplayAlert("Info", "Please select a subsidiary.", null, "Okay");
        else
        {
            var _view = new FilterSalesPersonView
            { Itemsource = viewmodel.SalesList };
            void closeAction(bool okay)
            {
                if (okay)
                {
                    if (_view.Selected != null)
                    {
                        viewmodel.FilterModel!.UserId = _view.Selected.Id;
                        viewmodel.FilterModel!.UserName = _view.Selected.Id;
                    }
                }
            }
            await App.DisplayAlert("Sales Person", null, _view, "Okay", "Cancel", closeAction);
        }
    }
    #endregion
}