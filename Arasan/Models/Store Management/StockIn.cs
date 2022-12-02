using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StockIn
    {
        public string ID { get; set; }

        public string Item { get; set; }

        public string Unit { get; set; }

       
        //public string GoodQty { get; set; }

      
        public string Qty { get; set; }
     
        
        public string Branch { get; set; }
        public string Location { get; set; }
       
    }
}
