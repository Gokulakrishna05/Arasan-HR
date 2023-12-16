using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models 
{
    public class ContToAsset
    {
        public ContToAsset()
        {
            this.Loc = new List<SelectListItem>();
            this.Typelst = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ChellanNo { get; set; }
        public string Reason { get; set; }
        public double Gro { get; set; }
        public double Net { get; set; }
        public List<SelectListItem> assignList;
        public string Entered { get; set; }
        public string Narr { get; set; }
        public string status { get; set; }


        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public List<SelectListItem> Typelst;
        public string Location { get; set; }
        public string ToLoc { get; set; }
        public string Type { get; set; }
        public string bin { get; set; }
        public string ddlStatus { get; set; }
        public List<ConItem> Itlst { get; set; }
    }
    public class ConItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public string Stock { get; set; }
        public double BinID { get; set; }

        public List<SelectListItem> PURLst;
        public string PurType { get; set; }
        public double Quantity { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        //public string Center { get; set; }

        public List<SelectListItem> Processlst;
        public string Process { get; set; }
        public double TotalAmount { get; set; }
        public string Isvalid { get; set; }


    }
    public class ConGrid
    {
        public string id { get; set; }
        public string did { get; set; }
        public string ddate { get; set; }

        public string reason { get; set; }
        public string loc { get; set; }
        public string toloc { get; set; }
        public string viewrow { get; set; }

        public string approve { get; set; }

        public string generate { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}

