using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PO
    {
        public PO()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.desplst = new List<SelectListItem>();
            this.Paymenttermslst = new List<SelectListItem>();
            this.deltermlst = new List<SelectListItem>();
            this.warrantytermslst = new List<SelectListItem>();
        }



        public List<SelectListItem> Brlst;
        public List<SelectListItem> desplst;
        public List<SelectListItem> Paymenttermslst;
        public List<SelectListItem> deltermlst;
        public List<SelectListItem> warrantytermslst;
        public string desp { get; set; }
        public string Paymentterms { get; set; }
        public string delterms { get; set; }
        public string warrantyterms { get; set; }
        public string ID { get; set; }
        public string Branch { get; set; }
        public string user { get; set; }

        public string PONo { get; set; }

        public string POdate { get; set; }
        public string QuoteNo { get; set; }
        public string ExRate { get; set; }
        public string ParNo { get; set; }
        public string QuoteDate { get; set; }
        public string Recid { get; set; }

        public string assignid { get; set; }

        public string Supplier { get; set; }
        public string statetype { get; set; }
        public string Status { get; set; }
        public string Active { get; set; }
        public List<SelectListItem> Suplst;

        public string Cur { get; set; }

        public List<SelectListItem> Curlst;

        public List<POItemlst> PoItemlst { get; set; }

        public List<POItem> PoItem { get; set; }

        public List<SelectListItem> RecList;

        public List<SelectListItem> assignList;

        public double Gross { get; set; }
        public double Net { get; set; }

        public double Frieghtcharge { get; set; }
        public double Packingcharges { get; set; }
        public double Disc { get; set; }
        public double Othercharges { get; set; }
        public double Round { get; set; }
        public string Narration { get; set; }
        public string Fax { get; set; }


        public string PhoneNo { get; set; }
        public string DespatchAddr { get; set; }
        public double Roundminus { get; set; }
        public double otherdeduction { get; set; }
        public string POID { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }

        public string BranchId { get; set; }
        public string SuppId { get; set; }
        public string Amount { get; set; }
        public string spec { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public double ddlStatus { get; set; }


    }
    public class POItems
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string supplier { get; set; }
        public string quono { get; set; }
        public string podate { get; set; }
        public string mailrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string genpo { get; set; }
        public string pdf { get; set; }
        public string view { get; set; }
        public string move { get; set; }
        public string pono { get; set; }
        public string download { get; set; }
    }
    public class GateInward
    {
        public GateInward()
        {
            this.Suplst = new List<SelectListItem>();
            this.POlst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string POId { get; set; }
        public string GateInDate { get; set; }
        public string GateInTime { get; set; }
        public string Narration { get; set; }
        public double TotalQty { get; set; }
        public List<SelectListItem> Suplst;
        public List<SelectListItem> POlst;
        public List<POGateItem> PoItem { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public List<GateInwardItem> GateInlst { get; set; }
    }
    public class GateInwardItem
    {
        public string POID { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string GateInDate { get; set; }
        public string GateInTime { get; set; }
        public string Narration { get; set; }
        public string PONo { get; set; }
        public double TotalQty { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }
        public string inQuantity { get; set; }
        public string QC { get; set; }
        public string ItemId { get; set; }
    }
    public class POItemlst
    {
        public string ItemId { get; set; }
        public int POID { get; set; }
        public string ProName { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }

    }
    public class POItem
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> gstlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public List<SelectListItem> PURLst { get; set; }
        public string ItemGroupId { get; set; }
        public string gst { get; set; }
        public string Percentage { get; set; }
        public double per { get; set; }
        public string Purtype { get; set; }
        public string QC { get; set; }
        public string Desc { get; set; }
        public string Unit { get; set; }
        public string Conversionfactor { get; set; }
        public double Quantity { get; set; }
        public double Goodqty { get; set; }
        public double DamageQty { get; set; }
        public string unitprim { get; set; }
        public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public string Isvalid { get; set; }
        public double CostRate { get; set; }
        public string AcessValue { get; set; }
        public double BillQty { get; set; }
        public double ConvQty { get; set; }
        public double PendingQty { get; set; }
        public double DiscPer { get; set; }
        public double DiscAmt { get; set; }
        public double FrieghtAmt { get; set; }
        public double PackingAmt { get; set; }
        public double CGSTPer { get; set; }
        public double CGSTAmt { get; set; }
        public double SGSTPer { get; set; }
        public double SGSTAmt { get; set; }
        public double IGSTPer { get; set; }
        public double IGSTAmt { get; set; }
        public double TotalAmount { get; set; }
        public string Duedate { get; set; }
        public string Lotno { get; set; }
        public string LOTYN { get;set; }   
        public string grndetid { get;set; }
        public string EXPYN { get; set; }
        public string mdate { get; set; }
        public string  edate { get; set; }

    }
    public class POItemDetail
    {
        public string ITEMID { get; set; }

        public string PUNIT { get; set; }

        public double QTY { get; set; }

        public double RATE { get; set; }
        public double RNDOFF { get; set; }
        public double AMOUNT { get; set; }
        public double SGST { get; set; }

        public double IGST { get; set; }
        public double CGST { get; set; }
        public string DOCID { get; set; }
        public string SGSTP { get; set; }
        public string CGSTP { get; set; }
        public string IGSTP { get; set; }
        public string GROSS { get; set; }
        public string DOCDATE { get; set; }
        public string NET { get; set; }
        public string PARTYID { get; set; }
        public string AMTINWORDS { get; set; }
        public string PAYTERMS { get; set; }
        public string DELTERMS { get; set; }
        public string DESP { get; set; }
        public string WARRTERMS { get; set; }
        public string EXPR1 { get; set; }
        public string ADD1 { get; set; }
        public string OTHERSPEC { get; set; }
        public string ADD2 { get; set; }
        public string ADD3 { get; set; }
        public string CITY { get; set; }
        public string PINCODE { get; set; }
        public string STATE { get; set; }
        public string GSTNO { get; set; }
        public string MOBILE { get; set; }
        public string qty { get; set; }

    }
    public class PODetail
    {
        public string DOCID { get; set; }

        public string PARTYID { get; set; }

        public string DOCDATE { get; set; }

        public string GROSS { get; set; }

        public string NET { get; set; }



    }
    public class POGateItem
    {
        public string itemid { get; set; }
        public string itemname { get; set; }
        public string unit { get; set; }
        public string Conversionfactor { get; set; }
        public double quantity { get; set; }
        public string qc { get; set; }
    }
}
