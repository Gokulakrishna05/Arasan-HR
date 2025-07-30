using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class EmpAdvance
    {
        public EmpAdvance()
        {

            this.EmpIDLst = new List<SelectListItem>();
            this.AdvIDLst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string Adv { get; set; }
        public string Empe { get; set; }

        public string AdvTp { get; set; }
        public string Advamt { get; set; }
        public string Emi { get; set; }
        public string SMn { get; set; }
        public string Emid { get; set; }
        public string Rmks { get; set; }
        public string? Ddlstatus { get; set; }

        public List<SelectListItem> EmpIDLst;
        public List<SelectListItem> AdvIDLst;

    }

    public class EmpAdvanceList
    {
        public string adv { get; set; }
        public string empe { get; set; }
        public string dvTp { get; set; }
       

        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }

}
