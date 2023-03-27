using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class QCFinalValueEntry
    {
        public QCFinalValueEntry()
        {

            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Worklst;
        public string WorkCenter { get; set; }

        public List<SelectListItem> Processlst;
        public string Process { get; set; }

        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
    }
}
