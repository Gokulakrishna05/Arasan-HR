using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models
{
    public class ProductionEntry
    {
        public ProductionEntry()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Shiftlst=new List<SelectListItem>();
            this.Processlst=new List<SelectListItem>();
            this.ETypelst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Shiftdate { get; set; }
        public string ProcessId { get; set; }
        public string Selection { get; set; }
        public double ProdQty { get; set; }
        public double SchQty { get; set; }
        public string EntryType { get; set; }
        public string ProdLogId { get; set; }
        public string ProdSchNo { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> Processlst;
        public List<SelectListItem> ETypelst;
        public List<ProIn> inputlst { get; set; }
        public double totalinqty { get; set; }
        public double totaloutqty { get; set; }
        public double wastageqty { get; set; }  
        public double totalconsqty { get; set; }
    }
    public class ProIn
    {
        public string ItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ItemGrouplst { get; set; }
        public string ItemGroupId { get; set; }
        public string BinId { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }
        public double batchqty { get; set; }
        public double StockAvailable { get; set; }
        public double IssueQty { get; set; }
        public string MillLoadAdd { get; set; }
        public string Output { get; set; }
        public string Isvalid { get; set; }

    }
}
