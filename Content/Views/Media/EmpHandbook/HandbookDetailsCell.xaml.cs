using static wwrc_maui.Content.Model.EmpHandbookModel;

namespace wwrc_maui.Content.Views.Media.EmpHandbook;

public partial class HandbookDetailsCell : ContentView
{
	public EmployeeHandbookMainModel? model { get; set; } = null;

    public HandbookDetailsCell()
	{
		InitializeComponent();
		BindingContext = this;
	}
}