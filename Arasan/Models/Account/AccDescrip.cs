using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class AccDescrip
    {
        public string ID { get; set; }
        public string Branch { get; set; }
        public string TransactionName { get; set; }
        public string TransactionID { get; set; }
        public string SchemeName { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string Createdby { get; set; }
        public string CreatedOn { get; set; }
        public string CurrDate { get; set; }

    }
}
