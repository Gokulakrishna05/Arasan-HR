
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AssetStockModel
    {
        public AssetStockModel()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            //this.ItemGrouplst = new List<SelectListItem>();
            //this.Itemlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public List<SelectListItem> Loclst;
        //public List<SelectListItem> ItemGrouplst;
        //public List<SelectListItem> Itemlst;
        public string Branch { get; set; }
        public string Location { get; set; }
       

        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class AssetStockItems
    {
        public string itemid { get; set; }
        public string unitid { get; set; }
        public string branchid { get; set; }
        public string locid { get; set; }
        public string oq { get; set; }
        public string ov { get; set; }
        public string iq { get; set; }
        public string iv { get; set; }
        public string rq { get; set; }
        public string rv { get; set; }

    }

}
