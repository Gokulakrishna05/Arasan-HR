using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SalesEnquiry
    {
        public SalesEnquiry()
    {
        this.Brlst = new List<SelectListItem>();
        this.Suplst = new List<SelectListItem>();
        this.Curlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Prilst = new List<SelectListItem>();
            this.Enqlst = new List<SelectListItem>();
        }
    public List<SelectListItem> Brlst;
    public string ID { get; set; }

    public string Branch { get; set; }
        public string EnqDate { get; set; }
        public string Customer { get; set; }

    public List<SelectListItem> Suplst;

    public string Currency { get; set; }

    public List<SelectListItem> Curlst;
        public List<SelectListItem> RecList;
        public string Assign { get; set; }
        public List<SelectListItem> assignList;
        public string Recieved { get; set; }
        public List<SelectListItem> Prilst;
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> Enqlst;
        public string CustomerType { get; set; }
        public string EnqType { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string EnqNo { get; set; }
        public string ContactPersion { get; set; }
        public string Priority { get; set; }
    }
}
   