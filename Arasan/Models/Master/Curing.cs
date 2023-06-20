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

        public string Sub { get; set; }
        public string Shed { get; set; }

        public string Cap { get; set; }

        public List<SelectListItem> statuslst;
        public string status { get; set; }


    }
}
