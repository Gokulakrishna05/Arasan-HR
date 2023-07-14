using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arasan.Services
{
    public class POService : IPO
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public POService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<PO> GetAllPO(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "Yes";
            }
            List<PO> cmpList = new List<PO>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = " Select BRANCHMAST.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,POBASIC.IS_ACTIVE from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=POBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=POBASIC.MAINCURRENCY  ORDER BY POBASIC.POBASICID DESC";

                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PO cmp = new PO
                        {
                            ID = rdr["POBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoteNo = rdr["Quotno"].ToString(),
                            PONo = rdr["DOCID"].ToString(),
                            POdate = rdr["DOCDATE"].ToString(),
                            ExRate = rdr["EXRATE"].ToString(),
                            Cur = rdr["MAINCURR"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            Status = rdr["STATUS"].ToString(),
                               Active = rdr["IS_ACTIVE"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public IEnumerable<POItem> GetAllPOItem(string id)
        {
            List<POItem> cmpList = new List<POItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select PODETAIL.QTY,PODETAIL.PODETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from PODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where PODETAIL.POBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        POItem cmp = new POItem
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
        public IEnumerable<POItem> GetAllGateInwardItem(string gateinwardid)
        {
            List<POItem> cmpList = new List<POItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select GATE_INWARD_DETAILS.IN_QTY,GATE_INWARD_DETAILS.QCFLAG,ITEMMASTER.ITEMID,UNITMAST.UNITID from GATE_INWARD_DETAILS LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=GATE_INWARD_DETAILS.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where GATE_INWARD_DETAILS.GATE_IN_ID='" + gateinwardid + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        POItem cmp = new POItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["IN_QTY"].ToString()),
                            QC= rdr["QCFLAG"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetPObyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,CURRENCY.MAINCURR,PARTYRCODE.PARTY,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,to_char(PURQUOTBASIC.DOCDATE,'dd-MON-yyyy') Quotedate,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,POBASIC.FREIGHT,POBASIC.GROSS,POBASIC.NET from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=POBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=POBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND POBASIC.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPObySuppID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select POBASIC.POBASICID,POBASIC.DOCID from POBASIC where POBASIC.PARTYID='" + name + "' AND POBASIC.STATUS IS NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditPObyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select POBASIC.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,POBASIC.MAINCURRENCY,POBASIC.PARTYID,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,to_char(PURQUOTBASIC.DOCDATE,'dd-MON-yyyy') Quotedate,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,NARRATION,PAYTERMS,DELTERMS,DESP,WARRTERMS,POBASIC.REFNO,to_char(POBASIC.REFDT,'dd-MON-yyyy') REFDT,POBASIC.FREIGHT,POBASIC.GROSS,POBASIC.NET from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO  Where  POBASIC.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItembyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select PODETAIL.ITEMID as Itemi,PODETAIL.QTY,PODETAIL.PODETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID,PODETAIL.RATE,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT,DISCPER,DISCAMT,FREIGHTCHGS,PURTYPE,to_char(DUEDATE,'dd-MON-yyyy') DUEDATE from PODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where PODETAIL.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select PODETAIL.QTY,PODETAIL.PODETAILID,PODETAIL.ITEMID,UNITMAST.UNITID,PODETAIL.RATE,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT,DISCPER,DISCAMT,FREIGHTCHGS,PURTYPE,to_char(DUEDATE,'dd-MON-yyyy') DUEDATE from PODETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PODETAIL.UNIT  where PODETAIL.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllGateInward(string fromdate, string todate)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT GATE_INWARD.GATE_IN_ID,POBASIC.POBASICID,POBASIC.DOCID,GATE_INWARD.GATE_IN_TIME,to_char(GATE_IN_DATE,'dd-MON-yyyy') GATE_IN_DATE,GATE_INWARD.TOTAL_QTY,PARTYMAST.PARTYNAME,POBASIC.STATUS FROM GATE_INWARD LEFT OUTER JOIN POBASIC ON GATE_INWARD.POBASICID=POBASIC.POBASICID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND GATE_IN_DATE BETWEEN '" + fromdate + "' AND '"+ todate  + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string POtoGRN(string POID)
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

                int idc = datatrans.GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'GRN' AND IS_ACTIVE = 'Y'");
                string PONo = string.Format("{0} - {1} / {2}", "GRN", (idc + 1).ToString(), disp);

                string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='GRN' AND IS_ACTIVE ='Y'";
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
                    svSQL = "Insert into GRNBLBASIC (PARTYID,BRANCHID,POBASICID,EXRATE,MAINCURRENCY,DOCID,DOCDATE,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,FREIGHT,GROSS,NET,IS_ACTIVE) (Select PARTYID,BRANCHID,'" + POID + "',EXRATE,MAINCURRENCY,'" + PONo + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "',PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,FREIGHT,GROSS,NET ,'Yes' from POBASIC where POBASICID='" + POID + "')";
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

                string quotid = datatrans.GetDataString("Select GRNBLBASICID from GRNBLBASIC Where POBASICID=" + POID + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into GRNBLDETAIL (GRNBLBASICID,ITEMID,RATE,QTY,UNIT,AMOUNT,CF,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTAMT,DISCPER,DISC,PURTYPE) (Select '" + quotid + "',ITEMID,RATE,QTY,UNIT,AMOUNT,CF,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT,DISCPER,DISCAMT,PURTYPE FROM PODETAIL WHERE POBASICID=" + POID + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE POBASIC SET STATUS='GRN Generated' where POBASICID='" + POID + "'";
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

        public string GateInwardCRUD(GateInward cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("GATEINPROC", objConn);

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
                    objCmd.Parameters.Add("POBASICID", OracleDbType.NVarchar2).Value = cy.POId;
                    objCmd.Parameters.Add("GATE_IN_TIME", OracleDbType.NVarchar2).Value = cy.GateInTime;
                    objCmd.Parameters.Add("TOTAL_QTY", OracleDbType.NVarchar2).Value = cy.TotalQty;
                    objCmd.Parameters.Add("GATE_IN_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.GateInDate);
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("statu", OracleDbType.NVarchar2).Value = "Waiting";
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        foreach (POGateItem cp in cy.PoItem)
                        {
                            if (cp.itemid != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Insert")
                                    {
                                        Sql = "Insert into GATE_INWARD_DETAILS(GATE_IN_ID,ITEM_ID,QCFLAG,IN_QTY) Values('" + Pid + "','" + cp.itemid + "','" + cp.qc + "','" + cp.quantity + "')";
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

                        using (OracleConnection objConnE = new OracleConnection(_connectionString))
                        {
                            string Sql = "UPDATE POBASIC SET STATUS='GRN Generated' where POBASICID='" + cy.POId + "'";
                            OracleCommand objCmds = new OracleCommand(Sql, objConnE);
                            objConnE.Open();
                            objCmds.ExecuteNonQuery();
                            objConnE.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }


                ///////////////////////GRN Generation
                int cunt = datatrans.GetDataId("Select count(GRNBLBASICID) from GRNBLBASIC Where POBASICID=" + cy.POId + "");
                if (cunt == 0)
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

                int idc = datatrans.GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'GRN' AND IS_ACTIVE = 'Y'");
                string PONo = string.Format("{0} - {1} / {2}", "GRN", (idc + 1).ToString(), disp);

                string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='GRN' AND IS_ACTIVE ='Y'";
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
                    svSQL = "Insert into GRNBLBASIC (PARTYID,BRANCHID,POBASICID,EXRATE,MAINCURRENCY,DOCID,DOCDATE,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,FREIGHT,GROSS,NET) (Select PARTYID,BRANCHID,'" + cy.POId + "',EXRATE,MAINCURRENCY,'" + PONo + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "',PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,FREIGHT,GROSS,NET  from POBASIC where POBASICID='" + cy.POId + "')";
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

                string quotid = datatrans.GetDataString("Select GRNBLBASICID from GRNBLBASIC Where POBASICID=" + cy.POId + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into GRNBLDETAIL (GRNBLBASICID,ITEMID,RATE,QTY,UNIT,AMOUNT,CF,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTAMT,DISCPER,DISC,PURTYPE) (Select '" + quotid + "',ITEMID,RATE,QTY,UNIT,AMOUNT,CF,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT,DISCPER,DISCAMT,PURTYPE FROM PODETAIL WHERE POBASICID=" + cy.POId + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE GRNBLBASIC SET STATUS='GATE IN Verified' where GRNBLBASICID='" + quotid + "'";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnE);
                    objConnE.Open();
                    objCmds.ExecuteNonQuery();
                    objConnE.Close();
                }

                }
                ///////////////////////GRN Generation







            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public string PurOrderCRUD(PO cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("POPROC", objConn);
                   
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.POID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.POID;

                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.BranchId;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.SuppId;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    if(cy.RefDate == null)
                    {
                        objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    }
                    
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("FREIGHT", OracleDbType.NVarchar2).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("OTHER_CHARGES", OracleDbType.NVarchar2).Value = cy.Othercharges;
                    objCmd.Parameters.Add("ROUND_OFF_PLUS", OracleDbType.NVarchar2).Value = cy.Round;
                    objCmd.Parameters.Add("PACKING_CHRAGES", OracleDbType.NVarchar2).Value = cy.Packingcharges;
                    objCmd.Parameters.Add("OTHER_DEDUCTION", OracleDbType.NVarchar2).Value = cy.otherdeduction;
                    objCmd.Parameters.Add("ROUND_OFF_MINUS", OracleDbType.NVarchar2).Value = cy.Roundminus;
                    objCmd.Parameters.Add("PAYTERMS", OracleDbType.NVarchar2).Value = cy.Paymentterms;
                    objCmd.Parameters.Add("DELTERMS", OracleDbType.NVarchar2).Value = cy.delterms;
                    objCmd.Parameters.Add("DESP", OracleDbType.NVarchar2).Value = cy.desp;
                    objCmd.Parameters.Add("WARRTERMS", OracleDbType.NVarchar2).Value = cy.warrantyterms;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                   
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.Recid;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        foreach (POItem cp in cy.PoItem)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update PODETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.Conversionfactor + "',AMOUNT='" + cp.Amount + "',DISCPER='" + cp.DiscPer + "',DISCAMT='" + cp.DiscAmt + "',PURTYPE='" + cp.Purtype + "',FREIGHTCHGS='" + cp.FrieghtAmt + "',CGSTPER='" + cp.CGSTPer + "',CGSTAMT='" + cp.CGSTAmt + "',SGSTPER='" + cp.SGSTPer + "',SGSTAMT='" + cp.SGSTAmt + "',IGSTPER='" + cp.IGSTPer + "',IGSTAMT='" + cp.IGSTAmt + "',TOTALAMT='" + cp.TotalAmount + "' where POBASICID='" + cy.POID + "'  AND ITEMID='" + cp.saveItemId + "' ";
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
        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE POBASIC SET IS_ACTIVE ='No' WHERE POBASICID='" + id + "'";
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

        public async Task<IEnumerable<POItemDetail>> GetPOItem(string id)
        {
            using(OracleConnection db =new OracleConnection (_connectionString))
            {
                return await db.QueryAsync<POItemDetail>("select ITEMMASTER.ITEMID,PODETAIL.QTY,PUNIT,PODETAIL.RATE,PODETAIL.AMOUNT from PODETAIL left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID where PODETAIL.POBASICID='" + id + "'", commandType: CommandType.Text);
            }
        }

    }
}
