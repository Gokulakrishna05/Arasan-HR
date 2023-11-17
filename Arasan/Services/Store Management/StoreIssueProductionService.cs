using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class StoreIssueProductionService :IStoreIssueProduction
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StoreIssueProductionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
     
        public IEnumerable<SIPItem> GetAllStoreIssueItem(string id)
        {
            List<SIPItem> cmpList = new List<SIPItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select STORESISSDETAIL.QTY,STORESISSDETAIL.STORESISSDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from STORESISSDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESISSDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where STORESISSDETAIL.STORESISSBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SIPItem cmp = new SIPItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITMASTID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public IEnumerable<StoreIssueProduction> GetAllStoreIssuePro(string st, string ed)
        {
            List<StoreIssueProduction> cmpList = new List<StoreIssueProduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                    if (st != null && ed != null)
                    {
                        cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REQNO,to_char(REQDATE,'dd-MON-yyyy')REQDATE,TOLOCID,LOCIDCONS,PROCESSID,NARRATION,PSCHNO,WCID,STORESISSBASICID from STORESISSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESISSBASIC.BRANCHID  WHERE STORESISSBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "' ORDER BY STORESISSBASICID DESC";
                    }
                    else
                    {
                        cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REQNO,to_char(REQDATE,'dd-MON-yyyy')REQDATE,TOLOCID,LOCIDCONS,PROCESSID,NARRATION,PSCHNO,WCID,STORESISSBASICID from STORESISSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESISSBASIC.BRANCHID  WHERE STORESISSBASIC.DOCDATE > sysdate-30 ORDER BY STORESISSBASICID DESC";

                    }
                    OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            StoreIssueProduction cmp = new StoreIssueProduction
                            {

                                ID = rdr["STORESISSBASICID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),

                                DocNo = rdr["DOCID"].ToString(),
                                DocDate = rdr["DOCDATE"].ToString(),
                                ReqNo = rdr["REQNO"].ToString(),
                                ReqDate = rdr["REQDATE"].ToString(),
                                Location = rdr["TOLOCID"].ToString(),

                                LocCon = rdr["LOCIDCONS"].ToString(),
                                // net = rdr["NET"].ToString(),
                                Process = rdr["PROCESSID"].ToString(),

                                Narr = rdr["NARRATION"].ToString(),
                                SchNo = rdr["PSCHNO"].ToString(),
                                Work = rdr["WCID"].ToString()

                            };
                            cmpList.Add(cmp);
                        }
                    }
            
            }
            return cmpList;
        }
        public DataTable EditSIPbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  STORESISSBASIC.BRANCHID,STORESISSBASIC.DOCID,to_char(STORESISSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,STORESISSBASIC.REQNO,to_char(STORESISSBASIC.REQDATE,'dd-MON-yyyy')REQDATE,STORESISSBASIC.TOLOCID,STORESISSBASIC.FROMLOCID,STORESISSBASIC.LOCIDCONS,STORESISSBASIC.PROCESSID,PROCESSMAST.PROCESSNAME ,STORESISSBASIC.NARRATION,STORESISSBASIC.PSCHNO,STORESISSBASIC.WCID as work,WCBASIC.WCID,STORESISSBASIC.STORESISSBASICID from STORESISSBASIC left outer join PROCESSMAST ON PROCESSMASTID=STORESISSBASIC.PROCESSID left outer join WCBASIC ON WCBASICID=STORESISSBASIC.WCID  Where  STORESISSBASIC.STORESISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSICItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESISSDETAIL.QTY,STORESISSDETAIL.STORESISSDETAILID,STORESISSDETAIL.ITEMID,UNITMAST.UNITID,STORESISSDETAIL.RATE,CONVFACTOR,DRUMYN,SERIALYN,PENDQTY,REQQTY,SCHQTY,STORESISSDETAIL.UNIT,AMOUNT,CLSTOCK from STORESISSDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESISSDETAIL.UNIT  where STORESISSDETAIL.STORESISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StoreIssueProCRUD(StoreIssueProduction cy)
        {

            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                //string locid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='" + cy.ToLoc + "' ");
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Sis-' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "Sis-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Sis-' AND ACTIVESEQUENCE ='T'";
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
                    OracleCommand objCmd = new OracleCommand("SIPROPROC", objConn);


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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value =  cy.DocDate ;
                    objCmd.Parameters.Add("REQNO", OracleDbType.NVarchar2).Value = cy.ReqNo;
                    objCmd.Parameters.Add("ReqDate", OracleDbType.NVarchar2).Value =  cy.ReqDate;
                    objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("TOLOCID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                    objCmd.Parameters.Add("FROMLOCDETAILSID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("TOLOCDETAILSID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                    objCmd.Parameters.Add("LOCIDCONS", OracleDbType.NVarchar2).Value = cy.LocCon;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Processid;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.SchNo;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Workid;
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
                        foreach (SIPItem cp in cy.SIPLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("SIPDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("STORESISSBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unitid;
                                    objCmds.Parameters.Add("CONVFACTOR", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("PENDQTY", OracleDbType.NVarchar2).Value = cp.PendQty;
                                    objCmds.Parameters.Add("REQQTY", OracleDbType.NVarchar2).Value = cp.ReqQty;
                                    objCmds.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = cp.SchQty;
                                    objCmds.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cp.ClStock;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    Object did = objCmds.Parameters["OUTID"].Value;
                                    objConns.Close();


                                    try
                                    {


                                        using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                        {
                                            objConnT.Open();


                                            ///////////////////////////// Input Inventory
                                            double qty = cp.ReqQty;
                                            DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INV_OUT_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.ItemId + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.Location + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.Branch + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                                            if (dt.Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                {
                                                    double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                                    if (rqty >= qty)
                                                    {
                                                        double bqty = rqty - qty;

                                                        string Sql = string.Empty;
                                                        Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                        OracleCommand objCmdsz = new OracleCommand(Sql, objConnT);
                                                        objCmdsz.ExecuteNonQuery();




                                                        OracleCommand objCmdIn = new OracleCommand("INVENTORYITEMPROC", objConnT);
                                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                        objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = did;
                                                        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                                        objCmdIn.Parameters.Add("GRN_DATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                                                        objCmdIn.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                                        objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                                                        objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Workid;
                                                        objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                                                        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                        objCmdIn.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();

                                                        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = dt.Rows[i]["LOT_NO"].ToString(); 
                                                        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                        objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;


                                                        objCmdIn.ExecuteNonQuery();
                                                        Object inid = objCmdIn.Parameters["OUTID"].Value;
                                                        using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                                        {
                                                            OracleCommand objCmdIns = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                            objCmdIns.CommandType = CommandType.StoredProcedure;
                                                            objCmdIns.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                            objCmdIns.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                            objCmdIns.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = did;
                                                            objCmdIns.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                            objCmdIns.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();

                                                            objCmdIns.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = inid;
                                                            objCmdIns.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUEPROD";
                                                            objCmdIns.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                                            objCmdIns.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                                            objCmdIns.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUEPROD";
                                                            objCmdIns.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                            objCmdIns.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                            objCmdIns.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                            objCmdIns.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                            objCmdIns.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                                                            objCmdIns.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                            objCmdIns.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                            objCmdIns.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                            objCmdIns.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                            objCmdIns.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                            objConnIn.Open();
                                                            objCmdIns.ExecuteNonQuery();
                                                            objConnIn.Close();

                                                        }








                                                        break;
                                                    }
                                                    else
                                                    {
                                                        qty = qty - rqty;

                                                        /////////////////////////////////Outward Entry

                                                        string Sql = string.Empty;
                                                        Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + rqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                        OracleCommand objCmdsa = new OracleCommand(Sql, objConn);
                                                        objCmdsa.ExecuteNonQuery();



                                                        OracleCommand objCmdIn = new OracleCommand("INVENTORYITEMPROC", objConn);
                                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                        objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = did;
                                                        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();

                                                        objCmdIn.Parameters.Add("GRN_DATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                                                        objCmdIn.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                                        objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.Date).Value = cy.ToLoc;
                                                        objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Workid;
                                                        objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                                                        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                        objCmdIn.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INV_OUT_ID"].ToString();

                                                        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = cp.lotno;
                                                        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                        objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                                                        objCmdIn.ExecuteNonQuery();
                                                        Object inid = objCmdIn.Parameters["OUTID"].Value;
                                                        using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                                        {
                                                            OracleCommand objCmdIns = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                            objCmdIns.CommandType = CommandType.StoredProcedure;
                                                            objCmdIns.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                            objCmdIns.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                            objCmdIns.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = did;
                                                            objCmdIns.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                            objCmdIns.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();

                                                            objCmdIns.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = inid;
                                                            objCmdIns.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUEPROD";
                                                            objCmdIns.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                                            objCmdIns.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                                            objCmdIns.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUEPROD";
                                                            objCmdIns.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                            objCmdIns.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                            objCmdIns.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                            objCmdIns.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                            objCmdIns.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                                                            objCmdIns.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                            objCmdIns.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                            objCmdIns.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                            objCmdIns.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                            objCmdIns.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                            objConnIn.Open();
                                                            objCmdIns.ExecuteNonQuery();
                                                            objConnIn.Close();

                                                        }



                                                    }



                                                }
                                            }
                                            ///////////////////////////// Input Inventory






                                            objConn.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = "Error Occurs, While inserting / updating Data";
                                        throw ex;
                                    }
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
        public DataTable GetItem(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,INVENTORY_ITEM.ITEM_ID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID where ITEMMASTER.IGROUP ='RAW MATERIAL'  and LOCATION_ID='"+ ItemId +"' and BALANCE_QTY>0 GROUP BY ITEMMASTER.ITEMID,INVENTORY_ITEM.ITEM_ID  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getloc(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID from LOCDETAILS  where LOCDETAILSID='" + ItemId + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getstkqty(string ItemId, string locid, string brid)
        {
            string SvSql = string.Empty;
            SvSql = "select BALANCE_QTY as QTY,LOT_NO from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + locid + "' AND BRANCH_ID='" + brid + "' AND ITEM_ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getwork(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,PROCESSID,WCBASICID from WCBASIC  where ILOCATION='" + ItemId + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcess(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID from PROCESSMAST  where PROCESSMASTID='" + ItemId + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
