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
    }
    public class EmpBindList
    {
        public long piid { get; set; }
        public string emp { get; set; }
        public string edate { get; set; }
        public string EditRow { get; set; }
        public string DelRow { get; set; }
    }
    public class  EmpMultipleItemBindList
    {
        public long piid { get; set; }

        public string location { get; set; }
    }
}
