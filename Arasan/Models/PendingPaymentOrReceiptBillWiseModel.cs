using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PendingPaymentOrReceiptBillWiseModel
    { 
        public PendingPaymentOrReceiptBillWiseModel()
        {
            this.Brlst = new List<SelectListItem>();

        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }


        public string dtFrom { get; set; }
    }
    public class PendingPaymentOrReceiptBillWiseModelItems
    {

        public string grouporder { get; set; }
        public string slno { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string groupno { get; set; }
        public string mname { get; set; }
        public string matchdate { get; set; }
        public string amount { get; set; }
        public string pending { get; set; }
        public string userid { get; set; }
 


    }




}

