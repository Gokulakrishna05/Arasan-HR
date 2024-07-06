using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Currency
    {
        public string ID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCodes { get; set; }
        public string CurrencyInteger { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }
        public List<UsedCountries> Currencylst { get; set; }


        //public List<UsedCountries> Currencylst { get; set; }


    }
    public class Currencygrid
    {
        public string id { get; set; }
        public string currencycode { get; set; }
        public string currencyname { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }


    public class UsedCountries
    {

        public List<SelectListItem> Currencylst { get; set; }

        public string ConCode { get; set; }
        public string Country { get; set; }
        public string Isvalid { get; set; }



    }
}