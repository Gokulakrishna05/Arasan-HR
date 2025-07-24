using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class EmpShiftScheduleModel
    {

        public EmpShiftScheduleModel()
        {
            this.MonthLst = new List<SelectListItem>();
            this.EmpCategoryLst = new List<SelectListItem>();
            this.DepLst = new List<SelectListItem>();

        }
        public string? DocId { get; set; }
        public string? ID { get; set; }
        public string? Branch { get; set; }

        public string? Month { get; set; }
        public List<SelectListItem> MonthLst;

        public string? EmpCategory { get; set; }
        public List<SelectListItem> EmpCategoryLst;

        public string? Dep { get; set; }
        public List<SelectListItem> DepLst;

        public List<EmployeeShift>? EmpShiftSchedulelist { get; set; }

        public string? ddlStatus { get; set; }


    }

    public class EmployeeShift
    {
        //public string EmplID { get; set; }
        //public List<SelectListItem> EmplIdlst { get; set; }
        public string? empid { get; set; }
        public string? empname { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ShVal { get; set; }
        public string? Shift { get; set; }
        public List<SelectListItem>? Shiftlst { get; set; }
        public string? StTime { get; set; }
        public string? EndTime { get; set; }
        public string? WOFF { get; set; }
        public List<SelectListItem>? WOFFlst { get; set; }

        public string? Isvalid { get; set; }
        public List<SelectListItem> ShiftTypelst { get;  set; }
    }
    public class EmployeeShifScheduletList
    {
        public string? id { get; set; }
        public string? docid { get; set; }
        public string? month { get; set; }
        public string? category { get; set; }
        public string? department { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }

    }


}
