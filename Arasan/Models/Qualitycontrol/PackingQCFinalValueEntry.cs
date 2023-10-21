using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PackingQCFinalValueEntry
    {
        public PackingQCFinalValueEntry()
        {

            this.Doclst = new List<SelectListItem>();
            this.Entrylst = new List<SelectListItem>();
        }

        public List<SelectListItem> Entrylst;
        public string PEntryid { get; set; }

        public List<SelectListItem> Doclst;
        public string Docid { get; set; }

        public string ID { get; set; }
       
        public string DocDate { get; set; }
        
        public string PEntrydt { get; set; }
        public string PNoteid { get; set; }
        public string Schedule { get; set; }
        public string PacNo { get; set; }
        public string Item { get; set; }
        public string TestReq { get; set; }
        public string drumnos { get; set; }
        public string Same { get; set; }
        public string Checked { get; set; }
        public string Remarks { get; set; }
        public string active { get; set; }
        
        public List<Packingitem> DrumLst { get; set; }
        public List<PackingGasitem> TimeLst { get; set; }

    }

    public class Packingitem
    {
        public string Drum { get; set; }
        public string Batch { get; set; }
        public string Com { get; set; }
        public string Result { get; set; }
        public string Isvalid { get; set; }
    }
    public class PackingGasitem
    {
        public string Time { get; set; }
        public string vol25 { get; set; }
        public string vol35 { get; set; }
        public string vol45 { get; set; }
        public string vol { get; set; }
        public string Isvalid { get; set; }
    }

}
