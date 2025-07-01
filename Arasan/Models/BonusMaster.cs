using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class BonusMaster
    {
        public string ID { get; set; }
        public string BType { get; set; }
        public string ADes { get; set; }
        public string CType { get; set; }
        public string BValue { get; set; }
        public string EFrom { get; set; }
        public string Remarks { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string ddlStatus { get; set; }

    }
    public class BonusMasterList
    {
        public string id { get; set; }
        public string bonustype { get; set; }
        public string applicabledesignations { get; set; }
        public string bonusvalue { get; set; }


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }
}
