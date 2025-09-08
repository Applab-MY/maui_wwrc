namespace wwrc_maui.Content.Model
{
    public class MediaModel
    {
        public class MediaMainModel
        {
            public List<MediaInfo> PhotoGallery { get; set; } = [];
            public List<VideoInfo> VideoGallery { get; set; } = [];
        }

        public class MediaInfo
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DateTime AlbumDate { get; set; } = new DateTime();
            public string CreatedBy { get; set; } = "";
            public DateTime CreateDate { get; set; } = new DateTime();
            public List<ImageInfo> Images { get; set; } = [];
            public string IsRead { get; set; } = "";
        }

        public class ImageInfo
        {
            public string PhotoGalleryId { get; set; } = "";
            public string Image { get; set; } = "";
            public string ImageSize { get; set; } = "";
            public bool IsDefault { get; set; }
        }

        public class VideoInfo
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string YoutubeUrl { get; set; } = "";
            public string Image { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public DateTime CreateDate { get; set; } = new DateTime();
            public string IsRead { get; set; } = "";
        }

        public class AlbumInfo
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Image { get; set; } = "";
        }

        public class APIPhotoModel
        {
            public string dBase { get; set; } = "";
        }

        public class DB_Album
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DateTime AlbumDate { get; set; }
            public string CreatedBy { get; set; } = "";
            public DateTime CreateDate { get; set; } = new DateTime();
            public string IsRead { get; set; } = "";
        }
    }
}
