using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class HSNcode
    {
        public HSNcode()
        {
            this.CGstlst = new List<SelectListItem>();
            this.SGstlst = new List<SelectListItem>();
            this.IGstlst = new List<SelectListItem>();
           
        }

        public string ID { get; set; }

        public string HCode { get; set; }
        public string Dec { get; set; }


        public List<SelectListItem> CGstlst;
        public string CGst { get; set; }
        public List<SelectListItem> SGstlst;
        public string SGst { get; set; }
        public List<SelectListItem> IGstlst;
        public string IGst { get; set; }
        public string status { get; set; }

        public List<HSNItem> hsnlst { get; set; }
    }

    public class HSNItem
    {
        public string ID { get; set; } 
        public string tariff { get; set; }
        public string Isvalid { get; set; }
        public string savetariff { get; set; }
        public List<SelectListItem> tarifflst { get; set; }


    }
}
