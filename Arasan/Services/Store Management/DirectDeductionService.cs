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
        public IEnumerable<DirectDeduction> GetAllDirectDeduction(string st, string ed)
        {
           
            List<DirectDeduction> staList = new List<DirectDeduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    if (st != null && ed != null)
                    {
                        cmd.CommandText = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(DEDBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,DEDBASICID ,DEDBASIC.IS_ACTIVE from DEDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DEDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DEDBASIC.LOCID WHERE DEDBASIC.DOCDATE BETWEEN '" + st + "'  AND ' " + ed + "' ORDER BY DEDBASIC.DEDBASICID DESC";

                    }
                    else
                    {
                        cmd.CommandText = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(DEDBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,DEDBASICID ,DEDBASIC.IS_ACTIVE from DEDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DEDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DEDBASIC.LOCID WHERE DEDBASIC.DOCDATE > sysdate-30 order by DEDBASICID desc ";

                    }

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
                            status = rdr["IS_ACTIVE"].ToString(),
                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

        public string DirectDeductionCRUD(DirectDeduction ss)
           {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);
                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Dde-' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "Dde-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Dde-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ss.DocId = DocId;
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
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = ss.net;
                    objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = ss.Entered;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
                    objCmd.Parameters.Add("MATSUPP", OracleDbType.NVarchar2).Value = ss.Material;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";
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
                               
                                    OracleCommand objCmds = new OracleCommand("DEDDETAILPROC", objConn);
                                    if (ss.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                         objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("DEDBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemGroupId;
                                    //objCmds.Parameters.Add("ITEMACC", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = cp.BinID;
                                    objCmds.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cp.Process;
                                    objCmds.Parameters.Add("CONFAC", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("ITEMACC", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    objCmds.ExecuteNonQuery();
                                    Object Prid = objCmds.Parameters["OUTID"].Value;
                                //Inventory details
                                double qty = Convert.ToDouble(cp.Quantity);
                                string type = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.ItemId + "'");

                                if (type == "YES")
                                {
                                    //string SvSql1 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE) VALUES ('0','0','F','" + Prid + "','754368046','" + ss.DocId + "','" + ss.Docdate + "','" + ddlot + "' ,'0','" + ddqty + "','" + dddrum + "','" + cp.rate + "','" + cp.Amount + "','" + cp.ItemId + "','" + ss.Location + "','0','0','SUB DC' )";
                                    //OracleCommand objCmdss = new OracleCommand(SvSql1, objConn);
                                    //objCmdss.ExecuteNonQuery();


                                  string  SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,DOCTIME,STOCKTRANSTYPE) VALUES ('" + Prid + "','m','" + cp.ItemId + "','" + ss.Docdate + "','" + cp.Quantity + "' ,'" + ss.Location + "','0','0','0','0','0','0','0','0','" + cp.Amount + "','" + DateTime.Now.ToString("hh:mm:ss t") + "','Conversion Issue')RETURNING STOCKVALUEID INTO :STKID";
                                    OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                    objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmdsss.ExecuteNonQuery();
                                    
                                    string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                    string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + ss.DocId + "','" + ss.Narr + "','" + cp.rate + "')";
                                    OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();
                                    
                                }
                                else
                                {

                                    string SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,DOCTIME,STOCKTRANSTYPE) VALUES ('" + Prid + "','m','" + cp.ItemId + "','" + ss.Docdate + "','" + cp.Quantity + "' ,'" + ss.Location + "','0','0','0','0','0','0','0','0','" + cp.Amount + "','" + DateTime.Now.ToString("hh:mm:ss t") + "','Conversion Issue')RETURNING STOCKVALUEID INTO :STKID";
                                    OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                    objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmdsss.ExecuteNonQuery();

                                    string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                    string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + ss.DocId + "','" + ss.Narr + "','" + cp.rate + "')";
                                    OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();
                                   
                                }
                            }
                                }

                                //////////////////////////Inventory details
                             
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
        public DataTable BindProcess()
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSID,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,LSTOCKVALUE.ITEMID as item FROM LSTOCKVALUE LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=LSTOCKVALUE.ITEMID  WHERE LOCID='" + id + "' HAVING SUM(LSTOCKVALUE.PLUSQTY-LSTOCKVALUE.MINUSQTY) > 0 GROUP BY ITEMMASTER.ITEMID,LSTOCKVALUE.ITEMID UNION select ITEMMASTER.ITEMID,STOCKVALUE.ITEMID as item FROM STOCKVALUE LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=STOCKVALUE.ITEMID  WHERE LOCID='" + id + "' HAVING SUM(DECODE(STOCKVALUE.PlusOrMinus,'p',STOCKVALUE.qty,-STOCKVALUE.qty)) > 0 GROUP BY ITEMMASTER.ITEMID,STOCKVALUE.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
        //public DataTable EditSICbyID(string name)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select  SCISSBASIC.BRANCHID,SCISSBASIC.DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,SCISSBASIC.REQNO,to_char(REQDATE,'dd-MON-yyyy')REQDATE,SCISSBASIC.TOLOCID,SCISSBASIC.LOCIDCONS,SCISSBASIC.PROCESSID,SCISSBASIC.MCID,SCISSBASIC.MCNAME,SCISSBASIC.NARRATION,SCISSBASIC.USERID,SCISSBASIC.WCID,SCISSBASICID from SCISSBASIC Where  SCISSBASIC.SCISSBASICID='" + name + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
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
        public DataTable GetDDItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DEDDETAIL.QTY,DEDDETAIL.DEDDETAILID,DEDDETAIL.ITEMID,UNITMAST.UNITID,RATE,AMOUNT,BINID,PROCESSID  from DEDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DEDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where DEDDETAIL.DEDBASICID='" + id + "'";
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
                    cmd.CommandText = "Select DEDDETAIL.QTY,DEDDETAIL.DEDDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from DEDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DEDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where DEDDETAIL.DEDBASICID='" + id + "'";
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

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DEDBASIC SET IS_ACTIVE ='N' WHERE DEDBASICID='" + id + "'";
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
        public DataTable GetAllListDirectDeductionItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(DEDBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,DEDBASICID  from DEDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DEDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DEDBASIC.LOCID AND DEDBASIC.IS_ACTIVE='Y' ORDER BY DEDBASIC.DEDBASICID DESC";
            }
            else
            {
                SvSql = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(DEDBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,DEDBASICID  from DEDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DEDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DEDBASIC.LOCID AND DEDBASIC.IS_ACTIVE='N' ORDER BY DEDBASIC.DEDBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDDByName(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select BRANCHMAST.BRANCHID, ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy') ENQ_DATE ,PARTYRCODE.PARTY,SALES_ENQUIRY.CURRENCY_TYPE,SALES_ENQUIRY.CONTACT_PERSON,SALES_ENQUIRY.CUSTOMER_TYPE,SALES_ENQUIRY.ENQ_TYPE,SALES_ENQUIRY.ADDRESS,SALES_ENQUIRY.CITY,SALES_ENQUIRY.PINCODE,PRIORITY,SALES_ENQUIRY.SALESENQUIRYID,SALES_ENQUIRY.STATUS from SALES_ENQUIRY  LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=SALES_ENQUIRY.BRANCH_ID LEFT OUTER JOIN  PARTYMAST on SALES_ENQUIRY.CUSTOMER_NAME=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.PARTY Where PARTYMAST.TYPE IN ('Customer','BOTH') AND SALES_ENQUIRY.SALESENQUIRYID='" + name + "'";
            SvSql = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,MATSUPP,NET,ENTBY,NARRATION,NOOFD,DEDBASICID  from DEDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DEDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DEDBASIC.LOCID  \r\n where DEDBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDDItem(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select SALES_ENQ_ITEM.SALESENQITEMID,SALES_ENQ_ITEM.SAL_ENQ_ID,SALES_ENQ_ITEM.QUANTITY,ITEMMASTER.ITEMID,SALES_ENQ_ITEM.UNIT,SALES_ENQ_ITEM.ITEM_DESCRIPTION from SALES_ENQ_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALES_ENQ_ITEM.ITEM_ID   where SALES_ENQ_ITEM.SALESENQITEMID='" + name + "'";
            SvSql = "Select ITEMMASTER.ITEMID,DEDDETAIL.QTY,DEDDETAIL.CONFAC,DEDDETAIL.DEDDETAILID,UNIT,RATE,AMOUNT,BINID,PROCESSID  from DEDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DEDDETAIL.ITEMID\r\n where DEDDETAIL.DEDBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}

