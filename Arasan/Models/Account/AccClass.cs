using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class AccClass
    {

        //public AccClass()
        //{
            
        //    this.ATypelst = new List<SelectListItem>();

        //}

        public string AccountCode { get; set; }

        //public List<SelectListItem> ATypelst;
        public string ID { get; set; }


        public string Accounttype { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public string status { get; set; }
        public string currval { get; set; }
       
    }
}
