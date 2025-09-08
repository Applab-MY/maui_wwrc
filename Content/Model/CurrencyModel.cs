namespace wwrc_maui.Content.Model
{
    public class CurrencyModel
    {
        public class CurrencyMainModel
        {
            public string Country { get; set; } = "";
            public string CurrencyCode { get; set; } = "";
            public List<CurrencyItemsModel> ExchangeRate { get; set; } = [];
        }

        public class CurrencyItemsModel
        {
            public string CurrencyCode { get; set; } = "";
            public string CurrencyName { get; set; } = "";
            public double ExchangeRate { get; set; }
            public string CurrencyImage { get; set; } = "";
        }

        public class API_Currency
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string Date { get; set; } = "";
        }

        public class DB_Currency
        {
            public string CurrencyCode { get; set; } = "";
            public string CurrencyName { get; set; } = "";
            public double ExchangeRate { get; set; }
            public string CurrencyImage { get; set; } = "";
        }

        public class DBCurrency2
        {
            public string Country { get; set; } = "";
            public string CurrencyCode { get; set; } = "";
        }
    }
}