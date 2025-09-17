using Microsoft.Maui.Controls.Shapes;
using wwrc_maui.Content.Viewmodels.Media;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class PhotoListPage : ContentPage
{
    GalleryDetailsVm viewmodel = new();

    public PhotoListPage(string id)
    {
        InitializeComponent();
        viewmodel.albumId = id;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        viewmodel.OnFinishLoad += BuildView;
        BindingContext = viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewmodel.GetAlbumById();
    }

    void BuildView()
    {
        grid_content.Children.Clear();
        grid_content.RowDefinitions.Clear();
        if (viewmodel != null && viewmodel.AllPhotos.Count > 0)
        {
            var _style = Application.Current?.Resources["BorderSecondary"] as Style;
            var _styleTxt = Application.Current?.Resources["PopupPromptButton"] as Style;
            int col = 0; int row = 0;
            grid_content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            foreach (var item in viewmodel.AllPhotos)
            {
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
                    Source = item.Image,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 80,
                };
                var _id = new Label { Text = item.PhotoGalleryId, IsVisible = false };

                stack.Children.Add(_id);
                stack.Children.Add(img);
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
            var _grid = (Grid)view.Content;
            var _id = (Label)_grid.Children[0];
            lblId = _id.Text;
            await Navigation.PushAsync(new PhotoDetailsPage());
        }
    }
}