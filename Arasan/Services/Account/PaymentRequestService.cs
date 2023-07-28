using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;

namespace Arasan.Services 
{
    public class PaymentRequestService :IPaymentRequest
    {
        private readonly string _connectionString;
        public PaymentRequestService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPO(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID ,POBASICID from POBASIC where PARTYID='"+ id +"' ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select GROSS ,DOCID,GRNBLBASICID from GRNBLBASIC where DOCID='" + id + "'";  /*AND IS_ACCOUNT='N'*/
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPODetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select GROSS ,DOCID ,POBASICID from POBASIC where DOCID='" + id + "' AND STATUS IS NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<PaymentRequest> GetAllPaymentRequest()
        {
            List<PaymentRequest> cmpList = new List<PaymentRequest>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  DOCID,to_char(PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUEST.ACTIVE,PAYMENTREQUESTID from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND PAYMENTREQUEST.STATUS IS NULL";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PaymentRequest cmp = new PaymentRequest
                        {

                            ID = rdr["PAYMENTREQUESTID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Date = rdr["DOCDATE"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            status = rdr["ACTIVE"].ToString(),
                            GRN = rdr["PO_OR_GRN"].ToString(),
                            Type = rdr["TYPE"].ToString(),
                            Final = rdr["AMOUNT"].ToString(),
                            ReqBy = rdr["REQUESTEDBY"].ToString(),
                           


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string PaymentCRUD(PaymentRequest cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Date);
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("SUPPLIERID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.Type;
                    objCmd.Parameters.Add("PO_OR_GRN", OracleDbType.NVarchar2).Value = cy.GRN;
                    objCmd.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cy.Amount;
                    objCmd.Parameters.Add("FINAL_AMOUNT", OracleDbType.NVarchar2).Value = cy.Final;
                    objCmd.Parameters.Add("REQUESTEDBY", OracleDbType.NVarchar2).Value = cy.ReqBy;
                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "YES";
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
            SvSql = "Select  DOCID,to_char( PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,AMOUNT,REQUESTEDBY,PAYMENTREQUEST.ACTIVE,PAYMENTREQUEST.AMOUNT,PAYMENTREQUESTID from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND PAYMENTREQUESTID='" + id +"' ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;


        }
        public DataTable EditPaymentRequest(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select  DOCID,to_char( PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,SUPPLIERID,PAYMENTREQUEST.TYPE,PO_OR_GRN,FINAL_AMOUNT,REQUESTEDBY,PAYMENTREQUEST.ACTIVE,PAYMENTREQUEST.AMOUNT,PAYMENTREQUESTID from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND PAYMENTREQUESTID='" + id + "' ";
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
                    svSQL = "UPDATE PAYMENTREQUEST SET STATUS ='Approved' ,ACTIVE='NO' WHERE PAYMENTREQUESTID='" + cy.ID + "'";
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
        public string StatusChange(string tag, int id)
        {

            try
            {

                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PAYMENTREQUEST SET ACTIVE ='NO' WHERE STORESREQBASICID='" + id + "'";
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
                    cmd.CommandText = "Select  DOCID,to_char( PAYMENTREQUEST.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,PAYMENTREQUEST.TYPE,PO_OR_GRN,FINAL_AMOUNT,REQUESTEDBY,PAYMENTREQUEST.ACTIVE,PAYMENTREQUESTID from PAYMENTREQUEST LEFT OUTER JOIN  PARTYMAST on PAYMENTREQUEST.SUPPLIERID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND PAYMENTREQUEST.STATUS ='Approved'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PaymentRequest cmp = new PaymentRequest
                        {

                            ID = rdr["PAYMENTREQUESTID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Date = rdr["DOCDATE"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            status = rdr["ACTIVE"].ToString(),
                            GRN = rdr["PO_OR_GRN"].ToString(),
                            Type = rdr["TYPE"].ToString(),
                            Final = rdr["FINAL_AMOUNT"].ToString(),
                            ReqBy = rdr["REQUESTEDBY"].ToString(),



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
    }
}
