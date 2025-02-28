using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ExportInvoice
    {
        public ExportInvoice()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Voclst = new List<SelectListItem>();
            this.Templst = new List<SelectListItem>();
            this.Suplst2 = new List<SelectListItem>();
            this.Orderlst = new List<SelectListItem>();
            this.Termslst = new List<SelectListItem>();
           
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }
        public string Branch { get; set; }
        public List<SelectListItem> Suplst;
        public string Customer { get; set; }
        public List<SelectListItem> Curlst;
        public string Currency { get; set; }
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string ExRate { get; set; }
        public string VocherType { get; set; }

        public List<SelectListItem> Voclst;
        
        public List<SelectListItem> Templst;
        public string Template { get; set; }
        public string Sup { get; set; }
        
        public List<SelectListItem> Suplst2;
        public string Freight { get; set; }
        public string Insurance { get; set; }
        public string Country { get; set; }
        public List<SelectListItem> Orderlst;
        public string Order { get; set; }
        public string CCode { get; set; }
        public List<SelectListItem> Schemelst;
        public string Scheme { get; set; }
        public string Licence { get; set; }

        public List<SelectListItem> Termslst;
        public List<SelectListItem> arealist;
        public string Terms { get; set; }
        public string Delivery { get; set; }
        public string Handling { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string add3 { get; set; }
        public string add2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Gross { get; set; }
        public string Discount { get; set; }
        public string FrightCharge { get; set; }
        public string igst { get; set; }
        public string Net { get; set; }
        public string Round { get; set; }
        public string Narration { get; set; }
        public string Truck { get; set; }
        public string Distance { get; set; }
        public string Shipment { get; set; }
        public string ShipDate { get; set; }
        public string Words { get; set; }
        public string Lading { get; set; }
        public string LadingDate { get; set; }
        public string Remarks { get; set; }
        public string ByRoad { get; set; }
        public string Exinv { get; set; }
        public string Assign { get; set; }
        public string FollowUp { get; set; }
        public string Deatils { get; set; }
        public string Port { get; set; }
        public string PortPinCode { get; set; }
        public string DisCharge { get; set; }
        public string Acceptance { get; set; }
        public string Destination { get; set; }
        public string PlaceDelivery { get; set; }
        public string BankTerms { get; set; }

        public List<ExportInvoiceItem> InvoiceLst { get; set; }
    }
    public class ExportInvoiceItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
       
        public string Isvalid { get; set; }
        public string JobId { get; set; }
        public string JobDate { get; set; }
       
        public string Hsn { get; set; }
        public string Itemtype { get; set; }
        public string ItemSpe { get; set; }
        public string Unit { get; set; }
  
        public string Pack { get; set; }
        public string UnitPrimary { get; set; }
        public string QtyPrimary { get; set; }
     
        public string NaviRate { get; set; }
        public string Amount { get; set; }
        public string OtherDis { get; set; }
        public string Discount { get; set; }
        public string FrightKgs { get; set; }
        public string Fc { get; set; }
        public string InsKgs { get; set; }
        public string InsAmount { get; set; }
        public string AcessAmount { get; set; }
        public string ExciseType { get; set; }
        public string Traiff { get; set; }
        public string Igsts { get; set; }
        public string IGST { get; set; }
        public string work { get; set; }
        public string workid { get; set; }
        public string dcno { get; set; }
        public string dcid { get; set; }
        public string dcdate { get; set; }
        public string jodate { get; set; }
        public string itemss { get; set; }
        public string saveitem { get; set; }
        public string unitname { get; set; }
        public string itemtypes { get; set; }
        public string itemdesc { get; set; }
        public string des { get; set; }
        public string drum { get; set; }
        public string twt { get; set; }
        public string schid { get; set; }
        public string gwt { get; set; }
        public string nwt { get; set; }
        public double introdisc { get; set; }
        public double cashdis { get; set; }
        public double tradedis { get; set; }
        public double adddis { get; set; }
        public double specdis { get; set; }
        public double rate { get; set; }
        public double amountt { get; set; }
        public double discamt { get; set; }
        public double quantity { get; set; }
        public double frigcharges { get; set; }
        public string DrumIds { get; set; }

    }
}
