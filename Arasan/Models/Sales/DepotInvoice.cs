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
          
        }
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

        public List<DepotInvoiceItem> Depotlst { get; set; }
    }
    public class DepotInvoiceItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public double FrigCharge { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public string Isvalid { get; set; }
        public double Quantity { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }

        public double Disc { get; set; }
        public double DiscAmount { get; set; }

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
    }
}
