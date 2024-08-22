using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class FGStockDetailReportModel
    {
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
        public string Loc { get; set; }
        public List<SelectListItem> LocLst { get; set; }


    }
    public class FGStockDetailReportModelItems
    {
        public string icat { get; set; }
        public string sncat { get; set; }
        public string gra { get; set; }
        public string unit { get; set; }
        public string nob { get; set; }
        public string avg { get; set; }
        public string tqty { get; set; }
        public string sucat { get; set; }
        public string asdate { get; set; }
    }
}
