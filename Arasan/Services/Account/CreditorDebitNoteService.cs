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

        public string DebitNoteBillCRUD(CreditorDebitNote cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                string Location = "12418000000423";
                //DataTable dtt = new DataTable();
                //dtt = datatrans.GetData("select DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,BRANCHID,PARTYID from DBNOTEBASIC where DBNOTEBASICID='" + cy.ID + "'");
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
                                double netamt = 0;

                                grossamt = Convert.ToDouble(cy.amount);
                                netamt = Convert.ToDouble(cy.amount);
                                string type = "dn";
                                if(cy.VType== "CreditNote")
                                {
                                    type = "cn";
                                }


                                foreach (CreDebNoteItems cp in cy.NoteLst)
                                {
                                    t2cunt += 1;
                                 
                                        //grossamt = cp.CRAmount;
                                        //Grossledger = cp.Ledgername;
                                  
                                    
                                }
                                //DataTable dtv = datatrans.GetData("select PREFIX,LASTNO from Sequence where TRANSTYPE='vchdn' AND ACTIVESEQUENCE='T' ");
                                //string vNo = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["LASTNO"].ToString();
                                //string mno = DateTime.Now.ToString("yyyyMM");
                                //long TRANS1 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS1'");
                                //command.CommandText = "Insert into TRANS1 (TRANS1ID,APPROVAL,T1SOURCEID,VCHSTATUS,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,MSTATUS,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,TRANSID,BRANCHID,LOCID,CANCEL,MAXAPPROVED,LATEMPLATEID,T1VCHDT,T1NARR,T2COUNT,POSTYN,BRANCHMASTID,VTYPE,GENERATED,AMTWD,USERNAME,MODIFIEDON,T1SID) VALUES " +
                                //    "('" + TRANS1 + "','0','" + cy.ID + "','N','"+ type + "','" + cy.number + "','1','1','" + cy.Ref + "','" + cy.Refdate + "','" + mno + "','a','" + Grossledger + "','" + grossamt + "','" + cy.mid + "','" + netamt + "','','" + netamt + "','vchdn','" + cy.branchid + "','" + Location + "','F','0','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Voucher + "','" + t2cunt + "','Y','0','R','T','" + cy.Amtinwords + "','" + cy.createdby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','0')";
                                //command.ExecuteNonQuery();
                                //foreach (CreDebNoteItems cp in cy.NoteLst)
                                //{
                                //    string mledger = "";
                                //    //if (cp.TypeName == "NET")
                                //    //{
                                //    //    mledger = Grossledger;
                                //    //}
                                //    //else
                                //    //{
                                //    //    mledger = cy.mid;
                                //    //}
                                //    long TRANS2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                //    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2 + "','" + TRANS1 + "','" + cp.CRDR + "','" + cp.Ledgername + "','" + cp.DRAmount + "','" + cp.DRAmount + "','" + cp.CRAmount + "','" + cp.CRAmount + "','1','1','" + cp.DRAmount + "','" + cp.CRAmount + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','dn','N','F','" + mledger + "')";
                                //    command.ExecuteNonQuery();
                                //    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2 + 1).ToString() + "' where NAME='TRANS2'");

                                //}
                                //string updatetrans = " UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS1 + 1).ToString() + "' where NAME='TRANS1'";
                                //datatrans.UpdateStatus(updatetrans);


                                //command.CommandText = "UPDATE DBNOTEBASIC SET IS_ACCOUNTED ='Y' WHERE DBNOTEBASICID='" + cy.ID + "'";
                                //command.ExecuteNonQuery();

                                /////////////////transaction
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


    }
}
