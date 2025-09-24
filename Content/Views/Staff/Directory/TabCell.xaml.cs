using CommunityToolkit.Mvvm.Messaging;
using static wwrc_maui.Content.Helper.ReferenceMessenger;

namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class TabCell : ContentView
{
    #region bindables
    #region custom properties
    public int TabId { get => (int)GetValue(TabIdProperty); set { SetValue(TabIdProperty, value); } }
    public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); set { SetValue(IsSelectedProperty, value); } }
    public string Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
    public double SetHeight { get => (double)GetValue(SetHeightProperty); set { SetValue(SetHeightProperty, value); } }
    #endregion
    #region bindable properties
    public static BindableProperty TabIdProperty =
        BindableProperty.Create(nameof(TabId), typeof(int), typeof(TabCell), defaultValue: 0,
            propertyChanged: (bindable, oldVal, newVal) => { ((TabCell)bindable).UpdateTabId((int)newVal); });
    public static BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TabCell), defaultValue: false,
            propertyChanged: (bindable, oldVal, newVal) => { ((TabCell)bindable).UpdateIsSelected((bool)newVal); });
    public static BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TabCell), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((TabCell)bindable).UpdateTitle((string)newVal); });
    public static BindableProperty SetHeightProperty =
        BindableProperty.Create(nameof(SetHeight), typeof(double), typeof(TabCell), defaultValue: 0.0,
            propertyChanged: (bindable, oldVal, newVal) => { ((TabCell)bindable).UpdateSetHeight((double)newVal); });
    #endregion
    #region binding implementation
    public void UpdateTabId(int data) { }
    public void UpdateIsSelected(bool data)
    {
        var style = Application.Current?.Resources["PopupPromptButton"] as Style;
        if (data) { lbl_title.Style = style; line.IsVisible = true; }
    }
    public void UpdateTitle(string data) { lbl_title.Text = data; }
    public void UpdateSetHeight(double data) { grid_tab.HeightRequest = data + 15; }
    #endregion
    #endregion

    public TabCell()
	{
		InitializeComponent();
        var style = Application.Current?.Resources["PopupPromptButton"] as Style;
        WeakReferenceMessenger.Default.Register<KeyValueNotify>(this, async (receiver, message) =>
        {
            if (message.Value.Key.Equals("StaffTabSelected"))
            {
                var _tab = Convert.ToInt32(message.Value.Value);
                if (IsSelected && message.Value.Value == TabId.ToString()) return;
                if (IsSelected)
                {
                    IsSelected = false;
                    lbl_title.Style = default;
                    await line.FadeTo(0, 200);
                    line.IsVisible = false;
                }
                if (_tab == TabId)
                {
                    IsSelected = true;
                    lbl_title.Style = style;
                    await line.FadeTo(1, 200);
                    line.IsVisible = true;
                }
            }
        });
    }

    private async void OnCell_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        WeakReferenceMessenger.Default.Send(new KeyValueNotify
        (new KeyValue { Key = "StaffTabSelected", Value = TabId.ToString() }));
    }
}