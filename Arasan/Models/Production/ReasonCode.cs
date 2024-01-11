using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;


namespace Arasan.Models

{
    public class ReasonCode
    {
        public ReasonCode()
        {
            this.assignList = new List<SelectListItem>();
            this.Proclst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> assignList;
        public List<SelectListItem> Proclst;
        public string Process { get; set; }
        public string ddlStatus { get; set; }
       
        //public List<SelectListItem> Categorylst;

        //public string CategoryType { get; set; }
        //public string PartyCategory { get; set; }
        public List<ReasonItem> ReLst { get; set; }
    }
    public class ReasonItem
    {
        public string Reason { get; set; }

        //public string Type { get; set; }
        public string Description { get; set; }

        public List<SelectListItem> Grouplst { get; set; }
        public string GroupId { get; set; }
        public string Isvalid { get; set; }

        public List<SelectListItem> Categorylst { get; set; }

        public string Category { get; set; }
        public string saveCategory { get; set; }
    }

    public class Reasongrid
    {
        public string id { get; set; }
        public string process { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
    }
