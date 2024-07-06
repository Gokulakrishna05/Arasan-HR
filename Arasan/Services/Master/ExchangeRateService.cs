using Arasan.Interface;
using Arasan.Models;
using System.Data;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services
{
    public class ExchangeRateService : IExchangeRateService
    {

        private readonly string _connectionString;
        DataTransactions datatrans;
        public ExchangeRateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CRATE SET IS_ACTIVE ='N' WHERE CRATEID='" + id + "'";
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
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CRATE SET IS_ACTIVE = 'Y' WHERE CRATEID='" + id + "'";
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
        public DataTable GetAllExchangeGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select IS_ACTIVE,CRATEID,CURRID,CURRNAME,EXRATE,RTYPE,RATETD from CRATE WHERE IS_ACTIVE = 'Y' ";
            }
            else
            {
                SvSql = " Select IS_ACTIVE,CRATEID,CURRID,CURRNAME,EXRATE,RTYPE,RATETD from CRATE WHERE IS_ACTIVE = 'N' ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSym()
        {
            string SvSql = string.Empty;
            SvSql = "select CURRENCYID,SYMBOL from CURRENCY  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditExchangeDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CRATEID,CURRID,CURRNAME,EXRATE,RTYPE,RATETD from CRATE where CRATEID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ExchangeRateCRUD(ExchangeRate cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;

                string svSQL = "";

                if (cy.ID == null)
                {

                    //svSQL = " SELECT Count(COUNTRYCODE) as cnt FROM CONMAST WHERE COUNTRYCODE = LTRIM(RTRIM('" + cy.ConCode + "')) and COUNTRYCODE = LTRIM(RTRIM('" + cy.ConCode + "'))";
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

                    //objCmd.Parameters.Add("COUNTRY", OracleDbType.NVarchar2).Value = cy.ConName;
                    //objCmd.Parameters.Add("COUNTRYCODE", OracleDbType.NVarchar2).Value = cy.ConCode;
                    //objCmd.Parameters.Add("CURRENCY", OracleDbType.NVarchar2).Value = cy.Curr;
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
                        //if (cy.Curlst != null)
                        {
                            if (cy.ID == null)
                            {
                                //foreach (CurItem cp in cy.Curlst)
                                {
                                    //if (cp.Isvalid == "Y" && cp.pcode != "0")
                                    //{
                                    //    svSQL = "Insert into CONPORTDET (CONMASTID,PORTC,PORTN,PPINC,PORTS) VALUES ('" + Pid + "','" + cp.pcode + "','" + cp.pnum + "','" + cp.ppin + "','" + cp.psta + "')";
                                    //    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    //    objCmds.ExecuteNonQuery();

                                    //}
                                }
                            }
                            else
                            {
                                svSQL = "Delete CONPORTDET WHERE CONMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                //foreach (CurItem cp in cy.Curlst)
                                {
                                    //if (cp.Isvalid == "Y" && cp.pcode != "0")
                                    //{
                                    //    svSQL = "Insert into CONPORTDET (CONMASTID,PORTC,PORTN,PPINC,PORTS) VALUES ('" + Pid + "','" + cp.pcode + "','" + cp.pnum + "','" + cp.ppin + "','" + cp.psta + "')";
                                    //    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    //    objCmds.ExecuteNonQuery();

                                    //}
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
    }
}
