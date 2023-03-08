using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class ProductionSchedule
    {
        public ProductionSchedule()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();

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
        public string Type { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Schdate { get; set; }
        public string Formula { get; set; }
        public string Proddt { get; set; }
        public string Itemid { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Exprunhrs { get; set; }
        public string Refno { get; set; }
        public string Amdno { get; set; }
        public string Entered { get; set; }

        public List<ProductionScheduleItem> PrsLst { get; set; }
    }
    public class ProductionScheduleItem
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public string Desc { get; set; }
        public string Unit { get; set; }

        public string Input { get; set; }
        public string Qty { get; set; }

        public string Isvalid { get; set; }
    }
    //public class ProductionItem
    //{

    //}
}
