using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PaymentVoucherService : IPaymentVoucher
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PaymentVoucherService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetLocation(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select locdetails.LOCID ,EMPLOYEELOCATION.LOCID loc from EMPLOYEELOCATION  left outer join locdetails on locdetails.locdetailsid=EMPLOYEELOCATION.LOCID where EMPID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<PaymentVoucher> GetAllVoucher()
        {
            List<PaymentVoucher> cmpList = new List<PaymentVoucher>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  VOUCHERNO,to_char(PAYMENTVOUCHER.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,PAYMENTTYPE,PAYMENTVOUCHER.REFNO ,CURRENCY.MAINCURR,EXRATE ,PAYMENTVOUCHER.STATUS,PAYMENTVOUCHERID from PAYMENTVOUCHER LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PAYMENTVOUCHER.BRANCHID  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PAYMENTVOUCHER.LOCID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=PAYMENTVOUCHER.CURRENCY  where PAYMENTVOUCHER.STATUS='ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PaymentVoucher cmp = new PaymentVoucher
                        {
                            ID = rdr["PAYMENTVOUCHERID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            VoucherNo = rdr["VOUCHERNO"].ToString(),
                            Vdate = rdr["DOCDATE"].ToString(),
                            Currency = rdr["MAINCURR"].ToString(),
                            PType = rdr["PAYMENTTYPE"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string PaymentCRUD(PaymentVoucher cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                string VoucherID = datatrans.GetDataString("Select PAYMENTREQUESTID from PAYMENTREQUEST where PAYMENTREQUESTID='" + cy.ID + "' ");

                if (VoucherID != null)
                {
                    cy.ID = null;
                }
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM PAYMENTVOUCHER WHERE VOUCHERNO =LTRIM(RTRIM('" + cy.VoucherNo + "'))  ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = " VOUCHER NO Already Existed";
                        return msg;
                    }
                }
                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PVouc-' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "PVouc-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PVouc-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.VoucherNo = DocId;
                //if (cy.ID == null)
                //{
                //    double depit = cy.TotalDeAmount;
                //    double credit = cy.TotalCrAmount;
                //    double amount = depit - credit;
                //    if (cy.ReqAmount!=amount)
                //    {
                //        msg = "Please enter correct Amount";
                //        return msg;
                //    }

                //}
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PAYVOUCHERPROC", objConn);
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
                    objCmd.Parameters.Add("VOUCHERNO", OracleDbType.NVarchar2).Value = cy.VoucherNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Vdate;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("PAYMENTTYPE", OracleDbType.NVarchar2).Value = cy.PType;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("CURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("TOTCREDITAMT", OracleDbType.NVarchar2).Value = cy.TotalCrAmount;
                    objCmd.Parameters.Add("TOTDEPITAMT", OracleDbType.NVarchar2).Value = cy.TotalDeAmount;
                    objCmd.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cy.TotalAmount;
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
                        foreach (VoucherItem cp in cy.VoucherLst)
                        {
                            if (cp.Account != "0")
                            {
                              
                               svSQL = "Insert into PAYMENTVOUCHER (PAYMENTVOUCHERID,ACCTYPE,ACCTYPE,ACCNAME,ACCNAME,CREDIT_AMOUNT,CREDIT_AMOUNT,DEPIT_AMOUNT,DEPIT_AMOUNT) VALUES ('" + Pid + "','" + cp.Credit + "','" + cp.Cre + "','" + cp.Account + "','" + cp.Acc + "','" + cp.CreditAmount + "','" + cp.Cre + "','" + cp.DepitAmount + "','" + cp.DepAmount + "')";
                               OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                               objCmds.ExecuteNonQuery();

                            }
                               
                        }

                            
                    }
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    
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
        public DataTable EditVoucher(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select BRANCHID,REQUESTAMOUNT,PO_OR_GRN,AMOUNT,PARTYMAST.PARTYNAME from PAYMENTREQUEST left outer join PARTYMAST on PARTYMASTID=PAYMENTREQUEST.SUPPLIERID where PAYMENTREQUESTID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVoucherDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PAYMENTVOUCHERID,ACCTYPE,ACCNAME,CREDIT_AMOUNT,DEPIT_AMOUNT from PAYMENTVOUCHDET where PAYMENTVOUCHERID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVoucher()
        {
            string SvSql = string.Empty;
            SvSql = " select VCHTYPEID,DESCRIPTION from VCHTYPE where DESCRIPTION='Payment' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPaymentVoucherDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LEDGERNAME FROM  PARTYMAST WHERE PARTYNAME='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
