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

        }
        public string EmpNo { get; set; }

        public string ID { get; set; }
        public string EmpName { get; set; }
        public string Branch { get; set; }
        public string MultipleLoc { get; set; }
        public string Region { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }

        public string Address { get; set; }
        public string CityId { get; set; }
        public string StateId { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }

        //PersonalDetails
        public string eactive { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MaterialStatus { get; set; }
        public string BloodGroup { get; set; }
        public string Community { get; set; }
        public string PayType { get; set; }

        public string EmpType { get; set; }
        public string Disp { get; set; }

        //LoginDetails
        public string UserName { get; set; }
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

        public string PFdate { get; set; }
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
}
