using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class BatchProductionSummaryReportBatchwiseModel
    {

        public string ID { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }
    public class BatchProductionSummaryReportBatchwiseModelItems
    {
        public string batch { get; set; }
        public string etype { get; set; }
        public string wcid { get; set; }
        public string processid { get; set; }
        public string seq { get; set; }
        public string itemid { get; set; }
        public string unitid { get; set; }
        public string qty { get; set; }
        public string wipqty { get; set; }
        public string docdate { get; set; }
        public string mtono { get; set; }
        public string bitem { get; set; }



    }




}

