using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StockStatementDrumwiseModel
    {
        public StockStatementDrumwiseModel()
        {
            this.SNlst = new List<SelectListItem>();
            this.Locationlst = new List<SelectListItem>();

        }


        public string ID { get; set; }
            public string dtFrom { get; set; }
            public string dtTo { get; set; }
            public string SN { get; set; }
        public List<SelectListItem> SNlst;
        public List<SelectListItem> Locationlst;
        public string Location { get; set; }
        public string WEDyn { get; set; }
    }
        public class StockStatementDrumwiseModelItems
    {
            public string drummastid { get; set; }
            public string drumno { get; set; }
            public string itemid { get; set; }
            public string batchno { get; set; }
            public string qty { get; set; }
            public string rate { get; set; }
            public string locid { get; set; }
            public string completed { get; set; }
            public string inspection { get; set; }
            public string curinginward { get; set; }
            public string curinges { get; set; }
            public string curingoutward { get; set; }
            public string recharge { get; set; }
            public string ncrelease { get; set; }
            public string packing { get; set; }
            public string packings { get; set; }
            public string docdate { get; set; }
            public string subcategory { get; set; }
            public string batch { get; set; }
            public string fidrms { get; set; }
            public string curdays { get; set; }
            public string idle { get; set; }
            public string igroup { get; set; }
            public string status { get; set; }

        }
       
    }


