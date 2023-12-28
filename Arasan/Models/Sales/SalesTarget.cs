using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SalesTarget
    {
        public SalesTarget()
        {

            this.Brlst = new List<SelectListItem>();

        }
       
        public string ID { get; set; }

        public List<SelectListItem> Brlst;

        public string Branch { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string FMonth { get; set; }
        public string FYear { get; set; }
        public string FDay { get; set; }
        public string TDay { get; set; }
        public string ddlStatus { get; set; }

        public List<SalesTargetItem> Targetlst { get; set; }

    }
    public class SalesTargetItem
    {
        public string ID { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string ItemId { get; set; }

        public List<SelectListItem> Partylst { get; set; }
        public string PartyId { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }
        public string rate { get; set; }
        public string Amount { get; set; }

        public string saveItemId { get; set; }
        public string savePartyId { get; set; }
        public string Isvalid { get; set; }

    }
    public class ListSalesTargetItem
    {
        public double id { get; set; }
        public string branch { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
        public string mon { get; set; }
        public string view { get; set; }
        public string edit { get; set; }
        public string delrow { get; set; }
    }
}
