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
    public class CurrencyService : ICurrencyService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CurrencyService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        //public IEnumerable<Currency> GetAllCurrency(string status)
        //{
        //    List<Currency> cmpList = new List<Currency>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        if (string.IsNullOrEmpty(status))
        //        {
        //            status = "ACTIVE";
        //        }

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select SYMBOL,MAINCURR,CURRENCYID,STATUS from CURRENCY WHERE CURRENCY.STATUS='" + status + "' order by CURRENCY.CURRENCYID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                Currency cmp = new Currency
        //                {
        //                    ID = rdr["CURRENCYID"].ToString(),
        //                    CurrencyCode = rdr["SYMBOL"].ToString(),
        //                    CurrencyName = rdr["MAINCURR"].ToString(),
        //                    status = rdr["STATUS"].ToString()
        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}


        public Currency GetCurrencyById(string eid)
        {
            Currency currency = new Currency();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SYMBOL,MAINCURR,CURRENCYID from CURRENCY where CURRENCYID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Currency cmp = new Currency
                        {
                            ID = rdr["CURRENCYID"].ToString(),
                            CurrencyCode = rdr["SYMBOL"].ToString(),
                            CurrencyName = rdr["MAINCURR"].ToString()
                        };
                        currency = cmp;
                    }
                }
            }
            return currency;
        }

        public string CurrencyCRUD(Currency cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(SYMBOL) as cnt FROM CURRENCY WHERE  SYMBOL = LTRIM(RTRIM('" + cy.CurrencyCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = " Symbol Already Existed";
                        return msg;
                    }
                    
                }
               
               else
                    {
                        svSQL = " SELECT Count(MAINCURR) as cnt FROM CURRENCY WHERE MAINCURR = LTRIM(RTRIM('" + cy.CurrencyName + "')) ";
                        if (datatrans.GetDataId(svSQL) > 0)
                        {
                            msg = "Currency Already Existed";
                            return msg;
                        }

               }

               
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CURRENCYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "CURRENCYPROC";*/

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

                    objCmd.Parameters.Add("SYMBOL", OracleDbType.NVarchar2).Value = cy.CurrencyCode;
                    objCmd.Parameters.Add("MAINCURR", OracleDbType.NVarchar2).Value = cy.CurrencyName;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";
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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty; 
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CURRENCY SET IS_ACTIVE ='N' WHERE CURRENCYID='" + id + "'";
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
                    svSQL = "UPDATE CURRENCY SET IS_ACTIVE ='Y' WHERE CURRENCYID='" + id + "'";
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
        public DataTable GetAllCurrencygrid()
        {
            string SvSql = string.Empty;
            SvSql = " Select SYMBOL,MAINCURR,CURRENCYID from CURRENCY WHERE IS_ACTIVE = 'Y' ORDER BY CURRENCYID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
