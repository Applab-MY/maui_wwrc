using Microsoft.Maui.Controls.Shapes;
using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Media.Gallery;
using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class GalleryPhotoTab : ScrollView
{
    GalleryVm? viewmodel = null;

    public GalleryPhotoTab() { InitializeComponent(); }

    public void SetParentBinding(GalleryVm? viewmodel) { this.viewmodel = viewmodel; }

    public void BuildView()
    {
        grid_content.Children.Clear();
        grid_content.RowDefinitions.Clear();
        if (viewmodel != null && viewmodel.AllAlbums.Count == 0)
        { viewmodel.NoData = true; return; }
        if (viewmodel != null && viewmodel.AllAlbums.Count > 0)
        {
            viewmodel.NoData = false;
            var _style = Application.Current?.Resources["BorderSecondary"] as Style;
            var _styleTxt = Application.Current?.Resources["PopupPromptButton"] as Style;
            var _theme = Application.Current?.RequestedTheme;
            int col = 0; int row = 0; string _cover = "";
            grid_content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            foreach (var item in viewmodel.AllAlbums)
            {
                string _query = "SELECT * FROM ImageInfo WHERE PhotoGalleryId = '"
                        + item.Id.ToUpper() + "'";
                var _photo = AppDatabase.Instance.SqlConnection.Query<ImageInfo>(_query).FirstOrDefault();
                if (_photo != null)
                {
                    _cover = string.IsNullOrEmpty(_photo.Image) || _photo.Image.Equals("false")
                        ? (_theme == AppTheme.Light ? "ic_nophoto_light" : "ic_nophoto_dark") : _photo.Image;
                }
                else _cover = _theme == AppTheme.Light ? "ic_nophoto_light" : "ic_nophoto_dark";

                var container = new Border
                {
                    Style = _style,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Stroke = Colors.LightGray,
                    Padding = new Thickness(0),
                };
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
                var title = new Label
                {
                    Text = item.Title, MaxLines = 2, FontSize = 14,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Style = _styleTxt,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    Margin = new Thickness(10, 5, 10, 8),
                };
                var _id = new Label { Text = item.Id, IsVisible = false };
                if (item.IsRead == "true") title.Style = default;
                else title.Style = _style;

                stack.Children.Add(_id);
                stack.Children.Add(img);
                stack.Children.Add(title);
                container.Content = stack;

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

    private async void OnCell_Tapped(object? sender, EventArgs e)
    {
        if (sender is not Border view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        var lblId = "";
        if (view.Content != null)
        {
            var _stack = (VerticalStackLayout)view.Content;
            var _id = (Label)_stack.Children[0];
            lblId = _id.Text;
            await Navigation.PushAsync(new PhotoListPage(lblId));
        }
    }
}