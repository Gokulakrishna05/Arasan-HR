using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class ReceiptAgtRetDCService : IReceiptAgtRetDC
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ReceiptAgtRetDCService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public string ReceiptAgtRetDCCRUD(ReceiptAgtRetDC cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                string Did = "";
                datatrans = new DataTransactions(_connectionString);
                if (cy.ID == null)
                {
                     
                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Rec-' AND ACTIVESEQUENCE = 'T'  ");
                      Did = string.Format("{0}{1}", "Rec-", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Rec-' AND ACTIVESEQUENCE ='T'  ";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                string PARTY = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYID='" + cy.Party + "' ");
                string ENTER = datatrans.GetDataString("Select EMPNAME||' / '||EMPID as empcode from EMPMAST where EMPMASTID='" + cy.Entered + "' ");

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RECDCBASICPROC", objConn);
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
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = Did;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DDate;
                    objCmd.Parameters.Add("DCNO", OracleDbType.NVarchar2).Value = cy.Dcno;
                     objCmd.Parameters.Add("DCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DcDate);
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = PARTY;
                    objCmd.Parameters.Add("STKTYPE", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    if(cy.ID==null)
                    { objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = ENTER; 
                    }
                    else { objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = ENTER; }
                    
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = cy.user;
                    objCmd.Parameters.Add("LOCIDREJ", OracleDbType.NVarchar2).Value = "10989000000581";
                    objCmd.Parameters.Add("APPPER", OracleDbType.NVarchar2).Value = cy.Approved;
                    objCmd.Parameters.Add("APPPER2", OracleDbType.NVarchar2).Value = cy.Approval2;
                    objCmd.Parameters.Add("DCENTBY", OracleDbType.NVarchar2).Value = cy.dcey;

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
                        foreach (ReceiptAgtRetDCItem cp in cy.ReceiptLst)
                        {
                            if (cp.Isvalid == "Y" && cp.item != "0")
                            {

                                string UNIT = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");
                                DataTable itemdsc = datatrans.GetData("SELECT ITEMID,ITEMDESC,LOTYN,VALMETHOD,SERIALYN,BINNO FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.itemid + "'");

                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("RECDCDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("RECDCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("CITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                    objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = cp.bin;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Recd;
                                    objCmds.Parameters.Add("PENDQTY", OracleDbType.NVarchar2).Value = cp.Pend;
                                    objCmds.Parameters.Add("REJQTY", OracleDbType.NVarchar2).Value = cp.rej;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = UNIT;
                                    //objCmds.Parameters.Add("SERIALYN", OracleDbType.NVarchar2).Value = cp.serial;
                                    //objCmds.Parameters.Add("ACCQTY", OracleDbType.NVarchar2).Value = cp.Acc;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.amount;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = itemdsc.Rows[0]["ITEMID"].ToString();
                                    objCmds.Parameters.Add("ITEMDESC", OracleDbType.NVarchar2).Value = itemdsc.Rows[0]["ITEMDESC"].ToString();
                                    objCmds.Parameters.Add("VALMETHOD", OracleDbType.NVarchar2).Value = itemdsc.Rows[0]["VALMETHOD"].ToString();

                                    objCmds.Parameters.Add("LOTYN", OracleDbType.NVarchar2).Value = itemdsc.Rows[0]["LOTYN"].ToString();
                                    objCmds.Parameters.Add("SERIALYN", OracleDbType.NVarchar2).Value = itemdsc.Rows[0]["SERIALYN"].ToString();
                                    objCmds.Parameters.Add("RDELDETAILID", OracleDbType.NVarchar2).Value = cp.detid;
 
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
        public string ApproveReceiptAgtRetDCCRUD(ReceiptAgtRetDC cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    int l = 1;
                    foreach (ReceiptAgtRetDCItem cp in cy.ReceiptLst)
                    {
                        if (cp.saveItemId != "0")
                        {

                            /////////////////////////Inventory Update
                             
                                if (cy.Stock == "Stock")
                                {
                                    double qty = Convert.ToDouble(cp.qty);
                                    string type = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.saveItemId + "'");
                                    string itemacc = datatrans.GetDataString("SELECT ITEMACC FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.saveItemId + "'");

                                    if (type == "YES")
                                    {
                                        //string SvSql1 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE) VALUES ('0','0','F','" + detid + "','754368046','" + ss.DocId + "','" + ss.Docdate + "','" + ddlot + "' ,'0','" + ddqty + "','" + dddrum + "','" + cp.rate + "','" + cp.Amount + "','" + cp.ItemId + "','" + ss.Location + "','0','0','SUB DC' )";
                                        //OracleCommand objCmdss = new OracleCommand(SvSql1, objConn);
                                        //objCmdss.ExecuteNonQuery();


                                        string SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,STOCKTRANSTYPE,MASTERID) VALUES ('" + cp.detid + "','p','" + cp.saveItemId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.qty + "' ,'" + cy.Locationid + "','0','0','0','0','0','0','0','0','" + cp.amount + "','Receipt-Dc','" + itemacc + "') RETURNING STOCKVALUEID INTO :STKID";
                                        OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                        objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                        objCmdsss.ExecuteNonQuery();

                                        string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                        string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + cy.Did + "','" + cy.Narration + "','" + cp.rate + "')";
                                        OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                        objCmddts.ExecuteNonQuery();

                                    }
                                    else
                                    {

                                        string SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,STOCKTRANSTYPE,MASTERID) VALUES ('" + cp.detid + "','p','" + cp.saveItemId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.qty + "' ,'" + cy.Locationid + "','0','0','0','0','0','0','0','0','" + cp.amount + "','Receipt-Dc','" + itemacc + "') RETURNING STOCKVALUEID INTO :STKID";
                                        OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                        objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                        objCmdsss.ExecuteNonQuery();

                                        string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                        string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + cy.Did + "','" + cy.Narration + "','" + cp.rate + "')";
                                        OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                        objCmddts.ExecuteNonQuery();

                                    }
                                }
                                else
                            { 
                                DataTable dt = datatrans.GetData("Select ASSTOCKVALUEID,PLUSORMINUS,ITEMID,LOCID,DOCDATE,QTY,STOCKVALUE,DOCTIME,MASTERID from ASSTOCKVALUE where ASSTOCKVALUE.ITEMID ='" + cp.saveItemId + "' AND ASSTOCKVALUE.LOCID='" + cy.Locationid + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    string ADDate = DateTime.Now.ToString("dd-MMM-yyyy");

                                    svSQL = "Insert into ASSTOCKVALUE (ITEMID,LOCID,QTY,STOCKVALUE,PLUSORMINUS,DOCDATE,DOCTIME,MASTERID,T1SOURCEID,BINID,PROCESSID,FROMLOCID,STOCKTRANSTYPE) VALUES ('" + cp.saveItemId + "','" + cy.Locationid + "','" + cp.Recd + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','p','" + ADDate + "','10:00:00 PM','" + dt.Rows[0]["MASTERID"].ToString() + "','" + cp.detid + "','0','0','0','DC Receipt') RETURNING ASSTOCKVALUEID INTO :LASTCID";

                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmds.ExecuteNonQuery();
                                    string detailid = objCmds.Parameters["LASTCID"].Value.ToString();

                                    string narr = "Received from " + cy.Party;
                                    svSQL = "Insert into ASSTOCKVALUE2 (ASSTOCKVALUEID,RATE,DOCID,MDCTRL,NARRATION,ALLOWDELETE,ISSCTRL,RECCTRL) VALUES ('" + detailid + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cy.Did + "','T','" + narr + "','T','T','F')";

                                    objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }
                        
                            string Sqla = string.Empty;
                            Sqla = "Update RECDCBASIC SET  STATUS='Approve' WHERE RECDCBASICID='" + cy.ID + "'";
                            OracleCommand objCmdssa = new OracleCommand(Sqla, objConn);

                            objCmdssa.ExecuteNonQuery();
                         
                        objConn.Close();
                    }
                    
                    
                }

            }

            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
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

        public DataTable GetParty() 
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,PARTYMASTID from PARTYMAST order by PARTYMASTID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getdocno() 
        {
            string SvSql = string.Empty;
            //SvSql = "select DOCID ,RDELBASICID from RDELBASIC WHERE DCREFID IS NULL  order by RDELBASICID DESC";
            SvSql = "select R.DOCID ,R.RDELBASICID from RDELBASIC R   WHERE   R.DELTYPE = 'Returnable DC'   ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetdocnoS(string id) 
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID ,RDELBASICID from RDELBASIC WHERE DELTYPE = 'Returnable DC'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
         public DataTable Getbin() 
        {
            string SvSql = string.Empty;
            SvSql = "select BINID,BINBASICID from BINBASIC order by BINBASICID desc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPartys(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,PARTYMASTID from PARTYMAST WHERE PARTYMASTID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }public DataTable GetReceipt(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT LOCID,DCNO,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,REFNO,to_char(RECDCBASIC.REFDATE,'dd-MON-yyyy')REFDATE,STKTYPE,NARRATION,EBY,PARTYMAST.PARTYID ,DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,APPPER,APPPER2 FROM RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID WHERE RECDCBASIC.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetReceiptItem(string id)
        {
            string SvSql = string.Empty;
           // SvSql = " SELECT CITEMID,BINID,QTY,PENDQTY,REJQTY,UNITMAST.UNITID,SERIALYN,ACCQTY,RATE,AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT WHERE RECDCBASICID = '" + id + "' ";
            SvSql = "   SELECT ITEMMASTER.ITEMID, RECDCDETAIL.BINID,RECDCDETAIL.QTY,RECDCDETAIL.PENDQTY,RECDCDETAIL.REJQTY,UNITMAST.UNITID,RECDCDETAIL.SERIALYN,RECDCDETAIL.ACCQTY,RECDCDETAIL.RATE,RECDCDETAIL.AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID = RECDCDETAIL.CITEMID LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT WHERE RECDCDETAIL.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } 
        public DataTable GetDCDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  select to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,STKTYPE,PARTYMAST.PARTYID,EBY from RDELBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID where  RDELBASIC.RDELBASICID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getdctype(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  select DELTYPE,DOCID from RDELBASIC WHERE RDELBASIC.RDELBASICID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable Getviewdctype(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  select DELTYPE from RDELBASIC WHERE RDELBASIC.DOCID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemDetail(string id)
        {

            string SvSql = string.Empty;
            //SvSql = "   select UNITMAST.UNITID,ITEMID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID  where ITEMMASTER.ITEMDESC   = '" + id + "' ";
            SvSql = "    select UNITMAST.UNITID,ITEMID from ITEMMASTER LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = ITEMMASTER.PRIUNIT where ITEMMASTERID   = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ViewGetReceipt(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RECDCBASIC.EBY,LOCDETAILS.LOCID,RDELBASIC.DOCID,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,RECDCBASIC.LOCID as loc,RECDCBASIC.REFNO,to_char(RECDCBASIC.REFDATE,'dd-MON-yyyy')REFDATE,RECDCBASIC.STKTYPE,RECDCBASIC.NARRATION,PARTYMAST.PARTYID,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RECDCBASIC.APPPER,RECDCBASIC.APPPER2 FROM RECDCBASIC  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RECDCBASIC.LOCID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID WHERE RECDCBASIC.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
         public DataTable ViewGetReceiptitem(string id)
        {
            string SvSql = string.Empty;

            SvSql = " SELECT ITEMMASTER.ITEMID,CITEMID,BINBASIC.BINID,RECDCDETAILID,RECDCDETAIL.QTY,RECDCDETAIL.PENDQTY,RECDCDETAIL.REJQTY,UNITMAST.UNITID,RECDCDETAIL.SERIALYN,RECDCDETAIL.ACCQTY,RECDCDETAIL.RATE,RECDCDETAIL.AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN BINBASIC on BINBASIC.BINBASICID = RECDCDETAIL.BINID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = RECDCDETAIL.CITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID = RECDCDETAIL.UNIT WHERE RECDCDETAIL.RECDCBASICID = '" + id + "' ";



            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemgrpDetail(string id)
        {
            string SvSql = string.Empty;
               SvSql = "SELECT ITEMMASTER.ITEMID , RDELDETAIL.CITEMID AS IID,RDELDETAIL.UNIT,RDELDETAIL.QTY,RDELDETAIL.RATE,RDELDETAILID FROM RDELDETAIL LEFT OUTER JOIN  ITEMMASTER ON ITEMMASTER.ITEMMASTERID=RDELDETAIL.CITEMID LEFT OUTER JOIN  RDELBASIC ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID  WHERE RDELDETAIL.RDELBASICID =  '" + id + "' ";
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
                    svSQL = "UPDATE RECDCBASIC SET IS_ACTIVE ='N' WHERE RECDCBASICID='" + id + "'";
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
                    svSQL = "UPDATE RECDCBASIC SET IS_ACTIVE ='Y' WHERE RECDCBASICID = '" + id + "'";
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
        public DataTable GetAllReceipt(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select RECDCBASIC.RECDCBASICID,RECDCBASIC.IS_ACTIVE,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.DOCID,PARTYMAST.PARTYID,RECDCBASIC.STATUS from RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO where RECDCBASIC.IS_ACTIVE = 'Y' ORDER BY RECDCBASICID DESC  ";

            }
            else
            {
                SvSql = "select RECDCBASIC.RECDCBASICID,RECDCBASIC.IS_ACTIVE,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.DOCID,PARTYMAST.PARTYID,RECDCBASIC.STATUS  from RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO where RECDCBASIC.IS_ACTIVE = 'N' ORDER BY RECDCBASICID DESC  ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
