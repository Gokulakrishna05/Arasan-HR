using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Country
    {
        public string ID { get; set; }
        public string ConName { get; set; }
        public string ConCode { get; set; }
        public string Curr { get; set; }

        public List<SelectListItem> Cur;
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }

        public List<CurItem> Curlst { get; set; }

    }
    public class Countrygrid
    {
        public string id { get; set; }
        public string coname { get; set; }
        public string concode { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
    public class CurItem
    {
        public string pcode { get; set; }
        public string pnum { get; set; }
        public string ppin { get; set; }
        public string Isvalid { get; set; }
        public string psta { get; set; }
        public List<SelectListItem> pur;


    }
}