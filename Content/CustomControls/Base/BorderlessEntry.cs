namespace wwrc_maui.Content.CustomControls.Base
{
    public class BorderlessEntry : Entry
    {
        public BorderlessEntry()
        {
            PlaceholderColor = Colors.LightGray;
            TextColor = Colors.Black;
            FontSize = 14;
            HeightRequest = 50;
            HorizontalOptions = LayoutOptions.Fill;
        }
    }
}