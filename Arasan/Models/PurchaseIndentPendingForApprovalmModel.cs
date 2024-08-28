using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PurchaseIndentPendingForApprovalModel
    {
        public string ID { get; set; }

        public string dtFrom { get; set; }
    }
    public class PurchaseIndentPendingForApprovalModelItems
    {

        public string docid { get; set; }
        public string docdate { get; set; }
        public string itemid { get; set; }
        public string unitid { get; set; }
        public string ord_qty { get; set; }
        public string pur_qty { get; set; }
        public string pend_qty { get; set; }
        public string duedate { get; set; }
        public string locid { get; set; }
        public string narration { get; set; }
        public string app1dt { get; set; }
        public string app2dt { get; set; }
        public string entdt { get; set; }



    }




}

