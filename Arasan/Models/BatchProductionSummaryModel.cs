
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class BatchProductionSummaryModel
    {

        public string ID { get; set; }
 
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class BatchProductionSummaryModelItem
    {
        public string etype { get; set; }
        public string wcid { get; set; }
        public string processid { get; set; }
        public string seq { get; set; }
        public string itemid { get; set; }
        public string unitid { get; set; }
        public string qty { get; set; }
        public string wipqty { get; set; }
        public string mtono { get; set; }
         


    }

}
