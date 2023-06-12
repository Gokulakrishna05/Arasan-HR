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

        
        public string Currency { get; set; }
        
        public string Location { get; set; }
       
        public string Quo { get; set; }
        public string Customer { get; set; }
        public string CusNo { get; set; }
        public string Cusdate { get; set; }
        public string JopId { get; set; }
        public string JopDate { get; set; }
        public string ExRate { get; set; }
        public string RateType { get; set; }
        public string SalesLimit { get; set; }
        public string SalesValue { get; set; }
        public string CreditLimit { get; set; }
        public string TransAmount { get; set; }
        public string OrderType { get; set; }
        public string RateCode { get; set; }
        public string Narr { get; set; }
        public List<WorkItem> Worklst { get; set; }
    }

}
