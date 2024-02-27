using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Curing
    {
        public Curing()
        {
            this.Cur = new List<SelectListItem>();
            this.Sublst = new List<SelectListItem>();
            this.statuslst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public List<SelectListItem> Cur;
        public string Location { get; set; }

        public List<SelectListItem> Sublst;

        public string binid { get; set; }


        public string Cap { get; set; }

        public List<SelectListItem> statuslst;
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createdby { get; set; }


    } 
    
    public class CuringGrid
    {
        
        public string id { get; set; }
        public string location { get; set; }


        public string binid { get; set; }

        public string cap { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

        


    }
}
