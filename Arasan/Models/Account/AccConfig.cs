using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AccConfig
    {
        
        public string Scheme { get; set; }
        public string SchemeDes { get; set; }
        public string ID { get; set; }
        
        public string TransactionName { get; set; }
        public string TransactionID { get; set; }
        public string Branch { get; set; }
        
        public string CreatBy { get; set; }
        public string CreatOn { get; set; }
        public string CurrDate { get; set; }

        public string Active { get; set; }
        public string viewrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string ddlStatus { get; set; }

        //public List<ConfigItem> ConfigLst { get; set; }

        public List<ConfigItem> ConfigLst { get; set; }

    }

    public class ConfigItem
    {
       
       public string ID { get; set; }

        public string Type { get; set; }
        public string Tname { get; set; }
        public string Schname { get; set; }
        public List<SelectListItem> ledlst { get; set; }
        
        public string ledger { get; set; }
        public string saveledger { get; set; }
        public string Isvalid { get; set; }


    }
    public class Config
    {
       
       public string id { get; set; }

        public string schemedes { get; set; }
        public string scheme { get; set; }
        public string transactionName { get; set; }
        
        public string transactionid { get; set; }
        public string viewrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }
}
