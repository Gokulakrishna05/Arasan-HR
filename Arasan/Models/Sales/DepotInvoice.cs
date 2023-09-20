using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class DepotInvoice
    {
        public DepotInvoice()
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
        public List<TermsItem> TermsItemlst { get; set; }
        public List<DepotInvoiceItem> Depotlst { get; set; }
        public List<AreaItem> AreaItemlst { get; set; }
    }
    public class DepotInvoiceItem
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
    public class TermsItem
    {
        public string Isvalid { get; set; }
        public string ID { get; set; }

        public List<SelectListItem> Termslst { get; set; }

        public string Terms { get; set; }

    }
    public class DDrumdetails
    {
        public string lotno { get; set; }
        public string drumno { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string invid { get; set; }
        public bool drumselect { get; set; }
    }
    public class AreaItem
    {
        public string Isvalid { get; set; }

        public string ID { get; set; }

        public List<SelectListItem> Arealst { get; set; }

        public string Areaid { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string Fax { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Cst { get; set; }

        public string Tin { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string Email { get; set; }

    }
    public class Drumdetailstable
    {
        public List<DDrumdetails> Drumlst { get; set; }

        public bool selectall { get; set; }
    }
}
