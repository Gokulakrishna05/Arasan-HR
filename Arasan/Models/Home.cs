using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class Home
    {
        public List<QcNotify> qcNotifies { get; set; }
        public List<Notify>  Notifies { get; set; }
    }
    public class QcNotify
    {
        public string GateDate { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Unit { get; set; }
         
      
    }
    public class Notify
    {
        
        public string Doc { get; set; }
        public string Item { get; set; }
        public string Drum { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
    }
}
