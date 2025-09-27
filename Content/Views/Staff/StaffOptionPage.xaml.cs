using wwrc_maui.Content.Views.Staff.CustomerVisit;
using wwrc_maui.Content.Views.Staff.Directory;
using static wwrc_maui.Content.Model.StaffModel;

namespace wwrc_maui.Content.Views.Staff;

public partial class StaffOptionPage : ContentView
{
    List<OptionPageModel> Items = [];

    public StaffOptionPage()
    {
        InitializeComponent();
        BindingContext = this;
        Items =
        [
            new() { Index = 0, Name = "Staff Directory", Desc = "Staff of company and other country", Image = "menu_news_update" },
            new() { Index = 1, Name = "Customer Visit", Desc = "Schedule of customer visit", Image = "menu_customer_visit" },
        ];
        listview.ItemsSource = Items;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView list) return;
        list.SelectedItem = null;
        var item = e.Item as OptionPageModel;
        if (item != null)
        {
            if (item.Index == 0) await Navigation.PushAsync(new StaffDirectoryMainPage());
            //if (item.Index == 1) await Navigation.PushAsync(new CustomerVisitMainPage());
        }
    }
}