using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Arasan.Models
{
    public class PaymentRequest
    {
        public PaymentRequest()
        {
            this.Typelst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Grnlst = new List<SelectListItem>();
            this.Polst = new List<SelectListItem>();
            this.Reqlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Typelst;
        public string Type { get; set; }
        public string Branch { get; set; }
        public string ID { get; set; }
        public List<SelectListItem> Suplst;

        public string Supplier { get; set; }
        public List<SelectListItem> Grnlst;

        public string GRN { get; set; }
        public List<SelectListItem> Polst;

        public string PO { get; set; }
        public string Amount { get; set; }
        public string Final { get; set; }
        public List<SelectListItem> Reqlst;
        public string createdby { get; set; }
        public string ReqBy { get; set; }
        public string Reason { get; set; }
        public string DocId { get; set; }
        public string Date { get; set; }
        public string status { get; set; }
        public string Approve { get; set; }
 
        public string ddlStatus { get; set; }

        public string amountReceived { get; set; }
        public string pendingamt { get; set; }
        public List<PaymentRequestDetail> PREQlst { get; set; }
    }

    public class PaymentRequestGrid
    {
        public string docId { get; set; }

        public string date { get; set; }
        public string type { get; set; }

        public string id { get; set; }
        public string supplier { get; set; }

        public string grn { get; set; }
        public String editrow { get; set; }
        public String delrow { get; set; }


        public string amountReceived { get;set; }
        public string pendingamt { get;set; }
       
    }
    public class PaymentRequestDetail
    {
        public string ID { get; set; }
        public string Isvalid { get; set; }
        public string docid { get; set; }
        public string type { get; set; }
        public string amount { get; set; }
        public string reqby { get; set; }
        public string date { get; set; }
        //public string pogrn { get; set; }
 
    }

    public class PaymentReqVoucherGrid
    {
        public string docId { get; set; }

        public string date { get; set; }
        public string type { get; set; }

        public string id { get; set; }
        public string supplier { get; set; }
        public string amount { get; set; }

        public string grn { get; set; }
        public String voucher { get; set; }

    }
}
