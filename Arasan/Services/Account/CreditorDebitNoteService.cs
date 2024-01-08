using Arasan.Interface;
using Arasan.Interface.Account;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{ 
    public class CreditorDebitNoteService : ICreditorDebitNote
    { 
        private readonly string _connectionString;
        DataTransactions datatrans;


        //int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'ICNF' AND ACTIVESEQUENCE = 'T'  ");
        //string DocId = string.Format("{0}{1}", "OSA-", (idc + 1).ToString());

        public CreditorDebitNoteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetGroup()
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }  
        
        public DataTable GetGrp()
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } 
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }  
        
        public DataTable GetLed()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGrpDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCOUNTGROUP,ACCLEDGER.LEDNAME from ACCLEDGER left outer join ACCGROUP on ACCGROUPID=ACCLEDGER.ACCGROUP WHERE ACCLEDGER.ACCGROUP = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLedbyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select LEDNAME,LEDGERID from ACCLEDGER where ACCGROUP = '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetGRPbyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCGROUPID,ACCGROUP.ACCOUNTGROUP,ACCLEDGER.ACCGROUP FROM ACCGROUP LEFT OUTER JOIN ACCLEDGER  ON ACCLEDGER.ACCGROUP = ACCGROUP.ACCGROUPID WHERE ACCGROUP.ACCGROUPID ='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSeq(string id)
        {
            string SvSql = string.Empty;
            if (id == "CreditNote")
            {
                SvSql = "select PREFIX,LASTNO FROM SEQUENCE WHERE TRANSTYPE ='vchcn'";
            }
            else
            {
                SvSql = "select PREFIX,LASTNO FROM SEQUENCE WHERE TRANSTYPE ='vchdn'";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public string CreditorDebitNotesCRUD(CreditorDebitNote cy)
        //{
        //    string msg = "";
        //    try
        //    {
        //        string StatementType = string.Empty; string svSQL = "";
        //        datatrans = new DataTransactions(_connectionString);
        //        string Location = "12418000000423";
        //        DataTable dtt = new DataTable();
        //        dtt = datatrans.GetData("select VTYPE,to_char(REFDT,'dd-MON-yyyy') REFDT ,REFNO,PARTYID,BRANCHID from CRNOTEBASIC where CRNOTEBASICID='" + cy.ID + "'");
        //        using (OracleConnection objConn = new OracleConnection(_connectionString))

        //        {
        //            objConn.Open();

        //            using (OracleCommand command = objConn.CreateCommand())
        //            {
        //                using (OracleTransaction transaction = objConn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //                {
        //                    try
        //                    {
        //                        command.Transaction = transaction;

        //                        ////////////////////transaction
        //                        int t2cunt = 0;
        //                        double grossamt = 0;
        //                        string Grossledger = "";
        //                        double netamt = 0;

        //                        grossamt = Convert.ToDouble(cy.amount);
        //                        netamt = Convert.ToDouble(cy.amount);
        //                        string type = "dn";
        //                        if(cy.VType== "CreditNote")
        //                        {
        //                            type = "cn";
        //                        }


        //                        foreach (CreDebNoteItems cp in cy.NoteLst)
        //                        {
        //                            t2cunt += 1;

        //                            //grossamt = cp.CRAmount;
        //                            //Grossledger = cp.Ledgername;


        //                        }
        //                        DataTable dtv = datatrans.GetData("select PREFIX,LASTNO from Sequence where TRANSTYPE='CrN' AND ACTIVESEQUENCE='T' ");
        //                        string vNo = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["LASTNO"].ToString();
        //                        string mno = DateTime.Now.ToString("yyyyMM");
        //                        long TRANS1 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS1'");
        //                        command.CommandText = "Insert into TRANS1 (TRANS1ID,APPROVAL,T1SOURCEID,VCHSTATUS,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,MSTATUS,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,TRANSID,BRANCHID,LOCID,CANCEL,MAXAPPROVED,LATEMPLATEID,T1VCHDT,T1NARR,T2COUNT,POSTYN,BRANCHMASTID,VTYPE,GENERATED,AMTWD,USERNAME,MODIFIEDON,T1SID) VALUES " +
        //                            "('" + TRANS1 + "','0','" + cy.ID + "','N','" + type + "','" + cy.number + "','1','1','" + dtt.Rows[0]["REFNO"].ToString() + "','" + dtt.Rows[0]["REFDT"].ToString() + "','" + mno + "','a','" + Grossledger + "','" + grossamt + "','" + cy.mid + "','" + dtt.Rows[0]["PARTYID"].ToString() + "','','" + netamt + "','CrN','" + dtt.Rows[0]["BRANCHID"].ToString() + "','" + Location + "','F','0','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + dtt.Rows[0]["VTYPE"].ToString()  + "', '" + t2cunt + "','Y','0','R','T','" + cy.Amtinwords + "','" + cy.createdby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','0')";
        //                        command.ExecuteNonQuery();
        //                        foreach (CreDebNoteItems cp in cy.NoteLst)
        //                        {
        //                            string mledger = "";
        //                            //if (cp.TypeName == "NET")
        //                            //{
        //                            //    mledger = Grossledger;
        //                            //}
        //                            //else
        //                            //{
        //                            //    mledger = cy.mid;
        //                            //}
        //                            long TRANS2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
        //                            command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2 + "','" + TRANS1 + "','" + cp.CRDR + "','" + cp.Led + "','" + cp.DRAmount + "','" + cp.DRAmount + "','" + cp.CRAmount + "','" + cp.CRAmount + "','1','1','" + cp.DRAmount + "','" + cp.CRAmount + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','dn','N','F','" + mledger + "')";
        //                            command.ExecuteNonQuery();
        //                            datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2 + 1).ToString() + "' where NAME='TRANS2'");

        //                        }
        //                        string updatetrans = " UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS1 + 1).ToString() + "' where NAME='TRANS1'";
        //                        datatrans.UpdateStatus(updatetrans);


        //                        command.CommandText = "UPDATE DBNOTEBASIC SET IS_ACCOUNTED ='Y' WHERE DBNOTEBASICID='" + cy.ID + "'";
        //                        command.ExecuteNonQuery();

        //                        /////////////////transaction
        //                        transaction.Commit();
        //                    }
        //                    catch (DataException e)
        //                    {
        //                        transaction.Rollback();
        //                        Console.WriteLine(e.ToString());
        //                        Console.WriteLine("Neither record was written to database.");
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        msg = "Error Occurs, While inserting / updating Data";
        //        throw ex;
        //    }

        //    return msg;
        //}

        public string DebitNoteBillDetCRUD(DebitNoteBill cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty;
                string svSQL = "";
                if (cy.ID != null)
                {
                    cy.ID = null;
                }
                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'DNBF' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "DNBF", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='DNBF' AND ACTIVESEQUENCE ='T'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocId = docid;
                }
                DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from PARTYMAST P where P.PARTYMASTID='" + cy.Party + "'");
                string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                //string adscheme = datatrans.GetDataString("select ADSCHEME from PRETBASIC where PRETBASICID='" + cy.grnid + "'");
                string partyname = datatrans.GetDataString("Select PARTYNAME from PARTYMAST where PARTYMASTID='" + cy.Party + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DBNOTEBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DBNOTEBASICPROC";*/

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
                    objCmd.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cy.InvNo;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("VTYPE", OracleDbType.NVarchar2).Value = cy.Vocher;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("CUSTACC", OracleDbType.NVarchar2).Value = mid;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = partyname;
                    objCmd.Parameters.Add("PARTYBALANCE", OracleDbType.NVarchar2).Value = cy.PartyBal;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("BIGST", OracleDbType.NVarchar2).Value = cy.Bigst;
                    objCmd.Parameters.Add("BSGST", OracleDbType.NVarchar2).Value = cy.Bsgst;
                    objCmd.Parameters.Add("BCGST", OracleDbType.NVarchar2).Value = cy.Bcgst;
                    objCmd.Parameters.Add("ADSCHEME", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("TOTDIS", OracleDbType.NVarchar2).Value = cy.discount;
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

                        if (cy.Depdislst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DebitNoteFuture cp in cy.Depdislst)
                                {



                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,INVDT,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT,CGSTP,SGSTP,IGSTP) VALUES ('" + Pid + "','" + cy.InvNo + "','" + cp.invdate + "','" + cp.itemid + "','" + cp.cf + "','" + cp.unit + "','" + cp.inqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.cgst + "','" + cp.sgst + "','" + cp.igst + "','" + cp.total + "','" + cp.cgstp + "','" + cp.sgstp + "','" + cp.igstp + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                        svSQL = "UPDATE GRNBLDETAIL SET DND='" + cp.disamt + "' WHERE GRNBLBASICID='" + cy.InvNo + "' and ITEMID ='" + cp.itemid + "' ";
                                        objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                        svSQL = "UPDATE GRNBLBASIC SET TDND='" + cy.discount + "' WHERE GRNBLBASICID='" + cy.InvNo + "'  ";
                                        objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete DBNOTEDETAIL WHERE DBNOTEBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DebitNoteFuture cp in cy.Depdislst)
                                {




                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,INVDT,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT,CGSTP,SGSTP,IGSTP) VALUES ('" + Pid + "','" + cy.InvNo + "','" + cp.invdate + "','" + cp.itemid + "','" + cp.cf + "','" + cp.unit + "','" + cp.inqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.cgst + "','" + cp.sgst + "','" + cp.igst + "','" + cp.total + "','" + cp.cgstp + "','" + cp.sgstp + "','" + cp.igstp + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }

                        }

                        //svSQL = "UPDATE PRETBASIC SET STATUS='DN RAISED' WHERE PRETBASICID='" + cy.grnid + "' ";
                        //OracleCommand objCmdsts = new OracleCommand(svSQL, objConn);
                        //objCmdsts.ExecuteNonQuery();
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
    }
}
