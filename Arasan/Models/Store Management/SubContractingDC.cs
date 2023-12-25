using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SubContractingDC
    {
        public SubContractingDC()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
       
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public string Locationid { get; set; }
        public List<SelectListItem> Suplst;
        public string Supplier { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string City { get; set; }
        public string Through { get; set; }
        public string Entered { get; set; }
        public string Recived { get; set; }
        public string TotalQty { get; set; }
        public string ddlStatus { get; set; }
        public string Narration { get; set; }
        public List<SelectListItem> assignList;

        public List<SubContractingItem> SCDIlst { get; set; }
        public List<ReceiptDetailItem> RECDlst { get; set; }

    }
    public class SubContractDDDrumdetailstable
    {
        public List<SubContractDDrumdetails> SUBDDrumlst { get; set; }

        public bool selectall { get; set; }
    }
    public class SubContractDDrumdetails
    {
        public string lotno { get; set; }
        public string drumno { get; set; }
        public string qty { get; set; }
        public string stkid  { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string invid { get; set; }
        public bool drumselect { get; set; }
    }
    public class SubContractingItem
    {
        public string id { get; set; }
        public string Isvalid { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string ItemId { get; set; }
        public string item { get; set; }
        public string Unit { get; set; }
        public string detid { get; set; }
        public string ConFac { get; set; }
        public string Quantity { get; set; }
        public string rate { get; set; }
        public string Amount { get; set; }
        public string saveItemId { get; set; }
        public string group { get; set; }
        public string Lotno { get; set; }
        public string dqty { get; set; }
        public string drate { get; set; }
        public string DrumIds { get; set; } 
        public string invid { get; set; } 
       public string Drumsdesc { get; set; }
       public string stock { get; set; }
       public string Lot { get; set; }
    }
    public class ReceiptDetailItem
    {
        public string id { get; set; }
        public string Isvalid1 { get; set; }
        public List<SelectListItem> Itemlist { get; set; }
        public string ItemId { get; set; }
        public string ConFac { get; set; }
        public string Quantity { get; set; }
        public string rate { get; set; }
        public string Amount { get; set; }
        public string saveItemId { get; set; }
        public string Unit { get; set; }
    }
    public class ListSubContractingDCItem
    {
        public double id { get; set; }
        public string branch { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
        public string loc { get; set; }
        public string tot { get; set; }
        public string view { get; set; }
        public string approve { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }
}
