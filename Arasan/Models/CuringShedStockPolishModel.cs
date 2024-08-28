
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class CuringShedStockPolishModel
    {

        public string ID { get; set; }
 
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class CuringShedStockPolishModelItem
    {
        public string docdate { get; set; }
        public string opqty { get; set; }
        public string rqty { get; set; }
        public string ipkqty { get; set; }
        public string impkqty { get; set; }
        public string ipoqty { get; set; }
        public string ipyqty { get; set; }
        public string ipaqty { get; set; }
        public string stkpqty { get; set; }
        public string stkmqty { get; set; }
        public string fdrm { get; set; }
        public string itemqty { get; set; }
        public string pack { get; set; }
        public string drum { get; set; }
        public string str { get; set; }


    }

}
