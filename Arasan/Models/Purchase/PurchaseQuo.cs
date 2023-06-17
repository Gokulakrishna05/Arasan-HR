using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PurchaseQuo
    {
        public PurchaseQuo()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.QoLst = new List<QoItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }

        public string Supplier { get; set; }

        public List<SelectListItem> Suplst;
        public List<SelectListItem> RecList;

        public List<SelectListItem> assignList;
        public string Recid { get; set; }

        public string assignid { get; set; }
        public string Currency { get; set; }
        public string ExRate { get; set; }
        public string Active { get; set; }

        public List<SelectListItem> Curlst;
        public List<QoItem> QoLst;

        public string ID { get; set; }
        public string QuoId { get; set; }

        public string DocDate { get; set; }

        public string status { get; set; }


        public string EnqNo { get; set; }

        public string EnqDate { get; set; }
        public double Net { get; set; }
    }
        public class QoItem
        {
            public string ItemId { get; set; }

            public List<SelectListItem> Ilst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string saveItemId { get; set; }
        public string ItemGroupId { get; set; }
        public string Desc { get; set; }
            public string Unit { get; set; }
        public string ConsFa { get; set; }
        public double Quantity { get; set; }

            public double rate { get; set; }
        public double TotalAmount { get; set; }
    
        public string Isvalid { get; set; }


        }
    public class QuoFollowup
    {
        public string FolID { get; set; }
        public string QuoNo { get; set; }
        public string Supname { get; set; }
        public string Enqdate { get; set; }
        public string Followby { get; set; }
        public string Enquiryst { get; set; }
        public string Followdate { get; set; }
        public string Nfdate { get; set; }
        public string Rmarks { get; set; }
        public List<SelectListItem> EnqassignList;

        public string Quoteid { get; set; }
        public List<QuotationFollowupDetails> qflst { get; set; }

    }
    public class QuotationFollowupDetails
    {
        public string ID { get; set; }
        public string Followby { get; set; }
        public string Enquiryst { get; set; }
        public string Followdate { get; set; }
        public string Nfdate { get; set; }
        public string Rmarks { get; set; }
    }
}

