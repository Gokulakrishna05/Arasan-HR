using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{ 
    public class AccConfig
    {
    
        public List<SelectListItem> Schemelst { get; set; }
        public string Scheme { get; set; }
        public string ID { get; set; }
        
        public string TransactionName { get; set; }
        public string TransactionID { get; set; }

        //public List<ConfigItem> ConfigLst { get; set; }


        public string Type { get; set; }
        public string Tname { get; set; }
        public string Schname { get; set; }
        public List<SelectListItem> ledlst { get; set; }
        public string ledger { get; set; }
        public string saveledger { get; set; }
        public string Isvalid { get; set; }
        public string Active { get; set; }

    }
    //public class ConfigItem
    //{
    //    public string ID { get; set; }

    //    public string Type { get; set; }
    //    public string Tname { get; set; }
    //    public string Scheme { get; set; }
    //     public List<SelectListItem> ledlst { get; set; }
    //    public string ledger { get; set; }
    //    public string saveledger { get; set; }
    //    public string Isvalid { get; set; }

       
    //}
}
