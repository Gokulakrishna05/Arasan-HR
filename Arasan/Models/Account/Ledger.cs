using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class Ledger
    {
        public Ledger()
        {

            this.Typelst = new List<SelectListItem>();
            this.AccGrouplst = new List<SelectListItem>();

        }

        public string ID { get; set; }

        public List<SelectListItem> Typelst;
        public string AType { get; set; }
        public List<SelectListItem> AccGrouplst;
        public string AccGroup { get; set; }
        public string DisplayName { get; set; }


        public string LedName { get; set; }
        public string DocDate { get; set; }
        public string OpStock { get; set; }
        public string ClStock { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }
}
