using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models 
{
    public class CuringOutward
    {
        public CuringOutward()
        {
            this.Brlst = new List<SelectListItem>();
            this.FromWorklst = new List<SelectListItem>();
            this.ToWorklst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
            this.DrumLoclst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.Notelst = new List<SelectListItem>();
        }
        public string Branch { get; set; }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> FromWorklst;
        public List<SelectListItem> ToWorklst;
        public List<SelectListItem> Itemlst;
        public List<SelectListItem> Notelst;
        public string ItemId { get; set; }
        public string FromWork { get; set; }
        public string ToWork { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ddlStatus { get; set; }
        public string PackingNote { get; set; }
        public string ProdSchNo { get; set; }
        public string FRate { get; set; }
        public string enddate { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> RecList;
        public List<SelectListItem> DrumLoclst;
        public string Enterd { get; set; }
        public string Remark { get; set; }
        public string TotalQty { get; set; }
        public string TotalValue { get; set; }
        //public List<CuringDrumDetail> DrumDetlst { get; set; }
        public List<CuringDetail> Curinglst { get; set; }

    }
    //public class CuringDrumDetail
    //{
    //    public string ID { get; set; }
    //    public string DrumNo { get; set; }
    //    public List<SelectListItem> DrumNolst { get; set; }

    //    public List<SelectListItem> Batchlst { get; set; }
    //    public string BatchNo { get; set; }

    //    public string BatchQty { get; set; }


    //    public string Comp { get; set; }
    //    public string Shed { get; set; }
    //    public string Isvalid { get; set; }
    //    //public List<SelectListItem> outputlst;

    //}
    public class CuringDetail
    {
        public string ID { get; set; }
        public string drum { get; set; }
        public string drumid { get; set; }
        
        public string batch  { get; set; }

        public string qty { get; set; }


        public string comp { get; set; }
        public string shed { get; set; }
        public string Isvalid { get; set; }
        //public List<SelectListItem> outputlst;

    }

    public class CuringOutwardListItem
    {
        public string id { get; set; }
        public string branch { get; set; }
        public string docId { get; set; }
        public string docdate { get; set; }
        public string itemId { get; set; }
        public string shi { get; set; }
        public string viewrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
