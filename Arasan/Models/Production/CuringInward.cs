using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class CuringInward
    {
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string WorkCenter { get; set; }

        public List<SelectListItem> Shiftlst;
        public string Shift { get; set; }
      
        public CuringInward()
        {
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> assignList;
        public string RecevedBy { get; set; }

        public List<CIItem> CILst { get; set; }
    }
    public class CIItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ItemGrouplst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string Isvalid { get; set; }
        public string ItemGroupId { get; set; }

        public string batchno { get; set; }
        public double batchqty { get; set; }
    }
}
