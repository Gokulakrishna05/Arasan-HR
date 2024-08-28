using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class FGBalancePowderStockModel
    {

        public string ID { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class FGBalancePowderStockModelItems
    {
        public string itemid { get; set; }
        public string oq { get; set; }
        public string rq { get; set; }
        public string orq { get; set; }
        public string pkq { get; set; }
        public string oiss { get; set; }



    }




}

