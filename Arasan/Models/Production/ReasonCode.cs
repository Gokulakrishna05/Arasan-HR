using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models

{
    public class ReasonCode
    {
        public ReasonCode()
        {
            this.assignList = new List<SelectListItem>();
            //this.Categorylst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> assignList;
        public string ModBy { get; set; }

        //public List<SelectListItem> Categorylst;

        //public string CategoryType { get; set; }
        //public string PartyCategory { get; set; }
        public List<ReasonItem> ReLst;
    }
    public class ReasonItem
    {
        public string Reason { get; set; }

        //public string Type { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
        public string Isvalid { get; set; }

        public List<SelectListItem> Categorylst { get; set; }

        public string Category { get; set; }
    }
}
