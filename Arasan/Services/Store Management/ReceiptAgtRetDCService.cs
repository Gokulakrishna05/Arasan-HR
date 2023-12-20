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
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Rec-' AND ACTIVESEQUENCE = 'T'  ");
                string Did = string.Format("{0}{1}", "Rec-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Rec-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string PARTY = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYID='" + cy.Party + "' ");
                string ENTER = datatrans.GetDataString("Select EMPNAME from EMPMAST where EMPMASTID='" + cy.Entered + "' ");

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Did;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DDate;
                    objCmd.Parameters.Add("DCNO", OracleDbType.NVarchar2).Value = cy.Dcno;
                    //objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Dcno;
                    objCmd.Parameters.Add("DCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DcDate);
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = PARTY;
                    objCmd.Parameters.Add("STKTYPE", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = ENTER;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = 'Y';
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;

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
                                string ITEM = datatrans.GetDataString("Select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.itemid + "' ");

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
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ITEM;

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
                                DataTable lotnogen = datatrans.GetData("Select LOTYN  FROM ITEMMASTER where LOTYN ='YES' AND ITEMMASTERID='" + cp.saveItemId + "'");
                                string lotnumber = "";
                                if (lotnogen.Rows.Count > 0)
                                {
                                    string item = cp.itemname;
                                    string Docid = cy.Did;
                                    string DocDate = cy.DDate;

                                    lotnumber = string.Format("{0}-{1}-{2}-{3}", item, DocDate, Docid, l.ToString());
                                    l++;
                                }
                                using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                    objCmdI.CommandType = CommandType.StoredProcedure;
                                    objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveItemId;
                                    objCmdI.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.detid;
                                    objCmdI.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                    objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = cp.Recd;
                                    objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = cp.Recd;
                                    objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Locationid;
                                    objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;

                                    objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = lotnumber;
                                    objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    objConnI.Open();
                                    objCmdI.ExecuteNonQuery();
                                    Object Invid = objCmdI.Parameters["OUTID"].Value;



                                    OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveItemId;
                                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.detid;
                                    objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                    objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                    objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "DC Receipt";
                                    objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                    objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = cp.Recd;
                                    objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "DC Receipt ";
                                    objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Locationid;
                                    objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    objCmdIn.ExecuteNonQuery();




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
            SvSql = "select DOCID ,RDELBASICID from RDELBASIC order by RDELBASICID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetdocnoS(string id) 
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID ,RDELBASICID from RDELBASIC WHERE DELTYPE = '" + id + "' ";
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
            SvSql = " SELECT LOCID,DCNO,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,REFNO,to_char(RECDCBASIC.REFDATE,'dd-MON-yyyy')REFDATE,STKTYPE,NARRATION,EBY,PARTYMAST.PARTYID ,DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE FROM RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID WHERE RECDCBASIC.RECDCBASICID = '" + id + "' ";

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
            SvSql = "  select to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,STKTYPE,PARTYMAST.PARTYID from RDELBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID where  RDELBASIC.RDELBASICID  = '" + id + "' ";

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
            SvSql = "SELECT RECDCBASIC.EBY,LOCDETAILS.LOCID,RDELBASIC.DOCID,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,RECDCBASIC.LOCID as loc,RECDCBASIC.REFNO,to_char(RECDCBASIC.REFDATE,'dd-MON-yyyy')REFDATE,RECDCBASIC.STKTYPE,RECDCBASIC.NARRATION,PARTYMAST.PARTYID,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE FROM RECDCBASIC  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RECDCBASIC.LOCID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID WHERE RECDCBASIC.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
         public DataTable ViewGetReceiptitem(string id)
        {
            string SvSql = string.Empty;

            SvSql = " SELECT ITEMMASTER.ITEMID,CITEMID,RECDCDETAIL.BINID,RECDCDETAILID,RECDCDETAIL.QTY,RECDCDETAIL.PENDQTY,RECDCDETAIL.REJQTY,UNITMAST.UNITID,RECDCDETAIL.SERIALYN,RECDCDETAIL.ACCQTY,RECDCDETAIL.RATE,RECDCDETAIL.AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = RECDCDETAIL.CITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID = RECDCDETAIL.UNIT WHERE RECDCDETAIL.RECDCBASICID = '" + id + "' ";



            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemgrpDetail(string id)
        {
            string SvSql = string.Empty;
               SvSql = "SELECT ITEMMASTER.ITEMID , RDELDETAIL.CITEMID AS IID,RDELDETAIL.UNIT,RDELDETAIL.QTY,RDELDETAIL.RATE FROM RDELDETAIL LEFT OUTER JOIN  ITEMMASTER ON ITEMMASTER.ITEMMASTERID=RDELDETAIL.CITEMID LEFT OUTER JOIN  RDELBASIC ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID  WHERE RDELDETAIL.RDELBASICID =  '" + id + "' ";
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
