using System.ComponentModel;
using System.Runtime.CompilerServices;
using static wwrc_maui.Content.Model.CountryModel;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class CountryListCell : ContentView, INotifyPropertyChanged
{
    #region bindables properties
    #region beans
    private List<CountryMainModel> _list = [];
    #endregion
    #region props
    public List<CountryMainModel> Itemsource
    {
        get { return _list; }
        set { _list = value; NotifyPropertyChanged(); }
    }

    public new event PropertyChangedEventHandler? PropertyChanged = null;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    #endregion
    #endregion

    public CountryMainModel? Selected { get; set; } = null;

    public CountryListCell()
	{
		InitializeComponent();
		BindingContext = this;
	}

    private void listview_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        var item = e.Item as CountryMainModel;
        Selected = item;
        //lv.SelectedItem = null;
    }

    private void OnEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry view) return;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            var filter = Itemsource.Where(x => x.CountryCode.ToLower().Contains(e.NewTextValue.ToLower()) ||
                x.CountryName.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            listview.ItemsSource = filter;
        }
        else listview.ItemsSource = Itemsource;
    }
}