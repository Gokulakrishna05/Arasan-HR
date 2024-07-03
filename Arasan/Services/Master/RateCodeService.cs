using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;
using System;
using Arasan.Interface.Master;

namespace Arasan.Services.Master
{
    public class RateCodeService : IRateCodeService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RateCodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        
        public string RateCodeCRUD(RateCode cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RATECODEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "CITYPROC";*/

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

                    objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = cy.Ratecode;
                    objCmd.Parameters.Add("RATEDESC", OracleDbType.NVarchar2).Value = cy.RateDsc;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
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
        public DataTable GetAllListRateCodeItem(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT RATECODEMASTID,RATECODE,RATEDESC,IS_ACTIVE FROM RATECODEMAST WHERE RATECODEMAST.IS_ACTIVE='Y' ORDER BY RATECODEMAST.RATECODEMASTID DESC";
            }
            else
            {
                SvSql = "SELECT RATECODEMASTID,RATECODE,RATEDESC,IS_ACTIVE FROM RATECODEMAST WHERE RATECODEMAST.IS_ACTIVE='N' ORDER BY RATECODEMAST.RATECODEMASTID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditRateCode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATECODE,RATEDESC FROM RATECODEMAST Where RATECODEMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RATECODEMAST SET IS_ACTIVE ='N' WHERE RATECODEMASTID='" + id + "'";
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
                    svSQL = "UPDATE RATECODEMAST SET IS_ACTIVE = 'Y' WHERE RATECODEMASTID='" + id + "'";
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
