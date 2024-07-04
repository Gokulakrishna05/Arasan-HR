using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Rate
    {
        public Rate()
        {
            this.Brlst = new List<SelectListItem>();
            this.Ratelst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Ratelst;
        public string Branch { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Ratecode { get; set; }
        public string RateName { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string UF { get; set; }
        public string RateType { get; set; }
        public string ddlStatus { get; set; }

        public List<RateItem> RATElist { get; set; }
    }
    public class RateItem
    {
        public string ID { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string ItemId { get; set; }

        public List<SelectListItem> Ratelst { get; set; }
        public string RCode { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }
        public string rate { get; set; }
        public string Validfrom { get; set; }
        public string Validto { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }

        public string saveItemId { get; set; }
        public string saveRCode { get; set; }
        public string Isvalid { get; set; }

    }

    public class ListRateItem
    {
        public double id { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
        public string ratecode { get; set; }
        public string ratename { get; set; }
        public string view { get; set; }
        public string edit { get; set; }
        public string delrow { get; set; }
        public string revision { get; set; }
    }
}
