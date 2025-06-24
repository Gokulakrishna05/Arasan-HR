using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;

namespace Arasan.Models
{
    public class LeaveMaster
    {

      
        public string EmpId { get; set; }

        public string ID { get; set; }
        public string NOTEmp { get; set; }
        public string Dept { get; set; }

        public string ddlStatus { get; set; }

        public List<LeaveDetaillist> LeaveMasterDetaillist { get; set; }
    }
    public class LeaveDetaillist
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
}
