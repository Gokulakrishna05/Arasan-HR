using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();
        }
        public int EmpId { get; set; }

        public int ID { get; set; }
        public string EmpName { get; set; }
        public string Gender { get; set; }  
        public DateTime DOB { get; set; }
     
        public string Address { get; set; }
        public int  CityId { get; set; }
        public int  StateId { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }

        //PersonalDetails

       
        public  string FatherName { get; set; } 
        public string MotherName { get;set; }
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
        public int EMPDeptCode { get; set; }

        public DateTime JoinDate { get; set; }
        public DateTime ResignDate { get; set; }

        //Education Details

        public string Education { get; set; }
        public string College { get; set; }
        public string EcPlace { get; set; }
        public string YearPassing { get; set; }
        public int MPercentage { get; set; }

        public string SkillSet { get; set; }


       

        //Paycode

     
        public int EMPPayCategory { get; set; }
        public int EMPBasic { get; set; }
        public string  PFNo { get; set; }
        public string ESINo { get; set; }

        public DateTime PFdate { get; set; }
        public DateTime ESIDate { get; set; }

       
        public int EMPCost { get; set; }



        public List<SelectListItem> Statelst;

        public List<SelectListItem> Citylst;

    }

   


}
