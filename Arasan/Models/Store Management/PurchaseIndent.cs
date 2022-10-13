using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PurchaseIndent
    {
        public PurchaseIndent()
        {
            this.Brlst = new List<SelectListItem>();
            this.SLoclst = new List<SelectListItem>();
            this.PILst= new List<PIndentItem>();
            this.ELst = new List<SelectListItem>();
            this.PURLst = new List<SelectListItem>();
            this.EmpLst = new List<SelectListItem>();
            this.TANDClst = new List<PIndentTANDC>();
        }

        public List<SelectListItem> Brlst;
        public string ID { get; set; }
        public string Branch { get; set; }

        public List<SelectListItem> SLoclst;

        public List<PIndentItem> PILst;

        public List<PIndentTANDC> TANDClst;

        public List<SelectListItem> ELst;

        public List<SelectListItem> PURLst;

        public List<SelectListItem> EmpLst;

        public string SLocation { get; set; }

        public string IndentDate { get; set; }

        public string RefDate { get; set; }

        public string Erection { get; set; }

        public string Purtype { get; set; }

        public string PreparedBy { get; set; }

    }
    public class PIndentItem
    {
        public string ItemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public string LocId { get; set; }

        public List<SelectListItem> loclst { get; set; }

        public string QC { get; set; }
        public string Unit { get; set; }
        public string IndentPlaced { get; set; }
        public double Quantity { get; set; }
        public string Narration { get; set; }
        public string Duedate { get; set; }
        public double Stock { get; set; }
        public double WholeStock { get; set; }
        public string Isvalid { get; set; }

    }
    public class PIndentTANDC
    {
        public string TANDC { get; set; }
        public string Isvalid { get; set; }
    }
    }
