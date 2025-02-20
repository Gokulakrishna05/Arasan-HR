using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales_Export
{
    public class EnquiryQuotationService : IEnquiryQuotation
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public EnquiryQuotationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable Gettemplete()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TEMPID,TANDCBASICID FROM TANDCBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCondition()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TANDC,TANDCDETAILID,TANDCBASICID FROM TANDCDETAIL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('Customer','BOTH')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExRateDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURRNAME, EXRATE, CURRID, CRATEID, MODIFIEDON FROM CRATE WHERE CURRID = " + id + " ORDER BY MODIFIEDON DESC FETCH FIRST 1 ROWS ONLY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCustomerDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select CITY,PINCODE,ADD1,INTRODUCEDBY,MOBILE,ADD2,ADD3,EMAIL,MOBILE,PHONENO from PARTYMAST Where PARTYMAST.PARTYMASTID = '" + id + "' ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE IGROUP = 'FINISHED'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,LATPURPRICE,UNITMAST.UNITMASTID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListEnquiryQuotation(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT EQUOTBASIC.DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EQUOTBASIC.ENQNO,EQUOTBASICID,STATUS FROM EQUOTBASIC  WHERE EQUOTBASIC.STATUS='Y' ORDER BY  EQUOTBASIC.EQUOTBASICID DESC";
            }
            else
            {
                SvSql = "SELECT EQUOTBASIC.DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EQUOTBASIC.ENQNO,EQUOTBASICID,STATUS FROM EQUOTBASIC  WHERE EQUOTBASIC.STATUS='N' ORDER BY  EQUOTBASIC.EQUOTBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE EQUOTBASIC SET EQUOTBASIC.STATUS ='N' WHERE EQUOTBASICID='" + id + "'";
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
        public string ActStatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE EQUOTBASIC SET EQUOTBASIC.STATUS ='Y' WHERE EQUOTBASICID='" + id + "'";
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
        public IEnumerable<QuotationItem> GetAllEnquiryQuotationItem(string id)
        {
            List<QuotationItem> cmpList = new List<QuotationItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT EQUOTDETAILID,EQUOTBASICID,ITEMDESC,ITEMDETAILS,UNIT,QTY,RATE,AMOUNT FROM EQUOTDETAIL  where EQUOTDETAIL.EQUOTDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QuotationItem cmp = new QuotationItem
                        {
                            ID = rdr["EQUOTDETAILID"].ToString(),
                            ItemId = rdr["ITEMDESC"].ToString(),
                            Des = rdr["ITEMDETAILS"].ToString(),
                            Unit = rdr["UNIT"].ToString(),
                            Qty = rdr["QTY"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string EnquiryQuotationCRUD(EnquiryQuotation cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'EQuo' AND ACTIVESEQUENCE = 'T'");
                string QuoNo = string.Format("{0}{1}", "EQuo", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'EQuo' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.QuoNo = QuoNo;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EXPORTQUOPROC", objConn);
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
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.QuoNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.QuoDate;
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnqType;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.Rate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("ASSIGNEDTO", OracleDbType.NVarchar2).Value = cy.Assign;
                    objCmd.Parameters.Add("ENQRECDBY", OracleDbType.NVarchar2).Value = cy.Recieved;

                    objCmd.Parameters.Add("FOLLOWUPTIME", OracleDbType.NVarchar2).Value = cy.Time;
                    objCmd.Parameters.Add("FOLLOWDT", OracleDbType.NVarchar2).Value = cy.FollowUp;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Deatails;
                    objCmd.Parameters.Add("SMSDATE", OracleDbType.NVarchar2).Value = cy.Emaildate;
                    objCmd.Parameters.Add("SENDSMS", OracleDbType.NVarchar2).Value = cy.Send;



                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.NVarchar2).Value = cy.CreatedOn;
                    objCmd.Parameters.Add("UPDATED_BY", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = cy.UpdatedOn;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Y";
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
                        foreach (QuotationItem cp in cy.QuotLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("EXPORTQUODETAILPROC", objConns);
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

                                    string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("EQUOTBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMDESC", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("ITEMDETAILS", OracleDbType.NVarchar2).Value = cp.Des;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = unit;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
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
                    //objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetEnquiryQuotation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,FOLLOWUPTIME,SENDSMS,REMARKS,to_char(SMSDATE,'dd-MON-yyyy') SMSDATE,to_char(FOLLOWDT,'dd-MON-yyyy') FOLLOWDT,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,ENQNO,MAINCURRENCY,EXRATE,ASSIGNEDTO,ENQRECDBY,EQUOTBASICID,PARTYID FROM EQUOTBASIC    where EQUOTBASIC.EQUOTBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ADD1,ADD2,ADD3,CITY,PINCODE,STATE,COUNTRY,EMAIL,MOBILE,PHONENO FROM PARTYMAST WHERE PARTYMAST.PARTYMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEnquiryQuotationItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMDESC,ITEMDETAILS,UNITMAST.UNITID,QTY,RATE,AMOUNT,EQUOTBASICID FROM EQUOTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EQUOTDETAIL.UNIT\r\n  where EQUOTDETAIL.EQUOTDETAILID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEnquiryQuotationView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,ENQNO,CURRENCY.MAINCURR,EXRATE,EMPMAST.EMPNAME,EMPMAST.EMPNAME AS ENQRECDBY,EQUOTBASICID,PARTYMAST.PARTYID FROM EQUOTBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=EQUOTBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=EQUOTBASIC.PARTYID LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=EQUOTBASIC.ASSIGNEDTO   where EQUOTBASIC.EQUOTBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ADD1,ADD2,ADD3,CITY,PINCODE,STATE,COUNTRY,EMAIL,MOBILE,PHONENO FROM PARTYMAST WHERE PARTYMAST.PARTYMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEnquiryItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,ITEMDETAILS,UNITMAST.UNITID,QTY,EQUOTBASICID,RATE,AMOUNT FROM EQUOTDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EQUOTDETAIL.ITEMDESC LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EQUOTDETAIL.UNIT\r\n  where EQUOTDETAIL.EQUOTBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
