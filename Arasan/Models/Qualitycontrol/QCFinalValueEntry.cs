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
            this.drumlst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.Batchlst = new List<SelectListItem>();
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

        public List<SelectListItem> drumlst;
        public string DrumNo { get; set; }


        public List<SelectListItem> Itemlst;
        public string Itemid { get; set; }

        public List<SelectListItem> Batchlst;
        public string BatchNo { get; set; }

        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string Batch { get; set; }
        public string ProNo { get; set; }
        public string Rate { get; set; }
        public string ProDate { get; set; }
        public string SampleNo { get; set; }
        public string NozzleNo { get; set; }
        public string AirPress { get; set; }
        public string Additive { get; set; }
        public string Stime { get; set; }
        public string CTemp { get; set; }
        public string FResult { get; set; }
        public string RType { get; set; }

        public List<QCFVItem> QCFVLst { get; set; }

        public List<QCFVItemDeatils> QCFVDLst { get; set; }

    }
    public class QCFVItem
    {
        public string Des { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Sta { get; set; }
        public string En { get; set; }
        public string Test { get; set; }
        public string Manual { get; set; }
        public string Actual { get; set; }
        public string Result { get; set; }
        public string Isvalid { get; set; }

    }
    public class QCFVItemDeatils
    {
        public string Time { get; set; }
        public string Vol { get; set; }
        public string Volat { get; set; }
        public string Volc { get; set; }
        public string Stp { get; set; }

        public string Isvalid { get; set; }
    }
}
