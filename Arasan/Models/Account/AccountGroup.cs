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

        public long accGrpId { get; set; }
        public string ID { get; set; }
        public string AccGroup { get; set; }
        public string AType { get; set; }
        public string GCode { get; set; }
        public string GrpCode { get; set; }
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

    public class AGroup
    {
        public long accgrpid { get; set; }
        //public double id { get; set; }
        public string id { get; set; }
        public string accgroup { get; set; }
        public string atype { get; set; }
        public string gcode { get; set; }
        
        public string display { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
    }
