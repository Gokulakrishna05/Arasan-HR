using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class WorkCenterReport
    {
        public WorkCenterReport()
        {
            this.Worklst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            //this.ItemGrouplst = new List<SelectListItem>();
            //this.Itemlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        //public List<SelectListItem> Brlst;
        //public List<SelectListItem> Suplst;
        public List<SelectListItem> Worklst;
        public List<SelectListItem> Processlst;

        public string WorkCenter { get; set; }
        public string Process { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class WorkCenterReportItems
    {
        public string process { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string wc { get; set; }
    }
    public class APProdReportItems
    {
        public string process { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string wc { get; set; }

    }
}
