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
        public List<PFCPYROItem> PFCPYROILst { get; set; }

        public List<PFCPOLIItem> PFCPOLILst { get; set; }

        public string plantype { get; set; }

        public List<SelectListItem> mnthlst { get; set; }
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
        public string itemid { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> PItemlst { get; set; }
        public string Isvalid { get; set; }

        public string target { get; set; }
        public string min { get; set; }
        public string stock { get; set; }
        public string reqadditive { get; set; }
        public string rawmaterial { get; set; }
        public string ReqPyro { get; set; }

        public string required { get; set; }
        public string dgaddit { get; set; }

    }
    public class PFCPYROItem
    {

        public string ID { get; set; }
        public string itemid { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> PYItemlst { get; set; }

        public string WorkId { get; set; }
        public List<SelectListItem> Worklst { get; set; }
        public string Isvalid { get; set; }

        public string CDays { get; set; }
        public string minstock { get; set; }
        public string pasterej { get; set; }
        public string GradeChange { get; set; }
        public string rejqty { get; set; }
        public string required { get; set; }
        public string target { get; set; }
        public string ProdDays { get; set; }
        public string ProdQty { get; set; }
        public string RejMat { get; set; }
        public string RejMatReq { get; set; }
        public string BalanceQty { get; set; }
        public string Additive { get; set; }
        public string Per { get; set; }
        public string AllocAdditive { get; set; }
        public string ReqPowder { get; set; }
        public string WStatus { get; set; }
        public string PowderRequired { get; set; }
        public string stock { get; set; }

    }
    public class PFCPOLIItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public List<SelectListItem> POItemlst { get; set; }
        public string WorkId { get; set; }
        public List<SelectListItem> POWorklst { get; set; }
        public string Isvalid { get; set; }
        public string WCDays { get; set; }
        public string Target { get; set; }
        public string Capacity { get; set; }
        public string Stock { get; set; }
        public string MinStock { get; set; }
        public string Required { get; set; }
        public string Days { get; set; }
        public string Additive { get; set; }
        public string Add { get; set; }
        public string RejMat { get; set; }
        public string ReqPer { get; set; }
        public string RvdQty { get; set; }
        public string PyroPowder { get; set; }
        public string PyroQty { get; set; }
        public string PowderRequired { get; set; }

    }
}
