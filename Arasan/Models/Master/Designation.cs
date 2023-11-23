using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Models
{
    public class Designation
    {
        public Designation()
        {
            this.DeptNamelst = new List<SelectListItem>();
           
        }
        public string ID { get; set; }
        
        public string Design { get; set; }
       

        public List<SelectListItem> DeptNamelst;
        public string DeptName { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }

    }
    
    public class DesignationGrid
    {
        public string id { get; set; }
        
        public string design { get; set; }
        public string deptname { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
