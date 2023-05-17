using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class WorkCenters
    {
        public WorkCenters()
        {
            this.Loc = new List<SelectListItem>();
            this.QCLst = new List<SelectListItem>();
            this.WIPLst = new List<SelectListItem>();
            this.CONLst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.ConItemlst = new List<SelectListItem>();
            this.Drumlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Typelst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Typelst;
        public string Wid { get; set; }
        public string WType { get; set; }
        public string Docdate { get; set; }

        public List<SelectListItem> Loc;
        public string Iloc { get; set; }

        public List<SelectListItem> QCLst;
        public string QcLoc { get; set; }

        public List<SelectListItem> Suplst;
        public string Party { get; set; }

        public List<SelectListItem> Itemlst;
        public string WipItemid { get; set; }

        public string WipLocid { get; set; }

        public List<SelectListItem> WIPLst;
        public string ConvItem { get; set; }

        public List<SelectListItem> ConItemlst;

        public List<SelectListItem> CONLst;
        public string ConvLoc { get; set; }
        public string Bunker { get; set; }
        public string Opbbl { get; set; }
        public string Mill { get; set; }
        public string Opmlbal { get; set; }
        public string ProcLot { get; set; }
        public string ProdSch { get; set; }
        public string Uttl { get; set; }
        public string Production { get; set; }

        public List<SelectListItem> Drumlst;
        public string DrumLoc { get; set; }
        public string Energy { get; set; }
        public string Man { get; set; }
        public string Cap { get; set; }
        public string Cost { get; set; }
        public string Unit { get; set; }
        public string Remarks { get; set; }

        public List<WorkCentersDetail> WorkCenterlst { get; set; }

    }
    public class WorkCentersDetail
    {
        public string ID { get; set; }
        public string MId { get; set; }
        public string Isvalid { get; set; }
        public string MCost { get; set; }

    }
}
