using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;

namespace Arasan.Services.Sales
{
    public class DebitNoteBillService : IDebitNoteBillService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DebitNoteBillService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        //public IEnumerable<DebitNoteBill> GetAllDebitNoteBill()
        //{
        //    List<DebitNoteBill> cmpList = new List<DebitNoteBill>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DBNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DBNOTEBASICID from DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DBNOTEBASIC.BRANCHID";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                DebitNoteBill cmp = new DebitNoteBill
        //                {
        //                    ID = rdr["DBNOTEBASICID"].ToString(),
        //                    Branch = rdr["BRANCHID"].ToString(),
        //                    DocId = rdr["DOCID"].ToString(),
        //                    Docdate = rdr["DOCDATE"].ToString(),
        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}

        public IEnumerable<DebitNoteBill> GetAllDebitNoteBill()
        {
            List<DebitNoteBill> cmpList = new List<DebitNoteBill>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DBNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DBNOTEBASICID from DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DBNOTEBASIC.BRANCHID WHERE DBNOTEBASIC.IS_ACTIVE='Y' AND DBNOTEBASIC.IS_ACCOUNTED='N'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DebitNoteBill cmp = new DebitNoteBill
                        {
                            ID = rdr["DBNOTEBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string DebitNoteAcc(DebitNoteBill cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                string Location = "12418000000423";
                DataTable dtt = new DataTable();
                dtt = datatrans.GetData("select DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,BRANCHID,PARTYID from DBNOTEBASIC where DBNOTEBASICID='" + cy.ID + "'");
                using (OracleConnection objConn = new OracleConnection(_connectionString))

                {
                    objConn.Open();

                    using (OracleCommand command = objConn.CreateCommand())
                    {
                        using (OracleTransaction transaction = objConn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                command.Transaction = transaction;

                                ////////////////////transaction
                                int t2cunt = 0;
                                double grossamt = 0;
                                string Grossledger = "";
                                double totgst = 0;
                                double netamt = 0;
                                foreach (GRNAccount cp in cy.Acclst)
                                {
                                    t2cunt += 1;
                                    if (cp.TypeName == "GROSS")
                                    {
                                        grossamt = cp.CRAmount;
                                        Grossledger = cp.Ledgername;
                                    }
                                    if (cp.TypeName == "CGST")
                                    {
                                        totgst += cp.CRAmount;
                                    }
                                    if (cp.TypeName == "SGST")
                                    {
                                        totgst += cp.CRAmount;
                                    }
                                    if (cp.TypeName == "IGST")
                                    {
                                        totgst += cp.CRAmount;
                                    }
                                    if (cp.TypeName == "NET")
                                    {
                                        netamt = cp.DRAmount;
                                    }
                                }
                                DataTable dtv = datatrans.GetData("select PREFIX,LASTNO from Sequence where TRANSTYPE='vchdn' AND ACTIVESEQUENCE='T' ");
                                string vNo = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["LASTNO"].ToString();
                                string mno = DateTime.Now.ToString("yyyyMM");
                                long TRANS1 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS1'");
                                command.CommandText = "Insert into TRANS1 (TRANS1ID,APPROVAL,T1SOURCEID,VCHSTATUS,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,MSTATUS,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,TRANSID,BRANCHID,LOCID,CANCEL,MAXAPPROVED,LATEMPLATEID,T1VCHDT,T1NARR,T2COUNT,POSTYN,BRANCHMASTID,VTYPE,GENERATED,AMTWD,USERNAME,MODIFIEDON,T1SID,TOTGST) VALUES " +
                                    "('" + TRANS1 + "','0','" + cy.ID + "','N','dn','" + vNo + "','1','1','" + dtt.Rows[0]["DOCID"].ToString() + "','" + dtt.Rows[0]["DOCDATE"].ToString() + "','" + mno + "','a','" + Grossledger + "','" + grossamt + "','" + cy.mid + "','" + netamt + "','" + dtt.Rows[0]["PARTYID"].ToString() + "','" + netamt + "','vchdn','" + dtt.Rows[0]["BRANCHID"].ToString() + "','" + Location + "','F','0','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Vmemo + "','" + t2cunt + "','Y','0','R','T','" + cy.Amtinwords + "','" + cy.createdby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','0','" + totgst + "')";
                                command.ExecuteNonQuery();
                                foreach (GRNAccount cp in cy.Acclst)
                                {
                                    string mledger = "";
                                    if (cp.TypeName == "NET")
                                    {
                                        mledger = Grossledger;
                                    }
                                    else
                                    {
                                        mledger = cy.mid;
                                    }
                                    long TRANS2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2 + "','" + TRANS1 + "','" + cp.CRDR + "','" + cp.Ledgername + "','" + cp.DRAmount + "','" + cp.DRAmount + "','" + cp.CRAmount + "','" + cp.CRAmount + "','1','1','" + cp.DRAmount + "','" + cp.CRAmount + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','dn','N','F','" + mledger + "')";
                                    command.ExecuteNonQuery();
                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2 + 1).ToString() + "' where NAME='TRANS2'");

                                }
                                string updatetrans = " UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS1 + 1).ToString() + "' where NAME='TRANS1'";
                                datatrans.UpdateStatus(updatetrans);


                                command.CommandText = "UPDATE DBNOTEBASIC SET IS_ACCOUNTED ='Y' WHERE DBNOTEBASICID='" + cy.ID + "'";
                                command.ExecuteNonQuery();

                                ///////////////////transaction
                                transaction.Commit();
                            }
                            catch (DataException e)
                            {
                                transaction.Rollback();
                                Console.WriteLine(e.ToString());
                                Console.WriteLine("Neither record was written to database.");
                            }
                        }
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
 
        public string DebitNoteBillCRUD(DebitNoteBill cy)
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
                DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from PARTYMAST P where P.PARTYMASTID='" + cy.Partyid + "'");
                string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                string adscheme = datatrans.GetDataString("select ADSCHEME from PRETBASIC where PRETBASICID='" + cy.grnid + "'");
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
                    objCmd.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cy.grnid; 
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("VTYPE", OracleDbType.NVarchar2).Value = cy.Vocher;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Partyid;
                    objCmd.Parameters.Add("CUSTACC", OracleDbType.NVarchar2).Value = mid; 
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("PARTYBALANCE", OracleDbType.NVarchar2).Value = cy.PartyBal;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("BIGST", OracleDbType.NVarchar2).Value = cy.Bigst;
                    objCmd.Parameters.Add("BSGST", OracleDbType.NVarchar2).Value = cy.Bsgst;
                    objCmd.Parameters.Add("BCGST", OracleDbType.NVarchar2).Value = cy.Bcgst;
                    objCmd.Parameters.Add("ADSCHEME", OracleDbType.NVarchar2).Value = adscheme;
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
                        if (cy.Depitlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DebitNoteItem cp in cy.Depitlst)
                                {
                                    string grn = datatrans.GetDataString("Select GRNBLBASICID from GRNBLBASIC where DOCID='" + cp.InvNo + "' ");


                                    if (cp.Isvalid == "Y" && cp.InvNo != "0")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,INVDT,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + grn + "','" + cp.Invdate + "','" + cp.Itemid + "','" + cp.Cf + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete DBNOTEDETAIL WHERE DBNOTEBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DebitNoteItem cp in cy.Depitlst)
                                {
                                    string grn = datatrans.GetDataString("Select GRNBLBASICID from GRNBLBASIC where DOCID='" + cp.InvNo + "' ");



                                    if (cp.Isvalid == "Y" && cp.InvNo != "0")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,INVDT,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + grn + "','" + cp.Invdate + "','" + cp.Itemid + "','" + cp.Cf + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }

                        }
                        svSQL = "UPDATE PRETBASIC SET STATUS='DN RAISED' WHERE PRETBASICID='" + cy.grnid + "' ";
                        OracleCommand objCmdsts = new OracleCommand(svSQL, objConn);
                        objCmdsts.ExecuteNonQuery();
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
        public string CreditNoteStock(DebitNoteBill cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CRNBASICPROC", objConn);
                  
                    objCmd.CommandType = CommandType.StoredProcedure;

                   
                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                   
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Bra;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Loc;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXCHANGERATE", OracleDbType.NVarchar2).Value = cy.Exchange;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("VTYPE", OracleDbType.NVarchar2).Value = cy.Vocher;
                    objCmd.Parameters.Add("TCREDITAMOUNT", OracleDbType.NVarchar2).Value = cy.Credit;
                    objCmd.Parameters.Add("TDEBITAMOUNT", OracleDbType.NVarchar2).Value = cy.Debit;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        if (cy.Creditlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (CreditItem cp in cy.Creditlst)
                                {
                                    //string itemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.Item + "' ");


                                    if (cp.Isvalid == "Y" && cp.Dr != "0")
                                    {
                                        svSQL = "Insert into CRNDETAIL(CRNBASICID,DBCR,MID,DBAMOUNT,CRAMOUNT) VALUES ('" + Pid + "','" + cp.Dr + "','" + cp.Account + "','" + cp.DepitAmount + "','" + cp.CreditAmount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete CRNDETAIL WHERE CRNBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (CreditItem cp in cy.Creditlst)
                                {
                                    //string itemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.Item + "' ");


                                    if (cp.Isvalid == "Y" && cp.Dr != "0")
                                    {
                                        svSQL = "Insert into CRNDETAIL(CRNBASICID,DBCR,MID,DBAMOUNT,CRAMOUNT) VALUES ('" + Pid + "','" + cp.Dr + "','" + cp.Account + "','" + cp.DepitAmount + "','" + cp.CreditAmount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
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
        //public DataTable GetParty()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select GRNBLBASICID,PARTYNAME,PARTYID FROM GRNBLBASIC";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetGrn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,GRNBLBASICID FROM  GRNBLBASIC WHERE PARTYID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string itemId, string grnid)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CF,PUNIT,QTY,RATE,AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT FROM GRNBLDETAIL WHERE ITEMID='" + itemId + "' AND GRNBLBASICID='" + grnid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetInvoDates(string itemId)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,GRNBLBASICID FROM  GRNBLBASIC WHERE GRNBLBASICID='" + itemId + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public DataTable GetDebitNoteBillDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,VTYPE,DOCID,DOCDATE,REFNO,REFDT,PARTYID,GROSS,NET,AMTINWRD,BIGST,BSGST,BCGST,NARRATION FROM DBNOTEBASIC WHERE DBNOTEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDebitNoteBillItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DBNOTEBASICID,INVNO,INVDT,ITEMMASTER.ITEMID,DBNOTEDETAIL.CONVFACTOR,DBNOTEDETAIL.PRIUNIT,DBNOTEDETAIL.QTY,DBNOTEDETAIL.RATE,DBNOTEDETAIL.AMOUNT,DBNOTEDETAIL.CGST,DBNOTEDETAIL.SGST,DBNOTEDETAIL.IGST,DBNOTEDETAIL.TOTAMT FROM DBNOTEDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID =DBNOTEDETAIL.ITEMID WHERE DBNOTEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditProEntry(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select  BRANCHMAST.BRANCHID,NET,DBNOTEBASIC.DOCID,DBNOTEBASIC.BRANCHID AS BRA,DBNOTEBASIC.PARTYID FROM DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID =DBNOTEBASIC.BRANCHID where DBNOTEBASIC.DBNOTEBASICID ='" + PROID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT GRNBLDETAILID,GRNBLBASICID,ITEMMASTER.ITEMID,GRNBLDETAIL.ITEMID AS item FROM GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=GRNBLDETAIL.ITEMID WHERE GRNBLBASICID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVocher()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT VCHTYPEID,DESCRIPTION FROM VCHTYPE WHERE DESCRIPTION='Debit Note'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPurRet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRETBASIC.RGRNNO,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy') DOCDAT,PRETBASIC.BRANCHID,PRETBASIC.PARTYID ,PARTYMAST.PARTYNAME,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASICID  from PRETBASIC  left outer join PARTYMAST on PARTYMAST.PARTYMASTID = PRETBASIC.PARTYID  where PRETBASIC.PRETBASICID=" + id + "";
            //SvSql = "Select PRETBASIC.REJBY,CURRENCY.MAINCURR ,BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,PRETBASIC.DOCID,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PRETBASIC.REFNO,to_char(PRETBASIC.REFDT,'dd-MON-yyyy')REFDT,LOCDETAILS.LOCID,PRETBASIC.EXCHANGERATE,PRETBASIC.REASONCODE,PRETBASIC.TEMPFIELD,PRETBASIC.RGRNNO,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASIC.NARR,PRETBASICID  from PRETBASIC left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID = PRETBASIC.BRANCHID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID = PRETBASIC.LOCID left outer join PARTYMAST on PARTYMAST.PARTYMASTID = PRETBASIC.PARTYID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=PRETBASIC.MAINCURRENCY LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PRETBASIC.TRANSITLOCID where PRETBASIC.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDNDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select T1SOURCEID,BRANCHID,PARTYID,CUSTACC,GROSS,NET,BIGST,BCGST,BSGST,ADSCHEME from DBNOTEBASIC where DBNOTEBASICID='" + id + "'";
            //SvSql = "Select PRETBASIC.REJBY,CURRENCY.MAINCURR ,BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,PRETBASIC.DOCID,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PRETBASIC.REFNO,to_char(PRETBASIC.REFDT,'dd-MON-yyyy')REFDT,LOCDETAILS.LOCID,PRETBASIC.EXCHANGERATE,PRETBASIC.REASONCODE,PRETBASIC.TEMPFIELD,PRETBASIC.RGRNNO,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASIC.NARR,PRETBASICID  from PRETBASIC left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID = PRETBASIC.BRANCHID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID = PRETBASIC.LOCID left outer join PARTYMAST on PARTYMAST.PARTYMASTID = PRETBASIC.PARTYID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=PRETBASIC.MAINCURRENCY LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PRETBASIC.TRANSITLOCID where PRETBASIC.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable AccconfigLst()
        {
            string SvSql = string.Empty;
            SvSql = "select ADSCHEME,ADCOMPHID from ADCOMPH where ADTRANSID='po' AND ACTIVE='Yes'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPurRetDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,PRETDETAIL.ITEMID as item,PRIQTY,CLSTOCK,PRETDETAIL.QTY,UNITMAST.UNITID,PRETDETAIL.RATE,PRETDETAIL.AMOUNT,PRETDETAIL.TOTAMT,PRETDETAIL.CF,PRETDETAIL.CGSTP,PRETDETAIL.CGST,PRETDETAIL.SGSTP,PRETDETAIL.SGST,PRETDETAIL.IGSTP,PRETDETAIL.IGST,PRETBASICID,PRETDETAILID  from PRETDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PRETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PRETDETAIL.UNIT  where PRETDETAIL.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurRetDoc(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRETBASIC.RGRNNO,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE from PRETBASIC where PRETBASIC.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select ACCOUNTGROUP,ACCGROUPID from ACCGROUP";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DISPLAY_NAME, LEDGERID,LEDNAME from ACCLEDGER where ACCGROUP='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCLEDGER.DISPLAY_NAME from PARTYMAST left outer join ACCLEDGER on LEDGERID=PARTYMAST.ACCOUNTNAME where PARTYMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllDebitNoteBill(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DBNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DBNOTEBASICID from DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DBNOTEBASIC.BRANCHID WHERE DBNOTEBASIC.IS_ACTIVE = 'Y' ORDER BY DBNOTEBASIC.DBNOTEBASICID DESC ";


            }
            else
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DBNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DBNOTEBASICID from DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DBNOTEBASIC.BRANCHID WHERE DBNOTEBASIC.IS_ACTIVE = 'N' ORDER BY DBNOTEBASIC.DBNOTEBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
