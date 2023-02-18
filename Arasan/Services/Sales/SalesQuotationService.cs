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
                            City = rdr["CITY"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
       
        public DataTable GetItemCF(string ItemId, string unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + unitid + "'";
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
            SvSql = "Select CITY,PINCODE,ADD1,ADD2,ADD3 from PARTYMAST Where PARTYMAST.PARTYMASTID='" + id + "'";
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
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //Object Pid = objCmd.Parameters["OUTID"].Value;
                        foreach (QuoItem cp in cy.QuoLst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update PURQUOTDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.ConsFa + "'  where PURQUOTBASICID='" + cy.ID + "'  AND ITEMID='" + cp.saveItemId + "' ";
                                    }
                                    else
                                    {
                                        Sql = "";
                                    }
                                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                    objConnT.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConnT.Close();
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

        public DataTable GetSalesQuotationItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PURQUOTDETAIL.QTY,PURQUOTDETAIL.PURQUOTDETAILID,PURQUOTDETAIL.ITEMID,UNITMAST.UNITID,PURQUOTDETAIL.RATE  from PURQUOTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PURQUOTDETAIL.UNIT  where PURQUOTDETAIL.PURQUOTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
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
                    cmd.CommandText = "Select PURQUOTDETAIL.QTY,PURQUOTDETAIL.PURQUOTDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from PURQUOTDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PURQUOTDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where PURQUOTDETAIL.PURQUOTBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QuoItem cmp = new QuoItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
    }
}
