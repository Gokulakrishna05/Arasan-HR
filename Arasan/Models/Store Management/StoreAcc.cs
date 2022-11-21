using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StoreAcc
    {
        public string ID { get; set; }
        public string Docid { get; set; }
        public string Docdate { get; set; }
        public string Refno { get; set; }
        public string Refdate { get; set; }
        public string Retno { get; set; }
        public string Retdate { get; set; }
        public string Narr { get; set; }
        public StoreAcc()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<StoItem> Itlst { get; set; }
    }
    public class StoItem
    {
        public List<SelectListItem> Itlst { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string FromBinID { get; set; }
        public string ToBinID { get; set; }
        public string Batch { get; set; }
        public string Serial { get; set; }
        public string PendQty { get; set; }
        public string RetQty { get; set; }
        public string RejQty { get; set; }
        public string AccQty { get; set; }
        public string Rate { get; set; }
    }

}
