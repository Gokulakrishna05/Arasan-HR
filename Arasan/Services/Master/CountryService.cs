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
        //public IEnumerable<Country> GetAllCountry(/*string status*/)
        //{
        //    //if (string.IsNullOrEmpty(status))
        //    //{
        //    //    status = "ACTIVE";
        //    //}
        //    List<Country> cmpList = new List<Country>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select COUNTRY,COUNTRYCODE,COUNTRYMASTID,IS_ACTIVE from CONMAST order by CONMAST.COUNTRYMASTID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                Country cmp = new Country
        //                {
        //                    ID = rdr["COUNTRYMASTID"].ToString(),
        //                    ConName = rdr["COUNTRY"].ToString(),
        //                    ConCode = rdr["COUNTRYCODE"].ToString(),
        //                    status = rdr["IS_ACTIVE"].ToString()
        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}


        public Country GetCountryById(string eid)
        {
            Country country = new Country();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select COUNTRY,COUNTRYCODE,COUNTRYMASTID from CONMAST where COUNTRYMASTID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Country cmp = new Country 
                        {
                            ID = rdr["COUNTRYMASTID"].ToString(),
                            ConName = rdr["COUNTRY"].ToString(),
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

                    svSQL = " SELECT Count(COUNTRYCODE) as cnt FROM CONMAST WHERE COUNTRYCODE = LTRIM(RTRIM('" + cy.ConCode + "')) and COUNTRYCODE = LTRIM(RTRIM('" + cy.ConCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Country Code Already Existed";
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

                    objCmd.Parameters.Add("COUNTRY", OracleDbType.NVarchar2).Value = cy.ConName;
                    objCmd.Parameters.Add("COUNTRYCODE", OracleDbType.NVarchar2).Value = cy.ConCode;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                       
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
                    svSQL = "UPDATE CONMAST SET IS_ACTIVE ='N' WHERE COUNTRYMASTID='" + id + "'";
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

        } public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CONMAST SET IS_ACTIVE = 'Y' WHERE COUNTRYMASTID='" + id + "'";
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
        public DataTable GetAllCountryGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select IS_ACTIVE,COUNTRY,COUNTRYCODE,COUNTRYMASTID from CONMAST WHERE IS_ACTIVE = 'Y' ORDER BY COUNTRYMASTID DESC";
            }
            else
            {
                SvSql = " Select IS_ACTIVE,COUNTRY,COUNTRYCODE,COUNTRYMASTID from CONMAST WHERE IS_ACTIVE = 'N' ORDER BY COUNTRYMASTID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } 
    }
}
