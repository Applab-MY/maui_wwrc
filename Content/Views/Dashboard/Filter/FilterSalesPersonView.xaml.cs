using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class FilterSalesPersonView : ContentView, INotifyPropertyChanged
{
    #region bindables properties
    #region beans
    List<SalesPersonList> _list = [];
    ObservableCollection<SalesPersonList> _mainlist = [];
    bool _nodata = false;
    #endregion
    #region props
    public List<SalesPersonList> Itemsource
    {
        get { return _list; }
        set
        {
            _list = value;
            SetMainSourceList();
            NotifyPropertyChanged();
        }
    }
    public ObservableCollection<SalesPersonList> MainSource
    {
        get { return _mainlist; }
        set
        {
            _mainlist = value;
            NoData = value.Count == 0;
            NotifyPropertyChanged();
        }
    }
    public bool NoData
    {
        get { return _nodata; }
        set { _nodata = value; NotifyPropertyChanged(); }
    }

    public new event PropertyChangedEventHandler? PropertyChanged = null;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    #endregion
    #endregion

    public int MaxListSize { get; set; } = 15;
    public int IndexAt { get; set; } = 0;
    public bool IsFinish { get; set; } = false;
    public SalesPersonList? Selected { get; set; } = null;

    public FilterSalesPersonView()
    {
        InitializeComponent();
        NoData = true;
        BindingContext = this;
    }

    public void Reset() { entry_.Text = ""; NoData = false; }

    private void SetMainSourceList()
    {
        if (Itemsource.Count > MaxListSize)
        {
            IndexAt = MaxListSize;
            MainSource = new ObservableCollection<SalesPersonList>(Itemsource.GetRange(0, MaxListSize));
        }
        else MainSource = new ObservableCollection<SalesPersonList>(Itemsource);
    }

    private void listview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        foreach (var data in Itemsource) { data.Checked = false; }
        foreach (var data in MainSource) { data.Checked = false; }
        var item = e.Item as SalesPersonList;
        Selected = item;
        if (item != null) { item.Checked = !item.Checked; }
        //if (item.Title == "all" || item.Title == "All") { UserId = "ALL"; }
        //else UserId = item.Id;
        //UserName = item.Title;
        lv.SelectedItem = null;
    }

    private void listview_ItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
        if (sender is not ListView lv) return;
        if (e.Item == MainSource.Last() && Itemsource.Count > MaxListSize)
        {
            int count = 0;
            var isExceed = (IndexAt + MaxListSize) > Itemsource.Count;
            if (!isExceed)
            {
                for (count = IndexAt; count < (IndexAt + MaxListSize); count++)
                { MainSource.Add(Itemsource[count]); }
                IndexAt = count;
            }
            else
            {
                if (!IsFinish)
                {
                    for (count = IndexAt; count < Itemsource.Count; count++)
                    { MainSource.Add(Itemsource[count]); }
                    IsFinish = true;
                }
            }
        }
    }

    private void OnEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry view) return;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            var filter = Itemsource.Where(x => x.Id.ToLower().Contains(e.NewTextValue.ToLower()) ||
                x.Title.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            listview.ItemsSource = filter;
            NoData = filter.Count == 0;
        }
        else listview.ItemsSource = MainSource;
    }
}