using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DirectAddition
    {
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

        public DirectAddition()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string Branch { get; set; }
        public string user { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public string ddlStatus { get; set; }
        public List<DirectItem> Itlst { get; set; }
    }
    public class ListDirectAdditionItem
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string docNo { get; set; }
        public string loc { get; set; }
        public string docDate { get; set; }
        public string refno { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string entby { get; set; }
        public string view { get; set; }
    }
    public class DirectItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public string BinID { get; set; }

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
}
