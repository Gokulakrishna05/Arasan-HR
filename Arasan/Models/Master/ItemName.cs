
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemName
    {
        public ItemName()
        {
            this.IgLst = new List<SelectListItem>();
            this.Iclst = new List<SelectListItem>();
            this.Isglst = new List<SelectListItem>();
            
        }
        public List<SelectListItem> IgLst;
        public string ItemGroup { get; set; }
        public string ItemCategory { get; set; }

        public List<SelectListItem> Iclst;
        public string ItemSubGroup { get; set; }

        public List<SelectListItem> Isglst;


    }

}
