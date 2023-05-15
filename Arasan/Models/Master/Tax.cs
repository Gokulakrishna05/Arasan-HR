using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Tax
    {
        public string ID { get; set; }
        public string Taxtype{ get; set; }

        public string Percentage { get; set; }

    }
}
