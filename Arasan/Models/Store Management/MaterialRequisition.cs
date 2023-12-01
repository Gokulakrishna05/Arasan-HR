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
            this.Statuslst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            //this.MRlst = new List<MaterialRequistionItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string Branch { get; set; }
        public string BranchId { get; set; }
        public string DocDa { get; set; }
        public string DocId { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string Process { get; set; }
        public string WorkCenter { get; set; }
        public string WorkCenterid { get; set; }

        public string MaterialReqId { get; set; }
        public DateTime MaterialReqDate { get; set; }
        public string matno { get; set; }
        public string RequestType { get; set; }
        public bool selectall { get; set; }
        public string ddlStatus { get; set; }

        public string Amount { get; set; }
        public string Narration { get; set; }
        public string Entered { get; set; }
        public List<SelectListItem> Statuslst;
        public string status { get; set; }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Loclst;
        public List<SelectListItem> Worklst;
        public List<SelectListItem> Processlst;
        public List<MaterialRequistionItem> MRlst { get; set; }
        public List<StockItem> stklst { get; set; }
        public List<SelectListItem> assignList;
        public string Storeid { get; set; }
    }
    public class MaterialItem
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string location { get; set; }
        public string work { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
        public string iss { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        //public string follow { get; set; }
        //public string pdf { get; set; }
        public string view { get; set; }
        public string move { get; set; }
        //public string Account { get; set; }
    }
    public class MaterialReqItem
    {
        public long id { get; set; }
        
        public string location { get; set; }
        public string item { get; set; }
        public string docDate { get; set; }
        public string reqloc { get; set; }
        public string work { get; set; }
        public string iss { get; set; }
        public string qty { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        //public string follow { get; set; }
        //public string pdf { get; set; }
        public string view { get; set; }
        public string move { get; set; }
        //public string Account { get; set; }
    }
    public class StockItem
    {
        public string ID { get; set; }
        public string branch { get; set; }
        public string branchid { get; set; }
        public string location { get; set; }
        public string locationid { get; set; }
        public string reqlocation { get; set; }
        public string reqlocationid { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
        public bool select { get; set; }
        public string user { get; set; }
        public string item { get; set; }
        public string invid { get; set; }
        public string itemid { get; set; }
        public string qty { get; set; }
        public string reqqty { get; set; }
        
         
    }

    public class MaterialReq
    {
        public string ID { get; set; }
        public string branch { get; set; }
        public string branchid { get; set; }
        public string location { get; set; }
        public string locationid { get; set; }
      
        public string reqlocation { get; set; }
        public string reqlocationid { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
       
        public string user { get; set; }
        public string item { get; set; }
        public string invid { get; set; }
        public string itemid { get; set; }
        public string qty { get; set; }
        public double reqqty { get; set; }
        public string Entered { get; set; }


    }

    public class MaterialRequistionItem
    {
        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string ItemId { get; set; }
        public string Item { get; set; }
        public bool select { get; set; }
        public bool selectall { get; set; }
        public string UnitID { get; set; }
        public string Unit { get; set; }
        public string ClosingStock { get; set; }
        public string TotalStock { get; set; }
        public string ReqQty { get; set; }
        public double IndQty { get; set; }
        public double InvQty { get; set; }
        public string Isvalid { get; set; }
        public string Narration { get; set; }

        public string indentid { get; set; }
        public List<SelectListItem> Itemlst { get; set; }


    }
}
