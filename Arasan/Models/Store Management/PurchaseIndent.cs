using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PurchaseIndent
    {
        public PurchaseIndent()
        {
            this.Brlst = new List<SelectListItem>();
            this.SLoclst = new List<SelectListItem>();
            //this.PILst= new List<PIndentItem>();
            this.ELst = new List<SelectListItem>();
            this.PURLst = new List<SelectListItem>();
            this.EmpLst = new List<SelectListItem>();
            //this.TANDClst = new List<PIndentTANDC>();
        }

        public List<SelectListItem> Brlst;
        public string ID { get; set; }
        public string Branch { get; set; }

        public List<SelectListItem> SLoclst;

        public List<PIndentItem> PILst { get; set; }

        public List<PIndentTANDC> TANDClst { get; set; }

        public List<SelectListItem> ELst;

        public List<SelectListItem> PURLst;

        public List<SelectListItem> EmpLst;

        public string SLocation { get; set; }

        public string IndentDate { get; set; }

        public string RefDate { get; set; }

        public string Erection { get; set; }

        public string Purtype { get; set; }

        public string PreparedBy { get; set; }

        public string IndentId { get; set; }
        public List<TotalStockItem> stklst { get; set; }
        public string TotalQty { get; set; }

    }
    public class PIndentItem
    {
        public string ItemId { get; set; }
        public string saveItem { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public string LocId { get; set; }

        public List<SelectListItem> loclst { get; set; }

        public string QC { get; set; }
        public string Unit { get; set; }

        public string UnitID { get; set; }
        public string IndentPlaced { get; set; }
        public double Quantity { get; set; }
        public string Narration { get; set; }
        public string Duedate { get; set; }
        public double Stock { get; set; }
        public double WholeStock { get; set; }
        public string Isvalid { get; set; }

    }
    public class PIndentTANDC
    {
        public string TANDC { get; set; }
        public string Isvalid { get; set; }
    }
    public class IndentBindList
    {
        public long piid { get; set; }
        public string SuppName { get; set; }
        public string EditRow { get; set; }
        public string DelRow { get; set; }
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public string branch { get; set; }

    }
    public class IndentItemBindList
    {
        public long indentid { get; set; }
        public long piid { get; set; }
        public string itemname { get; set; } 
        public string unit { get; set; }
        public string quantity { get; set; }
        public string location { get; set; }
        public string itemid { get; set; }
        public string duedate { get; set; }
        public string assign { get; set; }
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public string branch { get; set; }
        public string approval { get; set; }

    }
    public class IndentSupAllocation
    {
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string UnitP { get; set; }
        public string ItemId { get; set; }
        public List<IndentSupAllocationList> HSupLst { get; set; }
        public List<IndentSuppAllocate> indentSuppAllocates { get; set; }
    }
    public class IndentSupAllocationList
    {
        public string PartyName { get; set; }
        public string Rate { get; set; }
        public string Unit { get; set; }
        public string LastPurchsePrice { get; set; }
        public string LastPurchaseDate { get; set; }
        public string Qty { get; set; }

    }
    public class IndentSuppAllocate
    {
        public string Partytype { get; set; }
        public string PartyName { get; set; }
        public List<SelectListItem> Partylst { get; set; }
        public string Rate { get; set; }
        public string Unit { get; set; }
        public string LastPurchsePrice { get; set; }
        public string LastPurchaseDate { get; set; }
        public string Qty { get; set; }
        public string Isvalid { get; set; }
        public string EnquiryQty { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string UnitP { get; set; }
        public string ItemId { get; set; }
    }
    public class IndentSuppMultipleAllocate
    {
        public string PartyName { get; set; }
        public List<SelectListItem> Partylst { get; set; }
    }
    public class TotalStockItem
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
}
