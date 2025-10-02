namespace wwrc_maui.Content.Views.Staff.Directory;

public partial class OfficeTabCell : ViewCell
{
    public OfficeTabCell() { InitializeComponent(); }

    private async void OnCell_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        try
        {
            var parent = (StaffDirectoryMainPage)Parent.Parent.Parent.Parent.Parent;
            if (parent.viewmodel.IsSelectable)
            {
                img_select.IsVisible = !img_select.IsVisible;
                var found = parent.viewmodel.selectedStaff.Where(x => x.Equals(lbl_id.Text)).FirstOrDefault();
                if (found == null) parent.viewmodel.selectedStaff.Add(lbl_id.Text);
                else parent.viewmodel.selectedStaff.Remove(lbl_id.Text);
            }
            else await parent.Navigation.PushAsync(new StaffDetailsPage(lbl_id.Text));
        }
        catch (Exception ex)
        {
            var parent = (OthersTabDetailsPage)Parent.Parent.Parent.Parent.Parent;
            if (parent.viewmodel.IsSelectable)
            {
                img_select.IsVisible = !img_select.IsVisible;
                var found = parent.viewmodel.selectedStaff.Where(x => x.Equals(lbl_id.Text)).FirstOrDefault();
                if (found == null) parent.viewmodel.selectedStaff.Add(lbl_id.Text);
                else parent.viewmodel.selectedStaff.Remove(lbl_id.Text);
            }
            else await parent.Navigation.PushAsync(new StaffDetailsPage(lbl_id.Text));
        }
    }
}