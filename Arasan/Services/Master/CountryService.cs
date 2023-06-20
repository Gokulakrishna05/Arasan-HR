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
    public class CountryService : ICountryService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CountryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<Country> GetAllCountry(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }
            List<Country> cmpList = new List<Country>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select COUNTRYNAME,COUNTRYCODE,COUNTRYMASTID,STATUS from CONMAST WHERE STATUS = '" + status + "' order by CONMAST.COUNTRYMASTID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Country cmp = new Country
                        {
                            ID = rdr["COUNTRYMASTID"].ToString(),
                            ConName = rdr["COUNTRYNAME"].ToString(),
                            ConCode = rdr["COUNTRYCODE"].ToString(),
                            status = rdr["STATUS"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }


        public Country GetCountryById(string eid)
        {
            Country country = new Country();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select COUNTRYNAME,COUNTRYCODE,COUNTRYMASTID from CONMAST where COUNTRYMASTID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Country cmp = new Country
                        {
                            ID = rdr["COUNTRYMASTID"].ToString(),
                            ConName = rdr["COUNTRYNAME"].ToString(),
                            ConCode = rdr["COUNTRYCODE"].ToString()
                        };
                        country = cmp;
                    }
                }
            }
            return country;
        }

        public string CountryCRUD(Country cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;

                string svSQL = "";

                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM CONMAST WHERE COUNTRYNAME = LTRIM(RTRIM('" + cy.ConName + "')) and COUNTRYCODE = LTRIM(RTRIM('" + cy.ConCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Country Already Existed";
                        return msg;
                    }
                }
                else
                {
                    svSQL = " SELECT Count(*) as cnt FROM CONMAST WHERE COUNTRYNAME = LTRIM(RTRIM('" + cy.ConName + "')) and COUNTRYCODE = LTRIM(RTRIM('" + cy.ConCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Country Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("COUNTRYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "COUNTRYPROC";*/

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

                    objCmd.Parameters.Add("ConName", OracleDbType.NVarchar2).Value = cy.ConName;
                    objCmd.Parameters.Add("ConCode", OracleDbType.NVarchar2).Value = cy.ConCode;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";

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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CONMAST SET STATUS ='INACTIVE' WHERE COUNTRYMASTID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }

    }
}
