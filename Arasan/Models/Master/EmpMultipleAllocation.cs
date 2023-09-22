using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class EmpMultipleAllocation
    {
        public string ID { get; set; }

        public EmpMultipleAllocation()
        {
            this.Emolst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }

        public string EDate { get; set; }
        public List<SelectListItem> Emolst;
        public string Emp { get; set; }
        public List<SelectListItem> Loclst;
        public string[] Location { get; set; }
        //public string Location { get; set; }


    }
    public class EmpBindList
    {
        public long piid { get; set; }
        public string emp { get; set; }
        public string edate { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
        public string reassign { get; set; }
    }
    public class EmpMultipleItemBindList
    {
        public long piid { get; set; }

        public string location { get; set; }
    }
    public class Viewdetails
    {
        public string ID { get; set; }

        public string Location { get; set; }
    }
    public class EmpView
    {
        public List<Viewdetails> Viewlst { get; set; }

        public string EDate { get; set; }
        public string ID { get; set; }

        public string Emp { get; set; }
        public string Location { get; set; }
    }
    public class EmpReasign
    {
        public string ID { get; set; }

        public List<SelectListItem> Emolst { get; set; }
        public string Emp { get; set; }
        public string Reassign { get; set; }
        public string Reason { get; set; }
        public string EDate { get; set; }
        public List<SelectListItem> Loclst { get; set; }
        public string Location { get; set; }



    }
}
