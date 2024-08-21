 using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ActPlanning
    {
        public string Docno { get; set; }
        public string DocDate { get; set; }
        public string MtId { get; set; }
        public string AcTyp { get; set; }
        public string AcDat { get; set; }
        public string AcFre { get; set; }
        public string DeTyp { get; set; }
        public string MaTyp { get; set; }
        public string FrDat { get; set; }
        public string ToDat { get; set; }
        public string FrTim { get; set; }
        public string ToTim { get; set; }
        public string PB { get; set; }
        public string JTyp { get; set; }
        public string AlTo { get; set; }
        public string BrDow { get; set; }
        public string ADes { get; set; }

        public List<SelectListItem> McTolst { get; set; }
        public List<SelectListItem> ActTyplst { get; set; }
        public List<SelectListItem> DepTyplst { get; set; }
        public List<SelectListItem> MaiTyplst { get; set; }
        public List<SelectListItem> PBlst { get; set; }
        public List<SelectListItem> JTyplst { get; set; }
        public List<SelectListItem> AlTolst { get; set; }
        public List<SelectListItem> BrDowlst { get; set; }

        public string ddlStatus { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string ID { get; set; }

        public List<Promst> Prolst { get; set; }
        public List<Wcid> wclst { get; set; }

    }
    public class Promst
    {

        public string tools { get; set; }
        public List<SelectListItem> ulst { get; set; }
        public string Isvalid { get; set; }

    }
    public class Wcid
    {
        public string date { get; set; }
        public string rea { get; set; }
        public string Isvalid { get; set; }

        public string Isvalid1 { get; set; }

    }

    public class ListActPlanning
    {
        public string dnum { get; set; }
        public string ddate { get; set; }
        public string mtid { get; set; }
        public string actyp { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
