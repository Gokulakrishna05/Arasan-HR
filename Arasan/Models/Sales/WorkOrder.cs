using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class WorkOrder
    {
        public string ID { get; set; }
        public WorkOrder()
        {
            this.Brlst = new List<SelectListItem>();
            this.Qolst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();
            this.partylst = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Curlst;
        public string Currency { get; set; }
        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<SelectListItem> Qolst;
        public List<SelectListItem> partylst;
        public string Quo { get; set; }
        public string Emp { get; set; }
        public string Customer { get; set; }
        public string CusNo { get; set; }
        public string Cusdate { get; set; }
        public string JopId { get; set; }
        public string JopDate { get; set; }
        public string ExRate { get; set; }
        public string RateType { get; set; }
        public string status { get; set; }
        public string SalesValue { get; set; }
        public string CreditLimit { get; set; }
        public string TransAmount { get; set; }
        public string OrderType { get; set; }
        public string RateCode { get; set; }
        public string Narr { get; set; }
        public string ddlStatus { get; set; }
        public string Gross { get; set; }
        public string Net { get; set; }
        public string Discount { get; set; }
        public string FrightCharge { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        public string Round { get; set; }
        public List<WorkItem> Worklst { get; set; }
    }
    public class WorkItem
    {
        public string ID { get; set; }
        public string items { get; set; }

        public string itemspec { get; set; }

        public string Jodetailid { get; set; }
        public string orderqty { get; set; }


        public string unit { get; set; }
        public string disqty { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string itemid { get; set; }
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
        public string matsupply { get; set; }
        public string packind { get; set; }
        public List<SelectListItem> taxlst;
        public List<SelectListItem> itemlst;
        public string tax { get; set; }
        public bool selectall { get; set; }
        public string search { get; set; }
        public string Totamt { get; set; }
        public string Isvalid { get; set; }
        public string cstp { get; set; }
        public string sgstp { get; set; }
        public string igstp { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        //public List<SelectListItem> outputlst;

        public List<Drumdetails> drumlst { get; set; }

    }
    public class WDrumAllocation
    {
        public string ID { get; set; }
        public string Branch { get; set; }
        public string Location { get; set; }
        public string JobId { get; set; }
        public string JobDate { get; set; }
        public string DOCId { get; set; }   
        public string DocDate { get; set; }
        public string Customername { get; set; }
        public string CustomerId { get; set; }
        public string JOId { get; set; }
        public string items { get; set; }

        public string Totalqty { get; set; }
        public List<WorkItem> Worklst { get; set; }

        public string Locid { get; set; }
        //public List<Drumdetails> drumlst { get; set; }
    }
    public class Drumdetails
    {
        public bool drumselect { get; set; }
        public string lotno { get; set; }
        public string drumno { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string invid { get; set; }
    }
    public class WorkOrderItems
    {
        public long id { get; set; }
        //public string branch { get; set; }
        public string enqno { get; set; }
        public string customer { get; set; }
        public string date { get; set; }
        public string loc { get; set; }
        public string clo { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string drum { get; set; }
        public string recept { get; set; }
    }
    public class ListWDrumAllocationItems
    {
        public long id { get; set; }
        //public string branch { get; set; }
        public string jopjd { get; set; }
        public string jopdate { get; set; }
        public string location { get; set; }
        public string customer { get; set; }
        public string drum { get; set; }
    }
    public class ListWDrumAlloItems
    {
        public string id { get; set; }
        //public string branch { get; set; }
        public string jobid { get; set; }
        public string location { get; set; }
        public string customername { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string view { get; set; }
        public string deactive { get; set; }
    }

    public class OrderItemDetail
    {
        public string DOCID { get; set; }
        //public string branch { get; set; }
        public string DOCDATE { get; set; }
        public string PARTYNAME { get; set; }
        public string GSTNO { get; set; }
        public string STATE { get; set; }
         
        public string ADDRESS { get; set; }
        public double BSGST { get; set; }
    
        public double BCGST { get; set; }
        public double BIGST { get; set; }
        public double BDISC { get; set; }
        public double GROSS { get; set; }
        public double NET { get; set; }

    }
    public class OrderDetail
    {
        
        public string ITEMID { get; set; }
        public string PACKSPEC { get; set; }
        public double RATE { get; set; }
        public double QTY { get; set; }
        public double AMOUNT { get; set; }
        public double DISCOUNT { get; set; }
        public string SCHDATE { get; set; }
    }
}
