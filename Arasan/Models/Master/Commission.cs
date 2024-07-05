using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Commission
    {
        public string ID { get; set; }

        public string Cid { get; set; }
       public string Date { get; set; }
       public string Code { get; set; }
       public string Value { get; set; }
       public string Valid { get; set; }
       public List<comlst> colst { get; set; }
       public List<SelectListItem> commtypelst { get; set; }

        public string ddlStatus { get; set; }



    }
    public class comlst
    {

        public string item { get; set; }
        public string unit { get; set; }
        public List<SelectListItem> ulst { get; set; }
        public List<SelectListItem> commtypelst { get; set; }

        public string type { get; set; }
        public string val { get; set; }
        public string Isvalid { get; set; }
      

        public List<SelectListItem> ilst { get; set; }
        public List<SelectListItem> tlst { get; set; }


    }
    public class Commissionlist
    {
        public string ID { get; set; }

        public string cid { get; set; }
        public string date { get; set; }
        public string code { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
    }
