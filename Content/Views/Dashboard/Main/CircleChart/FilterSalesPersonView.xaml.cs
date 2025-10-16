using wwrc_maui.Content.Viewmodels.Dashboard;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Views.Dashboard.Main;

public partial class FilterSalesPersonView : ContentView
{
    GraphDetailsVm? viewmodel = null;

    public FilterSalesPersonView() { InitializeComponent(); }

    public void SetParentBinding(GraphDetailsVm viewmodel)
    {
        this.viewmodel = viewmodel;
        BindingContext = viewmodel;
    }

    public void Reset()
    {
        entry_.Text = "";
        if (viewmodel != null)
            viewmodel.NoSalesPerson = false;
    }

    private void listview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        if (viewmodel != null)
            foreach (var data in viewmodel.AllSalesPerson)
            { data.Checked = false; }

        var item = e.Item as SalesPersonList;
        if (item != null && viewmodel != null)
        {
            item.Checked = !item.Checked;
            if (item.Checked)
            { viewmodel.SalesPerson = item.Id; viewmodel.isFilterSales = true; }
            else
            {
                viewmodel.SalesPerson = Preferences.Default.Get("userId", "");
                viewmodel.isFilterSales = false;
            }
        }
        //if (item.Title == "all" || item.Title == "All") { UserId = "ALL"; }
        //else UserId = item.Id;
        //UserName = item.Title;
        lv.SelectedItem = null;
    }

    private void listview_ItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
        if (sender is not ListView lv) return;
        if (e.Item == viewmodel?.AllSalesPerson.Last() &&
            viewmodel.filterPerson.Count > viewmodel.MaxListSize)
        {
            int count = 0;
            var isExceed = (viewmodel.IndexAt + viewmodel.MaxListSize) > viewmodel.filterPerson.Count;
            if (!isExceed)
            {
                for (count = viewmodel.IndexAt; count < (viewmodel.IndexAt + viewmodel.MaxListSize); count++)
                { viewmodel.AllSalesPerson.Add(viewmodel.filterPerson[count]); }
                viewmodel.IndexAt = count;
            }
            else
            {
                if (!viewmodel.IsFinish)
                {
                    for (count = viewmodel.IndexAt; count < viewmodel.filterPerson.Count; count++)
                    { viewmodel.AllSalesPerson.Add(viewmodel.filterPerson[count]); }
                    viewmodel.IsFinish = true;
                }
            }
        }
    }

    private void OnEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry view) return;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            var filter = viewmodel?.filterPerson.Where(x => x.Id.ToLower().Contains(e.NewTextValue.ToLower()) ||
                x.Title.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            listview.ItemsSource = filter;
            if (viewmodel != null) viewmodel.NoSalesPerson = filter?.Count == 0;
        }
        else listview.ItemsSource = viewmodel?.AllSalesPerson;
    }
}