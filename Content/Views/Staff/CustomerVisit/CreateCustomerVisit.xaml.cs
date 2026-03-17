using CommunityToolkit.Mvvm.Messaging;
using wwrc_maui.Content.Viewmodels.Staff;
using wwrc_maui.Content.Views.Staff.Directory;
using static wwrc_maui.Content.Helper.ReferenceMessenger;

namespace wwrc_maui.Content.Views.Staff.CustomerVisit;

public partial class CreateCustomerVisit : ContentPage
{
    CustomerVisitDetailsVm viewmodel = new();

    public CreateCustomerVisit()
    {
        InitializeComponent();
        navbar.OnLeftIconTapped += async () => { await Navigation.PopAsync(); };
        BindingContext = viewmodel;
        Initialize();

        WeakReferenceMessenger.Default.Register<StringNotify>(this, (receiver, message) =>
        {
            if (message.Value != null && message.Value.Equals("CustomerVisitSelectAttendees"))
            { viewmodel.SetAttendeesFromDB(); }
        });
    }

    async void Initialize()
    {
        viewmodel.IsBusy = true; viewmodel.IsRefreshing = true;
        await Task.Delay(300);
        await viewmodel.InitializeCreate();
        viewmodel.IsBusy = false; viewmodel.IsRefreshing = false;
    }

    async void OnAttendees_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Grid view) return;
        await view.FadeTo(0.3, 200);
        view.Opacity = 1;
        await Navigation.PushAsync(new StaffDirectoryMainPage(true));
    }

    void OnDate_DateSelected(object sender, DateChangedEventArgs e)
    { if (sender is not DatePicker view) return; }

    private async void OnSave_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button view) return;
        var _res = await viewmodel.SaveNewVisit();
        if (_res)
        {
            await App.DisplayAlert("Success", "Customer visit details saved.", null, "Okay");
            await Navigation.PopAsync();
        }
    }
}