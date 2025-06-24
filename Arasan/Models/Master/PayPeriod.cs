using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models 
{
    public class PayPeriod
    {
        public string DocId { get; set; }
        public string Set { get; set; }
        public List<Pay> PayLists { get; set; }
      

        public string PayPeriodType { get; set; }
        public List<SelectListItem> PPLists { get; set; }
        public string StartingDate { get;  set; }
        public string PayPeriods { get;  set; }
        public string EndingDate { get;  set; }
        public string SalaryDate { get;  set; }
        public string WeeklyHolidays { get;  set; }
        public string MonthlyHolidays { get;  set; }
        public string OtherHols { get;  set; }
        public string WorkingDays { get; set; }

        public string ID { get;  set; }
        public string ddlStatus { get; set; }

    }
    public class Pay
    {
        public string PayPeriods { get; set; }
        public string StartsAt { get; set; }
        public string EndsAt { get; set; }
        public string SalaryDate { get; set; }
        public string PayPeriodDays { get; set; }

        public string WeeklyHolidays { get; set; }
        public string MonthlyHolidays { get; set; }
        public string OtherHols { get; set; }
        public string WorkingDays { get; set; }
        public string Isvalid { get; set; }
        public string PayPeriod { get; set; }
        public string Paycode { get; set; }
    }

    public class IPayPeriodGrid
    {
        public string id { get; set; }

        public string docid { get; set; }
        public string docdate { get; set; }
        public string payperiodtype { get; set; }
        public string startingdate { get; set; }
        public string endingdate { get; set; }
        public string saldate { get; set; }



        public String editrow { get; set; }
        public String viewrow { get; set; }
        public String delrow { get; set; }

    }

}
