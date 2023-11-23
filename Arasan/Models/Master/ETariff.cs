using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class ETariff
    {
        public string ID { get; set; }
        public string Tariff { get; set; }
        public string Tariffdes { get; set; }
        public string Sgst { get; set; }
        public string Cgst { get; set; }
        public string Igst { get; set; }
        public string ddlStatus { get; set; }
    } 
    
    public class ETariffgrid
    {
        public string id { get; set; }
        public string tariff { get; set; }
        public string tariffdes { get; set; }
        public string sgst { get; set; }
        public string cgst { get; set; }
        public string igst { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
