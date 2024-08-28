
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class RVDPowderStockInPolishModel
    {

        public string ID { get; set; }

        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class RVDPowderStockInPolishModelItem
    {
        public string docdate { get; set; }
        public string opqty { get; set; }
        public string rqty { get; set; }
        public string pqty { get; set; }
        public string iqty { get; set; }
        

    }

}
