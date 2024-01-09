using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Models
{
    public class Stop
    {

        public string ID { get; set; }
        public string SID { get; set; }
        public string SDESC { get; set; }
        public string createby { get; set; }
        public string ddlStatus { get; set; }
    }
    public class Stopgrid
    {

        public string id { get; set; }
        public string sid { get; set; }
        public string sdesc { get; set; }
        public string createby { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string viewrow { get; set; }
    }
}


