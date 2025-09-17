using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Dashboard;
using wwrc_maui.Content.Views.Auth;
using wwrc_maui.Content.Views.Media;
using wwrc_maui.Content.Views.Media.EmpHandbook;
using wwrc_maui.Content.Views.Media.Gallery;
using wwrc_maui.Content.Views.Media.News;
using wwrc_maui.Content.Views.Media.Product;
using wwrc_maui.Content.Views.Profile;
using wwrc_maui.Content.Views.Profile.ChangePassword;
using wwrc_maui.Content.Views.Profile.Details;
using wwrc_maui.Content.Views.Sales;
using wwrc_maui.Content.Views.Sales.CustomerAging;
using wwrc_maui.Content.Views.Sales.PurchaseOrder;
using wwrc_maui.Content.Views.Staff;
using static wwrc_maui.Content.Helper.ReferenceMessenger;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class MainPage : ContentPage
{
    MainPageVm viewmodel = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = viewmodel;
        navbar.OnLeftIconTapped += OnFilter_Tapped;
        navbar.OnRightIconTapped += OnLogout_Tapped;
        viewmodel.OnCellMenuTap += OnCellMenu_Tapped;
        viewmodel.SetupData();

        #region reference messenger
        WeakReferenceMessenger.Default.Register<StringNotify>(this, (receiver, message) =>
        {
            if (message.Value != null && message.Value.Equals("RefreshDashboardFilterModel"))
            { viewmodel.SetupData(); }
        });
        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, (receiver, message) =>
        {
            if (message.Value.Key.Equals("DashboardBotTab"))
            { OnBottomTabSelected(Convert.ToInt32(message.Value.Value)); }
        });
        #endregion
    }

    private async void OnFilter_Tapped()
    { await Navigation.PushAsync(new DashboardFilter(viewmodel.DashboardData, viewmodel.FilterData)); }

    private async void OnLogout_Tapped()
    {
        viewmodel.IsBusy = true;
        try
        {
            await Task.Delay(300);
            AppDatabase.Instance.DeleteAllData();
            Preferences.Default.Clear();
            Application.Current?.Dispatcher.Dispatch(() =>
            { Application.Current.Windows[0].Page = new Login(); });
        }
        catch (Exception ex)
        { await App.DisplayAlert("Error", ex.Message, null, "Okay"); }
        viewmodel.IsBusy = false;
    }

    private async void OnDateCurrency_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        await Navigation.PushAsync(new CurrencyExchangeRate());
    }

    private async void OnCellMenu_Tapped(DashboardCarouselTemplate data)
    {
        if (data.Type.Equals("CustomerAging")) await Navigation.PushAsync(new CustomerAgingMainPage());
        if (data.Type.Equals("PurchaseOrder")) await Navigation.PushAsync(new PurchaseOrderMainPage());
        if (data.Type.Equals("News")) await Navigation.PushAsync(new NewsMainPage());
        if (data.Type.Equals("MediaGallery")) await Navigation.PushAsync(new GalleryMainPage());
        if (data.Type.Equals("EmployeeHandbook")) await Navigation.PushAsync(new EmpHandbookMainPage("*", ""));
        if (data.Type.Equals("ProductCatalogue")) await Navigation.PushAsync(new ProductMainPage("*"));
        if (data.Type.Equals("MyProfile")) await Navigation.PushAsync(new ProfileDetailsMainPage());
        if (data.Type.Equals("ResetPassword")) await Navigation.PushAsync(new ChangePasswordMainPage());
    }

    private void OnTabContent_Tapped(object sender, TappedEventArgs e)
    { if (sender is not Grid view) return; } //to disable clicking other tabcontent

    private void OnBottomTabSelected(int tabNo)
    {
        grid_tabcontent.Children.Clear();
        if (tabNo == 1)
        {
            navbar.Title = ""; navbar.BarImage = "logo_db";
            navbar.IconRight = "ic_logout"; navbar.IconLeft = "ic_filter";
            grid_tabcontent.IsVisible = false;
        }
        if (tabNo == 2)
        {
            navbar.Title = "Staff"; navbar.BarImage = "";
            navbar.IconRight = ""; navbar.IconLeft = "";
            var view = new StaffOptionPage();
            grid_tabcontent.Children.Add(view);
            grid_tabcontent.IsVisible = true;
        }
        if (tabNo == 3)
        {
            navbar.Title = "Sales"; navbar.BarImage = "";
            navbar.IconRight = ""; navbar.IconLeft = "";
            //var view = new SalesOptionPage
            //{ StockAlertCount = Convert.ToInt32(viewmodel.DashboardData?.StockAlertCount) };
            var view = new SalesOptionPage { StockAlertCount = 5 }; //for demo
            grid_tabcontent.Children.Add(view);
            grid_tabcontent.IsVisible = true;
        }
        if (tabNo == 4)
        {
            navbar.Title = "Media"; navbar.BarImage = "";
            navbar.IconRight = ""; navbar.IconLeft = "";
            //var view = new MediaOptionPage
            //{
            //    NewsCount = Convert.ToInt32(viewmodel.DashboardData?.NewsCount),
            //    MediaCount = Convert.ToInt32(viewmodel.DashboardData?.TotalMediaCount),
            //    EmpHandbookCount = Convert.ToInt32(viewmodel.DashboardData?.HandbookCount),
            //    ProductCount = Convert.ToInt32(viewmodel.DashboardData?.CatalogCount)
            //};
            var view = new MediaOptionPage
            { NewsCount = 5, MediaCount = 2, EmpHandbookCount = 3, ProductCount = 6 }; //for demo
            grid_tabcontent.Children.Add(view);
            grid_tabcontent.IsVisible = true;
        }
        if (tabNo == 5)
        {
            navbar.Title = "Profile";
            navbar.IconRight = ""; navbar.IconLeft = "";
            var view = new ProfileOptionPage(viewmodel);
            if (viewmodel.LoginData != null && viewmodel.LoginData.UserData != null)
            {
                view.IsOfficeCredential = viewmodel.LoginData.UserData.IsOfficeCredential;
                view.Name = viewmodel.LoginData.UserData.Name;
                view.Position = viewmodel.LoginData.UserData.Position;
                view.Picture = string.IsNullOrEmpty(viewmodel.LoginData.UserData.ProfileImage) ?
                    "ic_user_empty" : viewmodel.LoginData.UserData.ProfileImage;
            }
            grid_tabcontent.Children.Add(view);
            grid_tabcontent.IsVisible = true;
        }
    }

    private void OnSwipe_Changed(object sender, CurrentItemChangedEventArgs e)
    {
        //workaround for bugs: IndicatorView dots keep increasing after page refresh
        if (sender is not CarouselView view) return;
        var _primary = Application.Current?.Resources["Primary"] as Color;
        var _gray = Application.Current?.Resources["Gray200"] as Color;
        var _style = Application.Current?.Resources["BorderSecondary"] as Style;
        var _theme = Application.Current?.RequestedTheme;

        if (view.Position == 0)
        {
            indicator1.BackgroundColor = _primary;
            indicator2.BackgroundColor = _theme == AppTheme.Light ? _gray : Colors.Black;
        }
        else if (view.Position == 1)
        {
            indicator1.BackgroundColor = _theme == AppTheme.Light ? _gray : Colors.Black;
            indicator2.BackgroundColor = _primary;
        }
    }
}