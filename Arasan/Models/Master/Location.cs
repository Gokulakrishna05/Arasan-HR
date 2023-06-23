using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Location
    {
        public string ID { get; set; }
        public string LocationId { get; set; }
        public string LocType { get; set; }
        public string ContactPer { get; set; }
        public string PhoneNo { get; set; }
        //public string FaxNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string status { get; set; }

        //public string Bin { get; set; }
        //public string Trade { get; set; }

        //public string FlowOrd { get; set; }

        public Location()
        {
            this.Brlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
    }
}
