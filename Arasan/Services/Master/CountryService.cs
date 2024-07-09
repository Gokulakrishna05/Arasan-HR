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
        public DataTable GetEditCountDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRYMASTID, COUNTRY,COUNTRYCODE,CURRENCY from CONMAST where COUNTRYMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPortDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PORTC,PORTN,PPINC,PORTS from CONPORTDET where CONMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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

                    svSQL = " SELECT Count(CONCODE) as cnt FROM CONMAST WHERE CONCODE = LTRIM(RTRIM('" + cy.ConCode + "')) and CONCODE = LTRIM(RTRIM('" + cy.ConCode + "'))";
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
                    objCmd.Parameters.Add("CURRENCY", OracleDbType.NVarchar2).Value = cy.Curr;
                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        if (cy.Curlst != null)
                    {
                        if (cy.ID == null)
                        {
                            foreach (CurItem cp in cy.Curlst)
                            {
                                if (cp.Isvalid == "Y" && cp.pcode != "0")
                                {
                                    svSQL = "Insert into CONPORTDET (CONMASTID,PORTC,PORTN,PPINC,PORTS) VALUES ('" + Pid + "','" + cp.pcode + "','" + cp.pnum + "','" + cp.ppin + "','" + cp.psta + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }
                        else
                        {
                            svSQL = "Delete CONPORTDET WHERE CONMASTID='" + cy.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                            foreach (CurItem cp in cy.Curlst)
                            {
                                if (cp.Isvalid == "Y" && cp.pcode != "0")
                                {
                                        svSQL = "Insert into CONPORTDET (CONMASTID,PORTC,PORTN,PPINC,PORTS) VALUES ('" + Pid + "','" + cp.pcode + "','" + cp.pnum + "','" + cp.ppin + "','" + cp.psta + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }

                    }
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

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CONMAST SET IS_ACTIVE ='N' WHERE CONMASTID='" + id + "'";
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

        } public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CONMAST SET IS_ACTIVE = 'Y' WHERE CONMASTID='" + id + "'";
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
                SvSql = " Select IS_ACTIVE,COUNTRY,CONCODE,CONMASTID from CONMAST WHERE IS_ACTIVE = 'Y' ORDER BY CONMASTID DESC";
            }
            else
            {
                SvSql = "  Select IS_ACTIVE,COUNTRY,CONCODE,CONMASTID from CONMAST WHERE IS_ACTIVE = 'N' ORDER BY CONMASTID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCur()
        {
            string SvSql = string.Empty;
            SvSql = "select CURRENCYID,MAINCURR from CURRENCY  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSta()
        {
            string SvSql = string.Empty;
            SvSql = "select STATEMASTID,STATE from STATEMAST  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
