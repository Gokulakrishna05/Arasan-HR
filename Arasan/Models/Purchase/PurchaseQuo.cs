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
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }

        public string Supplier { get; set; }

        public List<SelectListItem> Suplst;

        public string Currency { get; set; }

        public List<SelectListItem> Curlst;
        public List<QoItem> QoLst;

        public string ID { get; set; }
        public string QuoId { get; set; }

        public string DocDate { get; set; }




        public string EnqNo { get; set; }

        public string EnqDate { get; set; }
    }
        public class QoItem
        {
            public string ItemId { get; set; }

            public List<SelectListItem> Ilst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }


        public string ItemGroupId { get; set; }
        public string Desc { get; set; }
            public string Unit { get; set; }

            public double Quantity { get; set; }

            public double rate { get; set; }
            public string Isvalid { get; set; }


        }
    }

