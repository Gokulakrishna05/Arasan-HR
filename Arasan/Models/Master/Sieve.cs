using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Models
{
    public class Sieve
    {
        public string ID { get; set; }
        public string Svalue { get; set; }
        public string Evalue { get; set; }
        public string SID { get; set; }
        public string createby { get; set; }
        public string ddlStatus { get; set; }

    } 
    
    public class Sievegrid
    {
        public string id { get; set; }
        public string svalue { get; set; }
        public string evalue { get; set; }
        public string sid { get; set; }
        public string viewrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
