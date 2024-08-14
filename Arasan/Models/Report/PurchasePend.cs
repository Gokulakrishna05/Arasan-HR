using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class PurchasePend
    {
        public PurchasePend()
        {
            this.Brlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;

        public string Branch { get; set; }
        public string Sdate { get; set; }
        public string Edate { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }

    }

    public class PurchasePendItem
    {
        public long id { get; set; }
        public string loc { get; set; }
        public string did { get; set; }
        public string dcdate { get; set; }
        public string item { get; set; }
        public string unit { get; set; }
        public string pend { get; set; }
        public string pur { get; set; }
        public string due { get; set; }
        
        public string ord { get; set; }
        
        public string narr { get; set; }
        public string app2 { get; set; }
        public string trans { get; set; }
        public string entry { get; set; }
        public string pdays { get; set; }
    }
}