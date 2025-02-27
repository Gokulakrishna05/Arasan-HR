using Arasan.Interface;
using Arasan.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.CodeAnalysis;
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
            SvSql = "SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('Customer','BOTH')  AND COUNTRY NOT IN ('INDIA','null')";
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
                SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,CURRENCY.MAINCURR,EJOBASIC.EJOBASICID,STATUS FROM EJOBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=EJOBASIC.MAINCURRENCY,EJODETAIL  WHERE EJOBASIC.STATUS='Y' AND EJODETAIL.QTY > 0 AND EJODETAIL.QTY-EJODETAIL.PRECLQTY > 0 AND EJOBASIC.EJOBASICID=EJODETAIL.EJOBASICID ORDER BY to_date(EJOBASIC.DOCDATE) DESC,EJOBASIC.DOCID DESC";
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
            string updateCMd = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                if(cy.Order=="ORDER")
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'ESO#' AND ACTIVESEQUENCE = 'T'");
                    string Job = string.Format("{0}{1}", "ESO#", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX = 'ESO#' AND ACTIVESEQUENCE ='T'";

                    cy.Job = Job;
                }
                else
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'ESJF' AND ACTIVESEQUENCE = 'T'");
                    string Job = string.Format("{0}{1}", "ESJF", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX = 'ESJF' AND ACTIVESEQUENCE ='T'";

                    cy.Job = Job;
                }
                

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();

                    using (OracleCommand command = objConn.CreateCommand())
                    {
                        using (OracleTransaction transaction = objConn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                command.Transaction = transaction;
                                //OracleCommand objCmd = new OracleCommand("EJOBASICPROC", objConn);
                                ///*objCmd.Connection = objConn;
                                //objCmd.CommandText = "DIRECTPURCHASEPROC";*/

                                //objCmd.CommandType = CommandType.StoredProcedure;
                                //if (cy.ID == null)
                                //{
                                //    StatementType = "Insert";
                                //    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                //}
                                //else
                                //{
                                //    StatementType = "Update";
                                //    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                //}
                                //objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                                //objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Job;
                                //objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.jobDate;
                                //objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = cy.active;

                                //objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                                //objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.Rate;
                                //objCmd.Parameters.Add("ORDTYPE", OracleDbType.NVarchar2).Value = cy.Order;
                                //objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Customer;
                                //objCmd.Parameters.Add("CREFNO", OracleDbType.NVarchar2).Value = cy.Refno;
                                //objCmd.Parameters.Add("CREFDATE", OracleDbType.NVarchar2).Value = cy.Refdate;
                                ////objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.QuoNo;
                                //objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.Officer;

                                //objCmd.Parameters.Add("SMSDATE", OracleDbType.NVarchar2).Value = cy.Emaildate;
                                //objCmd.Parameters.Add("SENDSMS", OracleDbType.NVarchar2).Value = cy.Send;

                                //objCmd.Parameters.Add("ASSIGNTO", OracleDbType.NVarchar2).Value = cy.Assign;
                                //objCmd.Parameters.Add("RECDBY", OracleDbType.NVarchar2).Value = cy.Recieved;

                                //objCmd.Parameters.Add("FOLLOWUPTIME", OracleDbType.NVarchar2).Value = cy.Time;
                                //objCmd.Parameters.Add("FOLLOWDT", OracleDbType.NVarchar2).Value = cy.FollowUp;
                                //objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Deatails;
                                //objCmd.Parameters.Add("TRANSPORTER", OracleDbType.NVarchar2).Value = cy.Transporter;
                                //objCmd.Parameters.Add("TEST", OracleDbType.NVarchar2).Value = cy.Test;

                                //objCmd.Parameters.Add("CREATED_BY", OracleDbType.Date).Value = DateTime.Now;
                                //objCmd.Parameters.Add("CREATED_ON", OracleDbType.NVarchar2).Value = cy.CreatedOn;
                                //objCmd.Parameters.Add("UPDATED_BY", OracleDbType.Date).Value = DateTime.Now;
                                //objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = cy.UpdatedOn;
                                //objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Y";
                                //objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                //objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                string parentid = "Job Order " + cy.Job;
                                string partyname=datatrans.GetDataString("SELECT PARTYNAME FROM PARTYMAST WHERE PARTYMASTID='"+ cy.Customer +"'");
                                command.CommandText = "Insert into EJOBASIC (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,BRANCHID,DOCID,DOCDATE,ACTIVE,MAINCURRENCY,EXRATE,ORDTYPE,PARTYID,CREFNO,CREFDATE,TYPE,SMSDATE,SENDSMS,ASSIGNTO,RECDBY,FOLLOWUPTIME,FOLLOWDT,REMARKS,TRANSPORTER,TEST,CREATED_ON,CREATED_BY,STATUS,USERID,TRANSID,PARTYNAME,RATECODE,TEMPID,REFNO,PARENTACTIVITYID,PTERMS,SALESREP)" +
                    " VALUES ('0','0','F','0','0' ,'" + cy.Branch + "','" + cy.Job + "','" + cy.jobDate + "','0','" + cy.Currency + "','" + cy.Rate + "','" + cy.Order + "','"+cy.Customer + "','" + cy.Refno + "','" + cy.Refdate + "','" + cy.Officer + "','" + cy.Emaildate + "','" + cy.Send + "','" + cy.Assign + "' ,'" + cy.Recieved + "','" + cy.Time + "','" + cy.FollowUp + "','" + cy.Deatails + "','" + cy.Transporter + "','" + cy.Test + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" +  cy.user + "','Y','" + cy.user + "','eso','" + partyname + "','" + cy.arc + "','10074001335751','NONE','" + parentid + "','" + cy.payterms + "','"+cy.salesrep+"') RETURNING EJOBASICID INTO :OUTID";
                                command.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                command.ExecuteNonQuery();
                               string Pid = command.Parameters["OUTID"].Value.ToString();
                                command.Parameters.Clear();

                                 
                                
                                //string Pid = "0";
                                if (cy.ID != null)
                                {
                                    Pid = cy.ID;
                                }
                                int row = 1;
                                foreach (WorkOrderItem cp in cy.WorkOrderLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                    {
                                        //using (OracleConnection objConns = new OracleConnection(_connectionString))
                                        //{
                                        //    OracleCommand objCmds = new OracleCommand("EJODETAILPROC", objConns);
                                        //    if (cy.ID == null)
                                        //    {
                                        //        StatementType = "Insert";
                                        //        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        //    }
                                        //    else
                                        //    {
                                        //        StatementType = "Update";
                                        //        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                        //    }

                                             string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");


                                        //    objCmds.CommandType = CommandType.StoredProcedure;
                                        //    objCmds.Parameters.Add("EENQUIRYBASICID", OracleDbType.NVarchar2).Value = Pid;
                                        //    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                        //    objCmds.Parameters.Add("ITEMSPEC", OracleDbType.NVarchar2).Value = cp.Des;
                                        //    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = unit;
                                        //    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                        //    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                        //    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                        //    objCmds.Parameters.Add("QDISC", OracleDbType.NVarchar2).Value = cp.QtyDisc;
                                        //    objCmds.Parameters.Add("CDISC", OracleDbType.NVarchar2).Value = cp.CashDisc;
                                        //    objCmds.Parameters.Add("IDISC", OracleDbType.NVarchar2).Value = cp.Introduction;
                                        //    objCmds.Parameters.Add("TDISC", OracleDbType.NVarchar2).Value = cp.Trade;
                                        //    objCmds.Parameters.Add("ADISC", OracleDbType.NVarchar2).Value = cp.Addition;
                                        //    objCmds.Parameters.Add("SDISC", OracleDbType.NVarchar2).Value = cp.Special;
                                        //    objCmds.Parameters.Add("DISCOUNT", OracleDbType.NVarchar2).Value = cp.Discount;
                                        //    objCmds.Parameters.Add("BED", OracleDbType.NVarchar2).Value = cp.Bed;
                                        //    //objCmds.Parameters.Add("DUEDATE", OracleDbType.NVarchar2).Value = cp.Due;
                                        //    objCmds.Parameters.Add("MATSUPP", OracleDbType.NVarchar2).Value = cp.Supply;
                                        //    objCmds.Parameters.Add("PACKSPEC", OracleDbType.NVarchar2).Value = cp.Packing;
                                        //    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                            string itemname = datatrans.GetDataString("SELECT ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='"+cp.ItemId+"'");
                                           string itemsep = cp.itemspec + " " + itemname;
                                            command.CommandText = "Insert into EJODETAIL (EJOBASICID,ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,DISCOUNT,MATSUPP,PACKSPEC,ITEMTYPE,EJODETAILROW,DUEDATE,PARTYCTRL,BLOCKQTY,PEQTY,POQTY,PRECLQTY,DCQTY,MRPQTY,EXCISEQTY,REWORKQTY,REJQTY,INVQTY,FREIGHT,FREIGHTAMT) " +
                                   "VALUES ('" + Pid + "','" + cp.ItemId + "','" + itemsep + "','" + unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.QtyDisc + "','" + cp.CashDisc + "','" + cp.Introduction + "','" + cp.Trade + "','" + cp.Addition + "','" + cp.Special + "','" + cp.Discount + "','OWN','" + cp.Packing + "','" + cp.itemspec + "','" + row + "','" + cp.Due + "','F','0','0','0','0','" + cp.Qty + "','0','0','0','0','0','0','0') RETURNING EJODETAILID INTO :OUTID";
                                            command.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            command.ExecuteNonQuery();
                                            string did = command.Parameters["OUTID"].Value.ToString();

                                            command.Parameters.Clear();
                                        if (!string.IsNullOrEmpty(cp.schqty))
                                        {
                                            string[] sqty = cp.schqty.Split('/');
                                            string[] sdate = cp.schdate.Split('/');
                                            int r = 1;


                                            for (int i = 0; i < sqty.Length; i++)
                                            {
                                                itemname = datatrans.GetDataString("Select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "' ");

                                                string schno = cy.Job + " - " + itemname + " - " + r;
                                                string ssqty = sqty[i];
                                                string scdate = sdate[i];



                                                command.CommandText = "Insert into EJOSCHEDULE(EJOBASICID,PARENTRECORDID,EJOSCHEDULEROW,SCHNO,SCHQTY,SCHDATE,SCHSUPPQTY,PARENTROW,SCHITEMID) VALUES ('" + Pid + "','" + did + "','" + r + "','" + schno + "','" + ssqty + "','" + scdate + "','0','" + r + "','" + cp.ItemId + "')";
                                                command.ExecuteNonQuery();

                                                r++;
                                            }
                                        }
                                    }
                                    row++;

                                    }
                                if (cy.TermsDeaLst != null)
                                {
                                    if (cy.ID == null)
                                    {
                                        int r = 1;
                                        foreach (TermsDeatils cp in cy.TermsDeaLst)
                                        {

                                            if (cp.Isvalid1 == "Y" && cp.Conditions != "")
                                            {

                                                command.CommandText = "Insert into EJOTANDC(EJOBASICID,EJOTANDCROW,TERMSANDCONDITION,JOTANDCROW) VALUES ('" + Pid + "','" + r + "','" + cp.Conditions + "','"+r+"')";
                                                command.ExecuteNonQuery();

                                                r++;
                                            }
                                        }
                                    }
                                }


                                transaction.Commit();
                                datatrans.UpdateStatus(updateCMd);

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Console.WriteLine(ex.ToString());
                                msg = ex.ToString();
                                Console.WriteLine("Neither record was written to database.");
                            }
                        }
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
        public string DrumAllocationCRUD(EWDrumAllocation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);





                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'EJDL' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "EJDL", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='EJDL' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DOCId = DocId;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EDRUMALLOPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;

                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DOCId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Locid;
                    objCmd.Parameters.Add("JOPID", OracleDbType.NVarchar2).Value = cy.JOId;
                    objCmd.Parameters.Add("CUSTOMERID", OracleDbType.NVarchar2).Value = cy.CustomerId;
                    objCmd.Parameters.Add("EJOSCHEDULEID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("TRUCKNO", OracleDbType.NVarchar2).Value = cy.truckno;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    Object Pid = objCmd.Parameters["OUTID"].Value;
                    //if (cy.ID != null)
                    //{
                    //    Pid = cy.ID;
                    //}
                    foreach (EWorkItem cp in cy.Worklst)
                    {
                        foreach (EDrumdetails ca in cp.drumlst)
                        {
                            if (ca.drumselect == true)
                            {
                                OracleCommand objCmds = new OracleCommand("EDRUMALLODETPROC", objConn);
                                objCmds.CommandType = CommandType.StoredProcedure;
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                objCmds.Parameters.Add("EJODRUMALLOCATIONBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("PLSTOCKID", OracleDbType.NVarchar2).Value = ca.invid;
                                objCmds.Parameters.Add("JOPDETAILID", OracleDbType.NVarchar2).Value = cp.Jodetailid;
                                objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.drumno;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ca.qty;
                                objCmds.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = ca.lotno;
                                objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = ca.rate;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.ExecuteNonQuery();
                                string svql = "Update PLSTOCKVALUE SET  IS_LOCK='Y' WHERE PLSTOCKVALUEID='" + ca.invid + "'";
                                OracleCommand objCmdss = new OracleCommand(svql, objConn);
                                objCmdss.ExecuteNonQuery();

                            }

                        }
                    }
                    string allocate = "Update EJOBASIC SET  IS_ALLOCATE='Y' WHERE EJOBASICID='" + cy.JOId + "'";
                    OracleCommand objCmdssa = new OracleCommand(allocate, objConn);
                    objCmdssa.ExecuteNonQuery();
                    allocate = "Update EJOSCHEDULE SET  IS_ALLOCATE='Y' WHERE EJOSCHEDULEID='" + cy.ID + "'";
                    objCmdssa = new OracleCommand(allocate, objConn);
                    objCmdssa.ExecuteNonQuery();
                    allocate = "Update EJODISPDRUMDET SET  IS_ALLOCATE='Y' WHERE SCHID='" + cy.ID + "'";
                    objCmdssa = new OracleCommand(allocate, objConn);
                    objCmdssa.ExecuteNonQuery();

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
        public string DispDrumCRUD(ExportWorkOrder cy)
        {
            string msg = "";
            string StatementType = string.Empty; string svSQL = "";
            datatrans = new DataTransactions(_connectionString);





            int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'EJDP'    ");
            string DocId = string.Format("{0}{1}", "EJDP", (idc + 1).ToString());

            string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='EJDP'   ";

            cy.JopId = DocId;

            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                objConn.Open();
                using (OracleCommand command = objConn.CreateCommand())
                {
                    using (OracleTransaction transaction = objConn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            command.Transaction = transaction;
                            command.CommandText = "Insert into EJODISPDRUMBASIC(DOCID,DOCDATE) VALUES ('" + cy.JopId + "','" + cy.JopDate + "') RETURNING EJODISPDRUMBASICID INTO :STKID";


                            command.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                            command.ExecuteNonQuery();


                            string Pid = command.Parameters["STKID"].Value.ToString();
                            command.Parameters.Clear();

                            foreach (SchItem cp in cy.schlst)
                            {
                                if (cp.Isvalid == "Y" && cp.itemid != "0")
                                {
                                    command.CommandText = "Insert into EJODISPDRUMDET(EJODISPDRUMBASICID,JOPID,SCHNO,SCHDATE,SCHQTY,SCHID,ITEM) VALUES ('" + Pid + "','" + cp.jobno + "','" + cp.schno + "','" + cp.schdate + "','" + cp.qty + "','" + cp.schid + "','" + cp.itemid + "')";


                                    command.ExecuteNonQuery();

                                    command.CommandText = "Update EJOSCHEDULE SET  IS_DRUMDISP='Y' WHERE EJOSCHEDULEID='" + cp.schid + "'";
                                    command.ExecuteNonQuery();
                                }

                            }

                            //allocate = "Update JOSCHEDULE SET  IS_ALLOCATE='Y' WHERE JOSCHEDULEID='" + cy.ID + "'";
                            //objCmdssa = new OracleCommand(allocate, objConn);
                            //objCmdssa.ExecuteNonQuery();


                            transaction.Commit();
                            datatrans.UpdateStatus(updateCMd);
                        }

                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine(ex.ToString());
                            Console.WriteLine("Neither record was written to database.");
                        }
                    }
                }
                objConn.Close();
            }

            return msg;
        }
        public DataTable GetAllListWorkScheduleItems()
        {
            string SvSql = string.Empty;
            SvSql = "Select JS.EJOSCHEDULEID,JS.SCHNO,JS.SCHQTY-JS.SCHSUPPQTY SCHQTY,J.DOCID,J.PARTYNAME,to_char(JS.SCHDATE,'dd-MON-yyyy')SCHDATE,to_char(J.DOCDATE,'dd-MON-yyyy')DOCDATE,JD.QTY-JD.EXCISEQTY-JD.PRECLQTY QTY,JS.IS_ALLOCATE,JB.DOCID dipid,to_char(JB.DOCDATE,'dd-MON-yyyy')dispdate from EJOSCHEDULE JS,EJOBASIC J,EJODETAIL JD,EJODISPDRUMDET JP,EJODISPDRUMBASIC JB    WHERE JB.EJODISPDRUMBASICID=JP.EJODISPDRUMBASICID AND JP.SCHID=JS.EJOSCHEDULEID AND J.EJOBASICID =JS.EJOBASICID AND JS.PARENTRECORDID =JD.EJODETAILID AND   J.ACTIVE='0'  AND  JD.QTY-JD.PRECLQTY > 0  AND JP.STATUS IS null   ORDER BY  JB.DOCID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusStockRelease(string id, string jid, string bid)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    objConnT.Open();
                    svSQL = "UPDATE PLSTOCKVALUE SET IS_LOCK ='' WHERE PLSTOCKVALUEID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);

                    objCmds.ExecuteNonQuery();
                    svSQL = "UPDATE EJOBASIC SET IS_ALLOCATE ='N' WHERE EJOBASICID='" + jid + "'";
                    objCmds = new OracleCommand(svSQL, objConnT);

                    objCmds.ExecuteNonQuery();
                    svSQL = "UPDATE EJODRUMALLOCATIONBASIC  SET IS_ALLOCATE ='N' WHERE EJOSCHEDULEID='" + bid + "'";
                    objCmds = new OracleCommand(svSQL, objConnT);

                    objCmds.ExecuteNonQuery();

                    objCmds.ExecuteNonQuery();
                    svSQL = "UPDATE EJOSCHEDULE  SET IS_ALLOCATE ='N' WHERE EJOSCHEDULEID='" + bid + "'";
                    objCmds = new OracleCommand(svSQL, objConnT);

                    objCmds.ExecuteNonQuery();
                    svSQL = "UPDATE EJOSCHEDULE  SET SCHSUPPQTY ='0' WHERE EJOSCHEDULEID='" + bid + "'";
                    objCmds = new OracleCommand(svSQL, objConnT);

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
        public DataTable GetDrumAllByID(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select Ejodrumallocationbasic.DOCID,to_char(Ejodrumallocationbasic.DOCDATE,'dd-MON-yyyy')DOCDATE,Ejobasic.DOCID as jobid,PARTYMAST.PARTYNAME ,LOCDETAILS.LOCID,EJODRUMALLOCATIONBASICID from Ejodrumallocationbasic  left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=Ejodrumallocationbasic.LOCID  LEFT OUTER JOIN  PARTYMAST on Ejodrumallocationbasic.CUSTOMERID=PARTYMAST.PARTYMASTID left outer join Ejobasic on Ejobasic.Ejobasicid= Ejodrumallocationbasic.JOPID WHERE  Ejodrumallocationbasic.Ejodrumallocationbasicid='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumAllDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,EJODRUMALLOCATIONDETAIL.EJODRUMALLOCATIONDETAILID from EJODRUMALLOCATIONDETAIL left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EJODRUMALLOCATIONDETAIL.ITEMID  Where Ejodrumallocationbasicid='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllocationDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EJODRUMALLOCATIONDETAIL.DRUMNO,EJODRUMALLOCATIONDETAIL.RATE,LOTNO,EJODRUMALLOCATIONDETAIL.QTY,EJODRUMALLOCATIONDETAILID from EJODRUMALLOCATIONDETAIL     Where Ejodrumallocationbasicid='" + id + "' ORDER BY EJODRUMALLOCATIONDETAIL.DRUMNO ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetWorkOrderByID(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select J.DOCID,to_char(J.DOCDATE,'dd-MON-yyyy')DOCDATE,J.PARTYNAME,BRANCHMAST.BRANCHID,JS.EJOBASICID,J.STATUS,J.PARTYID as CUSTOMERID,J.EJOBASICID,JS.SCHNO,to_char(JS.SCHDATE,'dd-MON-yyyy')SCHDATE from EJOSCHEDULE JS, EJOBASIC J   left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID=J.BRANCHID  WHERE J.EJOBASICID =JS.EJOBASICID AND  JS.EJOSCHEDULEID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkOrderDetailsss(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select JD.EJOBASICID,JD.QTY-JD.PRECLQTY-JD.EXCISEQTY QTY,JS.SCHQTY-JS.SCHSUPPQTY SCHQTY,JD.ITEMID as item,ITEMMASTER.ITEMID,DCQTY,RATE,AMOUNT,UNITMAST.UNITID,ITEMSPEC,PACKSPEC,DISCOUNT,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,JD.EJODETAILID from EJODETAIL JD left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JD.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT,EJOSCHEDULE JS  Where JD.EJODETAILID =JS.PARENTRECORDID AND EJOSCHEDULEID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);

        
            return dtt;
        }
        public DataTable GetDrumDetails(string Itemid, string locid)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,SUM(PLUSQTY-MINUSQTY) QTY,lotno from plstockvalue where ITEMID='" + Itemid + "' AND LOCID='" + locid + "' AND IS_LOCK IS NULL group by DRUMNO,lotno having sum(Plusqty-Minusqty)>0 order by DRUMNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
