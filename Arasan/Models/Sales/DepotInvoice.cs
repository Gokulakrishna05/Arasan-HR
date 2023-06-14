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
        public string Order { get; set; }

        public List<SelectListItem> Dislst;
        public string Dis { get; set; }

        public List<SelectListItem> Inspelst;
        public string Inspect { get; set; }

        public List<SelectListItem> Doclst;
        public string Doc { get; set; }

        public List<SelectListItem> Voclst;
        public string Vocher { get; set; }
    }
}
