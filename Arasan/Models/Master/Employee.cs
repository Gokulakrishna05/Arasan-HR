using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();
            this.EMPDeptlst = new List<SelectListItem>();
            this.EMPDesignlst = new List<SelectListItem>();
            this.BranchsLst = new List<SelectListItem>();
            this.DeptLst = new List<SelectListItem>();
            this.PayCateLst = new List<SelectListItem>();
            this.WeekLst = new List<SelectListItem>();
            this.PaymentLst = new List<SelectListItem>();
            this.BankLst = new List<SelectListItem>();
            this.ShiftLst = new List<SelectListItem>();
            this.BankNameLst = new List<SelectListItem>();
            this.DesigLst = new List<SelectListItem>();
            this.UserNameLst = new List<SelectListItem>();
            this.AdAccountLst = new List<SelectListItem>();
            this.BloodGroupLst = new List<SelectListItem>();
            this.CommunityLst = new List<SelectListItem>();
            this.DispLst = new List<SelectListItem>();

        }
        public string EmpName { get; set; }
        public string EmpNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string StateId { get; set; }
        public string CityId { get; set; }

        public string PhoneNo { get; set; }
        public string PinCode { get; set; }
        public string Branch { get; set; }
        public string createdby { get; set; }
        public string Branchs { get; set; }
        public List<SelectListItem> BranchsLst;

        public string ID { get; set; }
        //public string EmpID { get; set; }
        //  public string JoiningDate { get; set; }
        //public string Name { get; set; }

        public string Dept { get; set; }
        public List<SelectListItem> DeptLst;

        public string PayCate { get; set; }
        public List<SelectListItem> PayCateLst;

        public string Week { get; set; }
        public List<SelectListItem> WeekLst;

        public string BankName { get; set; }
        public List<SelectListItem> BankNameLst;

        public string Payment { get; set; }
        public List<SelectListItem> PaymentLst;
        public string Bank { get; set; }
        public List<SelectListItem> BankLst;
        public string Shift { get; set; }
        public List<SelectListItem> ShiftLst;
        public string ESI { get; set; }
        public string ESIR { get; set; }

        public string Active { get; set; }
        public string Bonus { get; set; }
        public string CL { get; set; }
        public string IFSC { get; set; }
        public string EPF { get; set; }
        public string pfclose { get; set; }
        public string UAN { get; set; }
        public string Cost { get; set; }
        public string OTamt { get; set; }
        public string OT { get; set; }
        public string BAccount { get; set; }
        public string Meals { get; set; }
        public string Appren { get; set; }
        public string LOP { get; set; }
        public string Desig { get; set; }
        public List<SelectListItem> DesigLst;

        public string AdAccount { get; set; }
        public List<SelectListItem> AdAccountLst;
        public string createby { get; set; }
        public string oldpf { get; set; }
        public string dependantes { get; set; }
        public string Mainexp { get; set; }
        public string Pffrom { get; set; }
        public string Pfto { get; set; }
        public string Adbal { get; set; }
        public string oldpass { get; set; }
        public string newpass { get; set; }
        public string confpass { get; set; }
        public string MultipleLoc { get; set; }
        public string Region { get; set; }
        public string Gender { get; set; }
        public string Phychal { get; set; }
        public string Aadhar { get; set; }
        public string imgpath { get; set; }
        public string DOB { get; set; }

        public string PF { get; set; }
        public string PFR { get; set; }
        public string PFdate { get; set; }
        //  public string CityId { get; set; }
        // public string StateId { get; set; }
        public string EmailId { get; set; }
        // public string PhoneNo { get; set; }

        //PersonalDetails
        public string eactive { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string GaurdName { get; set; }
        public string Voucher { get; set; }
        public string MaterialStatus { get; set; }
        public string BloodGroup { get; set; }
        public List<SelectListItem> BloodGroupLst;

        public string Community { get; set; }
        public List<SelectListItem> CommunityLst;
        public string PayType { get; set; }

        public string EmpType { get; set; }
        public string Disp { get; set; }
        public List<SelectListItem> DispLst;


        //LoginDetails
        public string UserName { get; set; }
        public List<SelectListItem> UserNameLst;
        public string Password { get; set; }
        public string EMPDeptment { get; set; }
        public string EMPDesign { get; set; }
        public string EMPDeptCode { get; set; }

        public string JoinDate { get; set; }
        public string ResignDate { get; set; }
        public string status { get; set; }




        //Paycode


        public string EMPPayCategory { get; set; }
        public string EMPBasic { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }

        //  public string PFdate { get; set; }
        public string ESIDate { get; set; }


        public string EMPCost { get; set; }


        public string Education { get; set; }
        public string College { get; set; }
        public string EcPlace { get; set; }
        public string YearPassing { get; set; }
        public double MPercentage { get; set; }

        public List<SelectListItem> Statelst;

        public List<SelectListItem> Citylst;

        public List<SelectListItem> EMPDeptlst;

        public List<SelectListItem> EMPDesignlst;
        //public List<EduDeatils> EduLst { get; set; }
        public string SkillSet { get; set; }
        public string ddlStatus { get; set; }
        public List<Pcod> Pclst { get; set; }
        public List<Edet> Edlst { get; set; }
        public List<Eatt> Atlst { get; set; }
        public List<Brch> Brlst { get; set; }
        public List<Dcod> Dclst { get; set; }
        public List<Perf> pelst { get; set; }
        public List<Emrc> Emlst { get; set; }
        public List<Eins> Inlst { get; set; }
        public List<Prhs> Phlst { get; set; }
        public List<LeaveDet> Leavelst { get; set; }

    }
    public class Pcod
    {


        public string pc { get; set; }
        public string pf { get; set; }
        public string Isvalid { get; set; }


    }

    public class Edet
    {
        public string qua { get; set; }
        public string clg { get; set; }
        public string plc { get; set; }
        public string yop { get; set; }
        public string peo { get; set; }
        public string Isvalid { get; set; }
    }
    public class LeaveDet
    {
        public List<SelectListItem> lelst { get; set; }

        public string leavecode { get; set; }
        public string leavetype { get; set; }
        public string allwoleaves { get; set; }
        public string caried { get; set; }
        public string encash { get; set; }
        public string Isvalid { get; set; }
    }

    public class Eatt
    {
        public string mon { get; set; }
        public string pre { get; set; }
        public string abs { get; set; }
        public string lea { get; set; }
        public string nhd { get; set; }
        public string nhw { get; set; }
        public string wo { get; set; }
        public string wds { get; set; }
        public string hds { get; set; }
        public string Isvalid { get; set; }
    }

    public class Brch
    {
        public List<SelectListItem> wlst { get; set; }

        public string wc { get; set; }
        public string Isvalid { get; set; }
    }
    public class Dcod
    {
        public List<SelectListItem> wlst { get; set; }

        public string wc { get; set; }
        public string Isvalid { get; set; }
    }
    public class Perf
    {
        public string pps { get; set; }
        public string res { get; set; }
        public string rat { get; set; }
        public string awd { get; set; }
        public string Isvalid { get; set; }


    }
    public class Emrc
    {
        public string cna { get; set; }
        public string nor { get; set; }
        public string pho { get; set; }
        public string mob { get; set; }
        public string fax { get; set; }
        public string Isvalid { get; set; }
    }

    public class Eins
    {
        public string pno { get; set; }
        public string nop { get; set; }
        public string bad { get; set; }
        public string apr { get; set; }
        public string dpr { get; set; }
        public string psd { get; set; }
        public string ped { get; set; }
        public string Isvalid { get; set; }
    }
    public class Prhs
    {
        public string emp { get; set; }
        public string city { get; set; }
        public List<SelectListItem> wlst { get; set; }

        public string wc { get; set; }
        public string lsd { get; set; }
        public string wrm { get; set; }
        public string Isvalid { get; set; }


    }
    public class MultipleLocation
    {


        public string ID { get; set; }
        public string EmpName { get; set; }

        public List<SelectListItem> Loclst;
        public string[] Location { get; set; }
        public string CreatedOn { get; set; }

        public string CreadtedBy { get; set; }
        public string Status { get; set; }

        //public class EduDeatils
        //{

        //    //Education Details
        //    public string ID { get; set; }
        //    public string Education { get; set; }
        //    public string College { get; set; }
        //    public string EcPlace { get; set; }
        //    public string YearPassing { get; set; }
        //    public double MPercentage { get; set; }
        //    public string EmpId { get; set; }
        //    public string SkillSet { get; set; }
        //}

    }
    public class EmployeeDetails
    {
        public string EMPNAME { get; set; }
        public string EMPDOB { get; set; }
        public string EMPSEX { get; set; }
        public string ECPHNO { get; set; }
        public string ECMAILID { get; set; }
        public string ECADD1 { get; set; }
        public string EMPID { get; set; }
        public string EMPDESIGN { get; set; }
        public string EMPDEPT { get; set; }
        public string SHIFTCATEGORY { get; set; }
        public string EMPPAYCAT { get; set; }
        public string BANK { get; set; }
        public string WOFF { get; set; }
        public string FATHERNAME { get; set; }
        public string MOTHERNAME { get; set; }
        public string HANDICAPPED { get; set; }
        public string ECSTATE { get; set; }
        public string ECCITY { get; set; }
        public string EPINCODE { get; set; }
        public string ADDRESS { get; set; }
        public string JOINDATE { get; set; }


    }
    public class EmpOthDetails
    {

        public string MARITALSTATUS { get; set; }
        public string BLOODGROUP { get; set; }
        public string COMMUNITY { get; set; }

    }
    public class EmpEduDetails
    {

        public string EDUCATION { get; set; }
        public string ECPLACE { get; set; }
        public string UC { get; set; }
        public string YRPASSING { get; set; }
        public string MPER { get; set; }

    }
    public class EmployeeGrid
    {
        public string empno { get; set; }

        public string id { get; set; }
        public string empname { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string pass { get; set; }
        public string depart { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string multi { get; set; }
        public string print { get; set; }
    }


}
