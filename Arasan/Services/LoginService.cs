using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace Arasan.Services
{
    public class LoginService : ILoginService
    {
        private readonly string _connectionString;
        DataTransactions _dtransactions;
        public LoginService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public bool LoginCheck(string username, string password)
        {
            List<LoginViewModel> LoginList = new List<LoginViewModel>();
            bool isValidUser = false;
            try
            {
                string _selUser = @"Select Username,password,eactive,empdept,EMPMASTID,IMGPATH from empmast where  eactive='" + username + "' and  username='"+ password + "' and password   ='Arasan'";
                DataTable _dtUser = new DataTable();
                _dtUser = _dtransactions.GetData(_selUser);
                if (_dtUser.Rows.Count > 0)
                {
                    
                    //HttpContext.Current.Session["UserId"] = _dtUser.Rows[0]["EMPMASTID"].ToString();
                    //HttpContext.Current.Session["UserName"] = _dtUser.Rows[0]["Username"].ToString();
                    //HttpContext.Current.Session["Department"] = _dtUser.Rows[0]["empdept"].ToString();
                    isValidUser = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //using (OracleConnection con = new OracleConnection(_connectionString))
            //{
            //    using (OracleCommand cmd = con.CreateCommand())
            //    {
            //        con.Open();
            //        cmd.CommandText = "Select Username,password,eactive,empdept from empmast where  eactive='Yes' and  username='balaji' and password   ='Arasan'";
            //        OracleDataReader rdr = cmd.ExecuteReader();

            //    }
            //}

            return isValidUser;
        }




    }
}
