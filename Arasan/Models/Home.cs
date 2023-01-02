using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class Home
    {
        public List<QcNotify> qcNotifies { get; set; }
    }
    public class QcNotify
    {
        public string GateDate { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Unit { get; set; }
            
    }
}
