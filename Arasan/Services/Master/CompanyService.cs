using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class CompanyService : ICompanyService
    {
        private readonly string _connectionString;
        public CompanyService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
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
                            ID = rdr["COMPANYMASTID"].ToString(),
                            CompanyId = rdr["COMPANYID"].ToString(),
                            CompanyName = rdr["COMPANYDESC"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }


        public Company GetCompanyById(string eid)
        {
            Company company = new Company();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select COMPANYID,COMPANYDESC,COMPANYMASTID from COMPANYMAST where COMPANYMASTID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Company cmp = new Company
                        {
                            ID = rdr["COMPANYMASTID"].ToString(),
                            CompanyId = rdr["COMPANYID"].ToString(),
                            CompanyName = rdr["COMPANYDESC"].ToString()
                        };
                        company = cmp;
                    }
                }
            }
            return company;
        }

        public string CompanyCRUD(Company cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; 
                //string svSQL = "";
                
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("COMPANYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "COMPANYPROC";*/
                    
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    }
                    
                    objCmd.Parameters.Add("Company", OracleDbType.NVarchar2).Value = cy.CompanyId;
                    objCmd.Parameters.Add("CompanyName", OracleDbType.NVarchar2).Value = cy.CompanyName;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception ex)
                    { 
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }



    }
}