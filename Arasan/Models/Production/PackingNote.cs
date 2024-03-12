using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PackingNote
    {
        public PackingNote()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
            this.DrumLoclst = new List<SelectListItem>();
            this.ToLoclst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.Schlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Worklst;
        public List<SelectListItem> Itemlst;
        public List<SelectListItem> Schlst;
        public string ItemId { get; set; }
        public string WorkId { get; set; }
        public string toloc { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string DrumLoc { get; set; }
        public string ddlStatus { get; set; }
        public string ProdSchNo { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> RecList;
        public List<SelectListItem> DrumLoclst;
        public List<SelectListItem> ToLoclst;
        public string Enterd { get; set; }
        public string Remark { get; set; }
        public string LotNo { get; set; }
        public string PackYN { get; set; }
        public List<DrumDetail> DrumDetlst { get; set; }
       
    }
    public class DrumDetail
    {
        public string ID { get; set; }
        public string drum { get; set; }
        public string drumid { get; set; }
        public string soid { get; set; }
        public List<SelectListItem> DrumNolst { get; set; }

        public List<SelectListItem> Batchlst { get; set; }
        public string batch { get; set; }



        public string lotid { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
  

        public string qty { get; set; }


        public string comp { get; set; }
        //public string Output { get; set; }
        public string Isvalid { get; set; }
        //public List<SelectListItem> outputlst;

    }
    public class PackingListItem
    {
        public string id { get; set; }
        public string branch { get; set; }
        public string loc { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string doc { get; set; }
        public string item { get; set; }
        public string work { get; set; }
        public string viewrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
