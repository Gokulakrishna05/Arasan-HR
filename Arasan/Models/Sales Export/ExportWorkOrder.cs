using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class ExportWorkOrder
    {
        public ExportWorkOrder()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Prilst = new List<SelectListItem>();
            this.Officelst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
        public string Customer { get; set; }

        public List<SelectListItem> Suplst;
        public string Currency { get; set; }

        public List<SelectListItem> Curlst;

        public List<SelectListItem> RecList;
        public string Assign { get; set; }

        public List<SelectListItem> assignList;
        public string Recieved { get; set; }
        public List<SelectListItem> Prilst;
        public string Order { get; set; }
        public List<SelectListItem> Officelst;
        public string Officer { get; set; }
        public string Job { get; set; }
        public string jobDate { get; set; }
        public string active { get; set; }
        public string Rate { get; set; }
        public string Refno { get; set; }
        public string Refdate { get; set; }
        public string QuoNo { get; set; }
        public string Emaildate { get; set; }
        public string Send { get; set; }
        public string Emailid { get; set; }
        public string Time { get; set; }
        public string FollowUp { get; set; }
        public string Deatails { get; set; }
        public string Transporter { get; set; }
        public string Test { get; set; }
        public string Spec { get; set; }
        public string ddlStatus { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public List<WorkOrderItem> WorkOrderLst { get; set; }
        public List<TermsDeatils> TermsDeaLst { get; set; }

    }
    public class WorkOrderItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Des { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string QtyDisc { get; set; }
        public string CashDisc { get; set; }
        public string Introduction { get; set; }
        public string Trade { get; set; }
        public string Addition { get; set; }
        public string Special { get; set; }
        public string Discount { get; set; }
        public string Bed { get; set; }
        public string Due { get; set; }
        public string Supply { get; set; }
        public string Packing { get; set; }

    }
    public class TermsDeatils
    {
        public string ID { get; set; }
        public string Isvalid1 { get; set; }
        public string Template { get; set; }
        public List<SelectListItem> Tandclst { get; set; }
        public string Conditions { get; set; }
        public List<SelectListItem> Condlst { get; set; }
    }
    public class ExportWorkOrderItems
    {
        public long id { get; set; }
        public string jobno { get; set; }
        public string jobdate { get; set; }
        public string currency { get; set; }
        public string sendmail { get; set; }
        public string followup { get; set; }
        public string move { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
    }
}
