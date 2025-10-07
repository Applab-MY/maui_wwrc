using static wwrc_maui.Content.Model.MediaModel;

namespace wwrc_maui.Content.Views.Media.Gallery.Photos;

public partial class InfoCell : ContentView
{
	public DB_Album? Album = null;

    public InfoCell() { InitializeComponent(); }

	public void Initialize()
	{
		if (Album != null)
		{
			lbl_title.Text = Album.Title;
			lbl_date.Text = Album.AlbumDate.ToString("dd/MM/yyyy");
            lbl_desc.Text = Album.Description;
            lbl_createOn.Text = Album.CreateDate.ToString("dd/MM/yyyy");
            lbl_createBy.Text = Album.CreatedBy;
        }
    }
}