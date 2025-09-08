namespace wwrc_maui.Content.Model
{
    public class NewsModel
    {
        public class NewsMainModel
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string Date { get; set; } = "";
            public string Attachment { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public string IsRead { get; set; } = "";
            public List<NewsInfo> Images { get; set; } = [];
        }

        public class NewsInfo
        {
            public string NewsId { get; set; } = "";
            public string ImageUrl { get; set; } = "";
            public string IsDefault { get; set; } = "";
        }

        public class NewsInfoTable
        {
            public string NewsId { get; set; } = "";
            public string ImageUrl { get; set; } = "";
            public string IsDefault { get; set; } = "";
        }

        public class NewsTable
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string Date { get; set; } = "";
            public string Attachment { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public string IsRead { get; set; } = "";
            public string DefaultImg { get; set; } = "";
            public string DefaultImgFalse { get; set; } = "";
        }

        public class APINewsModel
        {
            public string DBase { get; set; } = "";
        }
    }
}
