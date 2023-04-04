using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ProductionForecasting
    {
        public ProductionForecasting()
        {
            this.Brlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string PType { get; set; }
        public string ForMonth { get; set; }
        public string Ins { get; set; }
        public string Hd { get; set; }
        public string Fordate { get; set; }
        public string Enddate { get; set; }

        public List<PFCItem> PFCILst { get; set; }

        public List<PFCDGItem> PFCDGILst { get; set; }
    }
    public class PFCItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Isvalid { get; set; }

        public string Unit { get; set; }
        public string PType { get; set; }
        public string Fqty { get; set; }

        public string PtmQty { get; set; }
        public string PysQty { get; set; }
       
    }
    public class PFCDGItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> PItemlst { get; set; }
        public string Isvalid { get; set; }

        public string Target { get; set; }
        public string Min { get; set; }
        public string Stock { get; set; }
        public string ReqAdditive { get; set; }
        public string RawMaterial { get; set; }
        public string ReqPyro { get; set; }

        public string Required { get; set; }
        public string DgAdditID { get; set; }



    }
}
