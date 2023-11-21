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
            //if (string.IsNullOrEmpty(Active))
            //{
            //    Active = "Yes";
            //}
            List<AccConfig> cmpList = new List<AccConfig>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    //cmd.CommandText = " SELECT ADSCHEME ,ADCOMPD.ADSCHEMENAME,ADTYPE,ADNAME,ADACCOUNT,TRANSDESC,TRANSID ,ADCOMPDID,ADCOMPD.ACTIVE FROM ADCOMPD  WHERE ADCOMPD.ACTIVE ='" + Active + "'  ";
                    cmd.CommandText = " SELECT BRANCHMAST.BRANCHID,ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ACTIVE,ADCOMPHID FROM ADCOMPH LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ADCOMPH.BRANCHID WHERE ADCOMPH.ACTIVE = 'Yes' order by ADCOMPH.ADCOMPHID DESC  ";
;

                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccConfig cmp = new AccConfig
                        {

                            ID = rdr["ADCOMPHID"].ToString(),

                            SchemeDes = rdr["ADSCHEMEDESC"].ToString(),
                            Scheme = rdr["ADSCHEME"].ToString(),
                            TransactionName = rdr["ADTRANSDESC"].ToString(),
                            TransactionID = rdr["ADTRANSID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            //CreatBy = rdr["CREATED_BY"].ToString(),
                            //CreatOn = rdr["CREATED_ON"].ToString(),
                            //CurrDate = rdr["CURRENT_DATE"].ToString(),
                            Active = rdr["ACTIVE"].ToString()
                            
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        //public IEnumerable<ConfigItem> GetAllConfigItem(string id)
        //{
        //    List<ConfigItem> cmpList = new List<ConfigItem>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "SELECT ADTYPE,ADNAME,ADSCHEMENAME,ADACCOUNT FROM ADCOMPD WHERE ADCOMPD.ACTIVE ='" + id + "'";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ConfigItem cmp = new ConfigItem
        //                {
        //                    Type = rdr["ADTYPE"].ToString(),
        //                    Tname = rdr["ADNAME"].ToString(),
        //                    Schname = rdr["ADSCHEMENAME"].ToString(),
        //                    ledger = rdr["ADACCOUNT"].ToString()
        //            };
                        
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
        public string ConfigCRUD(AccConfig cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty;
                string svSQL = "";
                string sv = "";
                
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM ADCOMPH WHERE ADSCHEME =LTRIM(RTRIM('" + cy.Scheme + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "ETariff Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACONFIGPROC", objConn);
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
                    
                    objCmd.Parameters.Add("TRANSDESC", OracleDbType.NVarchar2).Value = cy.TransactionName;
                    objCmd.Parameters.Add("TRANSID", OracleDbType.NVarchar2).Value = cy.TransactionID;
                    
                    objCmd.Parameters.Add("ADSCHEME", OracleDbType.NVarchar2).Value = cy.Scheme;
                    objCmd.Parameters.Add("ADSCHEMEDESC", OracleDbType.NVarchar2).Value = cy.SchemeDes;

                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.CreatBy;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CURRENT_DATE", OracleDbType.Date).Value = DateTime.Now;

                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "Yes";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    try
                    {

                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;

                            sv = "DELETE ADCOMPD WHERE ADCOMPHID = '" + Pid + "' ";
                            OracleCommand objCmdd = new OracleCommand(sv, objConn);
                            objCmdd.ExecuteNonQuery();
                        }
                        foreach (ConfigItem cp in cy.ConfigLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ledger != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("ADCOMPD_PROC", objConns);
                                   
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                   
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("ADCOMPHID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ADTYPE", OracleDbType.NVarchar2).Value = cp.Type;
                                    objCmds.Parameters.Add("ADNAME", OracleDbType.NVarchar2).Value = cp.Tname;
                                    objCmds.Parameters.Add("ADSCHEMENAME", OracleDbType.NVarchar2).Value = cp.Schname;
                                    objCmds.Parameters.Add("ADACCOUNT", OracleDbType.NVarchar2).Value = cp.ledger;

                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                            }

                        }
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

       
        public DataTable GetAccConfig(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,CURRENT_DATE ,CREATED_BY,CREATED_ON,CURRENT_DATE,BRANCHID,ADCOMPHID FROM ADCOMPH WHERE ADCOMPHID = '" + id + "' ";
            SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID FROM ADCOMPH WHERE ADCOMPHID = '" + id + "' ";
            
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
        public DataTable Getledger()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDNAME,LEDGERID from ACCLEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccConfigItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADTYPE,ADNAME,ADSCHEMENAME,ADACCOUNT from ADCOMPD where ADCOMPD.ADCOMPHID = '" + id + "' "; 
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ADCOMPH SET ACTIVE ='No' WHERE ADCOMPHID='" + id + "'";
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

        //public string RemoveChange(string tag, int id)
        //{

        //    try
        //    {
        //        string svSQL = string.Empty;
        //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
        //        {
        //            svSQL = "UPDATE ADCOMPD SET ACTIVE ='YES' WHERE ADCOMPDID='" + id + "'";
        //            OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
        //            objConnT.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConnT.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return "";

        //}

        public DataTable GetConfig(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID FROM ADCOMPH WHERE ADCOMPHID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetConfigItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADTYPE,ADNAME,ADSCHEMENAME,ACCLEDGER.LEDNAME from ADCOMPD LEFT OUTER JOIN ACCLEDGER ON ACCLEDGER.LEDGERID = ADCOMPD.ADACCOUNT where ADCOMPD.ADCOMPHID= '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllConfig()
        {
            string SvSql = string.Empty;
            //SvSql = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRICE,ITEMMASTERID from ITEMMASTER";
            SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID FROM ADCOMPH WHERE ACTIVE = 'Yes' ORDER BY ADCOMPHID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
