using System.Collections.ObjectModel;

namespace wwrc_maui.Content.Model
{
    public class CustomerAgingModel
    {
        //public class CAModel
        //{
        //    public string Name { get; set; } = "";
        //    public string Id { get; set; } = "";
        //    public string Date { get; set; } = "";
        //    public string Status { get; set; } = "";
        //    public double Outstanding { get; set; }
        //    public string Icon { get; set; } = "";
        //}

        public class CustomerAgingMainModel
        {
            public string TotalOverdue { get; set; } = "";
            public ObservableCollection<CustAgingModel> Data { get; set; } = [];
        }

        public class CustAgingModel
        {
            public string Id { get; set; } = "";
            public string BPCurrency { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string SlpCode { get; set; } = "";
            public string SlpName { get; set; } = "";
            public string email { get; set; } = "";
            public double CreditLimit { get; set; }
            public double AvailableLimit { get; set; }
            public double UtilizedLimit { get; set; }
            public string StartDate { get; set; } = "";
            public string EndDate { get; set; } = "";
            public string Terms { get; set; } = "";
            public string ALERT { get; set; } = "";
            public double Current { get; set; }
            public double Days30 { get; set; }
            public double Days60 { get; set; }
            public double Days90 { get; set; }
            public double Days120 { get; set; }
            public double Days150 { get; set; }
            public double Over150Days { get; set; }
            public double TotalOverdue { get; set; }
        }

        public class API_CustomerAging
        {
            public string DBase { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string Age { get; set; } = "";
            public string UserId { get; set; } = "";
        }

        #region database model
        public class DB_CustAging
        {
            public string Id { get; set; } = "";
            public string BPCurrency { get; set; } = "";
            public string Country { get; set; } = "";
            public string Company { get; set; } = "";
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string SlpCode { get; set; } = "";
            public string SlpName { get; set; } = "";
            public string email { get; set; } = "";
            public double CreditLimit { get; set; }
            public double AvailableLimit { get; set; }
            public double UtilizedLimit { get; set; }
            public string Terms { get; set; } = "";
            public string startDate { get; set; } = "";
            public string endDate { get; set; } = "";
            public string ALERT { get; set; } = "";
            public double Current { get; set; }
            public double Days30 { get; set; }
            public double Days60 { get; set; }
            public double Days90 { get; set; }
            public double Days120 { get; set; }
            public double Days150 { get; set; }
            public double Over150Days { get; set; }
            public string status { get; set; } = "";
            public double TotalOverdue { get; set; }
        }

        public class DB_MonthsModel
        {
            public string CardCode { get; set; } = "";
            public string CardName { get; set; } = "";
            public string AgingId { get; set; } = "";
            public string Month { get; set; } = "";
            public string MonthString { get; set; } = "";
            public double TotalOutstanding { get; set; }
        }

        public class DB_DocListModel
        {
            public string AgingId { get; set; } = "";
            public string Month { get; set; } = "";
            public string DocNum { get; set; } = "";
            public string DocDate { get; set; } = "";
            public double Outstanding { get; set; }
        }
        #endregion

        #region aging details
        public class AgingDetailMainModel
        {
            public string Age { get; set; } = "";
            public double TotalOutstanding { get; set; }
            public List<DocListModel> DocList { get; set; } = [];
        }

        public class AgingDetailModel
        {
            public string CustomerAgingId { get; set; } = "";
            public double TotalOutstanding { get; set; }
            public List<DocListModel> DocList { get; set; } = [];
        }

        public class DocListModel
        {
            public string CardCode { get; set; } = "";
            public string DocNum { get; set; } = "";
            public string DocDate { get; set; } = "";
            public double Outstanding { get; set; }
        }
        #endregion
    }
}