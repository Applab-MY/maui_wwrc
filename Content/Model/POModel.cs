namespace wwrc_maui.Content.Model
{
    public class POModel
    {
        public class POMainModel
        {
            public string Date { get; set; } = "";
            public string Records { get; set; } = "";
            public List<PurchaseItem> Data { get; set; } = [];
        }

        public class PurchaseItem
        {
            public string Id { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserID { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string PONO { get; set; } = "";
            public string PostingDate { get; set; } = "";
            public string CurrencyCode { get; set; } = "";
            public string DocStatus { get; set; } = "";
            public double DocTotal { get; set; }
            public List<POItem> Items { get; set; } = [];
        }

        public class POItem
        {
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string UnitPrice { get; set; } = "";
            public string POOrder { get; set; } = "";
            public string Quantity { get; set; } = "";
            public string ETD { get; set; } = "";
            public string ETA { get; set; } = "";
            public string OpenQty { get; set; } = "";
        }

        public class API_PurchaseModel
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserId { get; set; } = "";
        }

        #region database model
        public class DB_POItem
        {
            public string Id { get; set; } = "";
            public string PONO { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string UnitPrice { get; set; } = "";
            public string POOrder { get; set; } = "";
            public string Quantity { get; set; } = "";
            public string ETD { get; set; } = "";
            public string ETA { get; set; } = "";
            public string OpenQty { get; set; } = "";
        }

        public class DB_Purchase
        {
            public string Date { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserID { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string PONO { get; set; } = "";
            public string PostingDate { get; set; } = "";
            public string CurrencyCode { get; set; } = "";
            public string DocStatus { get; set; } = "";
            public double DocTotal { get; set; }
        }

        public class DB_PurchaseMonth
        {
            public string Date { get; set; } = "";
            public string Records { get; set; } = "";
        }
        #endregion
    }
}
