using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models.Transaction
{
    public class MissingPunchEntry
    {
        public MissingPunchEntry()
        {
            this.EmpNamelst = new List<SelectListItem>();
            this.MissingPunch = new List<SelectListItem>();
        }
        public string? ID { get; set; }
        public string? createby { get; set; }

        public List<SelectListItem> EmpNamelst;
        public List<SelectListItem> MissingPunch;
        public string EmployeeId { get; set; }
        public string PunchDate { get; set; }
        public string? MissingIn { get; set; }
        public string? MissingOut { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string? ApprovedBy { get; set; }
        public string? ApprovedDate { get; set; }
        public string? Missing { get; set; }
        public string? Device { get; set; }
        public string? ddlStatus { get; set; }
    }
    public class MissingPunchEntrygrid
    {
        public string id { get; set; }
        public string emp { get; set; }
        public string attendance { get; set; }
        public string missing { get; set; }
        public string device { get; set; }
        public string reason { get; set; }
        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }
    }
}
