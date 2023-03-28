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
            this.RecList = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Worklst;
        public List<SelectListItem> Itemlst;
        public string ItemId { get; set; }
        public string WorkId { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string DrumLoc { get; set; }
        public string ProdSchNo { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> RecList;
        public List<SelectListItem> DrumLoclst;
        public string Enterd { get; set; }
        public string Remark { get; set; }
        public string LotNo { get; set; }
        public string PackYN { get; set; }
        public List<DrumDetail> DrumDetlst { get; set; }

    }
    public class DrumDetail
    {
        public string ID { get; set; }
        public string DrumNo { get; set; }
        public List<SelectListItem> DrumNolst { get; set; }

        public List<SelectListItem> Batchlst { get; set; }
        public string BatchNo { get; set; }
  
        public string BatchQty { get; set; }
             
        
        public string Comp { get; set; }
        //public string Output { get; set; }
        public string Isvalid { get; set; }
        //public List<SelectListItem> outputlst;

    }
}
