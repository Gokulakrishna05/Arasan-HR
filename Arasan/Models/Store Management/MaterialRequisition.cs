
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class MaterialRequisition
    {
        public MaterialRequisition()
            {
            this.Brlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.MRlst = new List<MaterialRequistionItem>();

        }
        public int Id { get; set; }
        public int Branch { get; set; }
  

        public int LocationConsumedId { get; set; }

        public string Location { get; set; }
        public int WorkCenterId { get; set; }
        public string WorkCenter { get; set; }

        public int MaterialReqId { get; set; }
        public DateTime MaterialReqDate { get; set; }

        public string RequestType { get; set; }
        public int ItemGroupId { get; set; }
        public string ItemGroup { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }

        public int unit { get; set; }
        public int ClosingStock { get; set; }
        public int ReqQty { get; set; }

        public List<SelectListItem> Brlst;
        public List<SelectListItem> Loclst;
        public List<SelectListItem> Worklst;
        public List<MaterialRequistionItem> MRlst;
    }


    public class MaterialRequistionItem
    {
        public int ItemGroupId { get; set; }
        public string ItemGroup { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }

        public int Unit { get; set; }
        public int ClosingStock { get; set; }
        public int ReqQty { get; set; }

        public string Isvalid { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }
    }
}
