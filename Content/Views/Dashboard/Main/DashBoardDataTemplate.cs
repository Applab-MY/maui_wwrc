namespace wwrc_maui.Content.Views.Dashboard.Main
{
    public class DashBoardDataTemplate : DataTemplateSelector
    {
        public DataTemplate? FirstTemplate { get; set; }
        public DataTemplate? SecondTemplate { get; set; }
        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item.Equals("1")) return FirstTemplate;
            else if (item.Equals("2")) return SecondTemplate;
            else return null;
        }
    }
}