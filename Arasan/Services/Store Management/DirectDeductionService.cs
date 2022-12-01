using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class DirectDeductionService : IDirectDeductionService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DirectDeductionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<DirectDeduction> GetAllDirectDeduction()
        {
            List<DirectDeduction> staList = new List<DirectDeduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NOOFD,DEDBASICID from DEDBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectDeduction sta = new DirectDeduction
                        {
                            ID = rdr["DEDBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Dcno = rdr["DCNO"].ToString(),
                            Reason = rdr["REASON"].ToString(),
                            Gro = rdr["GROSS"].ToString(),
                            Entered = rdr["ENTBY"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            NoDurms = rdr["NOOFD"].ToString(),




                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

        public DirectDeduction GetDirectDeductionById(string eid)
        {
            DirectDeduction DirectDeduction = new DirectDeduction();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NOOFD,DEDBASICID  from DEDBASIC where DEDBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectDeduction sta = new DirectDeduction
                        {
                            ID = rdr["DEDBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Dcno = rdr["DCNO"].ToString(),
                            Reason = rdr["REASON"].ToString(),
                            Gro = rdr["GROSS"].ToString(),
                            Entered = rdr["ENTBY"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            NoDurms = rdr["NOOFD"].ToString(),
                        };
                        DirectDeduction = sta;
                    }
                }
            }
            return DirectDeduction;
        }

        public string DirectDeductionCRUD(DirectDeduction ss)
           {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DEDBASICPROC", objConn);
                   

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = ss.Branch;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("DCNO", OracleDbType.NVarchar2).Value = ss.Dcno;
                    objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = ss.Reason;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = ss.Gro;
                    objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = ss.Entered;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
                    objCmd.Parameters.Add("NOOFD", OracleDbType.NVarchar2).Value = ss.NoDurms;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }
                        foreach (DeductionItem cp in ss.Itlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("DEDDETAILPROC", objConns);
                                    if (ss.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;

                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("DEDBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = cp.BinID;
                                    objCmds.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cp.Process;
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

        public DataTable GetDirectDeductionDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NOOFD,DEDBASICID  from DEDBASIC where DEDBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditSICbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  SCISSBASIC.BRANCHID,SCISSBASIC.DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,SCISSBASIC.REQNO,to_char(REQDATE,'dd-MON-yyyy')REQDATE,SCISSBASIC.TOLOCID,SCISSBASIC.LOCIDCONS,SCISSBASIC.PROCESSID,SCISSBASIC.MCID,SCISSBASIC.MCNAME,SCISSBASIC.NARRATION,SCISSBASIC.USERID,SCISSBASIC.WCID,SCISSBASICID from SCISSBASIC Where  SCISSBASIC.SCISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
        public DataTable GetDDItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select DEDDETAIL.QTY,DEDDETAIL.DEDDETAILID,DEDDETAIL.ITEMID,UNITMAST.UNITID,RATE,BINID,PROCESSID,AMOUNT from DEDDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=DEDDETAIL.UNIT  where DEDDETAIL.DEDDETAILID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<DeductionItem> GetAllStoreIssueItem(string id)
        {
            List<DeductionItem> cmpList = new List<DeductionItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DEDDETAIL.QTY,DEDDETAIL.DEDDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from DEDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DEDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where DEDDETAIL.DEDDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DeductionItem cmp = new DeductionItem
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
        public DataTable GetItemGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMGROUPID,GROUPCODE from itemgroup";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE  ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
      
    }
}

