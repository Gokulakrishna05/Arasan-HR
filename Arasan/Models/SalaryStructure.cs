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
        public string? Ddlstatus { get; set; }

        // Salary Details
        public double BasicSalary { get; set; }
        public double DA { get; set; }
        public double HRA { get; set; }
        public double Conveyance { get; set; }
        public double WA { get; set; }
        public double EA { get; set; }
        public double SA { get; set; }
        public double OT { get; set; }
        public string? Bonus { get; set; }
        public double BonusAmt { get; set; }

        // Salary Deductions
        public double PF { get; set; }
        public double ESI { get; set; }
        public double LoanAdv { get; set; }
        public double Insurance { get; set; }
        public double Meals { get; set; }
        public double Fine { get; set; }
        public double TDS { get; set; }
        public double OtherDeductions { get; set; }
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
