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
            //this.MRlst = new List<MaterialRequistionItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string Branch { get; set; }
        public string BranchId { get; set; }
        public string DocDa { get; set; }
        public string  DocId { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string Process { get; set; }
        public string WorkCenter { get; set; }

        public int MaterialReqId { get; set; }
        public DateTime MaterialReqDate { get; set; }

        public string RequestType { get; set; }
       
        public string Amount { get; set; }
        public string Narration { get; set; }
        public string Entered { get; set; }
        public string status { get; set; }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Loclst;
        public List<SelectListItem> Worklst;
        public List<MaterialRequistionItem> MRlst { get; set; }
        public List<SelectListItem> assignList;
    }


    public class MaterialRequistionItem
    {
        public int ItemGroupId { get; set; }
        public string ItemGroup { get; set; }
        public string ItemId { get; set; }
        public string Item { get; set; }
        public string UnitID { get; set; }
        public string Unit { get; set; }
        public string ClosingStock { get; set; }
        public string ReqQty { get; set; }
        public double IndQty { get; set; }
        public double InvQty { get; set; }
        public string Isvalid { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

       
    }
}
