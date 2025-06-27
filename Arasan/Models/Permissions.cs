using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Permissions
    {
        public Permissions()
        {
            this.EmpNamelst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpNamelst;
        public string? PID { get; set; }
        public string? EmpName { get; set; }
        public string? PerDate { get; set; }
        public string? FromTime { get; set; }
        public string? ToTime { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }
        public string? ddlStatus { get; set; }


    }

    public class PermissionsList
    {
        public string? pid { get; set; }
        public string? empname { get; set; }
        public string? perdate { get; set; }
        public string? fromtime { get; set; }
        public string? totime { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }

    }

}
