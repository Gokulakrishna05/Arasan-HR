using System.Collections.Generic;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class IssueToProduction
    {
        public IssueToProduction()
        {
           
            this.Loc = new List<SelectListItem>();
            this.ToLoc = new List<SelectListItem>();
            
            this.Itemlst = new List<SelectListItem>();
           
           
        }
        public string ID { get; set; }

       
        public string Branch { get; set; }

      

        public List<SelectListItem> Loc;

        public String FromLoc { get; set; }

        public List<SelectListItem> ToLoc;

        public String Toloc { get; set; }

        public List<SelectListItem> Itemlst;

        public String Itemid { get; set; }

       


        public String Docid { get; set; }
        public String Docdate { get; set; }
        public String Unit { get; set; }
        public String Stock { get; set; }
       
        public Double Qty { get; set; }
        public String Purpose { get; set; }
        

        public List<IssueItem> Issuelst { get; set; }
    }
    public class IssueItem
    {
        public string ID { get; set; }
        public string item { get; set; }
        public string itemid { get; set; }

        

        public Double totalqty { get; set; }
       
        public string lotnoid { get; set; }
        public string lotno { get; set; }
        public Double qty { get; set; }
   
        public string Isvalid { get; set; }

    }
}

