using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class StoreIssueProductionService :IStoreIssueProduction
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StoreIssueProductionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
     
        public IEnumerable<SIPItem> GetAllStoreIssueItem(string id)
        {
            List<SIPItem> cmpList = new List<SIPItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select STORESISSDETAIL.QTY,STORESISSDETAIL.STORESISSDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from STORESISSDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESISSDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where STORESISSDETAIL.STORESISSBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SIPItem cmp = new SIPItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public IEnumerable<StoreIssueProduction> GetAllStoreIssuePro()
        {
            List<StoreIssueProduction> cmpList = new List<StoreIssueProduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REQNO,to_char(REQDATE,'dd-MON-yyyy')REQDATE,TOLOCID,LOCIDCONS,PROCESSID,NARRATION,PSCHNO,WCID,STORESISSBASICID from STORESISSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESISSBASIC.BRANCHID  ORDER BY STORESISSBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreIssueProduction cmp = new StoreIssueProduction
                        {

                            ID = rdr["STORESISSBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),

                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            ReqNo = rdr["REQNO"].ToString(),
                            ReqDate = rdr["REQDATE"].ToString(),
                            Location = rdr["TOLOCID"].ToString(),
                             
                            LocCon = rdr["LOCIDCONS"].ToString(),
                            // net = rdr["NET"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            
                            Narr = rdr["NARRATION"].ToString(),
                            SchNo = rdr["PSCHNO"].ToString(),
                            Work = rdr["WCID"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable EditSIPbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  STORESISSBASIC.BRANCHID,STORESISSBASIC.DOCID,to_char(STORESISSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,STORESISSBASIC.REQNO,to_char(STORESISSBASIC.REQDATE,'dd-MON-yyyy')REQDATE,STORESISSBASIC.TOLOCID,STORESISSBASIC.LOCIDCONS,STORESISSBASIC.PROCESSID,STORESISSBASIC.NARRATION,STORESISSBASIC.PSCHNO,STORESISSBASIC.WCID,STORESISSBASIC.STORESISSBASICID from STORESISSBASIC Where  STORESISSBASIC.STORESISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSICItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESISSDETAIL.QTY,STORESISSDETAIL.STORESISSDETAILID,STORESISSDETAIL.ITEMID,UNITMAST.UNITID,STORESISSDETAIL.RATE,CONVFACTOR,DRUMYN,SERIALYN,PENDQTY,REQQTY,SCHQTY,AMOUNT,CLSTOCK from STORESISSDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESISSDETAIL.UNIT  where STORESISSDETAIL.STORESISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StoreIssueProCRUD(StoreIssueProduction cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SIPROPROC", objConn);


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
                                      
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("REQNO", OracleDbType.NVarchar2).Value = cy.ReqNo;
                    objCmd.Parameters.Add("ReqDate", OracleDbType.Date).Value = DateTime.Parse(cy.ReqDate);
                    objCmd.Parameters.Add("TOLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("LOCIDCONS", OracleDbType.NVarchar2).Value = cy.LocCon;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    //objCmd.Parameters.Add("MCID", OracleDbType.NVarchar2).Value = cy.MCNo;
                    //objCmd.Parameters.Add("MCNAME", OracleDbType.NVarchar2).Value = cy.MCNa;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.SchNo;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Work;
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
                        foreach (SIPItem cp in cy.SIPLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("SIPDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("STORESISSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("PENDQTY", OracleDbType.NVarchar2).Value = cp.PendQty;
                                    objCmds.Parameters.Add("REQQTY", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = cp.SchQty;
                                    objCmds.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cp.ClStock;
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
        public DataTable GetItemCF(string ItemId, string unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
      

    }
}
