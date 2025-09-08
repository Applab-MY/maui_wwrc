namespace wwrc_maui.Content.Model
{
    public class SalesModel
    {
        public class SalesResultModel
        {
            public string ALERT { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string InvoiceTotalAmount { get; set; } = "";
        }

        public class SalesMainModel
        {
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string CARDCODE { get; set; } = "";
            public string CARDNAME { get; set; } = "";
            public string UserID { get; set; } = "";
            public double DocTotal { get; set; }
        }
    }
}