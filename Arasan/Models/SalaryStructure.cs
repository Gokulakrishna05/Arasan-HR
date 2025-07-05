using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SalaryStructure
    {
        public SalaryStructure()
        {
            this.EmpNamelst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpNamelst;

        public string? ID { get; set; }
        public string? EmpName { get; set; }
        public string? Salary { get; set; }
        public string? HRA { get; set; }
        public string? AllowanceAmt { get; set; }
        public string? OTRate { get; set; }
        public string? Incentive { get; set; }
        public string? Bonus { get; set; }
        public string? Ddlstatus { get; set; }

    }

    public class SalaryStructuregrid
    {
        public string? id { get; set; }
        public string? empname { get; set; }
        public string? bonus { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }
    }
}
