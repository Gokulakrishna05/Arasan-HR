using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace Arasan.Models
{
    public class SalesQuotation
    {
        public SalesQuotation()
        {
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Categorylst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }

        public List<SelectListItem> assignList;

        public string Emp { get; set; }

        public List<SelectListItem> Curlst;

        public string Currency { get; set; }

        public List<SelectListItem> Categorylst;
        public string Through { get; set; }

        public string Sent { get; set; }
        public string ID { get; set; }
    }
}
