using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Paycode
    {
        public string ID { get; set; }
        public string DOCDATE { get; set; }
        public string Set { get; set; }
        public List<subgroup> PayLists { get; set; }
        //public string Paycode { get; internal set; }
        public string Print { get; internal set; }
        public string Printas { get; internal set; }
        public string ddlStatus { get; set; }

    }
    public class subgroup
    {
        public string Paycode { get; set; }
        public string Print { get; set; }
        public string PrintAs { get; set; }
        public string Addorless { get; set; }
        public List<SelectListItem> Less;
        public List<SelectListItem> callst;
        public string CalculateFrom { get; set; }
        public string BasedOn { get; set; }
        public string Sno { get; set; }
        public string Formula { get; set; }
        public string Order { get; set; }
        public string Display { get; set; }
        public string Isvalid { get; set; }

        public static implicit operator subgroup(Pay v)
        {
            throw new NotImplementedException();
        }
    }
    public class IPaycodeGrid
    {
        public string id { get; set; }

        public string docid { get; set; }
        public string docdate { get; set; }
       
       

        public String editrow { get; set; }
        public String viewrow { get; set; }
        public String delrow { get; set; }

    }
}
