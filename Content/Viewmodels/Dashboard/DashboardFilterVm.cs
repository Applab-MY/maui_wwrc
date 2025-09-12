using System.Diagnostics.Metrics;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.DashboardModel;

namespace wwrc_maui.Content.Viewmodels.Dashboard
{
    public class DashboardFilterVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private List<string> _salesPerson = [];
        private bool _isSubsVisible = false;
        private bool _isSalesVisible = false;
        #endregion
        #region properties
        public List<string> SalesPerson
        {
            get { return _salesPerson; }
            set { SetProperty(ref _salesPerson, value); }
        }
        public bool IsSubsVisible
        {
            get { return _isSubsVisible; }
            set { SetProperty(ref _isSubsVisible, value); }
        }
        public bool IsSalesVisible
        {
            get { return _isSalesVisible; }
            set { SetProperty(ref _isSalesVisible, value); }
        }
        #endregion
        #endregion
        #region fields props
        public DashboardMainModel? MainModel { get; set; } = null;
        public FilterDataModel? FilterModel { get; set; } = null;
        public List<string> CountryList { get; set; } = [];
        public List<string> SubsList { get; set; } = [];
        public List<SalesPersonList> SalesList { get; set; } = [];
        public Command? RefreshCommand { get; set; } = null;
        public Command? ClearFilterCommand { get; set; } = null;
        public Action? OnRefreshTap { get; set; } = null;
        public Action? OnClearFilterTap { get; set; } = null;
        #endregion

        public DashboardFilterVm()
        {
            IsSubsVisible = false; IsSalesVisible = false;
            RefreshCommand = new Command(RefreshDashboard);
            ClearFilterCommand = new Command(ClearFilter);
        }

        public void SetupCountry()
        {
            CountryList = [];
            if (MainModel != null)
            {
                foreach (var data in MainModel.Filter)
                    CountryList.Add(data.Country);
            }
        }

        public void SetupSubsidiary(string country)
        {
            SubsList = [];
            if (MainModel != null)
            {
                var found = MainModel.Filter.Where(x => x.Country.Equals(country)).FirstOrDefault();
                if (found != null)
                {
                    foreach (var item in found.SubsidiaryList)
                        if (!SubsList.Contains(item.Subsidiary))
                            SubsList.Add(item.Subsidiary);
                }
            }
        }

        public void SetupSalesList(string country, string subs)
        {
            SalesList = [];
            if (MainModel != null)
            {
                var found = MainModel.Filter.Where(x => x.Country.Equals(country)).FirstOrDefault();
                if (found != null)
                {
                    if (found.SubsidiaryList != null && found.SubsidiaryList.Count > 0)
                    {
                        var _subs = found.SubsidiaryList.Where(x => x.Subsidiary.Equals(subs)).FirstOrDefault();
                        if (_subs != null)
                            SalesList = [.. _subs.SalesPersonList];
                    }
                }
            }
        }

        void RefreshDashboard()
        {
            if (FilterModel != null)
            {
                Preferences.Default.Set("country", FilterModel.Country);
                Preferences.Default.Set("subsidiary", FilterModel.Subsidiary);
                Preferences.Default.Set("userId", FilterModel.UserId);
                Preferences.Default.Set("userTitle", FilterModel.UserId);
                OnRefreshTap?.Invoke();
            }
        }

        void ClearFilter()
        {
            FilterModel = new FilterDataModel();
            Preferences.Default.Set("country", "");
            Preferences.Default.Set("subsidiary", "");
            Preferences.Default.Set("userId", "");
            Preferences.Default.Set("userTitle", "");
            OnClearFilterTap?.Invoke();
        }
    }
}