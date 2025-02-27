using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class ExWorkOrderClose
    {
        public ExWorkOrderClose()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Reasonlst = new List<SelectListItem>();
           
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }
        public string Branch { get; set; }

        public List<SelectListItem> Suplst;
        public string Customer { get; set; }
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string Customername { get; set; }

        public List<SelectListItem> Reasonlst;
        public string Reason { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Narration { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string ddlStatus { get; set; }
        public List<OrderItem> OrderLst { get; set; }
    }
    public class OrderItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string JobId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Des { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string OrdQty { get; set; }
        public string PendQty { get; set; }
        public string ShortQty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Dc { get; set; }
        public string Excise { get; set; }

    }
    public class ListWorkOrderItems
    {
        public long id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string customer { get; set; }
        public string sendmail { get; set; }
        public string followup { get; set; }
        public string move { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
    }
}
