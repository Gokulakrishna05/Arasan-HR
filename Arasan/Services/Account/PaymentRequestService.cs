using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;

namespace Arasan.Services 
{
    public class PaymentRequestService : IPaymentRequest
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PaymentRequestService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME FROM PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            //SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetGRN(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID ,GRNBLBASICID from GRNBLBASIC where PARTYID='"+ id +"'";
            //SvSql = "Select DOCID ,GRNBLBASICID from GRNBLBASIC ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPO(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID ,POBASICID from POBASIC where PARTYID='"+ id +"' ";
           // SvSql = "Select DOCID ,POBASICID from POBASIC ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select GRNBLBASIC.NET,DOCID,GRNBLBASICID from GRNBLBASIC where GRNBLBASICID='" + id + "'";  /*AND IS_ACCOUNT='N'*/
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPODetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select NET,DOCID ,POBASICID from POBASIC where POBASICID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


       // public IEnumerable<PaymentRequest> GetAllPaymentRequest()
       // {
       //     List<PaymentRequest> cmpList = new List<PaymentRequest>();
       //     using (OracleConnection con = new OracleConnection(_connectionString))
       //     {

       //         using (OracleCommand cmd = con.CreateCommand())
       //         {
       //             con.Open();
       //             cmd.CommandText = "Select  DOCID,to_char(PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUEST.IS_ACTIVE,PAYMENTREQUESTID,PAYMENTREQUEST.IS_APPROVED from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID  Where  PAYMENTREQUEST.IS_ACTIVE = 'Y' ORDER BY PAYMENTREQUESTID DESC";
       //             OracleDataReader rdr = cmd.ExecuteReader();
       //             while (rdr.Read())
       //             {
       //                 PaymentRequest cmp = new PaymentRequest
       //                 {
       //                     ID = rdr["PAYMENTREQUESTID"].ToString(),
       //                     DocId = rdr["DOCID"].ToString(),
       //                     Date = rdr["DOCDATE"].ToString(),
       //                     Supplier = rdr["PARTYNAME"].ToString(),
       //                     status = rdr["IS_ACTIVE"].ToString(),
       //                     GRN = rdr["PO_OR_GRN"].ToString(),
       //                     Type = rdr["TYPE"].ToString(),
       //                     Final = rdr["AMOUNT"].ToString(),
       //                     ReqBy = rdr["REQUESTEDBY"].ToString(),
							//Approve = rdr["IS_APPROVED"].ToString()
                           
