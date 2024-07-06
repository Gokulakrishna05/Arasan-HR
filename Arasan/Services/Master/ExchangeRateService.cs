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
                SvSql = " Select IS_ACTIVE,CRATEID,CURRID,CURRNAME,EXRATE,RTYPE,to_char(RATEDT,'dd-MON-yyyy')RATEDT from CRATE WHERE IS_ACTIVE = 'Y' ";
            }
            else
            {
                SvSql = " Select IS_ACTIVE,CRATEID,CURRID,CURRNAME,EXRATE,RTYPE,to_char(RATEDT,'dd-MON-yyyy')RATEDT from CRATE WHERE IS_ACTIVE = 'N' ";

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
            SvSql = "select CRATEID,CURRID,CURRNAME,EXRATE,RTYPE,to_char(RATEDT,'dd-MON-yyyy')RATEDT from CRATE where CRATEID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ExchangeRateCRUD(ExchangeRate cp)
        {
            string msg = "";
            string bsid = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cp.ID == null)
                    {


                        svSQL = "Insert into CRATE (CURRID,CURRENCYID,CURRNAME,EXRATE,RTYPE,RATEDT) VALUES ('" + cp.CurrencySymbol + "','" + cp.CurrencySymbol + "','" + cp.CurrencyName + "','" + cp.Exchange + "','" + cp.RateType + "','" + cp.ExchangeDate + "') ";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                    }
                    else
                    {
                        svSQL = "UPDATE  CRATE set CURRID='" + cp.CurrencySymbol + "',CURRENCYID='" + cp.CurrencySymbol + "',CURRNAME='" + cp.CurrencyName + "',EXRATE='" + cp.Exchange + "',RTYPE='" + cp.RateType + "',RATEDT='" + cp.ExchangeDate + "' Where CRATEID='" + cp.ID + "'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
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
