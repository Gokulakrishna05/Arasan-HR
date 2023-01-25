using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Arasan.Models
{
    public class StoresReturn
    {
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Narr { get; set; }

        public StoresReturn()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<StoreItem> StrLst { get; set; }
    }
    public class StoreItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }

        public double rate { get; set; }
        public double Amount { get; set; }

        public double TotalAmount { get; set; }

        public string FromBin { get; set; }

        public string ToBin { get; set; }
        public string Isvalid { get; set; }

    }
}
