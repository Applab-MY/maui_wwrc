using static wwrc_maui.Content.Model.EmpHandbookModel;

namespace wwrc_maui.Content.Views.Media.EmpHandbook;

public partial class EmpHandbookCell : ViewCell
{
    public EmpHandbookCell()
    {
        InitializeComponent();
    }

    private async void OnStatus_Tapped(object sender, EventArgs e)
    {
        if (sender is not StackLayout view) return;
        var tap = (TapGestureRecognizer)view.GestureRecognizers[0];
        var model = tap.CommandParameter as EmployeeHandbookMainModel;
        var parent = (EmpHandbookMainPage)Parent.Parent.Parent.Parent.Parent;
        await parent.OpenDetailsView(model);
    }
}