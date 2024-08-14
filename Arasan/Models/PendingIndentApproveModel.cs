using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PendingIndentApproveModel
    {
        public PendingIndentApproveModel()
        {
            //this.Worklst = new List<SelectListItem>();
            //this.Processlst = new List<SelectListItem>();
            //this.Pschlst = new List<SelectListItem>();
            //this.Itemlst = new List<SelectListItem>();
        }
        public string Pschno { get; set; }

        public List<SelectListItem> Pschlst;
        public string ID { get; set; }

        //public List<SelectListItem> Worklst;
        //public List<SelectListItem> Processlst;

        //public string WorkCenter { get; set; }
        //public string Process { get; set; }
        public string dtFrom { get; set; }
        //public string dtTo { get; set; }
    }
    public class PendingIndentApproveModelItems
    {
        public string docid { get; set; }
        public string docdate { get; set; }
        public string itemid { get; set; }
        public string unitid { get; set; }
        public string ordqty { get; set; }
        public string purqty { get; set; }
        public string pendqty { get; set; }
        public string duedate { get; set; }
        public string locid { get; set; }
        public string narration { get; set; }
        public string app1dt { get; set; }
        public string app2dt { get; set; }
        public string enddate { get; set; }

    }
    //public class SchReportItems
    //{
    //    public string date { get; set; }
    //    public string work { get; set; }
    //    public string processid { get; set; }
    //    public string shiftid { get; set; }
    //    public string itemid { get; set; }
    //    public string drumid { get; set; }
    //    public string ibatchid { get; set; }
    //    public string qtyid { get; set; }
    //    public string rate { get; set; }
    //    public string amount { get; set; }
    //    public string schno { get; set; }
    //    public string batch { get; set; }

    //}
}
