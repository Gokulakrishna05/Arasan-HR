using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Models
{
    public class Department
    {
        public string ID { get; set; }
        public string DepartmentCode { get; set; }
        public string Departmentcode { get; set; }
        public string DepartmentName { get; set; }
        public string status { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        //public List<Designation> Designationlst { get; set; }
    }
 //public class Designation
 //   {
 //       public string ID { get; set; }
 //       public string Isvalid { get; set; }
 //       public string Design { get; set; }
 //   }
}

