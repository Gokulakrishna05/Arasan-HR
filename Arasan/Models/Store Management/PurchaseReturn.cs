using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models 
{
    public class PurchaseReturn
    {
        public PurchaseReturn()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Partylst = new List<SelectListItem>();
            this.currlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Partylst;
        public string party { get; set; }
        public List<SelectListItem> currlst;
        public string Curr { get; set; }

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
        public List<SelectListItem> assignList;
        public string Location { get; set; }
        public string ReqDate { get; set; }
        public string ReqNo { get; set; }
        public string Rej { get; set; }
        public string Temp { get; set; }
        public string ReasonCode { get; set; }
        public string Reason { get; set; }
        public string ExRate { get; set; }
        public string RetNo { get; set; }
        public List<SelectListItem> POlst { get; set; }
        public string Grn { get; set; }
        public string Narration { get; set; }
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
        public List<ReturnItem> returnlist { get; set; }
        public string Addr { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Pin { get; set; }
        public List<SelectListItem> Citylst;
        public string City { get; set; }
        public string SNO { get; set; }
        public string ddlStatus { get; set; }
   
        public string Isvalid { get; set; }

    }
    public class PurchaseReturnItems
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string docNo { get; set; }
        public string curr { get; set; }
        public string docDate { get; set; }
        //public string qcresult { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        //public string grn { get; set; }
        //public string pdf { get; set; }
        public string view { get; set; }
        //public string acc { get; set; }
        //public string pono { get; set; }
        //public string Account { get; set; }
    }
    public class RetItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string GRNNo { get; set; }
        public double FrigCharge { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
           public List<RetItem> RetLst { get; set; }
      
        public string Quantity { get; set; }
        //   public string unitprim { get; set; }
        //  public double QtyPrim { get; set; }
        public string rate { get; set; }
        public string Amount { get; set; }

        public double Disc { get; set; }
        public double DiscAmount { get; set; }

        public double CGSTPer { get; set; }
        public double CGSTAmt { get; set; }
        public double SGSTPer { get; set; }
        public double SGSTAmt { get; set; }
        public double IGSTPer { get; set; }
        public double IGSTAmt { get; set; }
        public string TotalAmount { get; set; }
        public string Isvalid { get; set; }
        public string binid { get; set; }
        public string Current { get; set; }
        public string Return { get; set; }

    }
    public class ReturnItem
    {
        public string id { get; set; }
        public string itemid { get; set; }
        public string itemname { get; set; }
        public string saveItemId { get; set; }
        public double frigcharge { get; set; }
        public string confac { get; set; }
        public string unit { get; set; }
        public string unitid { get; set; }
        public string quantity { get; set; }
        public string rqty { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public double disc { get; set; }
        public double discAmount { get; set; }
        public double cgstper { get; set; }
        public double cgstamt { get; set; }
        public double sgstper { get; set; }
        public double sgstamt { get; set; }
        public double igstper { get; set; }
        public double igstamt { get; set; }
        public double totalamount { get; set; }
        public string stkqty { get; set; }
        public string binid { get; set; }
        public string grnitemid { get; set; }
        public string Isvalid { get; set; }

    }
}
