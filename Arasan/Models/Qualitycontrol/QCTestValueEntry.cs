using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class QCTestValueEntry
    {
        public string ID { get; set; }
        public QCTestValueEntry()
        {
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();

        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> assignList;

        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Work { get; set; }
        public string Shift { get; set; }
        public string Process { get; set; }
        public string Drum { get; set; }
        public string Prodate { get; set; }
        public string Sample { get; set; }
        public string Sampletime { get; set; }
        public string Item { get; set; }
        public string Entered { get; set; }
        public string Remarks { get; set; }
    }
}
