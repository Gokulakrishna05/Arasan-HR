using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Arasan.Services.Sales
{
    public class DebitNoteBillService : IDebitNoteBillService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DebitNoteBillService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<DebitNoteBill> GetAllDebitNoteBill()
        {
            List<DebitNoteBill> cmpList = new List<DebitNoteBill>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DBNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DBNOTEBASICID from DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DBNOTEBASIC.BRANCHID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DebitNoteBill cmp = new DebitNoteBill
                        {
                            ID = rdr["DBNOTEBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string DebitNoteBillCRUD(DebitNoteBill cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty;
                string svSQL = "";

                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'DNBF' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "DNBF", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='DNBF' AND ACTIVESEQUENCE ='T'";
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
                    OracleCommand objCmd = new OracleCommand("DBNOTEBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DBNOTEBASICPROC";*/

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
                    objCmd.Parameters.Add("VTYPE", OracleDbType.NVarchar2).Value = cy.Vocher;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("AMTINWRD", OracleDbType.NVarchar2).Value = cy.Amount;
                    objCmd.Parameters.Add("BIGST", OracleDbType.NVarchar2).Value = cy.Bigst;
                    objCmd.Parameters.Add("BSGST", OracleDbType.NVarchar2).Value = cy.Bsgst;
                    objCmd.Parameters.Add("BCGST", OracleDbType.NVarchar2).Value = cy.Bcgst;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        if (cy.Depitlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DebitNoteItem cp in cy.Depitlst)
                                {
                                    //string itemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.Item + "' ");


                                    if (cp.Isvalid == "Y" && cp.InvNo != "0")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,INVDT,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + cp.InvNo + "','" + cp.Invdate + "','" + cp.Item + "','" + cp.Cf + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete DBNOTEDETAIL WHERE DBNOTEBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DebitNoteItem cp in cy.Depitlst)
                                {
                                    //string itemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.Item + "' ");


                                    if (cp.Isvalid == "Y" && cp.InvNo != "0")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,INVDT,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + cp.InvNo + "','" + cp.Invdate + "','" + cp.Item + "','" + cp.Cf + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {

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
        public string CreditNoteStock(DebitNoteBill cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CRNBASICPROC", objConn);
                  
                    objCmd.CommandType = CommandType.StoredProcedure;

                   
                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                   
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Bra;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Loc;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXCHANGERATE", OracleDbType.NVarchar2).Value = cy.Exchange;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("VTYPE", OracleDbType.NVarchar2).Value = cy.Vocher;
                    objCmd.Parameters.Add("TCREDITAMOUNT", OracleDbType.NVarchar2).Value = cy.Credit;
                    objCmd.Parameters.Add("TDEBITAMOUNT", OracleDbType.NVarchar2).Value = cy.Debit;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        if (cy.Creditlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (CreditItem cp in cy.Creditlst)
                                {
                                    //string itemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.Item + "' ");


                                    if (cp.Isvalid == "Y" && cp.Dr != "0")
                                    {
                                        svSQL = "Insert into CRNDETAIL(CRNBASICID,DBCR,MID,DBAMOUNT,CRAMOUNT) VALUES ('" + Pid + "','" + cp.Dr + "','" + cp.Account + "','" + cp.DepitAmount + "','" + cp.CreditAmount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete CRNDETAIL WHERE CRNBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (CreditItem cp in cy.Creditlst)
                                {
                                    //string itemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.Item + "' ");


                                    if (cp.Isvalid == "Y" && cp.Dr != "0")
                                    {
                                        svSQL = "Insert into CRNDETAIL(CRNBASICID,DBCR,MID,DBAMOUNT,CRAMOUNT) VALUES ('" + Pid + "','" + cp.Dr + "','" + cp.Account + "','" + cp.DepitAmount + "','" + cp.CreditAmount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {

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
        //public DataTable GetParty()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select GRNBLBASICID,PARTYNAME,PARTYID FROM GRNBLBASIC";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetGrn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,GRNBLBASICID FROM  GRNBLBASIC WHERE PARTYID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string itemId, string grnid)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CF,PUNIT,QTY,RATE,AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT FROM GRNBLDETAIL WHERE ITEMID='" + itemId + "' AND GRNBLBASICID='" + grnid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetInvoDates(string itemId)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,GRNBLBASICID FROM  GRNBLBASIC WHERE GRNBLBASICID='" + itemId + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public DataTable GetDebitNoteBillDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,VTYPE,DOCID,DOCDATE,REFNO,REFDT,PARTYID,GROSS,NET,AMTINWRD,BIGST,BSGST,BCGST,NARRATION FROM DBNOTEBASIC WHERE DBNOTEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDebitNoteBillItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DBNOTEBASICID,INVNO,INVDT,ITEMMASTER.ITEMID,DBNOTEDETAIL.CONVFACTOR,DBNOTEDETAIL.PRIUNIT,DBNOTEDETAIL.QTY,DBNOTEDETAIL.RATE,DBNOTEDETAIL.AMOUNT,DBNOTEDETAIL.CGST,DBNOTEDETAIL.SGST,DBNOTEDETAIL.IGST,DBNOTEDETAIL.TOTAMT FROM DBNOTEDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID =DBNOTEDETAIL.ITEMID WHERE DBNOTEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditProEntry(string PROID)
        {
            string SvSql = string.Empty;
            SvSql = "select  BRANCHMAST.BRANCHID,NET,DBNOTEBASIC.DOCID,DBNOTEBASIC.BRANCHID AS BRA FROM DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID =DBNOTEBASIC.BRANCHID where DBNOTEBASIC.DBNOTEBASICID ='" + PROID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT GRNBLDETAILID,GRNBLBASICID,ITEMMASTER.ITEMID,GRNBLDETAIL.ITEMID AS item FROM GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=GRNBLDETAIL.ITEMID WHERE GRNBLBASICID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVocher()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT VCHTYPEID,DESCRIPTION FROM VCHTYPE";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
