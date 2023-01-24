using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Arasan.Models
{
    public class StoresReturn
    {
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Narr { get; set; }

        public StoresReturn()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Loc;
        public string Location { get; set; }
    }
}
