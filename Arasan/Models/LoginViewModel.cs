using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;

namespace Arasan.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        //public int LoginCheck(LoginViewModel ad)
        //{
           
           
        //    return 1;
        //}
    }
   
}
