using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AccountGroup
    {
        public AccountGroup()
        {
            this.Brlst = new List<SelectListItem>();
            this.Typelst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string AccGroup { get; set; }
        public string AType { get; set; }
        public string GCode { get; set; }

        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        
        public List<SelectListItem> Typelst;
        public string Accounttype { get; set; }
        public string Status { get; set; }
        public string Display { get; set; }
        public string Grouptype { get; set; }
        public string Accclass { get; set; }
        public string docid { get; set; }
    }
}
