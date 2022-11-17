using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PO
    {
        public PO()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string ID { get; set; }
        public string Branch { get; set; }

        public string PONo { get; set; }

        public string POdate { get; set; }
        public string QuoteNo { get; set; }
        public string ExRate { get; set; }
        public string ParNo { get; set; }
        public string QuoteDate { get; set; }
        public string Recid { get; set; }

        public string assignid { get; set; }

        public string Supplier { get; set; }
        public string Status { get; set; }

        public List<SelectListItem> Suplst;

        public string Cur { get; set; }

        public List<SelectListItem> Curlst;

        public List<POItemlst> PoItemlst { get; set; }

        public List<POItem> PoItem { get; set; }

        public List<SelectListItem> RecList;

        public List<SelectListItem> assignList;

        public double Gross { get; set; }
        public double Net { get; set; }

    }

    public class POItemlst
    {
        public string ItemId { get; set; }
        public int POID { get; set; }
        public string ProName { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }

    }
    public class POItem
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public List<SelectListItem> PURLst { get; set; }
        public string ItemGroupId { get; set; }
        public string Purtype { get; set; }

        public string Desc { get; set; }
        public string Unit { get; set; }
        public string Conversionfactor { get; set; }
        public double Quantity { get; set; }
        public string unitprim { get; set; }
        public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public string Isvalid { get; set; }

        public double DiscPer { get; set; }
        public double DiscAmt { get; set; }
        public double FrieghtAmt { get; set; }
        public double CGSTPer { get; set; }
        public double CGSTAmt { get; set; }
        public double SGSTPer { get; set; }
        public double SGSTAmt { get; set; }
        public double IGSTPer { get; set; }
        public double IGSTAmt { get; set; }
        public double TotalAmount { get; set; }
        public string Duedate { get; set; }

    }
}
