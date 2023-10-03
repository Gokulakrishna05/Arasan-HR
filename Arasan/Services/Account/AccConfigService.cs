using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services 
{
    public class AccConfigService : IAccConfig
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccConfigService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<AccConfig> GetAllAccConfig(string Active)
        {
            if (string.IsNullOrEmpty(Active))
            {
                Active = "YES";
            }
            List<AccConfig> cmpList = new List<AccConfig>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    //cmd.CommandText = " SELECT ADCOMPH.ADSCHEME ,ADCOMPD.ADSCHEMENAME,ADTYPE,ADNAME,ADACCOUNT,TRANSDESC,TRANSID ,ADCOMPDID,ADCOMPD.ACTIVE FROM ADCOMPD LEFT OUTER JOIN ADCOMPH ON ADCOMPH.ADSCHEME = ADCOMPD.ADSCHEME WHERE ADCOMPD.ACTIVE =' + Active + ' ";
                    cmd.CommandText = " SELECT ADSCHEME ,ADCOMPD.ADSCHEMENAME,ADTYPE,ADNAME,ADACCOUNT,TRANSDESC,TRANSID ,ADCOMPDID,ADCOMPD.ACTIVE FROM ADCOMPD  WHERE ADCOMPD.ACTIVE ='" + Active + "'  ";

                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccConfig cmp = new AccConfig
                        {

                            ID = rdr["ADCOMPDID"].ToString(),

                            Scheme = rdr["ADSCHEME"].ToString(),
                            TransactionName = rdr["TRANSDESC"].ToString(),
                            TransactionID = rdr["TRANSID"].ToString(),
                            Type = rdr["ADTYPE"].ToString(),
                            Tname = rdr["ADNAME"].ToString(),
                            Schname = rdr["ADSCHEMENAME"].ToString(),
                            ledger = rdr["ADACCOUNT"].ToString(),
                            Active = rdr["ACTIVE"].ToString()


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

     

        public string ConfigCRUD(AccConfig cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; 
                string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ADCOMPDPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

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
                    objCmd.Parameters.Add("ADSCHEME", OracleDbType.NVarchar2).Value = cy.Scheme;
                    objCmd.Parameters.Add("TRANSDESC", OracleDbType.NVarchar2).Value = cy.TransactionName;
                    objCmd.Parameters.Add("TRANSID", OracleDbType.NVarchar2).Value = cy.TransactionID;
                    objCmd.Parameters.Add("ADTYPE", OracleDbType.NVarchar2).Value = cy.Type;
                    objCmd.Parameters.Add("ADNAME", OracleDbType.NVarchar2).Value = cy.Tname;
                    objCmd.Parameters.Add("ADSCHEMENAME", OracleDbType.NVarchar2).Value = cy.Schname;
                    objCmd.Parameters.Add("ADACCOUNT", OracleDbType.NVarchar2).Value = cy.ledger;

                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "YES";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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
        //            try
        //            {

        //                objConn.Open();
        //                objCmd.ExecuteNonQuery();
        //                Object Pid = objCmd.Parameters["OUTID"].Value;
        //                //string Pid = "0";
        //                if (cy.ID != null)
        //                {
        //                    Pid = cy.ID;
        //                }
        //                foreach (ConfigItem cp in cy.ConfigLst)
        //                {
        //                    if (cp.Isvalid == "Y" && cp.ledger != "0")
        //                    {
        //                        using (OracleConnection objConns = new OracleConnection(_connectionString))
        //                        {
        //                            OracleCommand objCmds = new OracleCommand("ADCOMPD_PROC", objConns);
        //                            if (cy.ID == null)
        //                            {
        //                                StatementType = "Insert";
        //                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
        //                            }
        //                            else
        //                            {
        //                                StatementType = "Update";
        //                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
        //                            }
        //                            objCmds.CommandType = CommandType.StoredProcedure;
                                    
        //                            objCmds.Parameters.Add("ADTYPE", OracleDbType.NVarchar2).Value = cp.Type;
        //                            objCmds.Parameters.Add("ADNAME", OracleDbType.NVarchar2).Value = cp.Tname;
        //                            objCmds.Parameters.Add("ADSCHEMENAME", OracleDbType.NVarchar2).Value = cp.Scheme;
        //                            objCmds.Parameters.Add("ADACCOUNT", OracleDbType.NVarchar2).Value = cp.ledger;

        //                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
        //                            objConns.Open();
        //                            objCmds.ExecuteNonQuery();
        //                            objConns.Close();
        //                        }

        //                    }

        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                //System.Console.WriteLine("Exception: {0}", ex.ToString());
        //            }
        //            objConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error Occurs, While inserting / updating Data";
        //        throw ex;
        //    }

        //    return msg;
        //}

        public DataTable GetSchemeDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ADSCHEME,ADTRANSDESC,ADTRANSID from ADCOMPH where ADCOMPH.ADCOMPHID =  '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable Getschemebyid(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ADSCHEME,ADCOMPHID from ADCOMPH where ACTIVE = 'Yes' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccConfig(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADSCHEMENAME,ADTYPE,ADNAME,ADACCOUNT,ADSCHEME,TRANSDESC,TRANSID ,ADCOMPDID FROM ADCOMPD WHERE ADCOMPDID ='" + id + "' ";
            
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetConfig()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT  ADSCHEME,ADCOMPHID FROM ADCOMPH WHERE ACTIVE = 'Yes' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getledger()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDNAME,LEDGERID from LEDGER where LEDGER.STATUS = 'Active' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetAccConfigItem(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select ADCOMPDID,ADTYPE,ADNAME,ADSCHEMENAME,ADACCOUNT  from ADCOMPD  where ADCOMPD.ADCOMPDID= '" + id + "' ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ADCOMPD SET ACTIVE ='NO' WHERE ADCOMPDID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ADCOMPD SET ACTIVE ='YES' WHERE ADCOMPDID='" + id + "'";
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
