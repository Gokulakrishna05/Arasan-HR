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
            this.emplst = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> emplst;
        public string Branch { get; set; }
        public string ddlStatus { get; set; }
    }

    public class Locationgrid
    {
        public string id { get; set; }
        public string locationid { get; set; }
        public string loctype { get; set; }
        public string contactper { get; set; }
        public string phoneno { get; set; }
        public string emailid { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }

    }
