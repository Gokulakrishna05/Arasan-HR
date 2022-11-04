using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;

namespace Arasan.Models
{

   // [SessionTimeout]
    public class LoginViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

       
            }
   
}
