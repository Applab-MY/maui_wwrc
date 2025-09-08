namespace wwrc_maui.Content.Model
{
    public class StockModel
    {
        #region warehouses items
        public class WarehouseItem
        {
            public string ItemCode { get; set; } = "";
            public string Warehouse { get; set; } = "";
            public double OnHandPW { get; set; }
            public IsCommitedPW IsCommitedPW { get; set; } = new IsCommitedPW();
            public double OnOrderPW { get; set; }
            public double AvailablePW { get; set; }
            public double ItemCostPW { get; set; }
            public string TotalCommited { get; set; } = "";
        }

        public class IsCommitedPW
        {
            public string ItemCode { get; set; } = "";
            public string TotalCommited { get; set; } = "";
            public List<CustomerItem> Customer { get; set; } = [];
        }

        public class CustomerItem
        {
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string Commited { get; set; } = "";
        }

        public class DB_WarehouseItem
        {
            public string ItemCode { get; set; } = "";
            public string Warehouse { get; set; } = "";
            public double OnHandPW { get; set; }
            public double OnOrderPW { get; set; }
            public double AvailablePW { get; set; }
            public double ItemCostPW { get; set; }
            public string TotalCommited { get; set; } = "";
        }

        public class DB_CustomerItem
        {
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string Commited { get; set; } = "";
            public string ItemCode { get; set; } = "";
        }

        public class DB_IsCommitedPW
        {
            public string ItemCode { get; set; } = "";
            public string TotalCommited { get; set; } = "";
            public string Warehouse { get; set; } = "";
            public double OnHandPW { get; set; }
            public double OnOrderPW { get; set; }
            public double AvailablePW { get; set; }
            public double ItemCostPW { get; set; }
        }

        public class DB_IsCommitedPW_Customer
        {
            public string ItemCode { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string Commited { get; set; } = "";
            public string TotalCommited { get; set; } = "";
            public string Warehouse { get; set; } = "";
        }

        public class DB_WarehouseTable
        {
            public string ItemCode { get; set; } = "";
            public string TotalCommited { get; set; } = "";
            public string Warehouse { get; set; } = "";
            public double OnHandPW { get; set; }
            public double OnOrderPW { get; set; }
            public double AvailablePW { get; set; }
            public double ItemCostPW { get; set; }
        }
        #endregion

        #region stock alert items
        public class StockAlertMainModel
        {
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string Buyer { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string ItmsGrpName { get; set; } = "";
            public double OnHand { get; set; }
            public double IsCommited { get; set; }
            public double OnOrder { get; set; }
            public double Available { get; set; }
            public double MinLevel { get; set; }
            public double ItemCost { get; set; }
            public string LastPurchPrice { get; set; } = "";
            public string LastPurchCurr { get; set; } = "";
            public string LastPurchDate { get; set; } = "";
            public string LastSalesPrice { get; set; } = "";
            public string LastSalesCur { get; set; } = "";
            public string LastSalesDate { get; set; } = "";
            public string Manufacturer { get; set; } = "";
            public string ALERT { get; set; } = "";
            public List<WarehouseItem> Warehouse { get; set; } = [];
            public string GetStockGroup() { return ALERT[0].ToString().ToUpper(); }
        }

        public class API_StockAlert
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string UserId { get; set; } = "";
        }

        public class DB_StockAlert
        {
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string Buyer { get; set; } = "";
            public string ItemCode { get; set; } = "";
            public string ItemName { get; set; } = "";
            public string ItmsGrpName { get; set; } = "";
            public double OnHand { get; set; }
            public double IsCommited { get; set; }
            public double OnOrder { get; set; }
            public double Available { get; set; }
            public double MinLevel { get; set; }
            public double ItemCost { get; set; }
            public string LastPurchPrice { get; set; } = "";
            public string LastPurchCurr { get; set; } = "";
            public string LastPurchDate { get; set; } = "";
            public string LastSalesPrice { get; set; } = "";
            public string LastSalesCur { get; set; } = "";
            public string LastSalesDate { get; set; } = "";
            public string Manufacturer { get; set; } = "";
            public string ALERT { get; set; } = "";
        }
        #endregion
    }
}
