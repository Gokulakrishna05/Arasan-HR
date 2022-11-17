using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StoreAcc
    {
        public string ID { get; set; }
        public string Docid { get; set; }
        public string Docdate { get; set; }
        public string Refno { get; set; }
        public string Refdate { get; set; }
        public string Retno { get; set; }
        public string Retdate { get; set; }
        public string Narr { get; set; }
        public StoreAcc()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
       
    }

}
