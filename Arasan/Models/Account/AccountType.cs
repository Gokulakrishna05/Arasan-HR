using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class AccountType
    {
        public AccountType()
        {

            this.ATypelst = new List<SelectListItem>();

        }

        public string Accountclass { get; set; }
        public List<SelectListItem> ATypelst;

        public string ID { get; set; }

        public string AccountCode { get; set; }
        public string AccCode { get; set; }

        public string Accounttype { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public string Status { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }

    }
    
    
    public class AType
    {
        public string accountclass { get; set; }
        public string id { get; set; }
        public string accountCode { get; set; }
        public string accounttype { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
