using RGPopup.Maui.Extensions;
using wwrc_maui.Content.CustomControls;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class CurrencyExchangeDateList : ContentView
{
    public List<DateTime> Itemsource = [];
    public Action<DateTime>? OnItemSelected = null;

    public CurrencyExchangeDateList()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public void Initialize() { listvw.ItemsSource = Itemsource; }

    private void listvw_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        var parent = (ShowAlert)Parent.Parent.Parent.Parent.Parent;
        var selected = (DateTime)lv.SelectedItem;
        lv.SelectedItem = null;
        parent.Navigation.PopPopupAsync();
        OnItemSelected?.Invoke(selected);
    }
}