       //                 };
       //                 cmpList.Add(cmp);
       //             }
       //         }
       //     }
       //     return cmpList;
       // }
        public string PaymentCRUD(PaymentRequest cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                
				if (cy.ID == null)
				{

					int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PAYREQ-' AND ACTIVESEQUENCE = 'T'  ");
					string DocId = string.Format("{0}{1}", "PAYREQ-", (idc + 1).ToString());

					string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PAYREQ-' AND ACTIVESEQUENCE ='T'  ";
					try
					{
						datatrans.UpdateStatus(updateCMd);
					}
					catch (Exception ex)
					{
						throw ex;
					}
					cy.DocId = DocId;
				}
				using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PAYMENTREQPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "LOCATIONPROC";*/

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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    //objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = cy.Date;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Date;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.Type;
                    objCmd.Parameters.Add("SUPPLIERID", OracleDbType.NVarchar2).Value = cy.Supplier; 
                    objCmd.Parameters.Add("PO_OR_GRN", OracleDbType.NVarchar2).Value = cy.GRN;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cy.Amount;
                    objCmd.Parameters.Add("REQUESTAMOUNT", OracleDbType.NVarchar2).Value = cy.Final;
                    objCmd.Parameters.Add("REQUESTEDBY", OracleDbType.NVarchar2).Value = cy.ReqBy;
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.createdby;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    // objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "YES";
                    //objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Approved";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
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
        public DataTable EditPayment(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select  DOCID,to_char( PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTAMOUNT,REQUESTEDBY,PAYMENTREQUEST.IS_ACTIVE,PAYMENTREQUEST.AMOUNT,PAYMENTREQUESTID from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID  Where  PAYMENTREQUESTID='" + id +"' ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;


        }
        public DataTable EditPaymentRequest(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select  DOCID,to_char( PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,SUPPLIERID,PAYMENTREQUEST.TYPE,PO_OR_GRN,REQUESTAMOUNT,REQUESTEDBY,PAYMENTREQUEST.IS_ACTIVE,PAYMENTREQUEST.AMOUNT,PAYMENTREQUESTID from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID  Where  PAYMENTREQUESTID='" + id + "' ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public string PaymentApprove(PaymentRequest cy)
        {

            try
            {

                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PAYMENTREQUEST SET IS_APPROVED ='Y' WHERE PAYMENTREQUESTID='" + cy.ID + "'";
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
        //public string StatusChange(string tag, int id)
        //{
        //    try
        //    {
        //        string svSQL = string.Empty;
        //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
        //        {
        //            //svSQL = "UPDATE PAYMENTREQUEST SET ACTIVE ='NO' WHERE STORESREQBASICID='" + id + "'";
        //            svSQL = "UPDATE PAYMENTREQUEST SET ACTIVE ='NO' WHERE PAYMENTREQUESTID='" + id + "' ";
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
        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PAYMENTREQUEST SET IS_ACTIVE ='N' WHERE PAYMENTREQUESTID='" + id + "' ";
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
        public IEnumerable<PaymentRequest> GetAllApprovePaymentRequest()
        {
            List<PaymentRequest> cmpList = new List<PaymentRequest>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  DOCID,to_char( PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,REQUESTAMOUNT,REQUESTEDBY,PAYMENTREQUESTID,PAYMENTREQUEST.IS_ACTIVE from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID   AND PAYMENTREQUEST.IS_APPROVED ='Y' AND PAYMENTREQUEST.IS_ACTIVE='Y' ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PaymentRequest cmp = new PaymentRequest
                        {

                            ID = rdr["PAYMENTREQUESTID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Date = rdr["DOCDATE"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            status = rdr["IS_ACTIVE"].ToString(),
                            GRN = rdr["PO_OR_GRN"].ToString(),
                            Type = rdr["TYPE"].ToString(),
                            Final = rdr["REQUESTAMOUNT"].ToString(),
                            ReqBy = rdr["REQUESTEDBY"].ToString(),



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

 


 
        public DataTable GetAllrequest(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select DOCID,to_char(PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUESTID,PAYMENTREQUEST.IS_APPROVED from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID Where PAYMENTREQUEST.IS_ACTIVE = 'Y' ORDER BY PAYMENTREQUESTID DESC ";

            }
            else
            {
                SvSql = "Select DOCID,to_char(PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUESTID,PAYMENTREQUEST.IS_APPROVED from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID Where PAYMENTREQUEST.IS_ACTIVE = 'N' ORDER BY PAYMENTREQUESTID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
 
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllpayrequests(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select DOCID,to_char(PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUESTID,PAYMENTREQUEST.IS_APPROVED from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID Where PAYMENTREQUEST.IS_ACTIVE = 'Y' ORDER BY PAYMENTREQUESTID DESC ";

            }
            else
            {
                SvSql = "Select DOCID,to_char(PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUESTID,PAYMENTREQUEST.IS_APPROVED from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID Where PAYMENTREQUEST.IS_ACTIVE = 'N' ORDER BY PAYMENTREQUESTID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);

 
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPaymentRequestDetail(string id,string type)
        {
            string SvSql = string.Empty;
            if (type == "Advance Payment")
            {
                SvSql = "Select DOCID,TYPE,PAYMENTREQUESTID,REQUESTAMOUNT,E.EMPNAME,P.PO_OR_GRN,to_char(P.DOCDATE,'dd-MON-yyyy') DOCDATE from PAYMENTREQUEST P,EMPMAST E where P.REQUESTEDBY=E.EMPMASTID AND P.T1SOURCEID='" + id + "' AND P.TYPE='" + type + "'";
            }
            else
            {
                SvSql = "Select DOCID,TYPE,PAYMENTREQUESTID,REQUESTAMOUNT,E.EMPNAME,P.PO_OR_GRN,to_char(P.DOCDATE,'dd-MON-yyyy') DOCDATE from PAYMENTREQUEST P,EMPMAST E where P.REQUESTEDBY=E.EMPMASTID AND P.TYPE='Advance Payment' AND P.T1SOURCEID=(select POBASICID from GRNBLBASIC where GRNBLBASICId='" + id + "') " +
                        "UNION Select DOCID,TYPE,PAYMENTREQUESTID,REQUESTAMOUNT,E.EMPNAME,P.PO_OR_GRN,to_char(P.DOCDATE,'dd-MON-yyyy') DOCDATE from PAYMENTREQUEST P, EMPMAST E where P.REQUESTEDBY = E.EMPMASTID AND P.TYPE = 'Against Invoice' AND P.T1SOURCEID = '" + id + "'";
            }
            
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPaymentRequestDetail1(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYID,DOCID,POBASICID from POBASIC where PARTYID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
 
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
 

        //public DataTable GetPaymentRequestDetail2(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT PARTYID,POBASICID from GRNBLBASIC where POBASICID='" + id + "'";
        //    DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
 
    }
}
