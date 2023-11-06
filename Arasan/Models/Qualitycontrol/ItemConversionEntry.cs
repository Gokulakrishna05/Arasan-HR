using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemConversionEntry
    {
        public ItemConversionEntry()
        {
           
            this.Loc = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.TItemlst = new List<SelectListItem>();
            this.Entlst = new List<SelectListItem>();
            this.Approvelst = new List<SelectListItem>();
        }
        public List<SelectListItem> Loc;
        public List<SelectListItem> Itemlst;
        public List<SelectListItem> TItemlst;
        public List<SelectListItem> Entlst;
        public List<SelectListItem> Approvelst;

        public string Location { get; set; }
        public string Branch { get; set; }
        public string ID { get; set; }
        public string Docid { get; set; }
        public string DocDate { get; set; }
        public string Stock { get; set; }
        public string Item { get; set; }
        public string ToItem { get; set; }
        public string Unit { get; set; }
        public string Total { get; set; }
        public string Purpose { get; set; }
        public string Entered { get; set; }
        public string Approved { get; set; }
        public string Remarks { get; set; }
        public string TotAmount { get; set; }



        public List<ConEntryItem> ICEILst { get; set; }
    }
    public class ConEntryItem
    {
        public string Isvalid { get; set; } 
        public string drum { get; set; } 
        public string qty { get; set; } 
        public string sid { get; set; } 
        public string batchno { get; set; } 
        public string binid { get; set; } 
        public string rate { get; set; } 
        public string total { get; set; } 
        

    }
}
