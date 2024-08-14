using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class EmpLoginDetModel
    {

        public EmpLoginDetModel()
        {
            this.MonthLst = new List<SelectListItem>();
            this.PayCategoryLst = new List<SelectListItem>();
            //this.DepLst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string AttDate { get; set; }
        public string Holiday { get; set; }
        public string Month { get; set; }
        public List<SelectListItem> MonthLst;
        public string PayCategory { get; set; }
        public List<SelectListItem> PayCategoryLst;


        public List<EmployeeLogin> EmpLoginDetlists { get; set; }
        public string ddlStatus { get; set; }


    }

    public class EmployeeLogin
    {
        public string EmpName{ get; set; }
        public List<SelectListItem> EmpNamelst { get; set; }
        public string Mission { get; set; }
        public string ODTaken { get; set; }
        public string ODHrs { get; set; }
        public string WeekOff { get; set; }
        public List<SelectListItem> WeekOfflst { get; set; }
        public string ShiftLoginDate { get; set; }
        public string ShiftLoginTime { get; set; }
        public string  LoginDate { get; set; }
        public string LoginTime { get; set; }
        public string ShiftLogoutDate { get; set; }
        public string ShiftLogoutTime { get; set; }
        public string LogoutDate { get; set; }
        public string LogoutTime { get; set; }
        public string IntDiff { get; set; }
        public string OuttDiff { get; set; }
        public string TimeDiff { get; set; }
        public string rmission { get; set; }
        public string WHrs1 { get; set; }
        public string WHrs2 { get; set; }
        public string WorkedHrs { get; set; }
        public string HA { get; set; }
        public List<SelectListItem> HAlst { get; set; }
        public string OTHrs { get; set; }
        public string ensation { get; set; }
        public string Status { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
        public string Isvalid { get; set; }



    }
    public class EmployeeLoginDet
    {
        public string id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string attdate { get; set; }
        public string month { get; set; }
        public string holiday { get; set; }
        public string paycategory { get; set; }


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        public string rrow { get; set; }

    }
}
