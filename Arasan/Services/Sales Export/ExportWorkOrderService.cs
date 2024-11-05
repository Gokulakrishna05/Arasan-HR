using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class ExportWorkOrderService : IExportWorkOrder
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ExportWorkOrderService(IConfiguration _configuratio)
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
        public DataTable GetAllExportWorkOrder(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,CURRENCY.MAINCURR,EJOBASICID,STATUS FROM EJOBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=EJOBASIC.MAINCURRENCY  WHERE STATUS='Y' ORDER BY  EJOBASIC.EJOBASICID DESC";
            }
            else
            {
                SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,CURRENCY.MAINCURR,EJOBASICID,STATUS FROM EJOBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=EJOBASIC.MAINCURRENCY  WHERE STATUS='N' ORDER BY  EJOBASIC.EJOBASICID DESC";
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
                    svSQL = "UPDATE EJOBASIC SET EJOBASIC.STATUS ='N' WHERE EJOBASICID='" + id + "'";
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
                    svSQL = "UPDATE EJOBASIC SET EJOBASIC.STATUS ='Y' WHERE EJOBASICID='" + id + "'";
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
        public string ExportWorkOrderCRUD(ExportWorkOrder cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE = 'T'");
                string Job = string.Format("{0}{1}", "Ex-F", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.Job = Job;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EJOBASICPROC", objConn);
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Job;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.jobDate;
                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = cy.active;
                    
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.Rate;
                    objCmd.Parameters.Add("ORDTYPE", OracleDbType.NVarchar2).Value = cy.Order;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("CREFNO", OracleDbType.NVarchar2).Value = cy.Refno;
                    objCmd.Parameters.Add("CREFDATE", OracleDbType.NVarchar2).Value = cy.Refdate;
                    //objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.QuoNo;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.Officer;

                    objCmd.Parameters.Add("SMSDATE", OracleDbType.NVarchar2).Value = cy.Emaildate;
                    objCmd.Parameters.Add("SENDSMS", OracleDbType.NVarchar2).Value = cy.Send;

                    objCmd.Parameters.Add("ASSIGNTO", OracleDbType.NVarchar2).Value = cy.Assign;
                    objCmd.Parameters.Add("RECDBY", OracleDbType.NVarchar2).Value = cy.Recieved;

                    objCmd.Parameters.Add("FOLLOWUPTIME", OracleDbType.NVarchar2).Value = cy.Time;
                    objCmd.Parameters.Add("FOLLOWDT", OracleDbType.NVarchar2).Value = cy.FollowUp;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Deatails;
                    objCmd.Parameters.Add("TRANSPORTER", OracleDbType.NVarchar2).Value = cy.Transporter;
                    objCmd.Parameters.Add("TEST", OracleDbType.NVarchar2).Value = cy.Test;

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
                        foreach (WorkOrderItem cp in cy.WorkOrderLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("EJODETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("EENQUIRYBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("ITEMSPEC", OracleDbType.NVarchar2).Value = cp.Des;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = unit;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("QDISC", OracleDbType.NVarchar2).Value = cp.QtyDisc;
                                    objCmds.Parameters.Add("CDISC", OracleDbType.NVarchar2).Value = cp.CashDisc;
                                    objCmds.Parameters.Add("IDISC", OracleDbType.NVarchar2).Value = cp.Introduction;
                                    objCmds.Parameters.Add("TDISC", OracleDbType.NVarchar2).Value = cp.Trade;
                                    objCmds.Parameters.Add("ADISC", OracleDbType.NVarchar2).Value = cp.Addition;
                                    objCmds.Parameters.Add("SDISC", OracleDbType.NVarchar2).Value = cp.Special;
                                    objCmds.Parameters.Add("DISCOUNT", OracleDbType.NVarchar2).Value = cp.Discount;
                                    objCmds.Parameters.Add("BED", OracleDbType.NVarchar2).Value = cp.Bed;
                                    //objCmds.Parameters.Add("DUEDATE", OracleDbType.NVarchar2).Value = cp.Due;
                                    objCmds.Parameters.Add("MATSUPP", OracleDbType.NVarchar2).Value = cp.Supply;
                                    objCmds.Parameters.Add("PACKSPEC", OracleDbType.NVarchar2).Value = cp.Packing;
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
        public IEnumerable<WorkOrderItem> GetAllExportWorkOrderItem(string id)
        {
            List<WorkOrderItem> cmpList = new List<WorkOrderItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT EJODETAIL,EENQUIRYBASICID,ITEMID,ITEMSPEC,UNIT,QTY FROM EJODETAIL  where EJODETAIL.EJODETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        WorkOrderItem cmp = new WorkOrderItem
                        {
                            ID = rdr["EJODETAILID"].ToString(),
                            ItemId = rdr["ITEMID"].ToString(),
                            Des = rdr["ITEMSPEC"].ToString(),
                            Unit = rdr["UNIT"].ToString(),
                            Qty = rdr["QTY"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetExportWorkOrderView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EJOBASIC.BRANCHID,EJOBASIC.DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EJOBASIC.ACTIVE,CURRENCY.MAINCURR,EXRATE,ORDTYPE,PARTYMAST.PARTYID,CREFNO,CREFDATE,EJOBASIC.TYPE,SMSDATE,SENDSMS,EMPMAST.EMPNAME,EMPMAST.EMPNAME AS RECDBY,FOLLOWUPTIME,FOLLOWDT,EJOBASIC.REMARKS,TRANSPORTER,TEST FROM EJOBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=EJOBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=EJOBASIC.PARTYID LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=EJOBASIC.ASSIGNTO   where EJOBASIC.EJOBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportWorkOrderItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EJOBASICID,ITEMMASTER.ITEMID,ITEMSPEC,UNITMAST.UNITID,QTY,RATE,AMOUNT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,DISCOUNT,BED,MATSUPP,PACKSPEC FROM EJODETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EJODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EJODETAIL.UNIT  where EJODETAIL.EJOBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportWorkOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EJOBASIC.BRANCHID,EJOBASIC.DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EJOBASIC.ACTIVE,MAINCURRENCY,EXRATE,ORDTYPE,PARTYID,CREFNO,to_char(CREFDATE,'dd-MON-yyyy')CREFDATE,EJOBASIC.TYPE,to_char(SMSDATE,'dd-MON-yyyy')SMSDATE,SENDSMS,ASSIGNTO,RECDBY,FOLLOWUPTIME,to_char(FOLLOWDT,'dd-MON-yyyy')FOLLOWDT,EJOBASIC.REMARKS,TRANSPORTER,TEST FROM EJOBASIC   where EJOBASIC.EJOBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportWorkItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EJOBASICID,ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,DISCOUNT,BED,MATSUPP,PACKSPEC FROM EJODETAIL   where EJODETAIL.EJOBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
