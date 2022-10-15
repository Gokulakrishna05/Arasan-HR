using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PurchaseQuo
    {
        public PurchaseQuo()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }

        public string Supplier { get; set; }

        public List<SelectListItem> Suplst;

        public string Currency { get; set; }

        public List<SelectListItem> Curlst;
        public string ID { get; set; }
        public string QuoId { get; set; }

        public string DocDate { get; set; }

        public double ExchangeRate { get; set; }
        

        public string EnqNo { get; set; }
       
        public string EnqDate { get; set; }
    }
}
