namespace wwrc_maui.Content.Model
{
    public class CountryModel
    {
        public class CountryMainModel
        {
            public string Id { get; set; } = "";
            public string CountryCode { get; set; } = "";
            public string CountryName { get; set; } = "";
            public string CountryImage { get; set; } = "";
        }

        public class Root
        {
            public int SystemCode { get; set; }
            public string SystemMessage { get; set; } = "";
            public string SystemDebugMessage { get; set; } = "";
            public List<CountryMainModel> Items { get; set; } = [];
        }

        public class API_CountryModel
        {
            public string DBase { get; set; } = "";
        }
    }
}
