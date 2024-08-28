
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PasteStockInMixingModel
    {

        public string ID { get; set; }




        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class PasteStockInMixingModelItem
    {
        public string docdate { get; set; }
        public string opqty { get; set; }
        public string rqty { get; set; }
        public string piqty { get; set; }
        public string riqty { get; set; }
        public string rmiqty { get; set; }
        


    }

}
