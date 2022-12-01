using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemTransfer
    {

        public string ID { get; set; }
        public string Docid { get; set; }
        public string Docdate { get; set; }
        public string Toloc { get; set; }
        public string Reason { get; set; }
        public string Gro { get; set; }
        public string Net { get; set; }
        public string Narr { get; set; }
        public ItemTransfer()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<Itemtran> Itlst { get; set; }
    }
    public class Itemtran
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itlst { get; set; }
        public string Isvalid { get; set; }
        public double Lot { get; set; }
        public double FromBinID { get; set; }
        public double ToBinID { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double Serial { get; set; }
        public string ConFac { get; set; }
    }
}
