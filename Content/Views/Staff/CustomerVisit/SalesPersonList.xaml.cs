using RGPopup.Maui.Extensions;
using wwrc_maui.Content.CustomControls;

namespace wwrc_maui.Content.Views.Staff.CustomerVisit;

public partial class SalesPersonList : ContentView
{
    public List<string> Itemsource = [];
    public Action<string>? OnItemSelected = null;

    public SalesPersonList()
	{
		InitializeComponent();
        BindingContext = this;
	}

    public void Initialize() { listvw.ItemsSource = Itemsource; }

    private void listvw_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        var parent = (ShowAlert)Parent.Parent.Parent.Parent.Parent;
        var selected = lv.SelectedItem.ToString();
        lv.SelectedItem = null;
        parent.Navigation.PopPopupAsync();
        if (!string.IsNullOrEmpty(selected))
            OnItemSelected?.Invoke(selected);
    }
}