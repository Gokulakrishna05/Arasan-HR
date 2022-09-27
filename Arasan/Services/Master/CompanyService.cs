using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services.Master
{
    public class CompanyService : ICompanyService
    {
        private readonly string _connectionString;
        public CompanyService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Company> GetAllCompany()
        {
            List<Company> cmpList = new List<Company>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select COMPANYID,COMPANYDESC,COMPANYMASTID from COMPANYMAST";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Company cmp = new Company
                        {
                            //Id = Convert.ToInt32(rdr["Id"]),
                            CompanyId = rdr["COMPANYID"].ToString(),
                            CompanyName = rdr["COMPANYDESC"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
    }
}
