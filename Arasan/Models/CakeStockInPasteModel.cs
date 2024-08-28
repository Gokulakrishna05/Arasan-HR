
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class CakeStockInPasteModel
    {
        public CakeStockInPasteModel()
        {
            this.Brlst = new List<SelectListItem>();
            
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        
        public string Branch { get; set; }
 

        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class CakeStockInPasteItems
    {
        public string docdate { get; set; }
        public string oqty { get; set; }
        public string rqty { get; set; }
        public string iqty { get; set; }
        public string pqty { get; set; }
        public string mqty { get; set; }
         
    }

}
