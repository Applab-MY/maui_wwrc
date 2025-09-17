using Microsoft.Maui.Controls.Shapes;
using wwrc_maui.Content.Viewmodels.Media.Gallery;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class GalleryVideoTab : StackLayout
{
    GalleryVm? viewmodel = null;

    public GalleryVideoTab(GalleryVm viewmodel)
    {
        InitializeComponent();
        this.viewmodel = viewmodel;
        BuildView();
    }

    void BuildView()
    {
        grid_content.Children.Clear();
        grid_content.RowDefinitions.Clear();
        if (viewmodel != null && viewmodel.VideoAlbums.Count == 0)
        { viewmodel.NoData = true; return; }
        if (viewmodel != null && viewmodel.VideoAlbums.Count > 0)
        {
            viewmodel.NoData = false;
            var _style = Application.Current?.Resources["BorderSecondary"] as Style;
            var _styleTxt = Application.Current?.Resources["PopupPromptButton"] as Style;
            int col = 0; int row = 0;
            grid_content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            foreach (var item in viewmodel.VideoAlbums)
            {
                string _cover = "";
                if (item != null)
                {
                    _cover = string.IsNullOrEmpty(item.Image) || item.Image.Equals("false")
                        ? "ic_upload_photo" : item.Image;

                    var container = new Border
                    {
                        Style = _style,
                        StrokeShape = new RoundRectangle { CornerRadius = 10 },
                        Stroke = Colors.LightGray,
                        Padding = new Thickness(0),
                    };
                    var grid = new Grid
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        RowDefinitions = [new RowDefinition { Height = GridLength.Star }],
                        ColumnDefinitions = [new ColumnDefinition { Width = GridLength.Star }],
                    };
                    var shadow = new StackLayout { BackgroundColor = Colors.Black, Opacity = 0.2 };
                    var stack = new VerticalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Spacing = 0,
                    };
                    var img = new Image
                    {
                        Source = _cover,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Aspect = Aspect.AspectFill,
                        HeightRequest = 80,
                    };
                    var imgPlay = new Image
                    {
                        Source = "ic_play",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 40,
                        Aspect = Aspect.AspectFit,
                    };
                    var title = new Label
                    {
                        Text = item.Title, MaxLines = 2, FontSize = 14,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Style = _styleTxt,
                        LineBreakMode = LineBreakMode.TailTruncation,
                        Margin = new Thickness(10,5,10,8),
                    };
                    var _id = new Label { Text = item.Id, IsVisible = false };
                    if (item.IsRead == "true") title.Style = default;
                    else title.Style = _style;
                    stack.Children.Add(img);
                    stack.Children.Add(title);
                    grid.Children.Add(_id);
                    grid.Children.Add(stack);
                    grid.Children.Add(shadow);
                    grid.Children.Add(imgPlay);
                    container.Content = grid;

                    var tap = new TapGestureRecognizer();
                    tap.Tapped += OnCell_Tapped;
                    tap.NumberOfTapsRequired = 1;
                    container.GestureRecognizers.Add(tap);

                    if (col < 2)
                    { grid_content.Add(container, col, row); col++; }
                    else
                    {
                        col = 0; row++;
                        grid_content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        grid_content.Add(container, col, row);
                        col++;
                    }
                }
            }
        }
    }

    private async void OnCell_Tapped(object? sender, EventArgs e)
    {
        if (sender is not Border view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        var lblId = "";
        if (view.Content != null)
        {
            var _grid = (Grid)view.Content;
            var _id = (Label)_grid.Children[0];
            lblId = _id.Text;
            await Navigation.PushAsync(new VideoDetailsPage(lblId));
        }
    }
}