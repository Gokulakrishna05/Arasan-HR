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
            this.Planlst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Itemlst;

        public string Itemid { get; set; }
        public string saveitemid { get; set; }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Worklst;
        public string WorkCenter { get; set; }
        public string detid { get; set; }
        public string ttype { get; set; }
        public string WorkCenterid { get; set; }
        public string Days { get; set; }

        public List<SelectListItem> Processlst;
        public string Process { get; set; }
        public string Processid { get; set; }

        public List<SelectListItem> RecList;
        public string Enterd { get; set; }

        public List<SelectListItem> Planlst;
        public string Type { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ddlStatus { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string Schdate { get; set; }
        public string Formula { get; set; }
        public string Proddt { get; set; }
       
        public string Unit { get; set; }
        public double Qty { get; set; }
        public double ProdQty { get; set; }
        public string Exprunhrs { get; set; }
        public string Refno { get; set; }
        public string Amdno { get; set; }
        public string Entered { get; set; }
        public List<ProductionScheduleItem> PrsLst { get; set; }
        public List<ProductionItem> ProLst { get; set; }
        public List<ProItem> Prlst { get; set; }
        public List<ProScItem> ProscLst { get; set; }

        public List<ProSchItem> ProschedLst { get; set; }
    }
    public class ProductionScheduleItem
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public string ID { get; set; }
        public string Addict { get; set; }
        public string AddictPer { get; set; }
        public string Desc { get; set; }
        public string Unit { get; set; }

        public string Input { get; set; }
        public string Qty { get; set; }

        public string Isvalid { get; set; }
    }
    public class ProductionItem
    {
        public string ID { get; set; }
        public string Item { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> PItemlst { get; set; }

        public List<SelectListItem> PItemGrouplst { get; set; }

        public string ItemGroup { get; set; }

        public string Des { get; set; }
        public string Unit { get; set; }

        public string Output { get; set; }
        public string Alam { get; set; }
        public string OutputType { get; set; }
        public string Sch { get; set; }
        public string Produced { get; set; }
        public string Isvalid { get; set; }

    }
    public class ProItem
    {
        public string ID { get; set; }
        public string Isvalid { get; set; }
        public string Parameters { get; set; }
        public string Unit { get; set; }
        public string Initial { get; set; }
        public string Final { get; set; }
        public string Remarks { get; set; }
    }
    public class ProScItem
    {
        public string ID { get; set; }
        public string itemd { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> SItemlst { get; set; }

        public List<SelectListItem> SItemGrouplst { get; set; }

        public string ItemGrp { get; set; }
        public string schdate { get; set; }
        public string hrs { get; set; }

        public double qty { get; set; }
        public string Change { get; set; }
        public string isvalid { get; set; }

    }
    public class ProSchItem
    {
        public string ID { get; set; }
        public string Isvalid { get; set; }
        public string item { get; set; }
        public string Pack { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string Qty { get; set; }

    }

    public class ProdSchItem
    {
        public long id { get; set; }
        public string doc { get; set; }
        public string work { get; set; }
        public string item { get; set; }
        public string docDate { get; set; }
       
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
        public string Accrow { get; set; }
        //public string Status { get; set; }
        //public string Account { get; set; }
    }
}
