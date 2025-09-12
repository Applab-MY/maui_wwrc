using MauiPopup;
using MauiPopup.Views;
using Microsoft.Maui.Controls.Shapes;

namespace wwrc_maui.Content.CustomControls;

public partial class CustomPicker : ContentView
{
    #region bindables
    #region custom properties
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set { SetValue(TitleProperty, value); }
    }
    public List<string> ItemsSource
    {
        get => (List<string>)GetValue(ItemsSourceProperty);
        set { SetValue(ItemsSourceProperty, value); }
    }
    public string? SelectedItem
    {
        get => (string)GetValue(SelectedItemProperty);
        set { SetValue(SelectedItemProperty, value); }
    }
    public int CornerRadius
    {
        get => (int)GetValue(CornerRadiusProperty);
        set { SetValue(CornerRadiusProperty, value); }
    }
    #endregion
    #region bindable properties
    public static BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomPicker), defaultValue: string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomPicker)bindable).UpdateTitle((string)newVal); });
    public static BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(List<string>), typeof(CustomPicker), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomPicker)bindable).UpdateItemsSource((List<string>)newVal); });
    public static BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(CustomPicker), defaultValue: string.Empty,
            BindingMode.TwoWay, propertyChanged: (bindable, oldVal, newVal) =>
            { ((CustomPicker)bindable).UpdateSelectedItem((string)newVal); });
    public static BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(CustomPicker), defaultValue: 0,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomPicker)bindable).UpdateCornerRadius((int)newVal); });
    #endregion
    #region binding implementation
    public void UpdateTitle(string data) { lbl_title.Text = data; }
    public void UpdateItemsSource(List<string> data)
    {
        var view = BuildPickerView(data);
        pickerView = new BasePopupPage
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            Content = view,
            Margin = new Thickness(30, 50),
            IsCloseOnBackgroundClick = false,
        };
    }
    public void UpdateSelectedItem(string data) { entry_selected.Text = data; }
    private void entrySelected_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry) return;
        lbl_selected.Text = string.Empty;
        lbl_selected.IsVisible = false;
        lbl_title.IsVisible = true;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (ItemsSource?.Count > 0)
            {
                var found = ItemsSource.Where(x => x.Equals(e.NewTextValue)).FirstOrDefault();
                if (found is not null)
                {
                    lbl_selected.Text = found;
                    lbl_selected.IsVisible = true;
                    lbl_title.IsVisible = false;
                    SelectedItem = found; //to send back to binding vm
                }
            }
        }
        else SelectedItem = "";
    }
    public void UpdateCornerRadius(int data)
    {
        if (data > 0) border_.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(data) };
        else border_.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0) };
    }
    #endregion
    #endregion

    public Action<bool>? IsListEmpty = null;
    BasePopupPage? pickerView = null;

    public CustomPicker()
    {
        // ItemsSource : binding in xaml as class object, need {}
        // SelectedItem : binding in xaml as string path, no {} needed
        InitializeComponent();
    }

    #region build picker components
    private VerticalStackLayout? BuildPickerView(List<string>? data = null)
    {
        #region create UI elements
        var _styleTitle = new Style(typeof(Label));
        var _styleBtn = new Style(typeof(Label));
        if (Application.Current != null)
        {
            var hasValue1 = Application.Current.Resources.TryGetValue("AlertTitle", out object style1);
            var hasValue2 = Application.Current.Resources.TryGetValue("PopupPrompt", out object style2);
            if (hasValue1) _styleTitle = (Style)style1;
            if (hasValue2) _styleBtn = (Style)style2;
        }

        var container = new VerticalStackLayout
        {
            Spacing = 0,
            Margin = new Thickness(0),
            Padding = new Thickness(15, 15, 15, 0),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
        };
        var lblTitle = new Label
        {
            Text = Title,
            Style = _styleTitle,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Start,
            LineBreakMode = LineBreakMode.TailTruncation,
            MaxLines = 1,
            Margin = new Thickness(0, 0, 0, 15)
        };
        var stackEmpty = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
        };
        var lblEmpty = new Label
        {
            Text = "no data.",
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(15)
        };
        stackEmpty.Children.Add(lblEmpty);
        var line = new BoxView
        {
            HeightRequest = 1,
            Margin = new Thickness(0, 15, 0, 0)
        };
        var stackCancel = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End,
            Padding = new Thickness(15),
        };
        var lblCancel = new Label { Text = "Cancel", Style = _styleBtn, };
        #endregion

        var tap = new TapGestureRecognizer();
        tap.Tapped += OnCancel_Tapped;
        tap.NumberOfTapsRequired = 1;
        stackCancel.GestureRecognizers.Add(tap);
        stackCancel.Children.Add(lblCancel);

        if (data is null || data.Count == 0)
        {
            container.Children.Add(lblTitle);
            container.Children.Add(stackEmpty);
            container.Children.Add(line);
            container.Children.Add(stackCancel);
            IsListEmpty?.Invoke(true);
            return container;
        }

        //20250729 :: using listview not working in ios
        var stackList = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start,
            Spacing = 0,
        };
        BindableLayout.SetItemsSource(stackList, ItemsSource);
        BindableLayout.SetItemTemplate(stackList, new DataTemplate(() =>
        {
            var lblValue = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                LineBreakMode = LineBreakMode.WordWrap,
            };
            lblValue.SetBinding(Label.TextProperty, ".");
            var container = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(15, 10),
                Children = { lblValue }
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += OnItem_Tapped;
            tap.NumberOfTapsRequired = 1;
            container.GestureRecognizers.Add(tap);
            return container;
        }));

        container.Children.Add(lblTitle);
        container.Children.Add(stackList);
        container.Children.Add(line);
        container.Children.Add(stackCancel);
        IsListEmpty?.Invoke(false);
        return container;
    }

    private async void OnItem_Tapped(object? sender, EventArgs e)
    {
        if (sender is StackLayout stack)
        {
            var lblValue = (Label)stack.Children[0];
            await lblValue.ScaleTo(0.9, 100);
            lblValue.Scale = 1;

            var found = ItemsSource?.Where(x => x.Equals(lblValue.Text)).FirstOrDefault();
            if (found is not null) entry_selected.Text = lblValue.Text;
            await PopupAction.ClosePopup();
        }
    }

    private async void OnCancel_Tapped(object? sender, EventArgs e)
    {
        if (sender is StackLayout stacklayout)
        {
            await stacklayout.ScaleTo(0.9, 100);
            stacklayout.Scale = 1;
            entry_selected.Text = string.Empty;
            await PopupAction.ClosePopup();
        }
    }
    #endregion

    private async void OnPicker_Tapped(object? sender, EventArgs e)
    {
        if (sender is not Grid) return;
        await img_dropdown.ScaleTo(0.8, 100);
        img_dropdown.Scale = 1;
        await PopupAction.DisplayPopup(pickerView);
    }
}