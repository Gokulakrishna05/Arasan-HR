using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales
{
    public class SalesReturnService : ISalesReturn
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesReturnService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<SalesReturn> GetAllSalesReturn(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }
            List<SalesReturn> cmpList = new List<SalesReturn>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = " Select SALERETBASIC.DOCID,to_char(SALERETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYNAME,BRANCHMAST.BRANCHID,SALERETBASICID,SALERETBASIC.STATUS from SALERETBASIC  left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=SALERETBASIC.BRANCHID  where SALERETBASIC.STATUS='"+ status+ "'  order by SALERETBASIC.SALERETBASICID DESC ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SalesReturn cmp = new SalesReturn
                        {
                            ID = rdr["SALERETBASICID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            custname = rdr["PARTYNAME"].ToString(),
                            //Location = rdr["LOCID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            status = rdr["STATUS"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string SalesReturnCRUD(SalesReturn cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Srt-' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "Srt-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Srt-' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = docid;
            
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SALESRETPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

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
                    
                    objCmd.Parameters.Add("INVOICENO", OracleDbType.NVarchar2).Value = cy.invoiceid;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.location;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.custname;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("INVOICEDATE", OracleDbType.NVarchar2).Value = cy.invoicedate;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.net;
                    objCmd.Parameters.Add("TRANSITLOCID", OracleDbType.NVarchar2).Value = cy.transitlocation;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.Vtype;

                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
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
                        if (cy.returnlist != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (SalesReturnItem cp in cy.returnlist)
                                {
                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");

                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        svSQL = "Insert into SALERETDETAIL (SALERETBASICID,QTY,QTYSOLD,ITEMID,RATE,AMOUNT,UNIT,TARIFFID,TOTAMT,INVDT,INVNO,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,EXCISETYPE,DISCOUNT) VALUES ('" + Pid + "','" + cp.quantity + "','" + cp.soldqty + "','" + cp.itemid + "','" + cp.rate + "','" + cp.amount + "','" + UnitId + "','" + cp.traiffid + "','" + cp.totalamount + "','" + cy.invoicedate + "','" + cy.invoiceid + "','" + cp.cgstper + "','" + cp.cgstamt + "','" + cp.sgstper + "','" + cp.sgstamt + "','" + cp.igstper + "','" + cp.igstamt + "','" + cp.exicetype + "','" + cp.disc + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete SALERETDETAIL WHERE SALERETBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (SalesReturnItem cp in cy.returnlist)
                                {

                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        svSQL = "Insert into SALERETDETAIL (SALERETBASICID,QTY,QTYSOLD,ITEMID,RATE,AMOUNT,UNIT,TARIFFID,TOTAMT,INVDT,INVNO,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,EXCISETYPE,DISCOUNT) VALUES ('" + Pid + "','" + cp.quantity + "','" + cp.soldqty + "','" + cp.itemid + "','" + cp.rate + "','" + cp.amount + "','" + UnitId + "','" + cp.traiffid + "','" + cp.totalamount + "','" + cy.invoicedate + "','" + cy.invoiceid + "','" + cp.cgstper + "','" + cp.cgstamt + "','" + cp.sgstper + "','" + cp.sgstamt + "','" + cp.igstper + "','" + cp.igstamt + "','" + cp.exicetype + "','" + cp.disc + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
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
        public DataTable GetInvoice()
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,PINVBASICID from PINVBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetInvoiceDetails(string invoiceid)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYNAME,to_char(PINVBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,GROSS,NET from PINVBASIC WHERE PINVBASICId='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetInvoiceItem(string invoiceid)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PINVDETAIL.PINVDETAILID,PINVDETAIL.ITEMID as itemi,UNITMAST.UNITID,QTY,RATE,AMOUNT,ITEMMASTER.ITEMID,DISCOUNT,DISCPER,FREIGHT,CGSTP,CGST,SGSTP,SGST,IGSTP,IGST,TOTAMT,PINVDETAIL.UNIT,PINVDETAIL.EXCISETYPE,PINVDETAIL.TARIFFID FROM PINVDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINVDETAIL.ITEMID  LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINVDETAIL.UNIT WHERE PINVBASICId ='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesRetDetails(string invoiceid)
        {
            string SvSql = string.Empty;
            SvSql = "select SALERETBASICID,SALERETDETAIL.QTY,QTYSOLD,SALERETDETAIL.ITEMID as item,ITEMMASTER.ITEMID,SALERETDETAIL.RATE,SALERETDETAIL.AMOUNT,UNITMAST.UNITID,SALERETDETAIL.TARIFFID,TOTAMT,INVDT,INVNO,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,SALERETDETAIL.EXCISETYPE,DISCOUNT from SALERETDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALERETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=SALERETDETAIL.UNIT WHERE SALERETBASICID='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesRet(string invoiceid)
        {
            string SvSql = string.Empty;
            SvSql = "select SALERETBASICID,INVOICENO,SALERETBASIC.DOCID,SALERETBASIC.BRANCHID,SALERETBASIC.PARTYNAME,SALERETBASIC.LOCID,SALERETBASIC. PARTYNAME,to_char(SALERETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(SALERETBASIC.REFDT,'dd-MON-yyyy')REFDT,to_char(SALERETBASIC.INVOICEDATE,'dd-MON-yyyy')INVOICEDATE,SALERETBASIC.GROSS,SALERETBASIC.NET,TRANSITLOCID,SALERETBASIC.NARRATION,SALERETBASIC.TYPE from SALERETBASIC  WHERE SALERETBASICID='" + invoiceid + "'";
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
                    svSQL = "UPDATE SALERETBASIC SET STATUS ='INACTIVE' WHERE SALERETBASICID='" + id + "'";
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
        //public DataTable GetSalesReturn(string invoiceid)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select SALERETBASICID,INVOICENO,SALERETBASIC.DOCID,SALERETBASIC.BRANCHID,SALERETBASIC.LOCID,SALERETBASIC. PARTYNAME,to_char(SALERETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(SALERETBASIC.REFDT,'dd-MON-yyyy')REFDT,to_char(SALERETBASIC.INVOICEDATE,'dd-MON-yyyy')INVOICEDATE,SALERETBASIC.GROSS,SALERETBASIC.NET,TRANSITLOCID,SALERETBASIC.NARRATION,SALERETBASIC.TYPE from SALERETBASIC  WHERE SALERETBASICID='" + invoiceid + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public DataTable GetRetByName(string invoiceid)
        {
            string SvSql = string.Empty;
            //SvSql = "Select BRANCHMAST.BRANCHID, ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy') ENQ_DATE ,PARTYRCODE.PARTY,SALES_ENQUIRY.CURRENCY_TYPE,SALES_ENQUIRY.CONTACT_PERSON,SALES_ENQUIRY.CUSTOMER_TYPE,SALES_ENQUIRY.ENQ_TYPE,SALES_ENQUIRY.ADDRESS,SALES_ENQUIRY.CITY,SALES_ENQUIRY.PINCODE,PRIORITY,SALES_ENQUIRY.SALESENQUIRYID,SALES_ENQUIRY.STATUS from SALES_ENQUIRY  LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=SALES_ENQUIRY.BRANCH_ID LEFT OUTER JOIN  PARTYMAST on SALES_ENQUIRY.CUSTOMER_NAME=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.PARTY Where PARTYMAST.TYPE IN ('Customer','BOTH') AND SALES_ENQUIRY.SALESENQUIRYID='" + name + "'";
            SvSql = "select SALERETBASICID,INVOICENO,SALERETBASIC.DOCID,SALERETBASIC.BRANCHID,SALERETBASIC.PARTYNAME,SALERETBASIC.LOCID,SALERETBASIC. PARTYNAME,to_char(SALERETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(SALERETBASIC.REFDT,'dd-MON-yyyy')REFDT,to_char(SALERETBASIC.INVOICEDATE,'dd-MON-yyyy')INVOICEDATE,SALERETBASIC.GROSS,SALERETBASIC.NET,TRANSITLOCID,SALERETBASIC.NARRATION,SALERETBASIC.TYPE,SALERETBASIC.NARRATION from SALERETBASIC  WHERE SALERETBASICID='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetRetItem(string invoiceid)
        {
            string SvSql = string.Empty;
            //SvSql = "Select SALES_ENQ_ITEM.SALESENQITEMID,SALES_ENQ_ITEM.SAL_ENQ_ID,SALES_ENQ_ITEM.QUANTITY,ITEMMASTER.ITEMID,SALES_ENQ_ITEM.UNIT,SALES_ENQ_ITEM.ITEM_DESCRIPTION from SALES_ENQ_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALES_ENQ_ITEM.ITEM_ID   where SALES_ENQ_ITEM.SALESENQITEMID='" + name + "'";
            SvSql = "select SALERETBASICID,SALERETDETAIL.QTY,QTYSOLD,SALERETDETAIL.ITEMID as item,ITEMMASTER.ITEMID,SALERETDETAIL.RATE,SALERETDETAIL.AMOUNT,UNITMAST.UNITID,SALERETDETAIL.TARIFFID,TOTAMT,INVDT,INVNO,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,SALERETDETAIL.EXCISETYPE,DISCOUNT from SALERETDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALERETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=SALERETDETAIL.UNIT WHERE SALERETBASICID='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
