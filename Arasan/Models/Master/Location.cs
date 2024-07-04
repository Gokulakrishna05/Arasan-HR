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
        public string Trader { get; set; }
        public string Consignee { get; set; }
        public string Requried { get; set; }
        public string Fax { get; set; }
        public string FlowOrd { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string PinCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Mail { get; set; }

        public Location()
        {
            this.Brlst = new List<SelectListItem>();
            this.emplst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            //this.Loclst = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> emplst;
        //public List<SelectListItem> Loclst;
        public List<SelectListItem> Suplst;
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public string Branch { get; set; }
        public string Party { get; set; }
        public string createby { get; set; }
        public string ddlStatus { get; set; }

        public List<LocationItem> Locationlst { get; set; }
        public List<LocItem> Loclst { get; set; }
    }
    public class LocationItem
    {
        public string id { get; set; }
        public string BinId { get; set; }
        public string BinDesc { get; set; }
        public string Capacity { get; set; }

        public string Isvalid { get; set; }
    }
    public class LocItem
    {
        public string id { get; set; }

        public List<SelectListItem> Loclst { get; set; }
        public string Location { get; set; }
        public string saveLocation { get; set; }
        public List<SelectListItem> Isslst { get; set; }
        public string Issuse { get; set; }
        public string saveIssuse { get; set; }

        public string Isvalid1 { get; set; }
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
        public string view { get; set; }
    }

}
