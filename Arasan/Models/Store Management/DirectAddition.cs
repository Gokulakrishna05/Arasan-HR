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
        public double Gro { get; set; }
        public double Net { get; set; }
        public List<SelectListItem> assignList;
        public string Entered { get; set; }
        public string Narr { get; set; }
        public string status { get; set; }

        public DirectAddition()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<DirectItem> Itlst { get; set; }
    }
    public class DirectItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public double BinID { get; set; }

        public List<SelectListItem> PURLst;
        public string PurType { get; set; }
        public double Quantity { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        //public string Center { get; set; }
        public double Process { get; set; }
        public double TotalAmount { get; set; }
        public string Isvalid { get; set; }


    }
}
