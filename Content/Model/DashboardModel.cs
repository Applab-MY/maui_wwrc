using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wwrc_maui.Content.Model
{
    public class DashboardModel
    {
        #region dashboard sales person
        public class SalesPersonList
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Position { get; set; } = "";
            public string ProfileImage { get; set; } = "";
        }

        public class SalesPersonChecked : INotifyPropertyChanged
        {
            private bool _checked;
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Position { get; set; } = "";
            public string ProfileImage { get; set; } = "";
            public bool Checked
            {
                get { return _checked; }
                set { _checked = value; NotifyPropertyChanged(); }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public class SubsidiaryListItem
        {
            public string Subsidiary { get; set; } = "";
            public ObservableCollection<SalesPersonList> SalesPersonList { get; set; } = [];
        }
        #endregion

        #region dashboard filter data
        public class FilterItem
        {
            public string Country { get; set; } = "";
            public List<SubsidiaryListItem> SubsidiaryList { get; set; } = [];
        }

        public class FilterDataModel
        {
            public string Country { get; set; } = "";
            public string Subsidiary { get; set; } = "";
            public string UserId { get; set; } = "";
            public string UserName { get; set; } = "";
        }
        #endregion

        public class DashboardMainModel
        {
            public string MTD { get; set; } = "";
            public string MTDP { get; set; } = "";
            public string YTD { get; set; } = "";
            public string YTDP { get; set; } = "";
            public string GPPMTD { get; set; } = "";
            public string GPPYTD { get; set; } = "";
            public string MTDTarget { get; set; } = "";
            public string YTDTarget { get; set; } = "";
            public string StockAlertCount { get; set; } = "";
            public string NewsCount { get; set; } = "";
            public string CatalogCount { get; set; } = "";
            public string HandbookCount { get; set; } = "";
            public double PhotoCount { get; set; }
            public double VideoCount { get; set; }
            public string TotalMediaCount { get; set; } = "";
            public List<FilterItem> Filter { get; set; } = [];
        }

        public class API_DashBoard
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserId { get; set; } = "";
        }

        public class DynamicDashBoardNavigation
        {
            public string Image { get; set; } = "";
            public string FirstLabel { get; set; } = "";
            public string SecondLabel { get; set; } = "";
            public string Type { get; set; } = "";
            public string CountType { get; set; } = "";
        }
    }
}
