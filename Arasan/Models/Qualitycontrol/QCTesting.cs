using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class QCTesting
    {
        public  QCTesting()
        {
         this.Typlst = new List<SelectListItem>();
            this.lst = new List<SelectListItem>();
        }
    public List<SelectListItem> Typlst;
        public string Id { get; set; }
        public string Type { get; set; }
        public List<SelectListItem> lst;
        public string POGRN { get; set; }
        public string GRN { get; set; }
    }

}