using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SubContractingMonthwiseReport
    {
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }

    public class SubContracMonthwisetItems
    {
        public string pid { get; set; }
        public string iid { get; set; }
        public string uid { get; set; }
        public string jan { get; set; }
        public string feb { get; set; }
        public string mar { get; set; }
        public string apr { get; set; }
        public string may { get; set; }
        public string jun { get; set; }
        public string jul { get; set; }
        public string aug { get; set; }
        public string sep { get; set; }
        public string oct { get; set; }
        public string nov { get; set; }
        public string dec { get; set; }
        public string tot { get; set; }
    }
}
