using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;

namespace wwrc_maui.Content.CustomControls;

public partial class ShowAlert : PopupPage
{
    #region bindables
    #region custom properties
    public new string? Title { get => (string)GetValue(TitleProperty); set { SetValue(TitleProperty, value); } }
    public string? Description { get => (string)GetValue(DescriptionProperty); set { SetValue(DescriptionProperty, value); } }
    public ContentView? Viewcell { get => (ContentView)GetValue(ViewcellProperty); set { SetValue(ViewcellProperty, value); } }
    public string? BtnOkay { get => (string)GetValue(BtnOkayProperty); set { SetValue(BtnOkayProperty, value); } }
    public string? BtnCancel { get => (string)GetValue(BtnCancelProperty); set { SetValue(BtnCancelProperty, value); } }
    #endregion
    #region bindable properties
    public new static BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ShowAlert), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((ShowAlert)bindable).UpdateTitle((string)newVal); });
    public static BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(ShowAlert), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((ShowAlert)bindable).UpdateDescription((string)newVal); });
    public static BindableProperty ViewcellProperty =
        BindableProperty.Create(nameof(Viewcell), typeof(ContentView), typeof(ShowAlert), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((ShowAlert)bindable).UpdateViewcell((ContentView)newVal); });
    public static BindableProperty BtnOkayProperty =
        BindableProperty.Create(nameof(BtnOkay), typeof(string), typeof(ShowAlert), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((ShowAlert)bindable).UpdateBtnOkay((string)newVal); });
    public static BindableProperty BtnCancelProperty =
        BindableProperty.Create(nameof(BtnCancel), typeof(string), typeof(ShowAlert), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((ShowAlert)bindable).UpdateBtnCancel((string)newVal); });
    #endregion
    #region binding implementation
    public void UpdateTitle(string data)
    {
        lbl_title.Text = data;
        lbl_title.IsVisible = !string.IsNullOrEmpty(data);
    }
    public void UpdateDescription(string data)
    {
        lbl_desc.Text = data;
        lbl_desc.IsVisible = !string.IsNullOrEmpty(data);
    }
    public void UpdateViewcell(ContentView data)
    {
        view_custom.MaximumHeightRequest = (App.ScreenHeight/2) - 120;
        view_custom.Content = data;
    }
    public void UpdateBtnOkay(string data)
    {
        lbl_ok.Text = data;
        grid_ok.IsVisible = !string.IsNullOrEmpty(data);
        if (string.IsNullOrEmpty(BtnCancel)) grid_ok.SetColumnSpan(grid_ok, 2);
        else grid_ok.SetColumnSpan(grid_ok, 1);
    }
    public void UpdateBtnCancel(string data)
    {
        lbl_cancel.Text = data;
        grid_cancel.IsVisible = !string.IsNullOrEmpty(data);
        if (string.IsNullOrEmpty(BtnOkay))
        {
            grid_cancel.SetColumn(grid_cancel, 0);
            grid_cancel.SetColumnSpan(grid_cancel, 2);
        }
        else
        {
            grid_ok.SetColumnSpan(grid_ok, 1);
            grid_cancel.SetColumn(grid_cancel, 1);
            grid_cancel.SetColumnSpan(grid_cancel, 1);
        }
    }
    #endregion
    #endregion

    public Action<bool>? ClosePopupAction = null;

    public ShowAlert() { InitializeComponent(); }

    private async void OnOkay_Tapped(object sender, EventArgs e)
    {
        if (sender is not Grid view) return;
        var lbl = (Label)view.Children[0];
        await lbl.ScaleTo(0.9, 100);
        lbl.Scale = 1;

        await Navigation.PopPopupAsync(true);
        ClosePopupAction?.Invoke(true);
    }

    private async void OnCancel_Tapped(object sender, EventArgs e)
    {
        if (sender is not Grid view) return;
        var lbl = (Label)view.Children[0];
        await lbl.ScaleTo(0.9, 100);
        lbl.Scale = 1;

        await Navigation.PopPopupAsync(true);
        ClosePopupAction?.Invoke(false);
    }
}