using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class QcTemplate
    {
        public  QcTemplate()
        {
             this.assignList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string Qc { get; set; }
        public string Test { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Procedure { get; set; }
        public string Samplingper { get; set; }
        public string ddlStatus { get; set; }

        public string Level { get; set; }

        public List<SelectListItem> assignList;
        public string Set { get; set; }

        public List<QcTemplateItem> QcLst { get; set; }
    }
    public class QcTemplateItem
    {
        public string ID { get; set; }
        public string Isvalid { get; set; }

        public List<SelectListItem> Desclst { get; set; }

        public string ItemDesc { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Un { get; set; }
    }
    public class QcTemplateItemGrid
    {
        public string id { get; set; }
        public string Isvalid { get; set; }

        public List<SelectListItem> Desclst { get; set; }

        public string qc { get; set; }
        public string test { get; set; }
        public string description { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string un { get; set; }
        public string view { get; set; }
    }
}
