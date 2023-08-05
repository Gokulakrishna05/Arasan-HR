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
            this.Worklst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Worklst;
        public List<SelectListItem> Shiftlst;
        
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
        public string APID { get; set; }
        public string Sample { get; set; }
        public string Sampletime { get; set; }
        public string Item { get; set; }
        public string Rate { get; set; }
        public string Nozzle { get; set; }
        public string Air { get; set; }
        public string AddCharge { get; set; }
        public string Ctemp { get; set; }
        public string Entered { get; set; }
        public string Remarks { get; set; }

        public List<QCTestValueEntryItem> QCTestLst { get; set; }
    }
    public class QCTestValueEntryItem
    {
        public string Description { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Startvalue { get; set; }
        public string Endvalue { get; set; }
        public string Test { get; set; }
        public string Manual { get; set; }
        public string Actual { get; set; }
        public string APID { get; set; }
        public string TestResult { get; set; }
        public string Isvalid { get; set; }

    }
}
