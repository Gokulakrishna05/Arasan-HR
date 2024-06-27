using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class ItemGroup
    {
        public  ItemGroup()
        {
           this.catlst = new List<SelectListItem>();
        }
        public string ID { get; set; }
       
        public string ItemGroups { get; set; }
        public string Type { get; set; }
        public string createby { get; set; }

        public List<SelectListItem> catlst;
        public string ItemCat { get; set; }
        public string ItemGroupDescription { get; set; } 
        public String status { get; set; }
        public string ddlStatus { get; set; }
        public List<subgrp> Sublst { get; set; }

    }
    public class ItemGroupGrid
    {
        public string id { get; set; }
       
        public string itemcat { get; set; }
        public string itemgroup { get; set; }
        public string itemgroupdescription { get; set; }
        public string type { get; set; }
        
        public String editrow { get; set; }
        public String delrow { get; set; }

    }
    public class subgrp
    {

        public string consyn { get; set; }
        public string subgrpname { get; set; }
        public string subgrpdecs { get; set; }
         public string Isvalid { get; set; }
        

    }
}
