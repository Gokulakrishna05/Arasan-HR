using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AssignAllowance
    {
        public AssignAllowance()
        {
            this.EmpNamelst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpNamelst;

        public string? ID { get; set; }
        public string? EmpName { get; set; }
        public string? Ddlstatus { get; set; }

        public List<SelectAllowance> Allowancelst { get; set; }
    }

    public class SelectAllowance
    {
        public string? AllowanceType { get; set; }
        public List<SelectListItem> AllowanceTypelst { get; set; }
        public string? AllowanceName { get; set; }
        public List<SelectListItem> AllowanceNamelst { get; set; }
        public string? AmtPerc { get; set; }
        public string? EffectiveDate { get; set; }
        public string? Description { get; set; }
        public string? Isvalid { get; set; }

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
