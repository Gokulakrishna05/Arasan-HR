using System.Collections.Generic;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DrumIssueEntry
    {
        public DrumIssueEntry()
        {
            this.Brlst = new List<SelectListItem>();
            this.Bin = new List<SelectListItem>();
            this.ToBin = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();
            this.ToLoc = new List<SelectListItem>();
            this.Type = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.Applst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> Bin;
        public String Frombin { get; set; }

        public List<SelectListItem> ToBin;
        public String Tobin { get; set; }

        public List<SelectListItem> Loc;

        public String FromLoc { get; set; }

        public List<SelectListItem> ToLoc;

        public String Toloc { get; set; }

        public List<SelectListItem> Itemlst;

        public String Itemid { get; set; }

        public List<SelectListItem> Type;
        public String type { get; set; }


        public String Docid { get; set; }
        public String Docdate { get; set; }
        public String Unit { get; set; }
        public String Stock { get; set; }
        public String Drum { get; set; }

        public List<SelectListItem> RecList;
        public String Entered { get; set; }

        public List<SelectListItem> Applst;
        public String Approved { get; set; }
        public Double Qty { get; set; }
        public String Purpose { get; set; }
        public Double IRate { get; set; }
        public Double IValue { get; set; }

        public List<DrumIssueEntryItem> Drumlst { get; set; }
    }
    public class DrumIssueEntryItem
    {
        public string ID { get; set; }
      
        public List<SelectListItem> FBinlst { get; set; }
        public string FBinId { get; set; }
        public List<SelectListItem> TBinlst { get; set; }
        public string TBinid { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string Drum { get; set; }

        public string Qty { get; set; }
        public string Batch { get; set; }
        public Double Rate { get; set; }
        public Double Amount { get; set; }
        public string Isvalid { get; set; }

    }
}
