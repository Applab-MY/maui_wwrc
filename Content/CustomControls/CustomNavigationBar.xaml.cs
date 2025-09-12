using CommunityToolkit.Maui.Behaviors;

namespace wwrc_maui.Content.CustomControls;

public partial class CustomNavigationBar : ContentView
{
    #region bindables
    #region custom properties
    public string BarBackgroundColorHex
    {
        get => (string)GetValue(BarBackgroundColorHexProperty);
        set { SetValue(BarBackgroundColorHexProperty, value); }
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set { SetValue(TitleProperty, value); }
    }
    public int TitleFontSize
    {
        get => (int)GetValue(TitleFontSizeProperty);
        set { SetValue(TitleFontSizeProperty, value); }
    }
    public string TitleTextColor
    {
        get => (string)GetValue(TitleTextColorProperty);
        set { SetValue(TitleTextColorProperty, value); }
    }
    public string BarImage
    {
        get => (string)GetValue(BarImageProperty);
        set { SetValue(BarImageProperty, value); }
    }
    public string IconLeft
    {
        get => (string)GetValue(IconLeftProperty);
        set { SetValue(IconLeftProperty, value); }
    }
    public string IconRight
    {
        get => (string)GetValue(IconRightProperty);
        set { SetValue(IconRightProperty, value); }
    }
    public int BadgeNo
    {
        get => (int)GetValue(BadgeNoProperty);
        set { SetValue(BadgeNoProperty, value); }
    }
    #endregion
    #region bindable properties
    public static BindableProperty BarBackgroundColorHexProperty =
        BindableProperty.Create(nameof(BarBackgroundColorHex), typeof(string), typeof(CustomNavigationBar), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateBarBackgroundColor((string)newVal); });
    public static BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateTitle((string)newVal); });
    public static BindableProperty TitleFontSizeProperty =
        BindableProperty.Create(nameof(TitleFontSize), typeof(int), typeof(CustomNavigationBar), defaultValue: 0,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateTitleFontSize((int)newVal); });
    public static BindableProperty TitleTextColorProperty =
        BindableProperty.Create(nameof(TitleTextColor), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateTitleTextColor((string)newVal); });
    public static BindableProperty BarImageProperty =
        BindableProperty.Create(nameof(BarImage), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateBarImage((string)newVal); });
    public static BindableProperty IconLeftProperty =
        BindableProperty.Create(nameof(IconLeft), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateIconLeft((string)newVal); });
    public static BindableProperty IconRightProperty =
        BindableProperty.Create(nameof(IconRight), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateIconRight((string)newVal); });
    public static BindableProperty BadgeNoProperty =
        BindableProperty.Create(nameof(BadgeNo), typeof(int), typeof(CustomNavigationBar), defaultValue: 0,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomNavigationBar)bindable).UpdateBadgeNo((int)newVal); });
    #endregion
    #region binding implementation
    public void UpdateBarBackgroundColor(string data) { grid_bar.BackgroundColor = Color.FromArgb(data); }
    public void UpdateTitle(string data)
    {
        lbl_title.Text = data;
        lbl_title.IsVisible = !string.IsNullOrEmpty(data);
    }
    public void UpdateTitleFontSize(int data) { lbl_title.FontSize = data; }
    public void UpdateTitleTextColor(string data) { lbl_title.TextColor = Color.FromArgb(data); }
    public void UpdateBarImage(string data)
    {
        img_center.Source = data;
        img_center.IsVisible = !string.IsNullOrEmpty(data);
    }
    public void UpdateIconLeft(string data)
    {
        img_left.Source = data;
        stack_topleft.IsVisible = !string.IsNullOrEmpty(data);
        var theme = Application.Current?.RequestedTheme;
        if (theme is AppTheme.Dark)
        {
            var behavior = new IconTintColorBehavior { TintColor = Colors.White };
            img_left.Behaviors.Add(behavior);
        }
    }
    public void UpdateIconRight(string data)
    {
        img_right.Source = data;
        stack_topright.IsVisible = !string.IsNullOrEmpty(data);
        var theme = Application.Current?.RequestedTheme;
        if (theme is AppTheme.Dark)
        {
            var behavior = new IconTintColorBehavior { TintColor = Colors.White };
            img_right.Behaviors.Add(behavior);
        }
    }
    public void UpdateBadgeNo(int data)
    {
        border_badge.IsVisible = data > 0 && !string.IsNullOrEmpty(IconRight);
        stack_topright.Padding = data > 0 && !string.IsNullOrEmpty(IconRight) ?
            new Thickness(8, 22, 22, 8) : new Thickness(15);

        if (data > 99) lbl_badgeno.Text = "99+";
        else lbl_badgeno.Text = data.ToString();
    }
    #endregion
    #endregion

    public Action? OnLeftIconTapped = null;
    public Action? OnRightIconTapped = null;

    public CustomNavigationBar() { InitializeComponent(); }

    async void OnSideMenu_Tapped(object sender, EventArgs e)
    {
        if (sender is not StackLayout) return;
        var _view = (StackLayout)sender;
        await _view.ScaleTo(0.9, 100);
        _view.Scale = 1;
        OnLeftIconTapped?.Invoke();
    }

    async void OnToolbar_Tapped(object sender, EventArgs e)
    {
        if (sender is not StackLayout) return;
        var _view = (StackLayout)sender;
        await _view.ScaleTo(0.9, 100);
        _view.Scale = 1;
        OnRightIconTapped?.Invoke();
    }

    async void OnBadgeNo_Tapped(object sender, EventArgs e)
    {
        if (sender is not Border) return;
        var _view = (Border)sender;
        await _view.ScaleTo(0.9, 100);
        _view.Scale = 1;
        OnRightIconTapped?.Invoke();
    }
}