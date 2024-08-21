using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class OnDuty
    {
        public string DocId { get; set; }
        public string EmplId { get; set; }
        public List<SelectListItem> EmplIdlst { get; set; }

        public string DocDate { get; set; }
        public string EmpName { get; set; }
        public string EDes { get; set; }
        public string EGen { get; set; }
        public string ddlStatus { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string ID { get; set; }

        public List<ODLS> OdLst { get; set; }

    }
    public class ODLS
    {


        public string StartDate { get; set; }
        public string FrTime { get; set; }
        public string ToTime { get; set; }
        public string ToHR { get; set; }
        public string DuSit { get; set; }
        public string Res { get; set; }
        public string Sts { get; set; }

        public List<SelectListItem> Stslst { get; set; }
        public string Isvalid { get; set; }


    }

    public class ListOnDuty
    {
        public string did { get; set; }
        public string eid { get; set; }
        public string ddat { get; set; }
        public string ename { get; set; }
        public string edes { get; set; }
        public string egen { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
