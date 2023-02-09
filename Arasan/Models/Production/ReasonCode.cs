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
        public string Intred { get; set; }

        //public List<SelectListItem> Categorylst;

        //public string CategoryType { get; set; }
        //public string PartyCategory { get; set; }

    }
    //public class ReasonItem
    //{

    //    public string Reason { get; set; }

    //    public string Isvalid { get; set; }
    //}
}
