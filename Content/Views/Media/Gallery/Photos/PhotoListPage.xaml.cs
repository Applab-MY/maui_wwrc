using Microsoft.Maui.Controls.Shapes;
using wwrc_maui.Content.Viewmodels.Media;
using wwrc_maui.Content.Views.Media.Gallery.Photos;

namespace wwrc_maui.Content.Views.Media.Gallery;

public partial class PhotoListPage : ContentPage
{
    GalleryDetailsVm viewmodel = new();
    InfoCell infoCell = new();

    public PhotoListPage(string id)
    {
        InitializeComponent();
        viewmodel.albumId = id;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        navbar.OnRightIconTapped += async () => { await App.DisplayAlert("Album Details", null, infoCell, "Close"); };
        BindingContext = viewmodel;
        Initialize();
    }

    async void Initialize()
    {
        await Task.Delay(300);
        await viewmodel.GetAlbumById();
        await viewmodel.GetPhotosFromAlbum();
        BuildView();
    }

    void BuildView()
    {
        infoCell.Album = viewmodel.Album;
        infoCell.Initialize();
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

                if (col < 4)
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

    async void OnCell_Tapped(object? sender, EventArgs e)
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
            await Navigation.PushAsync(new PhotoDetailsPage(viewmodel.albumId, lblId.Substring(5)));
        }
    }
}