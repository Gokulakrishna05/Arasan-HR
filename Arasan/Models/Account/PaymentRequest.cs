using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Arasan.Models 
{
    public class PaymentRequest
    {
        public  PaymentRequest()
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

        public string ReqBy { get; set; }
        public string Reason { get; set; }
        public string DocId { get; set; }
        public string Date { get; set; }
        public string status { get; set; }
        public string Approve { get; set; }
        
    }
    

}
