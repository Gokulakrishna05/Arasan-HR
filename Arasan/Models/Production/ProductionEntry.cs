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
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
    }
}
