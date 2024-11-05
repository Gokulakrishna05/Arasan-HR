using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
	public class ExportEnquiryService : IExportEnquiry
	{
		private readonly string _connectionString;
		DataTransactions datatrans;
		public ExportEnquiryService(IConfiguration _configuratio)
		{
			_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
        public DataTable GetAllListExportEnquiry(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select ENQNO,to_char(ENQDATE,'dd-MON-yyyy')ENQDATE,ENQREF,PARTYID,STATUS,EENQUIRYBASICID from EENQUIRYBASIC  WHERE STATUS='Y' ORDER BY  EENQUIRYBASIC.EENQUIRYBASICID DESC";
            }
            else
            {
                SvSql = "Select ENQNO,to_char(ENQDATE,'dd-MON-yyyy')ENQDATE,ENQREF,PARTYID,STATUS,EENQUIRYBASICID from EENQUIRYBASIC  WHERE STATUS='N' ORDER BY  EENQUIRYBASIC.EENQUIRYBASICID DESC";
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
                    svSQL = "UPDATE EENQUIRYBASIC SET EENQUIRYBASIC.STATUS ='N' WHERE EENQUIRYBASICID='" + id + "'";
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
                    svSQL = "UPDATE EENQUIRYBASIC SET EENQUIRYBASIC.STATUS ='Y' WHERE EENQUIRYBASICID='" + id + "'";
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
        public IEnumerable<ExportItem> GetAllExportEnquiryItem(string id)
        {
            List<ExportItem> cmpList = new List<ExportItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT EENQUIRYDETAILID,EENQUIRYBASICID,ITEMDESC,ITEMDETAILS,UNIT,QTY FROM EENQUIRYDETAIL  where EENQUIRYDETAIL.EENQUIRYDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ExportItem cmp = new ExportItem
                        {
                            ID = rdr["EENQUIRYDETAILID"].ToString(),
                            ItemId = rdr["ITEMDETAILS"].ToString(),
                            Des = rdr["ITEMDESC"].ToString(),
                            Unit = rdr["UNIT"].ToString(),
                            Qty = rdr["QTY"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetExportEnquiry(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ENQNO,FOLLOWUPTIME,SENDSMS,REMARKS,to_char(SMSDATE,'dd-MON-yyyy') SMSDATE,to_char(FOLLOWDT,'dd-MON-yyyy') FOLLOWDT,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,ENQREF,MAINCURRENCY,EXRATE,ASSIGNTO,ENQRECDBY,EENQUIRYBASICID,PARTYID FROM EENQUIRYBASIC    where EENQUIRYBASIC.EENQUIRYBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportEnquiryView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,ENQREF,CURRENCY.MAINCURR,EXRATE,EMPMAST.EMPNAME,EMPMAST.EMPNAME AS ENQRECDBY,EENQUIRYBASICID,PARTYMAST.PARTYID FROM EENQUIRYBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=EENQUIRYBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=EENQUIRYBASIC.PARTYID LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=EENQUIRYBASIC.ASSIGNTO   where EENQUIRYBASIC.EENQUIRYBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportEnquiryItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMDESC,ITEMDETAILS,UNIT,QTY,EENQUIRYBASICID FROM EENQUIRYDETAIL  where EENQUIRYDETAIL.EENQUIRYBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetExportItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,ITEMDETAILS,UNIT,QTY,EENQUIRYBASICID FROM EENQUIRYDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EENQUIRYDETAIL.ITEMDESC  where EENQUIRYDETAIL.EENQUIRYBASICID=" + id + "";
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
        public string Export_EnquiryCRUD(ExportEnquiry cy)
		{
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE = 'T'");
                string EnqNo = string.Format("{0}{1}", "Ex-F", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.EnqNo = EnqNo;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EXPORTENQPROC", objConn);
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
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.EnqDate;
                    objCmd.Parameters.Add("ENQREF", OracleDbType.NVarchar2).Value = cy.EnqType;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.Rate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("ASSIGNTO", OracleDbType.NVarchar2).Value = cy.Assign;
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
                        foreach (ExportItem cp in cy.ExportLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("EXPORTENQDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("EENQUIRYBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMDESC", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("ITEMDETAILS", OracleDbType.NVarchar2).Value = cp.Des;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Qty;
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
	}
}
