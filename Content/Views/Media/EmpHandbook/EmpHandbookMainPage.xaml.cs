using wwrc_maui.Content.Viewmodels.Media.EmpHandbook;
using static wwrc_maui.Content.Model.EmpHandbookModel;

namespace wwrc_maui.Content.Views.Media.EmpHandbook;

public partial class EmpHandbookMainPage : ContentPage
{
    EmpHandbookVm viewmodel = new();

    public EmpHandbookMainPage(string branch, string folderId)
    {
        InitializeComponent();
        viewmodel.branch = branch;
        viewmodel.folderId = folderId;
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();
    }

    public async void Initialize()
    {
        await Task.Delay(300);
        viewmodel.SetupData();
        if (viewmodel.branch.Equals("*"))
            viewmodel.SelectedBranch = "All";
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (sender is not ListView lv) return;
        lv.SelectedItem = null;
    }

    public async Task OpenDetailsView(EmployeeHandbookMainModel? model)
    {
        var cell = new HandbookDetailsCell { model = model };
        await App.DisplayAlert("Handbook Details", null, cell, "Close");
    }
}