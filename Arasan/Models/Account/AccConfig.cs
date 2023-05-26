using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{ 
    public class AccConfig
    {
    
        public List<SelectListItem> DisLedlst;
        public string DisLed { get; set; }
        public string Discount { get; set; }
        public List<SelectListItem> PackLedlst;
        public string PackLed { get; set; }
        public string Packing { get; set; }
        public List<SelectListItem> FriLedlst;
        public string FriLed { get; set; }
        public string Frieght { get; set; }
        public List<SelectListItem> OthLedlst;
        public string Other { get; set; }
        public string OthLed { get; set; }
        public List<SelectListItem> PlusLedlst;
        public string PlusLed { get; set; }
        public string Plus { get; set; }
        public List<SelectListItem> MinLedlst;
        public string MinLed { get; set; }
        public string Minus { get; set; }
        
        public List<SelectListItem> BankLedlst;
        public string BankLed { get; set; }
        public string Bank { get; set; }
        public List<SelectListItem> CashLedlst;
        public string CashLed { get; set; }
        public string Cash { get; set; }
        public string ID { get; set; }

        public string Type { get; set; }
        public List<ConfigItem> ConfigLst { get; set; }
    }
    public class ConfigItem
    {
        public string ID { get; set; }

        public string Type { get; set; }
        public List<SelectListItem> Ledlst;
        public string Ledger { get; set; }
    }
}
