using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class SalesInvoice
    {
        public SalesInvoice()
        {
            this.Brlst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Invlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Typelst = new List<SelectListItem>();
            this.Orderlst = new List<SelectListItem>();
            this.Dislst = new List<SelectListItem>();
            this.Inspelst = new List<SelectListItem>();
            this.Doclst = new List<SelectListItem>();
            this.Voclst = new List<SelectListItem>();
            this.Tranlst = new List<SelectListItem>();
            this.Tnamelst = new List<SelectListItem>();

        }
        public List<SelectListItem> Tranlst;
        public string Trans { get; set; }
        public string ID { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string ExRate { get; set; }
        public string Cancel { get; set; }

        public string Customer { get; set; }
        public string Sales { get; set; }
        public string RecBy { get; set; }
        public double Packing { get; set; }
        public double Round { get; set; }
        public double Gross { get; set; }
        public double TotalAccess { get; set; }


        public double Discount { get; set; }
        public double cgst { get; set; }
        public double sgst { get; set; }
        public double igst { get; set; }
        public double Net { get; set; }
        public string AinWords { get; set; }
        public string Serial { get; set; }
        public string Narration { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Curlst;
        public string Currency { get; set; }

        public List<SelectListItem> Loclst;
        public string Location { get; set; }

        public List<SelectListItem> Invlst;
        public string InvoType { get; set; }

        public List<SelectListItem> Suplst;
        public string Party { get; set; }
        public string arc { get; set; }

        public string partyarc { get; set; }
        public double crlimit { get; set; }
        public string PartyG { get; set; }
        public double limit { get; set; }
        public double asale { get; set; }
        public string statetype { get; set; }
        public double crd { get; set; }

        public List<SelectListItem> Typelst;
        public string Type { get; set; }

        public List<SelectListItem> Orderlst;
        public string Ordsam { get; set; }

        public List<SelectListItem> Dislst;
        public string Dis { get; set; }

        public List<SelectListItem> Inspelst;
        public string Inspect { get; set; }

        public List<SelectListItem> Doclst;
        public string Doc { get; set; }

        public List<SelectListItem> Voclst;
        public string Vocher { get; set; }

        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string Fax { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Vno { get; set; }
        public double FrightCharge { get; set; }
        public string InvoiceD { get; set; }
        public string TranCharger { get; set; }
        public string Outid { get; set; }
        public string Cst { get; set; }
        public string Areaid { get; set; }
        public string Tin { get; set; }
        public List<SelectListItem> Tnamelst;
        public string enterdby { get; set; }
        public string Tname { get; set; }
        public string Distance { get; set; }
        public string FrightYN { get; set; }
        public string ddlStatus { get; set; }
        public List<TermsItem> TermsItemlst { get; set; }
        public List<DepotInvoiceItem> Depotlst { get; set; }
        public List<AreaItem> AreaItemlst { get; set; }
    }
    public class SalesInvoiceItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public string HSN { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string DrumIds { get; set; }
        public double FrigCharge { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public string Isvalid { get; set; }
        public double Quantity { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public string ItemType { get; set; }
        public string ItemSpec { get; set; }
        public string binid { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public double Disc { get; set; }
        public double DiscAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double CashAmount { get; set; }
        public double CGSTPe { get; set; }
        public double SGSTCGST { get; set; }
        public double SGSTPe { get; set; }
        public double IGSTPe { get; set; }
        public double CGSTP { get; set; }
        public double SGSTP { get; set; }
        public double IGSTP { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }
        public double IntroDiscount { get; set; }
        public double CashDiscount { get; set; }
        public double TradeDiscount { get; set; }
        public double AddDiscount { get; set; }
        public double SpecDiscount { get; set; }
        public double TotalAmount { get; set; }
        public double CurrentStock { get; set; }
        public string Drumsdesc { get; set; }
        public string tarrifid { get; set; }
        public string FrieghtItemId { get; set; }
        public string HSNcode { get; set; }
        public string Frieght { get; set; }
        public string FriQty { get; set; }
        public string FrieghtAmount { get; set; }
    }
    public class ListSIItems
    {
        public long id { get; set; }
        public string docno { get; set; }
        public string date { get; set; }
        public string party { get; set; }
        public string delrow { get; set; }
        public string currency { get; set; }
        public string edit { get; set; }
        public string jsonrow { get; set; }

    }
    public class Jsonexport
    {
        public string Version { get; set; }
        public string Irn { get; set; }
        public transdetail TranDtls { get; set; }
        public docdetails DocDtls { get; set; }
        public sellerdetails SellerDtls { get; set; }
        public buyerdetails BuyerDtls {get;set;}
        public valuedtails ValDtls { get; set; }
        public List<ItemDetails> ItemList { get; set; }
        public string RefDtls { get; set; }
        public paydetails PayDtls { get; set; }
        public shipdetails ShipDtls { get; set; }
        public exportdetails ExpDtls { get; set; }
        public ewbdetails EwbDtls { get; set; }
    }
    public class transdetail
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string RegRev { get; set; }
        public string EcmGstin { get; set; }
        public string IgstOnIntra { get; set; }
       
    }
    public class docdetails
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }
        
    }
    public class sellerdetails
    {
        public string Gstin { get; set;}
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public string Stcd { get; set; }
        public string Pin { get; set; }
        public string Em { get; set; }
        public string Ph { get; set; }
    }
    public class buyerdetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string pos { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public string Stcd { get; set; }
        public string Pin { get; set; }
        public string Em { get; set; }
        public string Ph { get; set; }
    }
    public class ItemDetails
    {
    public string SlNo { get; set; }
    public string PrdDesc { get; set; }
    public string IsServc { get; set; }
    public string HsnCd { get; set; }
    public string Barcde { get; set; }
    public string Qty { get; set; }
    public string FreeQty { get; set; }
    public string Unit { get; set; }
    public string UnitPrice { get; set; }
    public string TotAmt { get; set; }
    public string Discount { get; set; }
        public string PreTaxVal { get; set; }
        public string AssAmt { get; set; }
        public string GstRt { get; set; }
        public string IgstAmt { get; set; }
        public string CgstAmt { get; set; }
        public string SgstAmt { get; set; }
        public string OthChrg { get; set; }
        public string TotItemVal { get; set; }
        public string OrdLineRef { get; set; }
        public string OrgCntry { get; set; }
        public string PrdSlNo { get; set; }
        public string BchDtls { get; set; }
        public string AttribDtls { get; set; }
    }
    public class paydetails
    {
        public string Nm { get; set; }
        public string AccDet { get; set; }
        public string Mode { get; set; }
        public string FinInsBr { get; set; }
        public string PayTerm { get; set; }
        public string PayInstr { get; set; }
        public string CrTrn { get; set; }
        public string DirDr { get; set; }
        public string CrDay { get; set; }
        public string PaidAmt { get; set; }
        public string PaymtDue { get; set; }
    }
    public class shipdetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public string Stcd { get; set; }
        public string Pin { get; set; }
        
    }
    public class exportdetails
    {
        public string ShipBNo { get; set; }
        public string ShipBDt { get; set; }
        public string Port { get; set; }
        public string RefClm { get; set; }
        public string ForCur { get; set; }
        public string CntCode { get; set; }
        public string ExpDuty { get; set; }
    }
    public class ewbdetails
    {
        public string TransId { get; set; }
        public string TransName { get; set; }
        public string TransMode { get; set; }
        public string Distance { get; set; }
        public string TransDocNo { get; set; }
        public string TransDocDt { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }
    }
    public class valuedtails
    {
        public string AssVal { get; set; }
        public string CgstVal { get; set; }
        public string SgstVal { get; set; }
        public string IgstVal { get; set; }
        public string CesVal { get; set; }
        public string StCesVal { get; set; }
        public string Discount { get; set; }
        public string OthChrg { get; set; }
        public string RndOffAmt { get; set; }
        public string TotInvVal { get; set; }
        public string TotInvValFc { get; set; }
    }
}
