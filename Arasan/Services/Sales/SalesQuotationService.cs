using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
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

        public IEnumerable<SalesQuotation> GetAllSalesQuotation()
        {
            List<SalesQuotation> cmpList = new List<SalesQuotation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,QUOTE_NO,QUOTE_DATE,ENQNO,ENQDATE,CURRENCY_TYPE,CUSTOMER,ADDRESS,CITY,ID from SALES_QUOTE";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SalesQuotation cmp = new SalesQuotation
                        {
                            ID = rdr["ID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoId = rdr["QUOTE_NO"].ToString(),
                            QuoDate = rdr["QUOTE_DATE"].ToString(),
                            EnNo = rdr["ENQNO"].ToString(),
                            EnDate = rdr["ENQDATE"].ToString(),
                            Currency = rdr["CURRENCY_TYPE"].ToString(),
                            Customer = rdr["CUSTOMER"].ToString(),
                            Address = rdr["ADDRESS"].ToString(),
                            City = rdr["CITY"].ToString(),
                            Mobile = rdr["CONTACT_PERSON_MOBILE"].ToString(),
                            Gmail = rdr["CONTACT_PERSON_MAIL"].ToString(),
                            PinCode = rdr["PINCODE"].ToString(),
                            Pro = rdr["PRIORITY"].ToString(),
                            Assign = rdr["ASSIGNED_TO"].ToString()
                           
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
            SvSql = "Select CUSTOMER_TYPE,ID From CUSTOMERTYPE";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCustomerDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select CITY,PINCODE,ADD1,ADD2,ADD3,INTRODUCEDBY from PARTYMAST Where PARTYMAST.PARTYMASTID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getcountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
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

                    StatementType = "Update";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("QUOTE_NO", OracleDbType.NVarchar2).Value = cy.QuoId;
                    objCmd.Parameters.Add("QUOTE_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.QuoDate);
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.EnDate;
                    objCmd.Parameters.Add("CURRENCY_TYPE", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("CUSTOMER", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("ADDRESS", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
                    objCmd.Parameters.Add("CONTACT_PERSON_MOBILE", OracleDbType.NVarchar2).Value = cy.Mobile;
                    objCmd.Parameters.Add("CONTACT_PERSON_MAIL", OracleDbType.NVarchar2).Value = cy.Gmail;
                    objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                    objCmd.Parameters.Add("PRIORITY", OracleDbType.NVarchar2).Value = cy.Pro;
                    objCmd.Parameters.Add("ASSIGNED_TO", OracleDbType.NVarchar2).Value = cy.Assign;
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
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
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
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("SALESQUOID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("ITEMDESC", OracleDbType.NVarchar2).Value = cp.Des;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cp.TotalAmount;
                                    objCmds.Parameters.Add("DISC", OracleDbType.NVarchar2).Value = cp.Disc;
                                    objCmds.Parameters.Add("DISCAMOUNT", OracleDbType.NVarchar2).Value = cp.DiscAmount;
                                    objCmds.Parameters.Add("IFREIGHTCH", OracleDbType.NVarchar2).Value = cp.FrigCharge;
                                    objCmds.Parameters.Add("CGSTPER", OracleDbType.NVarchar2).Value = cp.CGSTP;
                                    objCmds.Parameters.Add("SGSTPER", OracleDbType.NVarchar2).Value = cp.SGSTP;
                                    objCmds.Parameters.Add("IGSTPER", OracleDbType.NVarchar2).Value = cp.IGSTP;
                                    objCmds.Parameters.Add("CGSTAMT", OracleDbType.NVarchar2).Value = cp.CGST;
                                    objCmds.Parameters.Add("SGSTAMT", OracleDbType.NVarchar2).Value = cp.SGST;
                                    objCmds.Parameters.Add("IGSTAMT", OracleDbType.NVarchar2).Value = cp.IGST;
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
            SvSql = "Select SALES_QUOTE.BRANCHID,SALES_QUOTE.QUOTE_NO,to_char(SALES_QUOTE.QUOTE_DATE,'dd-MON-yyyy')QUOTE_DATE,PURENQ.ENQNO,to_char(PURENQ.ENQDATE,'dd-MON-yyyy')ENQDATE,SALES_QUOTE.CURRENCY_TYPE,SALES_QUOTE.CUSTOMER,SALES_QUOTE.ADDRESS,SALES_QUOTE.CITY,ID from SALES_QUOTE LEFT OUTER JOIN SALES_ENQUIRY ON SALES_ENQUIRY.ID=SALES_QUOTE.ENQID where SALES_QUOTE.ID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetSalesQuotationItemDetails(string name);
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select STORESACCDETAIL.QTY,SALESQUOTEDETAILID,SALESQUOTEDETAIL.ITEMID,UNIT,RATE,AMOUNT from SALESQUOTEDETAIL   where SALESQUOTEDETAIL.SALESQUOTEDETAILID='" + name + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public IEnumerable<QuoItem> GetAllSalesQuotationItem(string id)
        {
            List<QuoItem> cmpList = new List<QuoItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SALESQUOTEDETAIL.QTY,SALESQUOTEDETAIL.SALESQUOTEDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from SALESQUOTEDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALESQUOTEDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where SALESQUOTEDETAIL.SALESQUOTEDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QuoItem cmp = new QuoItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNIT"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetSalesQuotationItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESACCDETAIL.QTY,SALESQUOTEDETAILID,SALESQUOTEDETAIL.ITEMID,ITEMDESC,UNIT,RATE,AMOUNT from SALESQUOTEDETAIL   where SALESQUOTEDETAIL.SALESQUOTEDETAILID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
