using System.ComponentModel;
using System.Runtime.CompilerServices;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Views.Sales.CustomerAging;

public partial class FilterSalesPersonView : ContentView, INotifyPropertyChanged
{
    #region bindables properties
    #region beans
    private List<SalesPersonList> _list = [];
    string _searchTxt = "";
    #endregion
    #region props
    public List<SalesPersonList> Itemsource
    {
        get { return _list; }
        set { _list = value; NotifyPropertyChanged(); }
    }
    public string SearchTxt
    {
        get { return _searchTxt; }
        set { _searchTxt = value; NotifyPropertyChanged(); }
    }

    public new event PropertyChangedEventHandler? PropertyChanged = null;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    #endregion
    #endregion

    public SalesPersonList? Selected { get; set; } = null;

    public Command? SearchCommand { get; set; } = null;

    public FilterSalesPersonView()
	{
		InitializeComponent();
        BindingContext = this;
        SearchCommand = new Command(OnTextChanged);
    }

    void OnTextChanged()
    {
        if (!string.IsNullOrEmpty(SearchTxt))
        {
            var filter = Itemsource.Where(x => x.Id.ToLower().Contains(SearchTxt.ToLower()) ||
                x.Title.ToLower().Contains(SearchTxt.ToLower())).ToList();
            listview.ItemsSource = filter;
        }
        else listview.ItemsSource = Itemsource;
    }

    private void listview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        foreach (var data in Itemsource) { data.Checked = false; }
        var item = e.Item as SalesPersonList;
        Selected = item;
        if (item != null) { item.Checked = !item.Checked; }
        //if (item.Title == "all" || item.Title == "All") { UserId = "ALL"; }
        //else UserId = item.Id;
        //UserName = item.Title;
        lv.SelectedItem = null;
    }
}