using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace Arasan.Services
{
    public class StoreIssueConsumablesService : IStoreIssueConsumables
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StoreIssueConsumablesService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }


        public IEnumerable<SICItem> GetAllStoreIssueItem(string id)
        {
            List<SICItem> cmpList = new List<SICItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SCISSDETAIL.QTY,SCISSDETAIL.SCISSDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from SCISSDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SCISSDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where SCISSDETAIL.SCISSBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SICItem cmp = new SICItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITMASTID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public IEnumerable<StoreIssueConsumables> GetAllStoreIssue()
        {
            List<StoreIssueConsumables> cmpList = new List<StoreIssueConsumables>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,SCISSBASIC.DOCID,to_char(SCISSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SCISSBASIC.REQNO,to_char(SCISSBASIC.REQDATE,'dd-MON-yyyy')REQDATE, LOCDETAILS.LOCID,LOCIDCONS,PROCESSID,SCISSBASIC.MCID,SCISSBASIC.MCNAME,SCISSBASIC.NARRATION,USERID,WCID,SCISSBASICID from SCISSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SCISSBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=SCISSBASIC.TOLOCID  ORDER BY SCISSBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreIssueConsumables cmp = new StoreIssueConsumables
                        {

                            ID = rdr["SCISSBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),

                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            ReqNo = rdr["REQNO"].ToString(),
                            ReqDate = rdr["REQDATE"].ToString(),
                            Location = rdr["LOCID"].ToString(),

                            LocCon = rdr["LOCIDCONS"].ToString(),
                            // net = rdr["NET"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            MCNo = rdr["MCID"].ToString(),
                            MCNa = rdr["MCNAME"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            User = rdr["USERID"].ToString(),
                            Work = rdr["WCID"].ToString(),

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable EditSICbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  SCISSBASIC.BRANCHID,SCISSBASIC.DOCID,to_char(SCISSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SCISSBASIC.REQNO,to_char(SCISSBASIC.REQDATE,'dd-MON-yyyy')REQDATE,SCISSBASIC.TOLOCID,SCISSBASIC.LOCIDCONS,SCISSBASIC.PROCESSID,SCISSBASIC.MCID,SCISSBASIC.MCNAME,SCISSBASIC.NARRATION,SCISSBASIC.USERID,SCISSBASIC.WCID,SCISSBASICID from SCISSBASIC Where  SCISSBASIC.SCISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSICItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select SCISSDETAIL.QTY,SCISSDETAIL.SCISSDETAILID,SCISSDETAIL.ITEMID,UNITMAST.UNITID,RATE,PENDQTY,REQQTY,AMOUNT,INDP,CONVFACTOR from SCISSDETAIL LEFT OUTER JOIN  ITEMMASTER on ITEMMASTER.ITEMMASTERID=SCISSDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=SCISSDETAIL.UNIT   where SCISSDETAIL.SCISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StoreIssueCRUD(StoreIssueConsumables cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SICOSUPROC", objConn);


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
                    objCmd.Parameters.Add("MCID", OracleDbType.NVarchar2).Value = cy.MCNo;
                    objCmd.Parameters.Add("MCNAME", OracleDbType.NVarchar2).Value = cy.MCNa;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = cy.User;
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
                        foreach (SICItem cp in cy.SICLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("SICDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("SCISSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("PENDQTY", OracleDbType.NVarchar2).Value = cp.PendQty;
                                    objCmds.Parameters.Add("REQQTY", OracleDbType.NVarchar2).Value = cp.ReqQty;
                                    objCmds.Parameters.Add("INDP", OracleDbType.NVarchar2).Value = cp.Indp;
                                    objCmds.Parameters.Add("CONVFACTOR", OracleDbType.NVarchar2).Value = cp.ConFac;
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

