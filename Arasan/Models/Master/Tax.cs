using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Tax
    {
        public Tax()
        {
            this.Taxtypelst = new List<SelectListItem>();
            
        }
        public string ID { get; set; }

        public List<SelectListItem> Taxtypelst;
        public string Taxtype{ get; set; }

        public string Percentage { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }


    }
    public class Taxgrid
    {
        public string id { get; set; }
        public string tax { get; set; }
        public string percentage { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }

    }
