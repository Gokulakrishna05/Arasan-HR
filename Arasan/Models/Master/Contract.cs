using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Contract
    {
        public string ID { get; set; }

        public string Contype { get; set; }
        public List<SelectListItem> conlst;

        public string Salpd { get; set; }
        public string Dkg { get; set; }
        public List<SelectListItem> kglst;
    }
    public class ContractList
    {
        public string contype { get; set; }
        public string salpd { get; set; }
        public string dkg { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }
}
