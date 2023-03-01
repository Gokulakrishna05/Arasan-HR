using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class WorkOrder
    {
        public string ID { get; set; }
        public WorkOrder()
        {
            this.Brlst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Curlst;
        public string Currency { get; set; }
        public List<SelectListItem> Loc;
        public string Location { get; set; }
    }
}
