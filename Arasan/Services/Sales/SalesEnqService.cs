using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class SalesEnqService : ISalesEnq
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesEnqService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<SalesEnquiry> GetAllSalesEnq()
        {
            List<SalesEnquiry> cmpList = new List<SalesEnquiry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  PARTYRCODE.PARTY,ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy')ENQ_DATE, ENQ_TYPE,CUSTOMER_TYPE,SALES_ENQUIRY.ID from SALES_ENQUIRY LEFT OUTER JOIN  PARTYMAST on SALES_ENQUIRY.CUSTOMER_NAME=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SalesEnquiry cmp = new SalesEnquiry
                        {

                            ID = rdr["ID"].ToString(),
                         
                            Customer = rdr["PARTY"].ToString(),
                            EnqNo = rdr["ENQ_NO"].ToString(),
                            EnqDate = rdr["ENQ_DATE"].ToString(),
                           
                            EnqType = rdr["ENQ_TYPE"].ToString()
                         
                            
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public bool UpdateStatus(string query)
        {
            bool Saved = true;
            try
            {
                OracleConnection objConn = new OracleConnection(_connectionString);
                OracleCommand objCmd = new OracleCommand(query, objConn);
                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();
            }
            catch (Exception ex)
            {

                Saved = false;
            }
            return Saved;
        }
        public string SalesEnqCRUD(SalesEnquiry cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";

                if (cy.ID == null)
                {
                    DateTime theDate = DateTime.Now;
                    DateTime todate; DateTime fromdate;
                    string t; string f;
                    if (DateTime.Now.Month >= 4)
                    {
                        todate = theDate.AddYears(1);
                    }
                    else
                    {
                        todate = theDate;
                    }
                    if (DateTime.Now.Month >= 4)
                    {
                        fromdate = theDate;
                    }
                    else
                    {
                        fromdate = theDate.AddYears(-1);
                    }
                    t = todate.ToString("yy");
                    f = fromdate.ToString("yy");
                    string disp = string.Format("{0}-{1}", f, t);

                    int idc = GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'SE' AND IS_ACTIVE = 'Y'");
                    cy.EnqNo = string.Format("{0}/ {3} / {1} - {2} ", "TAAI","SE", (idc + 1).ToString(), disp);

                    string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='SE' AND IS_ACTIVE ='Y'";
                    try
                    {
                        UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SALESENQPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

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
                    objCmd.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("CUSTOMER_NAME", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("ENQ_NO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("ENQ_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.EnqDate);
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
              
                    objCmd.Parameters.Add("CUSTOMER_TYPE", OracleDbType.NVarchar2).Value = cy.CustomerType;
                    objCmd.Parameters.Add("ENQ_TYPE", OracleDbType.NVarchar2).Value = cy.EnqType;
                    objCmd.Parameters.Add("CURRENCY_TYPE", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("ADDRESS", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("CONTACT_PERSON", OracleDbType.NVarchar2).Value = cy.ContactPersion;
                    objCmd.Parameters.Add("CONTACT_PERSON_MOBILE", OracleDbType.NVarchar2).Value = cy.Mobile;
                    objCmd.Parameters.Add("PRIORITY", OracleDbType.NVarchar2).Value = cy.Priority;
                    objCmd.Parameters.Add("LEADBY", OracleDbType.NVarchar2).Value = cy.Recieved;
                    objCmd.Parameters.Add("ASSIGNED_TO", OracleDbType.NVarchar2).Value = cy.Assign;
                    objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                    //objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        foreach (SalesItem cp in cy.SalesLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("SALESENQITEMPROC", objConns);
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
                                    objCmds.Parameters.Add("SAL_ENQ_ID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("ITEM_DESCRIPTION", OracleDbType.NVarchar2).Value = cp.Des;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("QUANTITY", OracleDbType.NVarchar2).Value = cp.Qty;
                                  
                                    
                                 
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
        public string EnquirytoQuote(string QuoteId)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                DateTime theDate = DateTime.Now;
                DateTime todate; DateTime fromdate;
                string t; string f;
                if (DateTime.Now.Month >= 4)
                {
                    todate = theDate.AddYears(1);
                }
                else
                {
                    todate = theDate;
                }
                if (DateTime.Now.Month >= 4)
                {
                    fromdate = theDate;
                }
                else
                {
                    fromdate = theDate.AddYears(-1);
                }
                t = todate.ToString("yy");
                f = fromdate.ToString("yy");
                string disp = string.Format("{0}-{1}", f, t);

                int idc = datatrans.GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'SQ' AND IS_ACTIVE = 'Y'");
                string QUONo = string.Format("{0}/ {3} / {1} - {2}","TAAI", "SQ", (idc + 1).ToString(), disp);

                string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='SQ' AND IS_ACTIVE ='Y'";
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
                    svSQL = "Insert into SALES_QUOTE (BRANCHID,ENQNO,CURRENCY_TYPE,QUOTE_NO,QUOTE_DATE,CONTACT_PERSON,PRIORITY,ADDRESS,CITY,PINCODE,CUSTOMER_TYPE,CUSTOMER) (Select BRANCH_ID,'" + QuoteId + "',CURRENCY_TYPE,'" + QUONo + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ,CONTACT_PERSON,PRIORITY,ADDRESS,CITY,PINCODE,CUSTOMER_TYPE,CUSTOMER_NAME from SALES_ENQUIRY where SALES_ENQUIRY.ID='" + QuoteId + "')";
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

                string quotid = datatrans.GetDataString("Select ID from SALES_QUOTE Where QUOTE_NO=" + QuoteId + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into SALESQUOTEDETAIL (SALESQUOID,ITEMID,ITEMDESC,QTY,UNIT) (Select '" + quotid + "',ITEM_ID,ITEM_DESCRIPTION,QUANTITY,UNIT FROM SALES_ENQ_ITEM WHERE SAL_ENQ_ID=" + QuoteId + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE SALES_ENQUIRY SET STATUS='Generated' where ID='" + QuoteId + "'";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnE);
                    objConnE.Open();
                    objCmds.ExecuteNonQuery();
                    objConnE.Close();
                }

            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetSalesEnquiry(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select  BRANCH_ID,ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy')ENQ_DATE, ENQ_TYPE,CUSTOMER_TYPE,CUSTOMER_NAME,ADDRESS,CITY,PINCODE,CONTACT_PERSON,CONTACT_PERSON_MOBILE,PRIORITY,LEADBY,ASSIGNED_TO,CURRENCY_TYPE,SALES_ENQUIRY.ID from SALES_ENQUIRY  where SALES_ENQUIRY.ID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesEnquiryItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEM_ID,ITEM_DESCRIPTION,UNIT,QUANTITY  from SALES_ENQ_ITEM  where SAL_ENQ_ID=" + id + "";
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
            SvSql = "Select CITY,PINCODE,ADD1,INTRODUCEDBY from PARTYMAST Where PARTYMAST.PARTYMASTID='" + id +"'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
      
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEnqByName(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select   BRANCHMAST.BRANCHID, ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy') ENQ_DATE ,PARTYRCODE.PARTY,SALES_ENQUIRY.CURRENCY_TYPE,SALES_ENQUIRY.CONTACT_PERSON,SALES_ENQUIRY.CUSTOMER_TYPE,SALES_ENQUIRY.ENQ_TYPE,SALES_ENQUIRY.ADDRESS,SALES_ENQUIRY.CITY,SALES_ENQUIRY.PINCODE,PRIORITY,SALES_ENQUIRY.ID,SALES_ENQUIRY.STATUS from SALES_ENQUIRY  LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=SALES_ENQUIRY.BRANCH_ID LEFT OUTER JOIN  PARTYMAST on SALES_ENQUIRY.CUSTOMER_NAME=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND SALES_ENQUIRY.ID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEnqItem(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select SALES_ENQ_ITEM.QUANTITY,SALES_ENQ_ITEM.ID,ITEMMASTER.ITEMID,SALES_ENQ_ITEM.UNIT,SALES_ENQ_ITEM.ITEM_DESCRIPTION from SALES_ENQ_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=SALES_ENQ_ITEM.ITEM_ID   where SALES_ENQ_ITEM.SAL_ENQ_ID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}

