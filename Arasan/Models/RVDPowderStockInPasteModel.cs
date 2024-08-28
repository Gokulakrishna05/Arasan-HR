
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class RVDPowderStockInPasteModel
    {

        public string ID { get; set; }
 
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class RVDPowderStockInPasteModelItem
    {
        public string docdate { get; set; }
        public string opqty { get; set; }
        public string recqty { get; set; }
        public string issqty { get; set; }
        


    }

}
