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
                    objCmd.Parameters.Add("CURREP6", OracleDbType.NVarchar2).Value = cy.CurrencyCodes;
                    objCmd.Parameters.Add("CURWIDTH", OracleDbType.NVarchar2).Value = cy.CurrencyInteger;
                    objCmd.Parameters.Add("MAINCURR", OracleDbType.NVarchar2).Value = cy.CurrencyName;


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

                        if (cy.Currencylst != null)
                        {
                            if (cy.ID == null)
                            {
                                int r = 1;
                                foreach (UsedCountries cp in cy.Currencylst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into CONCURR (CURRENCYID,CONCURRROW,CONCODE,COUNTRY) VALUES ('" + Pid + "','" + r + "','" + cp.ConCode + "','" + cp.Country + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                    r++;
                                }

                            }
                            else
                            {
                                svSQL = "Delete CONCURR WHERE CURRENCYID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();

                                foreach (UsedCountries cp in cy.Currencylst)
                                {
                                    int r = 1;
                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into CONCURR (CURRENCYID,CONCURRROW,CONCODE,COUNTRY) VALUES ('" + Pid + "','" + r + "','" + cp.ConCode + "','" + cp.Country + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                    r++;
                                }
                            }

                        }
                   
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
        public DataTable GetAllCurrencygrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select IS_ACTIVE,SYMBOL,MAINCURR,CURRENCYID from CURRENCY WHERE IS_ACTIVE = 'Y' ORDER BY CURRENCYID DESC";
            }
            else
            {
                SvSql = " Select IS_ACTIVE,SYMBOL,MAINCURR,CURRENCYID from CURRENCY WHERE IS_ACTIVE = 'N' ORDER BY CURRENCYID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }




        public DataTable GetCurrencyEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select CURRENCYID,SYMBOL,MAINCURR,CURREP,CURWIDTH from CURRENCY WHERE CURRENCYID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCItem()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMMASTERID,ITEMMASTER.ITEMID  from ITEMMASTER ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
