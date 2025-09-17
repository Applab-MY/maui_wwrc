namespace wwrc_maui.Content.Helper
{
    public class MediaConverter
    {
        public static byte[] ImageSourceToByteArray(ImageSource source)
        {
            StreamImageSource streamImageSource = (StreamImageSource)source;
            CancellationToken cancellationToken = CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;

            byte[] b;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                b = ms.ToArray();
            }
            return b;
        }

        public static async Task<Stream> GetFileStream(FileResult file)
        { return await file.OpenReadAsync(); }
    }
}
