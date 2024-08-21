using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StockStatement
    {
        public string Brc { get; set; }
        public List<SelectListItem> Brchlst { get; set; }
        public string Loc { get; set; }
        public List<SelectListItem> LocLst { get; set; }
        public string dtFrom { get; set; }
        public string Typ { get; set; }
        public List<SelectListItem> TypLst { get; set; }
        public string Siw { get; set; }
        public List<SelectListItem> Siwlst { get; set; }


    }
    public class StockStatementItems
    {
        public string dno { get; set; }
        public string iid { get; set; }
        public string lno { get; set; }
        public string qty { get; set; }
        public string bid { get; set; }

    }
}
