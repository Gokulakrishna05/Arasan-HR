using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AssignAllowance
    {
        public AssignAllowance()
        {
            this.EmpNamelst = new List<SelectListItem>();
            this.AllowanceTypelst = new List<SelectListItem>();
            this.AllowanceNamelst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpNamelst;
        public List<SelectListItem> AllowanceTypelst;
        public List<SelectListItem> AllowanceNamelst;

        public string? ID { get; set; }
        public string? EmpName { get; set; }
        public string? AllowanceName { get; set; }
        public string? AllowanceType { get; set; }
        public string? AmtPerc { get; set; }
        public string? EffectiveDate { get; set; }
        public string? Description { get; set; }
        public string? Ddlstatus { get; set; }
    }

    public class AssignAllowancegrid
    {
        public string? id { get; set; }
        public string? empname { get; set; }
        public string? allowancename { get; set; }
        public string? allowancetype { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }
    }
}
