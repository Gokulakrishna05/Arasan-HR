using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class EmailConfig
    {
        public String ID { get; set; }
        public String SMTP { get; set; }
        public String Port { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String SSL { get; set; }
        public String Signature { get; set; }
        public String status { get; set; }
    }
}
