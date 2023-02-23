using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class BatchCreation
    {
        public BatchCreation()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkCenter { get; set; }
        public List<SelectListItem> Processlst;
        public string Process { get; set; }
        public string DocDate { get; set; }
        public string Seq { get; set; }
    }
}
