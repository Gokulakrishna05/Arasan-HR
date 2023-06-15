namespace Arasan.Models
{
    public class WorkOrderShortClose 
    {
        public string ID { get; set; }
        //public WorkOrder()
        //{
        //    this.Brlst = new List<SelectListItem>();
        //    this.Qolst = new List<SelectListItem>();
        //    this.Curlst = new List<SelectListItem>();
        //    this.Loc = new List<SelectListItem>();

        //}
       
        public string Branch { get; set; }

        
        public string DocDate { get; set; }
        
        public string Location { get; set; }
        public string LocationId { get; set; }
        public string DocId { get; set; }
        public string Customer { get; set; }
        public string CusNo { get; set; }
        public string Cusdate { get; set; }
        public string Ref { get; set; }
        public string RefDate { get; set; }
        public string ExRate { get; set; }
        public string JopId { get; set; }
        public string CustomerId { get; set; }
        public string SalesValue { get; set; }
        public string CreditLimit { get; set; }
        public string TransAmount { get; set; }
        public string OrderType { get; set; }
        public string RateCode { get; set; }
        public string Narr { get; set; }
        public List<WorkCloseItem> Closelst { get; set; }
    }
    public class WorkCloseItem
    {
        public string ID { get; set; }
        public string items { get; set; }

      
        public string orderqty { get; set; }


        public string unit { get; set; }
      
        public string rate { get; set; }
        public string PendQty { get; set; }
        public string ItemId { get; set; }
        public string UnitId { get; set; }
        public string clQty { get; set; }
    


        public string Isvalid { get; set; }
        //public List<SelectListItem> outputlst;

    }
}
