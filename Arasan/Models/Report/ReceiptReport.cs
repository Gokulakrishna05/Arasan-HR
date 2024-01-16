using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class ReceiptReport
    {
        public ReceiptReport()
        {
            this.Brlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        
        public string Branch { get; set; }
        public string Sdate { get; set; }
        public string Edate { get; set; }
        
    }

    public class ReceiptReportItem
    {
        public long id { get; set; }
        public string loc { get; set; }
        public string dcdate { get; set; }
        public string docNo { get; set; }
        public string des { get; set; }
        public string unit { get; set; }
        public string dcqty { get; set; }
        public string recdate { get; set; }
        public string recno { get; set; }
        public string recqty { get; set; }
        public string rejqty { get; set; }
        public string accqty { get; set; }
        public string pend { get; set; }
    }
}
