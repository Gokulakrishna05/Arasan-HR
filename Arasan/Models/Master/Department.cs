using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Models
{
    public class Department
    {
        public string ID { get; set; }
       // public string DepartmentCode { get; set; }
        public string Departmentcode { get; set; }
        public string DepartmentName { get; set; }
        public string status { get; set; }
        public string Descrip { get; set; }
        public string IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string ddlStatus { get; set; }


        //public List<Designation> Designationlst { get; set; }
    }
   public class Departmentgrid
    {
        public string id { get; set; }
        public string departmentcode { get; set; }
        public string departmentname { get; set; }
        public string description { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
       
        
    }
 
}

