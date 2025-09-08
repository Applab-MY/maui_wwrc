namespace wwrc_maui.Content.Model
{
    public class SOModel
    {
        public class SalesOrderMainModel
        {
            public string Date { get; set; } = "";
            public string Records { get; set; } = "";
            public List<SOList> Data { get; set; } = [];
        }

        public class SOList
        {
            public string Id { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserID { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string SONO { get; set; } = "";
            public string PostingDate { get; set; } = "";
            public string DeliveryDate { get; set; } = "";
            public string ApprovedDate { get; set; } = "";
            public string Currency { get; set; } = "";
            //public int ApprovedTime { get; set; }
            public string DODate { get; set; } = "";
            public string DocStatus { get; set; } = "";
            public string DocTotal { get; set; } = "";
            public List<SOItemList> Items { get; set; } = [];
            public List<DOList> DOListBySO { get; set; } = [];
        }

        public class SOItemList
        {
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string UnitPrice { get; set; } = "";
            public string Quantity { get; set; } = "";
            public string OpenQTY { get; set; } = "";
        }

        public class DOList
        {
            public string Id { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string DONO { get; set; } = "";
            public string DocNum { get; set; } = "";
            public string DocEntry { get; set; } = "";
            public string DODate { get; set; } = "";
            public string DeliveryDate { get; set; } = "";
            public string ReturnDate { get; set; } = "";
            public string CloseDate { get; set; } = "";
            public string DoStatus { get; set; } = "";
            public string Currency { get; set; } = "";
            public List<DOItemList> Items { get; set; } = [];
        }

        public class DOItemList
        {
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public double Quantity { get; set; }
        }

        public class DB_SalesOrderModel
        {
            public string Date { get; set; } = "";
            public string Records { get; set; } = "";
        }

        public class Db_SOList
        {
            public string Id { get; set; } = "";
            public string SalesDate { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserID { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string SONO { get; set; } = "";
            public string PostingDate { get; set; } = "";
            public string DeliveryDate { get; set; } = "";
            public string ApprovedDate { get; set; } = "";
            public string DODate { get; set; } = "";
            public string DocStatus { get; set; } = "";
            public string DocTotal { get; set; } = "";
            public string Currency { get; set; } = "";
        }

        public class Db_DOList
        {
            public string Id { get; set; } = "";
            public string DOId { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string DONO { get; set; } = "";
            public string DocNum { get; set; } = "";
            public string DocEntry { get; set; } = "";
            public string DODate { get; set; } = "";
            public string DeliveryDate { get; set; } = "";
            public string ReturnDate { get; set; } = "";
            public string CloseDate { get; set; } = "";
            public string DoStatus { get; set; } = "";
            public string ImageDotSource { get; set; } = "";
            public string Currency { get; set; } = "";
        }

        public class Db_SOItemList
        {
            public string Id { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string UnitPrice { get; set; } = "";
            public string Quantity { get; set; } = "";
            public string OpenQTY { get; set; } = "";
        }

        public class Db_DOItemsList
        {
            public string Id { get; set; } = "";
            public string SOId { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public double Quantity { get; set; }
        }

        public class API_SalesOrder
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserId { get; set; } = "";
        }

        public class API_DObySO
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string DocNum { get; set; } = "";
        }
    }
}
