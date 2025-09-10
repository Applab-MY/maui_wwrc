using Microsoft.Maui.Controls.Shapes;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Views.Dashboard;

public partial class CarouselCell : ContentView
{
    public List<string> MenuFilter { get; set; } = [];
    public List<DashboardCarouselTemplate> MenuList { get; set; } = [];

    public CarouselCell() { InitializeComponent(); }

    public void BuildView()
    {
        if (MenuFilter.Count > 0)
        {
            int row = 0, col = 0;
            grid_cell.Children.Clear();
            foreach (var item in MenuFilter)
            {
                var data = MenuList.Where(_item => _item.Type.Equals(item)).FirstOrDefault();
                if (data != null)
                {
                    var content = BuildCellView(data);
                    var countNo = BuildCountView(data);
                    if (col < 4)
                    {
                        grid_cell.Add(content, col, row);
                        grid_cell.Add(countNo, col, row);
                        col++;
                    }
                    else
                    {
                        row++; col = 0;
                        grid_cell.Add(content, col, row);
                        grid_cell.Add(countNo, col, row);
                        col++;
                    }
                }
            }
        }
    }

    VerticalStackLayout BuildCellView(DashboardCarouselTemplate data)
    {
        var style = Application.Current?.Resources["PopupPromptButton"] as Style;
        var content = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            Margin = new Thickness(10),
        };
        var imgIcon = new Image
        {
            Source = data.Image,
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        var lblTxt1 = new Label
        {
            Text = data.FirstLabel,
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            LineBreakMode = LineBreakMode.TailTruncation,
            MaxLines = 1,
            Style = style
        };
        var lblTxt2 = new Label
        {
            Text = data.SecondLabel,
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            LineBreakMode = LineBreakMode.TailTruncation,
            MaxLines = 1,
            Style = style
        };
        content.Children.Add(imgIcon);
        content.Children.Add(lblTxt1);
        content.Children.Add(lblTxt2);
        return content;
    }

    Border BuildCountView(DashboardCarouselTemplate data)
    {
        var content = new Border
        {
            BackgroundColor = Colors.Red,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            Padding = new Thickness(8, 5),
            IsVisible = !string.IsNullOrEmpty(data.CountType)
        };
        var lblCount = new Label
        {
            Text = data.CountType,
            FontSize = 12,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        content.Content = lblCount;
        return content;
    }
}