using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DirectAddition
    {
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ChellanNo { get; set; }
        public string Reason { get; set; }
        public string Gro { get; set; }
        public string Entered { get; set; }
        public string Narr { get; set; }


        public DirectAddition()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<DirectItem> Itlst { get; set; }
    }
    public class DirectItem
    {
        public List<SelectListItem> Itlst { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string BinID { get; set; }
        public string Batch { get; set; }
        public string serial { get; set; }
        public string Expiry { get; set; }
        public string Qty { get; set; }
        public string Cost { get; set; }
        public string Amount { get; set; }
        public string Center { get; set; }
        public string Process { get; set; }


    }
}
