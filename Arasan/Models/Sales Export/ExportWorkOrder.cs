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
        public string arc { get; set; }
        public string txttype { get; set; }
        public double crd { get; set; }

        public List<SelectListItem> Suplst;
        public string Currency { get; set; }

        public List<SelectListItem> Curlst;

        public List<SelectListItem> RecList;
        public string Assign { get; set; }
        public string JopDate { get; set; }
        public string JopId { get; set; }
        public string Location { get; set; }
        public string Emp { get; set; }
        public string user { get; set; }
        public bool selectall { get; set; }
        public string Loc { get; set; }

        public List<SelectListItem> assignList;
        public List<SelectListItem> Testlst;
        public List<SelectListItem> trancelst;
        public string Recieved { get; set; }
        public List<SelectListItem> Prilst;
        public string Order { get; set; }
        public List<SelectListItem> Officelst;
        public string Officer { get; set; }
        public string Job { get; set; }
        public string jobDate { get; set; }
        public string active { get; set; }
        public string item { get; set; }
        public string qty { get; set; }
        public string duedate { get; set; }
        public string Rate { get; set; }
        public string salesrep { get; set; }
        public string payterms { get; set; }
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
        public List<SchItem> schlst { get; set; }
        public List<ExWorkItem> Worklst { get; set; }
    }
    public class WorkOrderItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string itemspec { get; set; }
        public string saveItemId { get; set; }
        public string schqty { get; set; }
        public string schdate { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> itemspeclst { get; set; }
        public string Des { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string natrate { get; set; }
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
        public string close { get; set; }
        public string view { get; set; }
    }
    public class SchItem
    {
        public bool drumselect { get; set; }
        public string suppliar { get; set; }
        public string jobno { get; set; }
        public string schno { get; set; }
        public string schdate { get; set; }
        public string schid { get; set; }
        public string itemid { get; set; }
        public string Isvalid { get; set; }
        public string schqty { get; set; }
        public string item { get; set; }
        public string qty { get; set; }

    }
    public class ExWorkItem
    {
        public string ID { get; set; }
        public string items { get; set; }

        public string itemspec { get; set; }

        public string Jodetailid { get; set; }
        public string orderqty { get; set; }


        public string unit { get; set; }
        public string disqty { get; set; }
        public string rate { get; set; }
        public string rates { get; set; }
        public string amount { get; set; }
        public string itemid { get; set; }
        public string schqty { get; set; }
        public string schdate { get; set; }
        public double assamount { get; set; }
        public string qtydis { get; set; }

        public string spldis { get; set; }


        public string cashdis { get; set; }
        public string introdis { get; set; }
        public string freightamt { get; set; }
        public string freight { get; set; }
        public string tradedis { get; set; }
        public string additiondis { get; set; }

        public string discount { get; set; }

        public string Assesamt { get; set; }


        public string bed { get; set; }
        public string taxtype { get; set; }
        public string tariff { get; set; }
        public string matsupply { get; set; }
        public string packind { get; set; }
        public List<SelectListItem> taxlst;
        public List<SelectListItem> itemlst;
        public List<SelectListItem> itemspeclst;
        public string tax { get; set; }
        public bool selectall { get; set; }
        public string search { get; set; }
        public string drumid { get; set; }
        public string Totamt { get; set; }
        public string DueDate { get; set; }
        public string Isvalid { get; set; }
        public string cstp { get; set; }
        public string sgstp { get; set; }
        public string igstp { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        //public List<SelectListItem> outputlst;

        

    }
    public class ListEWSchItems
    {
        public string id { get; set; }
        //public string branch { get; set; }
        public string jobid { get; set; }
        public string location { get; set; }
        public string schid { get; set; }
        public string qty { get; set; }
        public string drumid { get; set; }
        public string customername { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string dispid { get; set; }
        public string dispdate { get; set; }
        public string schdate { get; set; }
        public string schqty { get; set; }
        public string view { get; set; }
        public string deactive { get; set; }
        public string drum { get; set; }
        public string moveinvoice { get; set; }
    }
    public class EWDrumAllocation
    {
        public string ID { get; set; }
        public string Branch { get; set; }
        public string Location { get; set; }
        public string JobId { get; set; }
        public string JobDate { get; set; }
        public string DOCId { get; set; }
        public string DocDate { get; set; }
        public string schid { get; set; }
        public string Customername { get; set; }
        public string CustomerId { get; set; }
        public string JOId { get; set; }
        public string items { get; set; }

        public string Totalqty { get; set; }
        public List<EWorkItem> Worklst { get; set; }

        public string Locid { get; set; }
        public string Schno { get; set; }
        public string truckno { get; set; }
        public string Schdate { get; set; }
        //public List<Drumdetails> drumlst { get; set; }
    }
    public class EWorkItem
    {
        public string ID { get; set; }
        public string items { get; set; }

        public string itemspec { get; set; }

        public string Jodetailid { get; set; }
        public string orderqty { get; set; }


        public string unit { get; set; }
        public string disqty { get; set; }
        public string rate { get; set; }
        public string rates { get; set; }
        public string amount { get; set; }
        public string itemid { get; set; }
        public string schqty { get; set; }
        public string schdate { get; set; }
        public double assamount { get; set; }
        public string qtydis { get; set; }

        public string spldis { get; set; }


        public string cashdis { get; set; }
        public string introdis { get; set; }
        public string freightamt { get; set; }
        public string freight { get; set; }
        public string tradedis { get; set; }
        public string additiondis { get; set; }

        public string discount { get; set; }

        public string Assesamt { get; set; }


        public string bed { get; set; }
        public string taxtype { get; set; }
        public string tariff { get; set; }
        public string matsupply { get; set; }
        public string packind { get; set; }
        public List<SelectListItem> taxlst;
        public List<SelectListItem> itemlst;
        public List<SelectListItem> itemspeclst;
        public string tax { get; set; }
        public bool selectall { get; set; }
        public string search { get; set; }
        public string drumid { get; set; }
        public string Totamt { get; set; }
        public string DueDate { get; set; }
        public string Isvalid { get; set; }
        public string cstp { get; set; }
        public string sgstp { get; set; }
        public string igstp { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        //public List<SelectListItem> outputlst;

        public List<EDrumdetails> drumlst { get; set; }

    }
    public class EDrumdetails
    {
        public bool drumselect { get; set; }
        public string lotno { get; set; }
        public string sno { get; set; }
        public string drumno { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string invid { get; set; }
    }
}
