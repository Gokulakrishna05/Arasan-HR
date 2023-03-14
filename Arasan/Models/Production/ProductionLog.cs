using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models 
{
    public class ProductionLog
    {
        public ProductionLog()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            
            this.RecList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }
        public string Itemname { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkId { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ProcessId { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> Processlst;
        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
    }
}
