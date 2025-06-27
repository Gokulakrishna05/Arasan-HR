using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Permissions
    {
        public string ID { get; set; }
        public string PID { get; set; }
        public string EmpID { get; set; }
        public string PerDate { get; set; }
        public string FTDate { get; set; }
        public string TTDate { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string ADDGDate { get; set; }

        public string ddlStatus { get; set; }


    }

    public class PermissionsList
    {
        public string id { get; set; }
        public string perdate { get; set; }
        public string ftdate { get; set; }
        public string ttdate { get; set; }


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }

}
