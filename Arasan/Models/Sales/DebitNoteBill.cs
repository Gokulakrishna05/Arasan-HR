using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DebitNoteBill
    {
        public string ID { get; set; }
        public DebitNoteBill()
        {
            this.Brlst = new List<SelectListItem>();
            this.Partylst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Vocherlst = new List<SelectListItem>();
           

        }
        public List<SelectListItem> Curlst;
        public List<SelectListItem> Vocherlst;
        public string Currency { get; set; }

        public List<SelectListItem> RecList;
        public string Entered { get; set; }
        public string Approved { get; set; }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
        public List<SelectListItem> Partylst;
        public string PartyBal { get; set; }
        public string Party { get; set; }
        public string Partyid { get; set; }
        public string ledger { get; set; }
        public string Vocher { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Gross { get; set; }
        public string Net { get; set; }
        public string Location { get; set; }
        public string Amount { get; set; }
        public double Bigst { get; set; }
        public double Bsgst { get; set; }
        public double Bcgst { get; set; }
        public string Narration { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string TotalAmount { get; set; }
        public string Exchange { get; set; }
        public string Appby { get; set; }
        public string Bra { get; set; }
        public string Loc { get; set; }
        public string grnid { get; set; }
        public List<DebitNoteItem> Depitlst { get; set; }
        public List<CreditItem> Creditlst { get; set; }
        public List<GRNAccount> Acclst { get; set; }
        public List<SelectListItem> Accconfiglst;
        public string ADCOMPHID { get; set; }
        public string Amtinwords { get; set; }
        public string createdby { get; set; }
        public string Vmemo { get; set; }
        public double TotalCRAmt { get; set; }
        public double TotalDRAmt { get; set; }

        public string mid { get; set; }
        public string ddlStatus { get; set; }

    }

    public class DebitNoteBillGrid
    {
        public string id { get; set; }
        public string branch { get; set; }
        public string docid { get; set; }
        public string Docdate { get; set; }
        public String approve { get; set; }
        public String editrow { get; set; }
        public String delrow { get; set; }
    }
    public class CreditItem
    {
        public string ID { get; set; }
        public List<SelectListItem> Crlst { get; set; }
        public string Dr { get; set; }
        public List<SelectListItem> Acclst { get; set; }
        public List<SelectListItem> grplst { get; set; }
        public string Account { get; set; }
        public string DepitAmount { get; set; }
        public string CreditAmount { get; set; }
        public string accgrp { get; set; }
        public string Isvalid { get; set; }
       

    }

   
        public class DebitNoteItem
    {
        public string ID { get; set; }

        public string InvNo { get; set; }
        public string Invdate { get; set; }
        public List<SelectListItem> Itemlst;
        public string Item { get; set; }
        public string Itemid { get; set; }
        public string Cf { get; set; }
        public string Unit { get; set; }

        public string InQty { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string CGST { get; set; }
        public string CGSTP { get; set; }
        public string SGSTP { get; set; }
        public string IGSTP { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Total { get; set; }

        public List<SelectListItem> Grnlst;
        public string Isvalid { get; set; }
       

    }
}
