using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls.Shapes;

namespace wwrc_maui.Content.CustomControls;

public partial class CustomEntry : Grid
{
    #region bindables
    #region custom properties
    public string SetText { get => (string)GetValue(SetTextProperty); set { SetValue(SetTextProperty, value); } }
    public string Placeholder { get => (string)GetValue(PlaceholderProperty); set { SetValue(PlaceholderProperty, value); } }
    public string EntryIcon { get => (string)GetValue(EntryIconProperty); set { SetValue(EntryIconProperty, value); } }
    public bool Ispassword { get => (bool)GetValue(IsPasswordProperty); set { SetValue(IsPasswordProperty, value); } }
    public bool Isfocus { get => (bool)GetValue(IsFocusProperty); set { SetValue(IsFocusProperty, value); } }
    public Keyboard KeyboardType { get => (Keyboard)GetValue(KeyboardTypeProperty); set { SetValue(KeyboardTypeProperty, value); } }
    public int CornerRadius { get => (int)GetValue(CornerRadiusProperty); set { SetValue(CornerRadiusProperty, value); } }
    public bool IsEnable { get => (bool)GetValue(IsEnableProperty); set { SetValue(IsEnableProperty, value); } }
    public double SetWidth { get => (double)GetValue(SetWidthProperty); set { SetValue(SetWidthProperty, value); } }
    #endregion
    #region bindable properties
    public static BindableProperty SetTextProperty =
        BindableProperty.Create(nameof(SetText), typeof(string), typeof(CustomEntry), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateBindingText((string)newVal); });
    public static BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntry), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdatePlaceholder((string)newVal); });
    public static BindableProperty EntryIconProperty =
        BindableProperty.Create(nameof(EntryIcon), typeof(string), typeof(CustomEntry), defaultValue: "",
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateEntryIcon((string)newVal); });
    public static BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(Ispassword), typeof(bool), typeof(CustomEntry), defaultValue: false,
            BindingMode.TwoWay, propertyChanged: (bindable, oldVal, newVal) =>
            { ((CustomEntry)bindable).UpdateIspassword((bool)newVal); });
    public static BindableProperty IsFocusProperty =
        BindableProperty.Create(nameof(Isfocus), typeof(bool), typeof(CustomEntry), defaultValue: false,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateIsfocus((bool)newVal); });
    public static BindableProperty KeyboardTypeProperty =
        BindableProperty.Create(nameof(KeyboardType), typeof(Keyboard), typeof(CustomEntry), defaultValue: Keyboard.Default,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateKeyboardType((Keyboard)newVal); });
    public static BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(CustomEntry), defaultValue: 0,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateCornerRadius((int)newVal); });
    public static BindableProperty IsEnableProperty =
        BindableProperty.Create(nameof(IsEnable), typeof(bool), typeof(CustomEntry), defaultValue: true,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateIsEnable((bool)newVal); });
    public static BindableProperty SetWidthProperty =
        BindableProperty.Create(nameof(SetWidth), typeof(double), typeof(CustomEntry), defaultValue: 0.0,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomEntry)bindable).UpdateSetWidth((double)newVal); });
    #endregion
    #region binding implementation
    public void UpdateBindingText(string data)
    { entry_.SetBinding(Entry.TextProperty, new Binding() { Path = data, Mode = BindingMode.TwoWay }); }
    public void UpdatePlaceholder(string data) { entry_.Placeholder = data; }
    public void UpdateEntryIcon(string data)
    {
        var _theme = Application.Current?.RequestedTheme;
        var tint = new IconTintColorBehavior { TintColor = _theme == AppTheme.Light ? Colors.Black : Colors.White };
        img_left.Source = data;
        img_left.Behaviors.Add(tint);
        stack_left.IsVisible = !string.IsNullOrEmpty(data);
        SetEntrySize();
    }
    public void UpdateIspassword(bool data)
    {
        if (entry_ is null) return;
        entry_.IsPassword = data;
        stack_right.IsVisible = data;
        SetEntrySize();
    }
    public void UpdateIsfocus(bool data) { if (data) entry_.Focus(); else entry_.Unfocus(); }
    public void UpdateKeyboardType(Keyboard data) { entry_.Keyboard = data; }
    public void UpdateCornerRadius(int data)
    {
        if (data > 0) border_.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(data) };
        else border_.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0) };
    }
    public void UpdateIsEnable(bool data)
    {
        entry_.IsEnabled = data;
        entry_.TextColor = data ? Colors.Black : Colors.LightGray;
        entry_.FontAttributes = data ? FontAttributes.None : FontAttributes.Italic;
    }
    public void UpdateSetWidth(double data) { SetEntrySize(); }
    private void SetEntrySize()
    {
        if (!Ispassword)
        {
            stack_clear.Padding = new Thickness(10,5,15,5);
            if (string.IsNullOrEmpty(EntryIcon)) // no left or right icon
            {
                if (stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 70; entry_.Margin = new Thickness(15,0,0,0); }
                if (!stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 30; entry_.Margin = new Thickness(15,0); }
            }
            else if (!string.IsNullOrEmpty(EntryIcon)) // no right icon
            {
                if (stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 110; entry_.Margin = new Thickness(0); }
                if (!stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 70; entry_.Margin = new Thickness(0,0,15,0); }
            }
        }
        else if (Ispassword)
        {
            stack_clear.Padding = new Thickness(10,5,0,5);
            if (string.IsNullOrEmpty(EntryIcon)) // no left icon
            {
                if (stack_clear.IsVisible) 
                { entry_.WidthRequest = SetWidth - 110; entry_.Margin = new Thickness(15,0,0,0); }
                if (!stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 70; entry_.Margin = new Thickness(15,0,0,0); }
            }
            else if (!string.IsNullOrEmpty(EntryIcon)) // have left & right icon
            {
                if (stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 150; entry_.Margin = new Thickness(0); }
                if (!stack_clear.IsVisible)
                { entry_.WidthRequest = SetWidth - 110; entry_.Margin = new Thickness(0); }
            }
        }
    }
    #endregion
    #endregion

    public Action? OnTextCleared = null;
    bool _isPwdTapped = true;

    public CustomEntry() { InitializeComponent(); }

    private async void OnImgLeft_Tapped(object? sender, EventArgs e)
    {
        if (sender is not StackLayout || entry_ is null) return;
        var view = (StackLayout)sender;
        await view.ScaleTo(0.9, 100);
        view.Scale = 1;
        entry_.Focus();
    }

    private async void OnTextClear_Tapped(object? sender, EventArgs e)
    {
        if (sender is not StackLayout || entry_ is null) return;
        var view = (StackLayout)sender;
        await view.ScaleTo(0.9, 100);
        view.Scale = 1;
        entry_.Text = "";
        OnTextCleared?.Invoke();
    }

    private async void OnImgRight_Tapped(object? sender, EventArgs e)
    {
        if (sender is not StackLayout || entry_ is null || !Ispassword) return;
        var view = (StackLayout)sender;
        await view.ScaleTo(0.9, 100);
        view.Scale = 1;

        _isPwdTapped = !_isPwdTapped;
        entry_.IsPassword = _isPwdTapped;
        if (_isPwdTapped) { img_right.Source = "ic_pwd_hide.png"; }
        else { img_right.Source = "ic_pwd_show.png"; }
    }

    private void OnEntry_TextChanged(object sender, TextChangedEventArgs e)
    { 
        stack_clear.IsVisible = !string.IsNullOrEmpty(e.NewTextValue);
        SetEntrySize();
    }
}