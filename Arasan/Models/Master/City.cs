using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class City
    {
        public City()
        {
            this.sta = new List<SelectListItem>();
            this.cuntylst = new List<SelectListItem>();
        }

        
        public string ID { get; set; }
       
        public string Cit { get; set; }


        public List<SelectListItem> sta;
        public String State { get; set; }
        public String status { get; set; }

        public List<SelectListItem> cuntylst;
        public String countryid { get; set; }


    }
}
