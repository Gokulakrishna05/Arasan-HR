using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models 
{
    public class PurchaseReturn
    {
        public PurchaseReturn()
        {
            this.Brlst = new List<SelectListItem>();
            //this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }

        public string Supplier { get; set; }

        public List<SelectListItem> Suplst;
        public string State { get; set; }

        public List<SelectListItem> Satlst;
        public string Currency { get; set; }

        public List<SelectListItem> Curlst;
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string ReqDate { get; set; }
        public string ReqNo { get; set; }
        public string Reason { get; set; }
        public string ExRate { get; set; }
        public string RetNo { get; set; }
        public string Grn { get; set; }
        public string Tax { get; set; }
        public string Trans { get; set; }
        public string RetDate { get; set; }
        public double Packingcharges { get; set; }
        public double Frieghtcharge { get; set; }
        public double Othercharges { get; set; }
        public double Round { get; set; }
        public double otherdeduction { get; set; }
        public double Roundminus { get; set; }
        public double Gross { get; set; }
        public double Net { get; set; }
        public List<RetItem> RetLst { get; set; }
    }
    public class RetItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> POlst { get; set; }

        public string POID { get; set; }
        public double FrigCharge { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
      
        public double Quantity { get; set; }
        //   public string unitprim { get; set; }
        //  public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }

        public double Disc { get; set; }
        public double DiscAmount { get; set; }

        public double CGSTPer { get; set; }
        public double CGSTAmt { get; set; }
        public double SGSTPer { get; set; }
        public double SGSTAmt { get; set; }
        public double IGSTPer { get; set; }
        public double IGSTAmt { get; set; }
        public double TotalAmount { get; set; }
        public string Isvalid { get; set; }

    }
}
