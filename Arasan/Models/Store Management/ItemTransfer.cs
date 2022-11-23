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
        public List<SelectListItem> Itlst { get; set; }
        public string Isvalid { get; set; }
        public string Lot { get; set; }
        public string FromBinID { get; set; }
        public string ToBinID { get; set; }
        public string Avaliable { get; set; }
        public string Qty { get; set; }
        public string Unit { get; set; }
        public string Rate { get; set; }
        public string CostRate { get; set; }
        public string Amount { get; set; }
        public string Serial { get; set; }
    }
}
