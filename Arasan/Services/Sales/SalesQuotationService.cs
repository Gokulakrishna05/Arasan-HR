using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales
{
    public class SalesQuotationService : ISalesQuotationService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesQuotationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<SalesQuotation> GetAllSalesQuotation(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "Active";
            }
            List<SalesQuotation> cmpList = new List<SalesQuotation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    //cmd.CommandText = "Select BRANCHID,QUOTE_NO,to_char(QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy')ENQ_DATE,CURRENCY_TYPE,SQ.CUSTOMER,SQ.CUSTOMER_TYPE,SQ.ADDRESS,SQ.CITY,SQ.CONTACT_PERSON_MOBILE,SQ.CONTACT_PERSON_MAIL,SQ.PINCODE,SQ.PRIORITY,SQ.ASSIGNED_TO,SQ.SALESQUOTEID,SQ.STATUS,PARTYRCODE.PARTY from SALES_QUOTE SQ LEFT OUTER JOIN  PARTYMAST on SQ.CUSTOMER=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.PARTY Where PARTYMAST.TYPE IN ('Customer','BOTH') AND  SQ.STATUS='" + status + "' order by SQ.SALESQUOTEID DESC ";
                    // cmd.CommandText = "Select BRANCHID,QUOTE_NO,to_char(QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,SQ.ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy')ENQ_DATE,SQ.CURRENCY_TYPE,SQ.CUSTOMER,SQ.CUSTOMER_TYPE,SQ.ADDRESS,SQ.CITY,SQ.CONTACT_PERSON_MOBILE,SQ.CONTACT_PERSON_MAIL,SQ.PINCODE,SQ.PRIORITY,SQ.ASSIGNED_TO,SQ.SALESQUOTEID,SQ.STATUS,PARTYRCODE.PARTY from SALES_QUOTE SQ LEFT OUTER JOIN  PARTYMAST on SQ.CUSTOMER=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.PARTY ";
                    //cmd.CommandText = "Select SALESQUOTEID,BRANCHID,QUOTE_NO,to_char(QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,SQ.CURRENCY_TYPE,SQ.CUSTOMER,SQ.CUSTOMER_TYPE,SQ.ADDRESS,SQ.CITY,SQ.CONTACT_PERSON_MOBILE,SQ.CONTACT_PERSON_MAIL,SQ.PINCODE,SQ.PRIORITY,SQ.ASSIGNED_TO,SQ.SALESQUOTEID,SQ.STATUS,PARTYRCODE.PARTY from SALES_QUOTE SQ LEFT OUTER JOIN  PARTYMAST on SQ.CUSTOMER=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.PARTY ";
                    cmd.CommandText = "Select SALESQUOTEID,BRANCHID,QUOTE_NO,to_char(QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,SQ.CURRENCY_TYPE,SQ.CUSTOMER,SQ.CUSTOMER_TYPE,SQ.ADDRESS,SQ.CITY,SQ.CONTACT_PERSON_MOBILE,SQ.CONTACT_PERSON_MAIL,SQ.PINCODE,SQ.PRIORITY,SQ.ASSIGNED_TO,SQ.SALESQUOTEID,SQ.STATUS from SALES_QUOTE SQ LEFT OUTER JOIN  PARTYMAST on SQ.CUSTOMER=PARTYMAST.PARTYMASTID   ";

                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SalesQuotation cmp = new SalesQuotation
                        {
                            ID = rdr["SALESQUOTEID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoId = rdr["QUOTE_NO"].ToString(),
                            QuoDate = rdr["QUOTE_DATE"].ToString(),
                            //EnNo = rdr["ENQ_NO"].ToString(),
                            //EnDate = rdr["ENQ_DATE"].ToString(),
                            Currency = rdr["CURRENCY_TYPE"].ToString(),
                            Customer = rdr["CUSTOMER"].ToString(),
                            //Customer = rdr["PARTY"].ToString(),
                            CustomerType = rdr["CUSTOMER_TYPE"].ToString(),
                            Address = rdr["ADDRESS"].ToString(),
                            City = rdr["CITY"].ToString(),
                            Mobile = rdr["CONTACT_PERSON_MOBILE"].ToString(),
                            Gmail = rdr["CONTACT_PERSON_MAIL"].ToString(),
                            PinCode = rdr["PINCODE"].ToString(),
                            Pro = rdr["PRIORITY"].ToString(),
                            Assign = rdr["ASSIGNED_TO"].ToString(),
                            status = rdr["STATUS"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetItemCF(string ItemId, string Unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + Unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
        public DataTable GetCusType()
        {
            string SvSql = string.Empty;
            SvSql = "Select CUSTOMER_TYPE,CUSTOMERTYPEID From CUSTOMERTYPE";
            //SvSql = "Select CUSTOMERTYPE.CUSTOMER_TYPE,SALES_ENQUIRY.SALESENQUIRYID From CUSTOMERTYPE  LEFT OUTER JOIN SALES_ENQUIRY ON SALES_ENQUIRY.SALESENQUIRYID=CUSTOMERTYPE.CUSTOMERTYPEID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCustomerDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select CITY,PINCODE,ADD1,ADD2,ADD3,INTRODUCEDBY from PARTYMAST Where PARTYMAST.PARTYMASTID='" + id + "' ";
            //SvSql = "Select PARTYMASTID,PARTYNAME from PARTYMAST ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEnqDetails(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "select ENQ_NO,ENQ_TYPE,ENQ_DATE,SALES_ENQUIRY.CURRENCY_TYPE,CUSTOMER_NAME,SALESENQUIRYID from SALES_ENQUIRY WHERE SALES_ENQUIRY.SALESENQUIRYID = '" + id + "' ";
            SvSql = "select ENQ_NO,ENQ_TYPE,ENQ_DATE,SALES_ENQUIRY.CURRENCY_TYPE,CUSTOMER_NAME,CUSTOMER_TYPE,SALES_ENQUIRY.ADDRESS,SALES_ENQUIRY.CITY,SALES_ENQUIRY.PINCODE,SALES_ENQUIRY.CONTACT_PERSON_MOBILE ,SALES_ENQUIRY.PRIORITY,SALESENQUIRYID from SALES_ENQUIRY WHERE SALES_ENQUIRY.SALESENQUIRYID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemgrpDetail(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "SELECT ITEMID,ITEMDESC,RATE,QTY,CF,UNIT,AMOUNT,DISC,DISCAMOUNT,IFREIGHTCH,TOTAMT,CGSTPER,SGSTPER,IGSTPER,SGSTAMT,IGSTAMT,CGSTAMT,SALESQUOTEDETAIL.SALESQUOTEDETAILID,SALES_ENQUIRY.SALESENQUIRYID FROM SALESQUOTEDETAIL LEFT OUTER JOIN  SALES_ENQUIRY ON SALESENQUIRYID=SALESQUOTEDETAIL.SALESQUOTEDETAILID WHERE SALESQUOTEDETAIL.SALESQUOTEDETAILID = '" + id + "' ";
            //SvSql = "SELECT ITEMID,ITEMDESC,UNIT,QTY FROM SALESQUOTEDETAIL LEFT OUTER JOIN  SALES_ENQUIRY ON SALESENQUIRYID=SALESQUOTEDETAIL.SALESQUOTEDETAILID WHERE SALESQUOTEDETAIL.SALESQUOTEDETAILID = '" + id + "' ";
            SvSql = "SELECT ITEMMASTER.ITEMID,ITEM_DESCRIPTION,UNIT,QUANTITY,RATE,AMOUNT,CF FROM SALES_ENQ_ITEM LEFT OUTER JOIN  ITEMMASTER ON ITEMMASTERID=SALES_ENQ_ITEM.ITEM_ID LEFT OUTER JOIN  SALES_ENQUIRY ON SALESENQUIRYID=SALES_ENQ_ITEM.SALESENQITEMID WHERE SALES_ENQ_ITEM.SALESENQITEMID   = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQuobyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_QUOTE.SALESQUOTEID,ENQ_TYPE,SALESENQUIRYID from SALES_ENQUIRY LEFT OUTER JOIN SALES_QUOTE ON SALESQUOTEID=SALES_ENQUIRY.SALESENQUIRYID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCurrbyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_QUOTE.SALESQUOTEID,SALES_ENQUIRY.CURRENCY_TYPE,SALESENQUIRYID from SALES_ENQUIRY LEFT OUTER JOIN SALES_QUOTE ON SALESQUOTEID=SALES_ENQUIRY.SALESENQUIRYID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";
            //SvSql = " SELECT MAINCURR, CURRENCY.CURRENCYID,SALES_ENQUIRY.SALESENQUIRYID FROM CURRENCY LEFT OUTER JOIN SALES_ENQUIRY ON SALES_ENQUIRY.SALESENQUIRYID=CURRENCY.CURRENCYID WHERE SALES_ENQUIRY.SALESENQUIRYID = '" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCustypebyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_QUOTE.SALESQUOTEID,SALES_ENQUIRY.CUSTOMER_TYPE,SALESENQUIRYID from SALES_ENQUIRY LEFT OUTER JOIN SALES_QUOTE ON SALESQUOTEID=SALES_ENQUIRY.SALESENQUIRYID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";
            //SvSql = "Select CUSTOMERTYPE.CUSTOMER_TYPE,SALES_ENQUIRY.SALESENQUIRYID From CUSTOMERTYPE  LEFT OUTER JOIN SALES_ENQUIRY ON SALES_ENQUIRY.SALESENQUIRYID=CUSTOMERTYPE.CUSTOMERTYPEID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTypelstbyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_QUOTE.SALESQUOTEID,SALES_ENQUIRY.CUSTOMER_NAME,SALESENQUIRYID from SALES_ENQUIRY LEFT OUTER JOIN SALES_QUOTE ON SALESQUOTEID=SALES_ENQUIRY.SALESENQUIRYID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPribyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_QUOTE.SALESQUOTEID,SALES_ENQUIRY.PRIORITY,SALESENQUIRYID from SALES_ENQUIRY LEFT OUTER JOIN SALES_QUOTE ON SALESQUOTEID=SALES_ENQUIRY.SALESENQUIRYID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        //public DataTable GetItemgroupbyId(string id)

        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT ITEMGROUP,ITEMID,ITEMDESC,RATE,QTY,CF,UNIT,AMOUNT,DISC,DISCAMOUNT,IFREIGHTCH,TOTAMT,CGSTPER,SGSTPER,IGSTPER,SGSTAMT,IGSTAMT,CGSTAMT FROM SALESQUOTEDETAIL LEFT OUTER JOIN  SALES_ENQUIRY ON SALESENQUIRYID=SALESQUOTEDETAIL.SALESQUOTEDETAILID WHERE SALESQUOTEDETAIL.SALESQUOTEDETAILID='" + id + "'";

        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable Getcountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRY,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string SalesQuotationCRUD(SalesQuotation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'SQUO#' AND ACTIVESEQUENCE = 'T'");
                string QuoId = string.Format("{0}{1}", "SQUO#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SQUO#' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.QuoId = QuoId;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SALESQUOPROC", objConn);
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


                    //objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("QUOTE_NO", OracleDbType.NVarchar2).Value = cy.QuoId;
                    objCmd.Parameters.Add("QUOTE_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.QuoDate);
                    objCmd.Parameters.Add("CURRENCY_TYPE", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("CUSTOMER", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("CUSTOMER_TYPE", OracleDbType.NVarchar2).Value = cy.CustomerType;
                    objCmd.Parameters.Add("ADDRESS", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
                    objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                    objCmd.Parameters.Add("CONTACT_PERSON_MAIL", OracleDbType.NVarchar2).Value = cy.Gmail;
                    objCmd.Parameters.Add("CONTACT_PERSON_MOBILE", OracleDbType.NVarchar2).Value = cy.Mobile;
                    objCmd.Parameters.Add("PRIORITY", OracleDbType.NVarchar2).Value = cy.Pro;
                    objCmd.Parameters.Add("ASSIGNED_TO", OracleDbType.NVarchar2).Value = cy.Emp;

                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    //objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        foreach (QuoItem cp in cy.QuoLst)
                        {
                            if (cp.Isvalid == "Y" && cp.itemid != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("SALESQUOTEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("SALESQUOTEDETAILID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                    objCmds.Parameters.Add("ITEMDESC", OracleDbType.NVarchar2).Value = cp.des;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.unit;
                                    objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.confac;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.quantity;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.amount;
                                    
                                    objCmds.Parameters.Add("DISC", OracleDbType.NVarchar2).Value = cp.disc;
                                    objCmds.Parameters.Add("DISCAMOUNT", OracleDbType.NVarchar2).Value = cp.discamount;
                                    objCmds.Parameters.Add("IFREIGHTCH", OracleDbType.NVarchar2).Value = cp.frigcharge;
                                    objCmds.Parameters.Add("CGSTPER", OracleDbType.NVarchar2).Value = cp.cgstp;
                                    objCmds.Parameters.Add("SGSTPER", OracleDbType.NVarchar2).Value = cp.sgstp;
                                    objCmds.Parameters.Add("IGSTPER", OracleDbType.NVarchar2).Value = cp.igstp;
                                    objCmds.Parameters.Add("CGSTAMT", OracleDbType.NVarchar2).Value = cp.cgst;
                                    objCmds.Parameters.Add("SGSTAMT", OracleDbType.NVarchar2).Value = cp.sgst;
                                    objCmds.Parameters.Add("IGSTAMT", OracleDbType.NVarchar2).Value = cp.igst;
                                    objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cp.totalamount;
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

        public DataTable GetSalesQuotation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_QUOTE.BRANCHID,SALES_QUOTE.QUOTE_NO,to_char(SALES_QUOTE.QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,SALES_QUOTE.ENQ_NO,to_char(SALES_QUOTE.ENQ_DATE,'dd-MON-yyyy')ENQ_DATE,SALES_QUOTE.CURRENCY_TYPE,SALES_QUOTE.CUSTOMER,SALES_QUOTE.CUSTOMER_TYPE,SALES_QUOTE.ADDRESS,SALES_QUOTE.CITY,SALES_QUOTE.PINCODE,SALES_QUOTE.CONTACT_PERSON_MAIL,SALES_QUOTE.CONTACT_PERSON_MOBILE,SALES_QUOTE.PRIORITY,SALES_QUOTE.ASSIGNED_TO,SALESQUOTEID from SALES_QUOTE  where SALES_QUOTE.SALESQUOTEID= " + id + " ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            //SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            SvSql = "SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('Customer','BOTH')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEnquiry()
        {
            string SvSql = string.Empty;
            SvSql = "select ENQ_NO,SALESENQUIRYID from SALES_ENQUIRY WHERE ISACTIVE = 'YES'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public IEnumerable<QuoItem> GetAllSalesQuotationItem(string id)
        {
            List<QuoItem> cmpList = new List<QuoItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SALESQUOTEDETAIL.QTY,SALESQUOTEDETAIL.SALESQUOTEDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNIT from SALESQUOTEDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALESQUOTEDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where SALESQUOTEDETAIL.SALESQUOTEDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QuoItem cmp = new QuoItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            unit = rdr["UNIT"].ToString(),
                            quantity = rdr["QTY"].ToString(),
                            //Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }


        public DataTable GetSalesQuotationItemDetails(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select SALESQUOTEDETAIL.QTY,SALESQUOTEDETAIL.SALESQUOTEDETAILID,SALESQUOTEDETAIL.ITEMID,SALESQUOTEDETAIL.UNIT,SALESQUOTEDETAIL.RATE  from SALESQUOTEDETAIL where SALESQUOTEDETAIL.SALESQUOID='" + id + "'";
            SvSql = "SELECT ITEMGROUP,ITEMID,ITEMDESC,RATE,QTY,CF,UNIT,AMOUNT,DISC,DISCAMOUNT,IFREIGHTCH,TOTAMT,CGSTPER,SGSTPER,IGSTPER,SGSTAMT,IGSTAMT,CGSTAMT ,SALES_ENQUIRY.SALESENQUIRYID,SALESQUOTEDETAIL.SALESQUOTEDETAILID FROM SALESQUOTEDETAIL LEFT OUTER JOIN  SALES_ENQUIRY ON SALES_ENQUIRY.SALESENQUIRYID=SALESQUOTEDETAIL.SALESQUOTEDETAILID WHERE SALESQUOTEDETAIL.SALESQUOTEDETAILID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPurchaseQuotationDetails(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select QUOTE_NO,PARTYRCODE.PARTY from SALES_QUOTE  LEFT OUTER JOIN  PARTYMAST on SALES_QUOTE.CUSTOMER=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')  AND SALES_QUOTE.ID='" + id + "'";
            SvSql = "Select QUOTE_NO,PARTYMAST.PARTYNAME from SALES_QUOTE  LEFT OUTER JOIN  PARTYMAST on SALES_QUOTE.CUSTOMER=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND SALES_QUOTE.SALESQUOTEID = '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SalesQuotationFollowupCRUD(QuotationFollowup cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SALESQUOFOLLOWPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURCHASEFOLLOWPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.FolID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.FolID;
                    }
                    objCmd.Parameters.Add("QUOTE_NO", OracleDbType.NVarchar2).Value = cy.QuoId;
                    objCmd.Parameters.Add("FOLLOW_BY", OracleDbType.NVarchar2).Value = cy.Followby;
                    objCmd.Parameters.Add("FOLLOW_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.Followdate);
                    objCmd.Parameters.Add("NEXT_FOLLOW_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.Nfdate);
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Rmarks;
                    objCmd.Parameters.Add("FOLLOW_STATUS", OracleDbType.NVarchar2).Value = cy.Enquiryst;
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

        public DataTable GetFollowup(string enqid)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALESQUOFOLLOWUP.QUOTE_NO,SALESQUOFOLLOWUP.FOLLOW_BY,to_char(SALESQUOFOLLOWUP.FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(SALESQUOFOLLOWUP.NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,SALESQUOFOLLOWUP.REMARKS,SALESQUOFOLLOWUP.FOLLOW_STATUS,SALESQUOFOLLOWID from SALESQUOFOLLOWUP WHERE QUOTE_NO='" + enqid + "'";
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
                    svSQL = "UPDATE SALES_QUOTE SET STATUS ='CLOSE' WHERE ID='" + id + "'";
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

        //public DataTable GetSalesQuotationByName(string name)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select  BRANCHMAST.BRANCHID, QUOTE_NO,to_char(QUOTE_DATE,'dd-MON-yyyy') QUOTE_DATE,PARTYRCODE.PARTY,SALES_QUOTE.ENQ_NO,to_char(SALES_QUOTE.ENQ_DATE,'dd-MON-yyyy') ENQ_DATE,ID,SALES_QUOTE.STATUS from SALES_QUOTE LEFT OUTER JOIN PURENQ on SALES_QUOTE.ENQID=PURENQ.PURENQID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SALES_QUOTE.BRANCHID LEFT OUTER JOIN  PARTYMAST on SALES_QUOTE.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND ID='" + name + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        //public DataTable GetSalesQuotationItem(string id)
        //{
        //    throw new NotImplementedException();
        //}


        public DataTable GetSalesQuo(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,SALES_QUOTE.QUOTE_NO,to_char(SALES_QUOTE.QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,SALES_QUOTE.ENQ_NO,to_char(SALES_QUOTE.ENQ_DATE,'dd-MON-yyyy')ENQ_DATE,SALES_QUOTE.CURRENCY_TYPE,PARTYRCODE.PARTY,SALES_QUOTE.CUSTOMER_TYPE,SALES_QUOTE.ADDRESS,SALES_QUOTE.CITY,SALES_QUOTE.PINCODE,SALES_QUOTE.CONTACT_PERSON_MAIL,SALES_QUOTE.CONTACT_PERSON_MOBILE,SALES_QUOTE.PRIORITY,SALES_QUOTE.ASSIGNED_TO,SALES_QUOTE.SALESQUOTEID from SALES_QUOTE LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SALES_QUOTE.BRANCHID LEFT OUTER JOIN  PARTYMAST on SALES_QUOTE.Customer=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')   AND SALES_QUOTE.SALESQUOTEID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesQuoItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALESQUOTEDETAIL.QTY,SALESQUOTEDETAIL.SALESQUOTEDETAILID,ITEMMASTER.ITEMID,UNIT,SALESQUOTEDETAIL.RATE,SALESQUOTEDETAIL.AMOUNT  from SALESQUOTEDETAIL left outer join ITEMMASTER ON ITEMMASTERID=SALESQUOTEDETAIL.ITEMID  where SALESQUOTEDETAIL.SALESQUOTEDETAILID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string QuotetoOrder(string id)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'JOB#' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "JOB#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='JOB#' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }



                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    string Customer = datatrans.GetDataString("Select CUSTOMER from SALES_QUOTE where SALES_QUOTE.SALESQUOTEID='" + id + "' ");

                    string party = datatrans.GetDataString("Select PARTYNAME from PARTYMAST where PARTYMASTID='" + Customer + "' ");

                    string currency = datatrans.GetDataString("Select SALES_QUOTE.CURRENCY_TYPE from SALES_QUOTE where SALES_QUOTE.SALESQUOTEID='" + id + "' ");

                    string symbol = datatrans.GetDataString("Select SYMBOL from CURRENCY where MAINCURR='" + currency + "' ");

                    svSQL = "Insert into JOBASIC (BRANCHID,QUOID,MAINCURRENCY,DOCID,DOCDATE,PARTYNAME,PARTYID,SYMBOL,STATUS) (Select BRANCHID,'" + id + "',CURRENCY_TYPE,'" + docid + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ,'" + party + "',CUSTOMER,'" + symbol + "' ,'ACTIVE' from SALES_QUOTE where SALES_QUOTE.SALESQUOTEID='" + id + "')";
                    OracleCommand objCmd = new OracleCommand(svSQL, objConn);
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }

                string quotid = datatrans.GetDataString("Select JOBASICID from JOBASIC Where QUOID=" + id + "");
                string unit = datatrans.GetDataString("Select UNIT from SALESQUOTEDETAIL Where SALESQUOTEDETAILID=" + id + "");
                string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + unit + "' ");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into JODETAIL (JOBASICID,ITEMID,QTY,UNIT,RATE,AMOUNT,DISCOUNT) (Select '" + quotid + "',ITEMID,QTY,'" + UnitId + "',RATE ,AMOUNT,DISCAMOUNT FROM SALESQUOTEDETAIL WHERE SALESQUOTEDETAILID=" + id + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    try
                    {
                        objConnT.Open();
                        objCmds.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConnT.Close();

                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
       


        public async Task<IEnumerable<SQuoItemDetail>> GetSQuoItem(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {

                return await db.QueryAsync<SQuoItemDetail>(" SELECT TAAIERP.PARTYMAST.PARTYNAME, TAAIERP.PARTYMAST.ADD1, TAAIERP.PARTYMAST.ADD2, TAAIERP.PARTYMAST.ADD3, TAAIERP.PARTYMAST.STATE, TAAIERP.PARTYMAST.CITY, TAAIERP.PARTYMAST.PINCODE, TAAIERP.PARTYMAST.GSTNO, TAAIERP.SALES_QUOTE.QUOTE_NO, TAAIERP.SALES_QUOTE.QUOTE_DATE, TAAIERP.SALES_QUOTE.DELIVERY_TERMS, TAAIERP.SALES_QUOTE.TOTAL_PRICE, TAAIERP.SALES_QUOTE.TOTAL_QUANTIRY, TAAIERP.SALESQUOTEDETAIL.ITEMID, TAAIERP.SALESQUOTEDETAIL.RATE, TAAIERP.SALESQUOTEDETAIL.QTY, TAAIERP.SALESQUOTEDETAIL.UNIT, TAAIERP.SALESQUOTEDETAIL.AMOUNT, TAAIERP.SALESQUOTEDETAIL.TOTAMT, TAAIERP.SALESQUOTEDETAIL.SGSTAMT, TAAIERP.SALESQUOTEDETAIL.IGSTAMT, TAAIERP.SALESQUOTEDETAIL.CGSTAMT FROM PARTYMAST INNER JOIN SALES_QUOTE ON PARTYMAST.PARTYMASTID = SALES_QUOTE.SALESQUOID INNER JOIN SALESQUOTEDETAIL ON SALES_QUOTE.SALESQUOID = SALESQUOTEDETAIL.SALESQUOID WHERE SALESQUOTEDETAIL.SALESQUOID='" + id + "' and SALES_QUOTE.SALESQUOID ='" + id + "' ", commandType: CommandType.Text);


                //return await db.QueryAsync<SQuoItemDetail>("SELECT TAAIERP.PARTYMAST.PARTYNAME, TAAIERP.PARTYMAST.ADD1, TAAIERP.PARTYMAST.ADD2, TAAIERP.PARTYMAST.ADD3, TAAIERP.PARTYMAST.STATE, TAAIERP.PARTYMAST.CITY, TAAIERP.PARTYMAST.PINCODE, TAAIERP.PARTYMAST.GSTNO, TAAIERP.SALES_QUOTE.QUOTE_NO, TAAIERP.SALES_QUOTE.QUOTE_DATE, TAAIERP.SALES_QUOTE.DELIVERY_TERMS, TAAIERP.SALES_QUOTE.TOTAL_PRICE, TAAIERP.SALES_QUOTE.TOTAL_QUANTIRY, TAAIERP.SALESQUOTEDETAIL.ITEMID, TAAIERP.SALESQUOTEDETAIL.RATE, TAAIERP.SALESQUOTEDETAIL.QTY, TAAIERP.SALESQUOTEDETAIL.UNIT, TAAIERP.SALESQUOTEDETAIL.AMOUNT, TAAIERP.SALESQUOTEDETAIL.TOTAMT, TAAIERP.SALESQUOTEDETAIL.SGSTAMT, TAAIERP.SALESQUOTEDETAIL.IGSTAMT, TAAIERP.SALESQUOTEDETAIL.CGSTAMT FROM PARTYMAST INNER JOIN SALES_QUOTE ON PARTYMAST.PARTYMASTID = SALES_QUOTE.SALESQUOID INNER JOIN SALESQUOTEDETAIL ON SALES_QUOTE.SALESQUOTEID = SALESQUOTEDETAIL.SALESQUOID WHERE SALESQUOTEDETAIL.SALESQUOID='" + id + "' and SALES_QUOTE.SALESQUOTEID ='" + id + "' ", commandType: CommandType.Text);



            }

        }

    }
}

