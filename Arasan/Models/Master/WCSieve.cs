using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class WCSieve
    {
        public WCSieve()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkCenter { get; set; }
        public string createby { get; set; }
        public string ddlStatus { get; set; }

        public List<WCSItem> WCSLst { get; set; }
    }
    public class WCSItem
    {
        public string ID { get; set; }
        public string Sieve { get; set; }
        public string saveSieve { get; set; }
        public List<SelectListItem> Sievelst { get; set; }
        public string Rate { get; set; }
        public string Type { get; set; }
        public string Isvalid { get; set; }

    }
    public class WCSieveItem
    {
        public long id { get; set; }
        public string work { get; set; }
        public string sieve { get; set; }
        public string rate { get; set; }
        public string type { get; set; }
        public string view { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }
}
