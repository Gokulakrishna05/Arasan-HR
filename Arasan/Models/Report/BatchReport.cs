using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class BatchReport
    {
        public BatchReport()
        {
            this.Worklst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            this.Pschlst = new List<SelectListItem>();
            //this.Itemlst = new List<SelectListItem>();
        }
        public string Pschno { get; set; }

        public List<SelectListItem> Pschlst;
        public string ID { get; set; }

        public List<SelectListItem> Worklst;
        public List<SelectListItem> Processlst;

        public string WorkCenter { get; set; }
        public string Process { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class BatchReportItems
    {
        public string type { get; set; }
        public string work { get; set; }
        public string process { get; set; }
        public string seq { get; set; }
        public string item { get; set; }
        public string unit { get; set; }
        public string qty { get; set; }
        public string wipqty { get; set; }
        public string mtono { get; set; }

    }
    public class SchReportItems
    {
        public string date { get; set; }
        public string work { get; set; }
        public string processid { get; set; }
        public string shiftid { get; set; }
        public string itemid { get; set; }
        public string drumid { get; set; }
        public string ibatchid { get; set; }
        public string qtyid { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string schno { get; set; }
        public string batch { get; set; }

    }
}
