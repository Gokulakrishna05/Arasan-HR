using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class OTEntry
    {
        public OTEntry()
        {
            this.EmpNamelst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpNamelst;
        public string? PID { get; set; }
        public string? EmpName { get; set; }
        public string? Date { get; set; }
        public string? Description { get; set; }
        public string? OTHours { get; set; }
        public string? OTON { get; set; }
        public string? Status { get; set; }

        public string? ddlStatus { get; set; }

    }


    public class OTEntryList
    {
        public string? pid { get; set; }
        public string? empname { get; set; }
        public string? date { get; set; }
        public string? othours { get; set; }
        public string? oton { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }
        public string? approverow { get; set; }
       

    }
}
