using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class ProcessMast
    {
        public ProcessMast()
        {
            this.Prodhrtypelst = new List<SelectListItem>();
            this.Costtypelst = new List<SelectListItem>();
        }

        public List<SelectListItem> Prodhrtypelst { get; set; }
        public List<SelectListItem> Costtypelst { get; set; }

        public string Prodhrtype { get; set; }
        public string Costtype { get; set; }

        public string Branch { get; set; }
        public string ID { get; set; }

        public string ProcessMastName { get; set; }

        public string Batch { get; set; }
        public string Qc { get; set; }
        public string Sno { get; set; }

        public string ddlStatus { get; set; }



    }
    public class ProcessMastItem
    {

        public string processmastid { get; set; }
        public string processmastname { get; set; }
        public string prodhrtype { get; set; }
        public string batchoravg { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
