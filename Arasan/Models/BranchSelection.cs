using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class BranchSelection
    {
        public BranchSelection()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }
        public string Branch { get; set; }
        public string Location { get; set; }
        public string User { get; set; }
        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loclst;

    }
}
