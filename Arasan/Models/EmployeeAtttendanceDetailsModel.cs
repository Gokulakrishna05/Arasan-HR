using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;

namespace Arasan.Models
{
    public class EmployeeAtttendanceDetailsModel
    {


        public string DocId { get; set; }
        public string ID { get; set; }
        public string Docdate { get; set; }

        public string ddlStatus { get; set; }

        public List<AttendanceDetails> EmployeeAttendanceDetailslist { get; set; }
    }
        public class AttendanceDetails
        {

            public List<SelectListItem> Itemlst { get; set; }
            public List<SelectListItem> Itemlsts { get; set; }

            public string EmpID { get; set; }
            public string EmpName { get; set; }
            public string Depart { get; set; }
            public string InOut { get; set; }
            public string Time { get; set; }
            public string Isvalid { get; set; }



        }

    public class EmployeeAttendanceList
    {
        public string id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
      

        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        public string rrow { get; set; }

    }
   

}
