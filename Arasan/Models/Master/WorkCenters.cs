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
            this.Contlst = new List<SelectListItem>();
        }
        public string ContType { get; set; }

        public List<SelectListItem> Contlst;
        public string ID { get; set; }

        public List<SelectListItem> Typelst;
        public string Wid { get; set; }
        public string status { get; set; }
        public string WType { get; set; }
        public string Docdate { get; set; }

        public List<SelectListItem> Loc;
        public string Iloc { get; set; }
        public string rloc { get; set; }
        public string rjloc { get; set; }

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
        public string ddlStatus { get; set; }
        public string createby { get; set; }

        public List<WorkCentersDetail> WorkCenterlst { get; set; }
        public List<ProdRate> ProdRatelst { get; set; }
        public List<Rejdet> Rejdetlst { get; set; }
        public List<ProdCap> ProdCaplst { get; set; }
        public List<ProdCapPerDay> ProdCapPerDaylst { get; set; }
        public List<ApSive> ApSivelst { get; set; }
        public List<PasteRun> PasteRunlst { get; set; }

    }
    public class WorkCentersDetail
    {
        public string ID { get; set; }
        public string MId { get; set; }
        public string Isvalid { get; set; }
        public string MCost { get; set; }
        public List<SelectListItem> mlst;

    }
    public class ProdRate
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public string Isvalid { get; set; }
        public string inputtype { get; set; }
        public string outputrate { get; set; }
        public List<SelectListItem> itemlst;
        public List<SelectListItem> inputlst;

    }
    public class PasteRun
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public string Isvalid { get; set; }
        public string runhrs { get; set; }
        public string mtoloss { get; set; }
        public string appowder { get; set; }
        public string cake { get; set; }
        public string noofchange { get; set; }
        public List<SelectListItem> itemlst;
 
    }
    public class ApSive
    {
        public string ID { get; set; }
        public string siveid { get; set; }
        public string Isvalid { get; set; }
        public string fuelqty { get; set; }
        public string mettqty { get; set; }
        public string minsive { get; set; }
       
        public List<SelectListItem> sivelst;
       

    }
    public class ProdCap
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public string Isvalid { get; set; }
        public string process { get; set; }
        public string outputcap { get; set; }
        public List<SelectListItem> itemlst;
        public List<SelectListItem> prolst;

    }
    public class Rejdet
    {
        public string ID { get; set; }
        public string rejtype { get; set; }
        public string Isvalid { get; set; }
        public string rejection { get; set; }
       
        public List<SelectListItem> rejlst;

    }

    public class ProdCapPerDay
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public string Isvalid { get; set; }
        public string Qty { get; set; }
        
        public List<SelectListItem> itemlst;
       

    }
    public class WorkCentersgrid
    {
        public string id { get; set; }
        public string wid { get; set; }
        public string wtype { get; set; }
        public string iloc { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string productionrate { get; set; }
        public string rejdet { get; set; }
        public string prodcap { get; set; }
        public string apsive { get; set; }
        public string prodcapday { get; set; }
        public string paste { get; set; }
    }

    }
