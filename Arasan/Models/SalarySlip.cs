using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SalarySlip
    {
        public SalarySlip()
        {
            this.EmpNamelst = new List<SelectListItem>();
        }
        public List<SelectListItem> EmpNamelst;
        public string ID { get; set; }
        public string? Ddlstatus { get; set; }

        // Employee Details
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string DOJ { get; set; }
        public string Dept { get; set; }
        public string Desg { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }

        // Bank Details
        public string BankName { get; set; }
        public string AccNo { get; set; }
        public string IFSC { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }


        public string SalDistDate { get; set; }
        public string GrossSalaryDay { get; set; }

        // Salary Details
        public double BasicSalary { get; set; }
        public double DA { get; set; }
        public double HRA { get; set; }
        public double Conveyance { get; set; }
        public double WA { get; set; }
        public double EA { get; set; }
        public double SA { get; set; }
        public double OT { get; set; }
        public double TotalEarnings { get; set; }

        // Salary Deductions
        public double PF { get; set; }
        public double ESI { get; set; }
        public double LoanAdv { get; set; }
        public double Insurance { get; set; }
        public double Meals { get; set; }
        public double Fine { get; set; }
        public double TDS { get; set; }
        public double OtherDeductions { get; set; }
        public double TotalDeductions { get; set; }

        // Days Detail
        public string TotWorkDays { get; set; }
        public string NHDays { get; set; }
        public string WeekOff { get; set; }
        public string WorkedDays { get; set; }
        public string LeaveDays { get; set; }
        public double OpCL { get; set; }
        public double CLTaken { get; set; }
        public double CloCL { get; set; }
        public string SalaryDays { get; set; }
        public double NetSalary { get; set; }
    }

    public class ListSalarySlip
    {
        public string? id { get; set; }
        public string? empname { get; set; }
        public string? dept { get; set; }
        public string? desg { get; set; }
        public string? pdf { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }
    }
}
