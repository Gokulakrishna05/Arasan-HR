using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StoreIssueConsumables
    {
        public StoreIssueConsumables()
        {
            this.Brlst = new List<SelectListItem>();
            this.EnqassignList = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }


        public List<SICItem> SICLst { get; set; }
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string ToLoc { get; set; }
      
        public string ReqNo { get; set; }
        //public string Voucher { get; set; }
        public string DocNo { get; set; }
      
        public string ReqDate { get; set; }
        public string DocDate { get; set; }
       
        public string Work { get; set; }
        public string MCNo { get; set; }
        public string MCNa { get; set; }
        public List<SelectListItem> EnqassignList;
        public string User { get; set; }
        public string Narr { get; set; }
        public string LocCon { get; set; }
        public string Process { get; set; }
        public string Processid { get; set; }
        public string Workid { get; set; }



    }
    public class SICItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public string lotno { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }
        public List<SelectListItem> DRLst { get; set; }
        public List<SelectListItem> SRLst { get; set; }
        public string ItemGroupId { get; set; }

        public string ConFac { get; set; }
        public string Unit { get; set; }
        public string Unitid { get; set; }
        public string Serial { get; set; }
        public double Quantity { get; set; }
        //   public string unitprim { get; set; }
        //  public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public string Drum { get; set; }
        public double Indp { get; set; }
        public double IndCtr { get; set; }
        public double PendQty { get; set; }
        public double ReqQty { get; set; }
        public double Stock { get; set; }
        public double FromBin { get; set; }
        //public double SGSTP { get; set; }
        //public double IGSTP { get; set; }
        //public double CGST { get; set; }
        //public double SGST { get; set; }
        //public double IGST { get; set; }
        //public double TotaAmount { get; set; }
        public string Isvalid { get; set; }

    }
}

