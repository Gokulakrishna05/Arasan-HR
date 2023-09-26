using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class AccDescrip
    {
        public string ID { get; set; }
        public string TransName { get; set; }
        public string TransID { get; set; }
        public string Scheme { get; set; }
        public string Descrip { get; set; }
        public string Status { get; set; }

    }
}
