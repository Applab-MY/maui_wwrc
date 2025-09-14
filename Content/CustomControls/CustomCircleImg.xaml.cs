using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls.Shapes;

namespace wwrc_maui.Content.CustomControls;

public partial class CustomCircleImg : Grid
{
    #region bindables
    #region custom properties
    public double SetSize { get => (double)GetValue(SetSizeProperty); set { SetValue(SetSizeProperty, value); } }
    public SolidColorBrush Border { get => (SolidColorBrush)GetValue(BorderProperty); set { SetValue(BorderProperty, value); } }
    public Aspect ImgAspect { get => (Aspect)GetValue(ImgAspectProperty); set { SetValue(ImgAspectProperty, value); } }
    public bool HasImage { get => (bool)GetValue(HasImageProperty); set { SetValue(HasImageProperty, value); } }
    public byte[] ImgFromByte { get => (byte[])GetValue(ImgFromByteProperty); set { SetValue(ImgFromByteProperty, value); } }
    public string ImgFromSource { get => (string)GetValue(ImgFromSourceProperty); set { SetValue(ImgFromSourceProperty, value); } }
    #endregion
    #region bindable properties
    public static BindableProperty SetSizeProperty =
        BindableProperty.Create(nameof(SetSize), typeof(double), typeof(CustomCircleImg), defaultValue: 0.0,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomCircleImg)bindable).UpdateSetSize((double)newVal); });
    public static BindableProperty BorderProperty =
        BindableProperty.Create(nameof(Border), typeof(SolidColorBrush), typeof(CustomCircleImg), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomCircleImg)bindable).UpdateBorder((SolidColorBrush)newVal); });
    public static BindableProperty ImgAspectProperty =
        BindableProperty.Create(nameof(ImgAspect), typeof(Aspect), typeof(CustomCircleImg), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomCircleImg)bindable).UpdateImgAspect((Aspect)newVal); });
    public static BindableProperty HasImageProperty =
        BindableProperty.Create(nameof(HasImage), typeof(bool), typeof(CustomCircleImg), defaultValue: false,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomCircleImg)bindable).UpdateHasImage((bool)newVal); });
    public static BindableProperty ImgFromByteProperty =
        BindableProperty.Create(nameof(ImgFromByte), typeof(byte[]), typeof(CustomCircleImg), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomCircleImg)bindable).UpdateImgFromByte((byte[])newVal); });
    public static BindableProperty ImgFromSourceProperty =
        BindableProperty.Create(nameof(ImgFromSource), typeof(string), typeof(CustomCircleImg), defaultValue: null,
            propertyChanged: (bindable, oldVal, newVal) => { ((CustomCircleImg)bindable).UpdateImgFromSource((string)newVal); });
    #endregion
    #region binding implementation
    public void UpdateSetSize(double data)
    {
        var rad = data / 2;
        border_default.HeightRequest = data;
        border_default.WidthRequest = data;
        border_default.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(rad) };
    }
    public void UpdateBorder(SolidColorBrush data) { border_default.Stroke = data; }
    public void UpdateImgAspect(Aspect data) { img_default.Aspect = data; }
    public void UpdateHasImage(bool data) { border_default.IsVisible = !data; }
    public void UpdateImgFromByte(byte[] data)
    {
        var theme = Application.Current?.RequestedTheme;
        img_default.Source = theme == AppTheme.Light ? "ic_elipse_info" : "ic_elipse_info_2";
        if (data != null)
        {
            var converter = new ByteArrayToImageSourceConverter();
            img_default.Source = converter.ConvertFrom(data);
            img_default.IsVisible = true;
        }
    }
    public void UpdateImgFromSource(string data)
    {
        var theme = Application.Current?.RequestedTheme;
        img_default.Source = theme == AppTheme.Light ? "ic_elipse_info" : "ic_elipse_info_2";
        if (!string.IsNullOrEmpty(data))
        {
            img_default.Source = data;
            img_default.IsVisible = true;
        }
    }
    #endregion
    #endregion

    public CustomCircleImg()
    {
        InitializeComponent();
    }
}