
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ExchangeRate
    {
        public string ID { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string Exchange { get; set; }
        public string RateType { get; set; }
        public string ExchangeDate { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }
        //public string Sym { get; set; }
        //public string RTyp { get; set; }

        public List<SelectListItem> Symlst { get; set; }
        public List<SelectListItem> Rtlst { get; set; }



    }
    public class Exchangegrid
    {
        public string id { get; set; }
        public string csym { get; set; }
        public string cname { get; set; }
        public string rtype { get; set; }
        public string erate { get; set; }
        public string date { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
