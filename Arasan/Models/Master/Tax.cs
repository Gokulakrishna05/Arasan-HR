using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Tax
    {
        public string ID { get; set; }

        public List<SelectListItem> Taxtypelst;
        public string Taxtype{ get; set; }

        public string Percentage { get; set; }

    }
}
