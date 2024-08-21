using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ReceiptAgainstReturnableGoodsModel
    {
        public ReceiptAgainstReturnableGoodsModel()
        {
            this.Brlst = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class ReceiptAgainstReturnableGoodsItems
    {
        public string branchid { get; set; }
        public string locid { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string partyid { get; set; }
        public string dcno { get; set; }
        public string dcdt { get; set; }
        //public string refno { get; set; }
        //public string refdate { get; set; }
        public string itemid { get; set; }
        public string unit { get; set; }
        public string qty { get; set; }
        public string rejqty { get; set; }
        public string accqty { get; set; }
        public string dcqty { get; set; }
 

    }




}

