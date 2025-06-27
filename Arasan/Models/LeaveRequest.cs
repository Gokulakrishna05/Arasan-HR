using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class LeaveRequest
    {
        public LeaveRequest()
        {
            this.EmpLst = new List<SelectListItem>();
            this.LeaveTypeLst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpLst;
        public List<SelectListItem> LeaveTypeLst;

        public string? LeaveID { get; set; }
        public string? EmpID { get; set; }
        public string? LeaveType { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? TotDays { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }
        public string? Ddlstatus { get; set; }

    }

    public class ListLeaveRequest
    {
        public string? leaveid { get; set; }
        public string? empid { get; set; }
        public string? leavetype { get; set; }
        public string? totdays { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }
    }
}
