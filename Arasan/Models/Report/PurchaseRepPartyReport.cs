using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PurchaseRepPartyReport
    {
        public PurchaseRepPartyReport()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.ItemGrouplst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public List<SelectListItem> Suplst;
        public List<SelectListItem> ItemGrouplst;
        public List<SelectListItem> Itemlst;
        public string Branch { get; set; }
        public string Customer { get; set; }
        public string ItemGroup { get; set; }
        public string Item { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class PurchaseRepPartyReportItems
    {
        public string branch { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string party { get; set; }
        public string item { get; set; }
        public string unit { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string sgst { get; set; }
        public string cgst { get; set; }
        public string igst { get; set; }
        public string rem { get; set; }
        public string ind { get; set; }
        public string net { get; set; }
    }
}
