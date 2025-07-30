using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Incentive
    {
        public Incentive()
        {
           
            this.EmpIDLst = new List<SelectListItem>();
            //    this.DepLst = new List<SelectListItem>();

        }

        public string ID { get; set; }
            public string Emp { get; set; }
            public string Des { get; set; }

            public string Dpt { get; set; }
            public string Icem { get; set; }
            public string Ictpe { get; set; }
            public string Amt { get; set; }
            public string Rean { get; set; }
            public string ddlStatus { get; set; }
        public string? Ddlstatus { get; set; }

        public List<SelectListItem> EmpIDLst;

    }


    public class IncentiveList
    {
        public string id { get; set; }
        public string empid { get; set; }
        public string desg { get; set; }
        public string dpt { get; set; }

        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }
}
