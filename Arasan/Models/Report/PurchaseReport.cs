using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PurchaseReport
    {
        public PurchaseReport()
        {
            this.Brlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
    }
}
