using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DebitNoteBill
    {
        public string ID { get; set; }
        public DebitNoteBill()
        {
            this.Brlst = new List<SelectListItem>();
            this.Partylst = new List<SelectListItem>();
           

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
        public List<SelectListItem> Partylst;
        public string Party { get; set; }
        public string Vocher { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Gross { get; set; }
        public string Net { get; set; }
        public string Amount { get; set; }
        public string Bigst { get; set; }
        public string Bsgst { get; set; }
        public string Bcgst { get; set; }
        public string Narration { get; set; }

        public List<DebitNoteItem> Depitlst { get; set; }
    }
    public class DebitNoteItem
    {
        public string ID { get; set; }

        public string InvNo { get; set; }
        public string Invdate { get; set; }

        public string Item { get; set; }
        public string Cf { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string CGST { get; set; }
        public string CGSTP { get; set; }
        public string SGSTP { get; set; }
        public string IGSTP { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Total { get; set; }

        public List<SelectListItem> Grnlst;
        public string Isvalid { get; set; }
    }
}
