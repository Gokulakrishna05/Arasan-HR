namespace Arasan.Models
{
    public class AttendanceReport
    {
        public string? ID { get; set; }
        public string? EmpID { get; set; }
        public string? EmpName { get; set; }
        public string? AttDate { get; set; }
        public string? MissIN { get; set; }
        public string? MissOUT { get; set; }
        public string? ShiftNo { get; set; }
        public string? ShiftStart { get; set; }
        public string? ShiftEnd { get; set; }
        public string? WeekOff { get; set; }
        public string? Ddlstatus { get; set; }
    }

    public class AttendanceReportgrid
    {
        public string? empid { get; set; }
        public string? empname { get; set; }
        public string? attdate { get; set; }
        public string? missin { get; set; }
        public string? missout { get; set; }
        public string? shiftno { get; set; }
        public string? shiftstart { get; set; }
        public string? shiftend { get; set; }
        public string? weekoff { get; set; }
        public string? attendance { get; set; }
        public string? instatus { get; set; }
        public string? outstatus { get; set; }
        
    }
}
