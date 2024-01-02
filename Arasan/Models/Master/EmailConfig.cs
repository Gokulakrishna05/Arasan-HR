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
        public String ddlStatus { get; set; }
        public String createby { get; set; }
        public object Attachment { get; internal set; }
    } 
    
    public class EmailConfigGrid
    {
        public String id { get; set; }
        public String smtp { get; set; }
        public String port { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public String ssl { get; set; }
        public String signature { get; set; }
        public String editrow { get; set; }
        public object delrow { get; internal set; }
    }
}
