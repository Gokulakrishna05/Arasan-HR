using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class CustomerComplaint
    {
        public CustomerComplaint()
        {
            this.CcirintName = new List<SelectListItem>();
            this.InvestigatedBy = new List<SelectListItem>();
            this.CcirName = new List<SelectListItem>();
            this.DisInvestigatedBy = new List<SelectListItem>();
            this.ReviewBy = new List<SelectListItem>();
           
        }
        public List<SelectListItem> CcirintName;
        public string ID { get; set; }
        public string CCIRINITNAME { get; set; }

        public List<SelectListItem> InvestigatedBy;
        public string INVESTIGATEDBY { get; set; }

        public List<SelectListItem> CcirName;
        public string CCIRNAME { get; set; }

        public List<SelectListItem> DisInvestigatedBy;
        public string DISINVESTIGATEDBY { get; set; }

        public List<SelectListItem> ReviewBy;
        public string REVIEWEDBY { get; set; }
        public string ComplaintNo { get; set; }
        public string CCIRNo { get; set; }
        public string ComplaintDate { get; set; }
        public string Brief { get; set; }
        public string Result { get; set; }
        public string Nature { get; set; }
        public string Remarks { get; set; }
        public string ddlStatus { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
    public class CustomerComplaintItems
    {
        public long id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string ccno { get; set; }
        public string view { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
