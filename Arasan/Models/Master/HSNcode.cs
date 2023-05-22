using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class HSNcode
    {


        public string ID { get; set; }

        public string HCode { get; set; }
        public string Dec { get; set; }

        public List<SelectListItem> GSTlst;
        public string Gt { get; set; }
    }
}
