using wwrc_maui.Content.Model;
using wwrc_maui.Content.Viewmodels.Common;
using static wwrc_maui.Content.Model.CurrencyModel;

namespace wwrc_maui.Content.Viewmodels.Dashboard
{
    public class CurrencyExchangeVm : BaseViewModel
    {
        #region bindable properties
        #region beans
        private List<DB_Currency> _currencies = [];
        private string _localCurrency = "";
        private bool _noCurrencies = false;
        private List<DateTime> _datelist = [];
        private string _selectedDate = "";
        #endregion
        #region properties
        public List<DB_Currency> Currencies
        {
            get { return _currencies; }
            set
            {
                SetProperty(ref _currencies, value);
                NoCurrencies = value == null || value.Count == 0;
            }
        }
        public string LocalCurrency
        {
            get { return _localCurrency; }
            set { SetProperty(ref _localCurrency, value); }
        }
        public bool NoCurrencies
        {
            get { return _noCurrencies; }
            set { SetProperty(ref _noCurrencies, value); }
        }
        public List<DateTime> DateList
        {
            get { return _datelist; }
            set { SetProperty(ref _datelist, value); }
        }
        public string SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        #endregion
        #endregion

        public CurrencyExchangeVm()
        {
            DateList = []; Currencies = [];
            SelectedDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void SetDateList()
        {
            DateList = [];
            var today = DateTime.Today;
            DateList.Add(new DateTime(today.Year, today.Month, 1));
            for (int a = 0; a < 7; a++)
            {
                var _day = today.AddDays((a + 1) * -1);
                DateList.Add(new DateTime(_day.Year, _day.Month, _day.Day));
            }
        }

        public async void CurrencyList()
        {
            Currencies = [];
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet && App.AppClient != null)
            {
                var model = new API_Currency
                {
                    Country = Preferences.Default.Get("country", ""),
                    Company = Preferences.Default.Get("subsidiary", ""),
                    Date = SelectedDate
                };
                var _res = await App.AppClient.GetCurrency(model);

                if (_res.SystemCode == 401) { }
                else if (_res.SystemCode == 200)
                {
                    AppDatabase.Instance.SqlConnection.DeleteAll<DB_Currency>();
                    Currencies = [];
                    if (_res.items != null)
                    {
                        var _list = new List<DB_Currency>();
                        LocalCurrency = _res.items.CurrencyCode;
                        foreach (var data in _res.items.ExchangeRate)
                        {
                            DB_Currency currency_db = new()
                            {
                                CurrencyCode = data.CurrencyCode,
                                CurrencyName = data.CurrencyName,
                                ExchangeRate = data.ExchangeRate,
                                CurrencyImage = data.CurrencyImage,
                            };
                            _list.Add(currency_db);
                            AppDatabase.Instance.SqlConnection.Insert(currency_db);
                        }
                        Currencies = _list;
                    }
                }
                //else await App.DisplayAlert("Error", "API error : " + _res.SystemCode.ToString()
                //    + ", " + _res.SystemMessage + "\r" +_res.SystemDebugMessage, null, "Okay");
            }
            else await App.DisplayAlert("No Internet", "Please check your internet connection.", null, "Okay");
        }
    }
}
