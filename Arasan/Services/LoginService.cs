using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services
{
    public class LoginService : ILoginService
    {
        private readonly string _connectionString;
        public LoginService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public int LoginCheck(string username, string password)
        {
            List<LoginViewModel> LoginList = new List<LoginViewModel>();
            //using (OracleConnection con = new OracleConnection(_connectionString))
            //{
            //    using (OracleCommand cmd = con.CreateCommand())
            //    {
            //        con.Open();
            //        cmd.CommandText = "SELECT LoginID,ADDRESS1,ADDRESS2,ADDRESS3 FROM LoginMAST";
            //        OracleDataReader rdr = cmd.ExecuteReader();
                    
            //    }
            //}
            if(username == "admin" && password=="admin")
            {
                return 1;
            }
            else
            {
                return 0;
            }
            
        }




    }
}
