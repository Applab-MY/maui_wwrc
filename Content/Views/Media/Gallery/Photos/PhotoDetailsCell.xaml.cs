using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Views.Media.Gallery.Photos;

public partial class PhotoDetailsCell : ContentView
{
	public DB_Album? Album = null;
	public ImageInfo? Photo = null;

    public PhotoDetailsCell() { InitializeComponent(); }

	public void Initialize()
	{
		if (Album != null && Photo != null)
		{
            string _file = Photo.Image.Split('/').Last();
            string _type = Photo.Image.Split('.').Last();

            img_file.Source = Photo.Image;
            lbl_filename.Text = _file;
            lbl_filesize.Text = Photo.ImageSize;

            lbl_type.Text = _type;
            lbl_createOn.Text = Album.CreateDate.ToString("dd/MM/yyyy");
            lbl_createBy.Text = Album.CreatedBy;
		}
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		if (sender is not Button btn) return;

    }
}