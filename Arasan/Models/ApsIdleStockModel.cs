using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ApsIdleStockModel
    {
 
        public string ID { get; set; }
         
        public string dtFrom { get; set; }
    }
    public class ApsIdleStockModelItems
    {

        public string locid { get; set; }
        public string drumno { get; set; }
        public string docdate { get; set; }
        public string itemid { get; set; }
        public string batchno { get; set; }
        public string qty { get; set; }
        public string rc { get; set; }
        public string asondt { get; set; }
        public string days { get; set; }
       


    }




}

