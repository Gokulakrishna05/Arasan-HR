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
        public List<SelectListItem> itemlst;

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> assignList;

        public string DocId { get; set; }
        public string TotalQty { get; set; }
        public string dis { get; set; }
        public string id { get; set; }
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
        public string ItemId { get; set; }
        public string Rate { get; set; }
        public string Nozzle { get; set; }
        public string Air { get; set; }
        public string AddCharge { get; set; }
        public string Ctemp { get; set; }
        public string Entered { get; set; }
        public string Remarks { get; set; }
        //public string ApId { get; set; }

        public List<QCTestValueEntryItem> QCTestLst { get; set; }
        public List<ViewAPOut> ViewAPOutlist { get; set; }
    }
    public class QCTestValueEntryItem
    {

        public string description { get; set; }
        public string testid { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
        public string startvalue { get; set; }
        public string endvalue { get; set; }
        public string test { get; set; }
        public string manual { get; set; }
        public string actual { get; set; }
        public string apid { get; set; }
        public string testresult { get; set; }
        public string Isvalid { get; set; }
        public string ItemName { get; set; }
        public string Drum { get; set; }
          public string Time { get; set; }
        //public string ApId { get; set; }

    }
    public class ViewAPOut
    {
        public string id { get; set; }
        public string Drum { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Time { get; set; }
        public string ApId { get; set; }
        public string dis { get; set; }

    }
}
