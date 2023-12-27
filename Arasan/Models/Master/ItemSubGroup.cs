using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace Arasan.Models
{
    public class ItemSubGroup
    {
        public ItemSubGroup()
        {
            this.catlst = new List<SelectListItem>();
            this.grplst = new List<SelectListItem>();
        }

        public List<SelectListItem> catlst;
        public string ItemCat { get; set; }

        public List<SelectListItem> grplst;
        public string Itemgrp { get; set; }
        public string createby { get; set; }
        public string ID { get; set; }

        public string itemSubGroup { get; set; }
        public string Descreption { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }
    }
    
    public class ItemSubGrid
    {
        public string id { get; set; }

        public string itemsubgroup { get; set; }
        public string descreption { get; set; }
        public string status { get; set; }

        public String editrow { get; set; }
        public String delrow { get; set; }

        public string itemcat { get; set; }
        public string itemgrp { get; set; }
    }
}
