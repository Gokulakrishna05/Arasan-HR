using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
 
    public class LeaveTypeMaster

    {

        //public LeaveTypeMaster()
        //{
        //    this.LTNamelst = new List<SelectListItem>();
        //}
        //public List<SelectListItem> LTNamelst;
        public string LTName { get; set; }

        public string ID { get; set; }
        public string Des { get; set; }
        public string Mapy { get; set; }

        public string ddlStatus { get; set; }

        
    }

    public class LeaveTypeMasterList
    {
        public string id { get; set; }
        public string leavetypename { get; set; }
        public string description { get; set; }
        public string allowedperyear { get; set; }

        
        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }
}
