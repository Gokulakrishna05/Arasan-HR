using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class RateCode
    {
        public RateCode()
        { 
            this.Brlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
        public string Ratecode { get; set; }
        public string RateDsc { get; set; }
        public string ddlStatus { get; set; }
    }
    public class ListRateCodeItem
    {
        public double id { get; set; }
        public string ratecode { get; set; }
        public string ratedsc { get; set; }
        public string edit { get; set; }
        public string delrow { get; set; }
    }
}
