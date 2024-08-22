using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class FinishedGoodsStockDetailsDrumwiseModel
    {
        public FinishedGoodsStockDetailsDrumwiseModel()
        {
             this.Loclst = new List<SelectListItem>();
            
        }
        public string ID { get; set; }

         public List<SelectListItem> Loclst;
        
         public string Location { get; set; }


        public string dtFrom { get; set; }
     }
    public class FinishedGoodsStockDetailsDrumwiseModelItems
    {
        public string docdate { get; set; }
        public string locid { get; set; }
        public string itemid { get; set; }
        public string branchno { get; set; }
        public string drumno { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string packinsflag { get; set; }
        public string status { get; set; }
        public string asondate { get; set; }
        public string laydays { get; set; }

    }

}
