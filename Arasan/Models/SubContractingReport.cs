using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SubContractingReport
    {
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
    }

    public class SubContractItems
    {
        public string pid { get; set; }
        public string dno { get; set; }
        public string ddt { get; set; }
        public string rno { get; set; }
        public string rdt { get; set; }
        public string typ { get; set; }
        public string iid { get; set; }
        public string ides { get; set; }
        public string uid { get; set; }
        public string mqty { get; set; }
        public string brat { get; set; }
        public string icat { get; set; }
        public string lid { get; set; }
        public string net { get; set; }

    }
}
