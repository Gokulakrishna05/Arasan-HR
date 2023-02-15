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
            this.cuntylst = new List<SelectListItem>();
        }
        public List<SelectListItem> cuntylst;

        public string Country { get; set; }

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

        public double Net { get; set; }

        public List<QuoItem> QuoLst;
    }
    public class QuoItem 
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
}
