
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class APPowderStockInPyroModel
    {

        public string ID { get; set; }




        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class APPowderStockInPyroModelItem
    {
        public string docdate { get; set; }
        public string opqty { get; set; }
        public string recqty { get; set; }
        public string issqty { get; set; }


    }

}
