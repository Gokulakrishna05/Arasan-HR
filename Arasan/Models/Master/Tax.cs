using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Tax
    {
        public string ID { get; set; }
        public string TaxType{ get; set; }

        public string TaxPercentage { get; set; }

    }
}
