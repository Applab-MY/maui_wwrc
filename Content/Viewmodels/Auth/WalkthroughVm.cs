using System.Collections.ObjectModel;
using wwrc_maui.Content.Model.Auth;
using wwrc_maui.Content.Viewmodels.Common;

namespace wwrc_maui.Content.Viewmodels.Auth
{
    public class WalkthroughVm : BaseViewModel
    {
        public ObservableCollection<WalkthroughModel> ItemsSource { get; set; } = [];
        public double ImgMaxHeigth { get; set; } = 0;

        public WalkthroughVm()
        {
            IsBusy = false;
            ImgMaxHeigth = Math.Round(App.ScreenHeight / 3, MidpointRounding.ToEven) - 20;
            var model = new List<WalkthroughModel>()
            {
                new()
                {
                    Image = "wt_staff",
                    Title = "Staff Directory",
                    Description1 = "Easy access to every employee ",
                    Description2 = "of your company in just ",
                    Description3 = "few clicks.",
                    ImgHeight = ImgMaxHeigth,
                },
                new()
                {
                    Image = "wt_stock_alert",
                    Title = "Stock Alert",
                    Description1 = "Real-Time stock level alert, you ",
                    Description2 = "will be notified when limited ",
                    Description3 = "stock on the item.",
                    ImgHeight = ImgMaxHeigth,
                },
                new()
                {
                    Image = "wt_so",
                    Title = "Sales Order",
                    Description1 = "Improve sales performance ",
                    Description2 = "by speeding up the  ",
                    Description3 = "ordering process.",
                    ImgHeight = ImgMaxHeigth,
                },
            };
            ItemsSource = new ObservableCollection<WalkthroughModel>(model);
        }
    }
}