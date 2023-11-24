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
        public string createby { get; set; }
        
        public string ddlStatus { get; set; }

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
    public class HsnList
    {
        public long id { get; set; }
        public string hcode { get; set; }
        public string dec { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
       
    }
    public class HsnRowList
    {
        public long id { get; set; }

        public string tariff { get; set; }
    }
}
