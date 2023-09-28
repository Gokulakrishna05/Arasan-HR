using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class PromotionMail
    {
        public string ID { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Sub { get; set; }
        public string editors { get; set; }
        public string files { get; set; }
        //public HttpPostedFileBase Empsign { get; set; }
    }
}
