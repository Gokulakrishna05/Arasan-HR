using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AllowanceMaster
    {
        public AllowanceMaster()
        {
            this.AllowanceTypelst = new List<SelectListItem>();
            this.ApplicableLevellst = new List<SelectListItem>();
        }
        public List<SelectListItem> AllowanceTypelst;
        public List<SelectListItem> ApplicableLevellst;

        public string? ID { get; set; }
        public string? AllowanceName { get; set; }
        public string? Description { get; set; }
        public string? AllowanceType { get; set; }
        public string? ApplicableLevel { get; set; }
        public string? IsRecurring { get; set; }
        public string? EffectiveDate { get; set; }
        public string? Ddlstatus { get; set; }
    }

    public class AllowanceMastergrid
    {
        public string? id { get; set; }
        public string? allowancename { get; set; }
        public string? allowancetype { get; set; }
        public string? editrow { get; set; }
        public string? viewrow { get; set; }
        public string? delrow { get; set; }
    }
}
