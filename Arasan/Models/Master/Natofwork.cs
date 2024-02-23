using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class Natofwork
    {
        public Natofwork()
        {


        }

        public string Branch { get; set; }
        public string ID { get; set; }

        public string Natofworkname { get; set; }
        public string ddlStatus { get; set; }



    }

    public class NatofworkItem
    {
        public string natofwork { get; set; }
        public string id { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
