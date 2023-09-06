using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services;

    public class BatchProductionService : IBatchProduction
    {
    private readonly string _connectionString;
    DataTransactions datatrans;
    public BatchProductionService(IConfiguration _configuratio)
    {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        datatrans = new DataTransactions(_connectionString);
    }
    public IEnumerable<BatchProduction> GetAllBatchProduction()
    {
        List<BatchProduction> cmpList = new List<BatchProduction>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        { 

            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "Select ETYPE,BRANCHMAST.BRANCHID,DOCID,PROCESSMAST.PROCESSID,WCBASIC.WCID,BPRODBASICID,IS_INV ,BPRODBASIC.STATUS from BPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BPRODBASIC.BRANCH  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BPRODBASIC.PROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BPRODBASIC.WCID WHERE BPRODBASIC.STATUS='ACTIVE'";
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    BatchProduction cmp = new BatchProduction
                    {
                        ID = rdr["BPRODBASICID"].ToString(),
                        Branch = rdr["BRANCHID"].ToString(),
                        Location = rdr["WCID"].ToString(),
                        ProcessId = rdr["PROCESSID"].ToString(),
                        DocId = rdr["DOCID"].ToString(),
                        IsInv= rdr["IS_INV"].ToString(),
                        EntryType= rdr["ETYPE"].ToString(),
                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
    }

    public double GetStockInQty(string Itemid,string barchid,string Locid)
    {
        string SvSql = string.Empty;
        SvSql = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY from INVENTORY_ITEM  where ITEM_ID='" + Itemid + "' AND INVENTORY_ITEM.BRANCH_ID='"+ barchid  + "' AND INVENTORY_ITEM.LOCATION_ID='"+ Locid  + "' GROUP BY INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        double stock = 0;
        if(dtt.Rows.Count > 0) {
            stock = Convert.ToDouble(dtt.Rows[0]["QTY"].ToString());
        }
        return stock;
    }

    public string BatchProductionCRUD(BatchProduction cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty; string svSQL = "";
            string[] sdateList = cy.startdate.Split(" - ");
            string sdate = "";
            string stime = "";
            if (sdateList.Length > 0)
            {
                sdate = sdateList[0];
                stime = sdateList[1];
            }
            string[] edateList = cy.enddate.Split(" - ");
            string endate = "";
            string endtime = "";
            if (sdateList.Length > 0)
            {
                endate = edateList[0];
                endtime = edateList[1];
            }
            if (cy.ID == null)
            {
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'BPE# AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "BPE#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='BPE#' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = docid;
            }
            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                OracleCommand objCmd = new OracleCommand("BATCHPRODUCTIONPROC", objConn);
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
                objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.ProcessId;
                objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Location;
                objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Shiftdate;
                objCmd.Parameters.Add("ETYPE", OracleDbType.NVarchar2).Value = cy.EntryType;
                objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                objCmd.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = cy.BatchNo;
                objCmd.Parameters.Add("STARTDATE", OracleDbType.NVarchar2).Value = sdate;
                objCmd.Parameters.Add("ENDDATE", OracleDbType.NVarchar2).Value = endate;
                objCmd.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = stime;
                objCmd.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = endtime;
                objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                objCmd.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = cy.SchQty;
                objCmd.Parameters.Add("PRODQTY", OracleDbType.NVarchar2).Value = cy.ProdQty;
                objCmd.Parameters.Add("PRODLOGID", OracleDbType.NVarchar2).Value = cy.ProdLogId;
                objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.ProdSchNo;
                objCmd.Parameters.Add("BATCHCOMP", OracleDbType.NVarchar2).Value = cy.batchcomplete;
                objCmd.Parameters.Add("TOTALINPUT", OracleDbType.NVarchar2).Value = cy.totalinqty;
                objCmd.Parameters.Add("TOTALOUTPUT", OracleDbType.NVarchar2).Value = cy.totaloutqty;
                objCmd.Parameters.Add("TOTCONSQTY", OracleDbType.NVarchar2).Value = cy.totalconsqty;
                objCmd.Parameters.Add("TOTRMQTY", OracleDbType.NVarchar2).Value = cy.totaRmqty;
                objCmd.Parameters.Add("TOTRMVALUE", OracleDbType.NVarchar2).Value = cy.totalRmValue;
                objCmd.Parameters.Add("TOTALWASTAGE", OracleDbType.NVarchar2).Value = cy.wastageqty;
                objCmd.Parameters.Add("TOTMACHINEVALUE", OracleDbType.NVarchar2).Value = cy.Machine;
                objCmd.Parameters.Add("TOTCONSVALUE", OracleDbType.NVarchar2).Value = cy.CosValue;
                objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
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
                    if (cy.inplst != null)
                    {
                        if (cy.ID == null)
                        {
                            foreach (ProInputItem cp in cy.inplst)
                            {

                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODINPDET (BPRODBASICID,IITEMID,IBINID,IDRUMNO,IBATCHNO,IBATCHQTY,IQTY) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.BinId + "','" + cp.drumno + "','" + cp.batchno + "','" + cp.batchqty + "','" + cp.IssueQty + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();




                                }
                            }
                        }
                        else
                        {

                            svSQL = "Delete BPRODINPDET WHERE BPRODBASICID='" + cy.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                            foreach (ProInputItem cp in cy.inplst)
                            {

                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODINPDET (BPRODBASICID,IITEMID,IBINID,IDRUMNO,IBATCHNO,IBATCHQTY,IQTY) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.BinId + "','" + cp.drumno + "','" + cp.batchno + "','" + cp.batchqty + "','" + cp.IssueQty + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();




                                }
                            }
                        }
                    }
                    if (cy.Binconslst != null)
                    {
                        if (cy.ID == null)
                        {
                            foreach (BProInCons cp in cy.Binconslst)
                            {
                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODCONSDET (BPRODBASICID,CITEMID,CBINID,CUNIT,CONSQTY,CVALUE) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.BinId + "','" + cp.consunit + "','" + cp.consQty + "','" + cp.ConsStock + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }
                        else
                        {
                            svSQL = "Delete BPRODCONSDET WHERE BPRODBASICID='" + cy.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                            foreach (BProInCons cp in cy.Binconslst)
                            {
                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODCONSDET (BPRODBASICID,CITEMID,CBINID,CUNIT,CONSQTY,CVALUE) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.BinId + "','" + cp.consunit + "','" + cp.consQty + "','" + cp.ConsStock + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                    if (cy.Boutlst != null)
                    {
                        if (cy.ID == null)
                        {
                            foreach (Boutput cp in cy.Boutlst)
                            {
                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODOUTDET (BPRODBASICID,OITEMID,DSDT,DEDT,STIME,ETIME,OBATCHNO,ODRUMNO,OSTOCK,OQTY,OXQTY,STATUS,TOLOCATION) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.startdate + "','" + cp.enddate + "','" + cp.starttime + "','" + cp.endtime + "','" + cp.batchno + "','" + cp.drumno + "','" + cp.OutStock + "','" + cp.OutQty + "','" + cp.ExcessQty + "','" + cp.status + "','" + cp.toloc + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();



                                }
                            }
                        }
                        else
                        {
                            svSQL = "Delete BPRODOUTDET WHERE BPRODBASICID='" + cy.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                            foreach (Boutput cp in cy.Boutlst)
                            {
                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODOUTDET (BPRODBASICID,OITEMID,DSDT,DEDT,STIME,ETIME,OBATCHNO,ODRUMNO,OSTOCK,OQTY,OXQTY,STATUS,TOLOCATION) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.startdate + "','" + cp.enddate + "','" + cp.starttime + "','" + cp.endtime + "','" + cp.batchno + "','" + cp.drumno + "','" + cp.OutStock + "','" + cp.OutQty + "','" + cp.ExcessQty + "','" + cp.status + "','" + cp.toloc + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();



                                }
                            }
                        }
                    }
                    if (cy.Bwastelst != null)
                    {
                        if (cy.ID == null)
                        {
                            foreach (Bwastage cp in cy.Bwastelst)
                            {
                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODWASTEDET (BPRODBASICID,WITEMID,WBINID,WLOCATION,WQTY,WBATCHNO) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.BinId + "','" + cp.toloc + "','" + cp.wastageQty + "','" + cp.batchno + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();




                                }
                            }
                        }
                        else
                        {
                            svSQL = "Delete BPRODWASTEDET WHERE BPRODBASICID='" + cy.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                            objCmdd.ExecuteNonQuery();
                            foreach (Bwastage cp in cy.Bwastelst)
                            {
                                if (cp.Isvalid == "Y" && cp.ItemId != "0" && cp.ItemId != null)
                                {
                                    svSQL = "Insert into BPRODWASTEDET (BPRODBASICID,WITEMID,WBINID,WLOCATION,WQTY,WBATCHNO) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.BinId + "','" + cp.toloc + "','" + cp.wastageQty + "','" + cp.batchno + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

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

    public string BPRODStock(ProductionEntry cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty; string svSQL = "";

            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {

                try
                {
                    if ((cy.inputlst?.Count ?? 0) != 0)
                    {
                        foreach (ProIn cp in cy.inputlst)
                        {
                            if (cp.saveitemId != "0")
                            {
                                //////////////////////Inventory Input Item /////////////////////
                                double qty = cp.IssueQty;


                                ////////////////////// Purchase Inventory
                                if (cp.Purchasestock == "Yes")
                                {


                                    DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRN_ID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.saveitemId + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.LOCID + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.BranchId + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                            if (rqty >= qty)
                                            {
                                                double bqty = rqty - qty;

                                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                                {
                                                    string Sql = string.Empty;
                                                    Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                                    objConnT.Open();
                                                    objCmds.ExecuteNonQuery();
                                                    objConnT.Close();
                                                }



                                                using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                                {
                                                    OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnIn);
                                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                    objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                                    objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                    objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                    objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                    objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                                    objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                    objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                    objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                    objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                                    objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                    objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                    objConnIn.Open();
                                                    objCmdIn.ExecuteNonQuery();
                                                    objConnIn.Close();

                                                }

                                                using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                                {
                                                    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                    objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinid;
                                                    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODIN";
                                                    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WCID;
                                                    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                                    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = 0;
                                                    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                                    objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                                    objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = cp.batchqty;
                                                    objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                                    objConnIn.Open();
                                                    objCmdIn.ExecuteNonQuery();
                                                    objConnIn.Close();

                                                }


                                                break;
                                            }
                                            else
                                            {
                                                qty = qty - rqty;

                                                /////////////////////////////////Outward Entry
                                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                                {
                                                    string Sql = string.Empty;
                                                    Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + rqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                                    objConnT.Open();
                                                    objCmds.ExecuteNonQuery();
                                                    objConnT.Close();
                                                }

                                                using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                                {
                                                    OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnIn);
                                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                    objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                                    objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                    objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                    objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                    objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
                                                    objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                    objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                    objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                    objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                                    objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                    objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                    objConnIn.Open();
                                                    objCmdIn.ExecuteNonQuery();
                                                    objConnIn.Close();

                                                }

                                                using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                                {
                                                    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                    objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinid;
                                                    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODIN";
                                                    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WCID;
                                                    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                                    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = 0;
                                                    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                                    objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                                    objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = cp.batchqty;
                                                    objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                                    objConnIn.Open();
                                                    objCmdIn.ExecuteNonQuery();
                                                    objConnIn.Close();

                                                }



                                            }



                                        }
                                    }
                                }
                                ////////////////////// Purchase Inventory
                                else
                                {
                                    ///////////////////Drum Inventory

                                    DataTable dt = datatrans.GetData("select DRUM_STOCK_ID from DRUM_STOCK where DRUM_ID='" + cp.drumid + "' and BALANCE_QTY!=0 AND LOCID='" + cy.LOCID + "' ");
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {


                                            using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                            {
                                                string Sql = string.Empty;
                                                Sql = "Update DRUM_STOCK SET  BALANCE_QTY='0' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() + "'";
                                                OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                                objConnT.Open();
                                                objCmds.ExecuteNonQuery();
                                                objConnT.Close();
                                            }



                                            using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            {
                                                OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinid;
                                                objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODINCONS";
                                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                                objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["DRUM_STOCK_ID"].ToString();
                                                objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "N";
                                                objConnIn.Open();
                                                objCmdIn.ExecuteNonQuery();
                                                objConnIn.Close();

                                            }

                                        }

                                    }
                                    ///////////////////Drum Inventory
                                }



                                //////////////////////Inventory Input Item /////////////////////
                            }
                        }
                    }
                    if ((cy.inconslst?.Count ?? 0) != 0)
                    {
                        foreach (ProInCons cp in cy.inconslst)
                            {
                        if (cp.saveitemId != "0")
                        {

                            //////////////////////Inventory Input Item /////////////////////
                            double qty = cp.consQty;


                            ////////////////////// Purchase Inventory
                            if (cp.Purchasestock == "Yes")
                            {


                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRN_ID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.saveitemId + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.LOCID + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.BranchId + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                        if (rqty >= qty)
                                        {
                                            double bqty = rqty - qty;

                                            using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                            {
                                                string Sql = string.Empty;
                                                Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                                objConnT.Open();
                                                objCmds.ExecuteNonQuery();
                                                objConnT.Close();
                                            }



                                            using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            {
                                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnIn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                                objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                objConnIn.Open();
                                                objCmdIn.ExecuteNonQuery();
                                                objConnIn.Close();

                                            }

                                            using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            {
                                                OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinconsid;
                                                objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODINCONS";
                                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WCID;
                                                objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                                objConnIn.Open();
                                                objCmdIn.ExecuteNonQuery();
                                                objConnIn.Close();

                                            }


                                            break;
                                        }
                                        else
                                        {
                                            qty = qty - rqty;

                                            /////////////////////////////////Outward Entry
                                            using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                            {
                                                string Sql = string.Empty;
                                                Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + rqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                                objConnT.Open();
                                                objCmds.ExecuteNonQuery();
                                                objConnT.Close();
                                            }

                                            using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            {
                                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnIn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
                                                objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUETOBPROD";
                                                objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LOCID;
                                                objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                objConnIn.Open();
                                                objCmdIn.ExecuteNonQuery();
                                                objConnIn.Close();

                                            }

                                            using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                            {
                                                OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                                objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.Proinconsid;
                                                objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODINCONS";
                                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WCID;
                                                objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = 0;
                                                objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                                objConnIn.Open();
                                                objCmdIn.ExecuteNonQuery();
                                                objConnIn.Close();

                                            }



                                        }



                                    }
                                }
                            }
                            ////////////////////// Purchase Inventory
                            else
                            {
                                    ///////////////////Drum Inventory
                                    
                                   ///////////////////Drum Inventory
                             }



                            //////////////////////Inventory Input Item /////////////////////

                        }
                    }
                }
                    if ((cy.outlst?.Count ?? 0) != 0)
                    {
                        foreach (output cp in cy.outlst)
                        {
                            if (cp.saveitemId != "0")
                            {

                                double qty = cp.OutQty;
                                using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value =0;
                                    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                    objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drumno;
                                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.outid;
                                    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODOUT";
                                    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cp.locid;
                                    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                    objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                    objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = 0;
                                    objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "N";
                                    objConnIn.Open();
                                    objCmdIn.ExecuteNonQuery();
                                    objConnIn.Close();

                                }

                            }
                        }
                    }
                    if ((cy.wastelst?.Count ?? 0) != 0)
                    {
                        foreach (wastage cp in cy.wastelst)
                        {
                            if (cp.saveitemId != "0")
                            {

                                double qty = cp.wastageQty;
                                using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConnIn);
                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = 0;
                                    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.saveitemId;
                                    objCmdIn.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = 0;
                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value ="";
                                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.wasteid;
                                    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "BPRODWASTE";
                                    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cp.locid;
                                    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = qty;
                                    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = 0;
                                    objCmdIn.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                    objCmdIn.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = 0;
                                    objCmdIn.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "N";
                                    objConnIn.Open();
                                    objCmdIn.ExecuteNonQuery();
                                    objConnIn.Close();

                                }


                            }
                        }
                    }
                       

                    using (OracleConnection objConnT = new OracleConnection(_connectionString))
                    {
                        string Sql = string.Empty;
                        Sql = "UPDATE bprodbasic SET IS_INV='Y' WHERE BPRODBASICID='" + cy.ID + "'";
                        OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                        objConnT.Open();
                        objCmds.ExecuteNonQuery();
                        objConnT.Close();
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
    public DataTable GetWorkCenter()
    {
        string SvSql = string.Empty;
        SvSql = "Select WCID,WCBASICID from WCBASIC ";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetBatchProduction(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select BRANCH,PROCESSID,WCID,SHIFT,to_char(BPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,ETYPE,DOCID,BATCH,ENTEREDBY,SCHQTY,PRODQTY,PRODLOGID,PSCHNO,BATCHCOMP,TOTALINPUT,TOTALOUTPUT,TOTCONSQTY,TOTRMQTY,TOTRMVALUE,TOTALWASTAGE,TOTMACHINEVALUE,TOTCONSVALUE from BPRODBASIC where BPRODBASICID ='" + id +"' ";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;

    }

    public DataTable EditProEntry(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select WCBASIC.WCBASICID,BRANCHMAST.BRANCHMASTID,BRANCHMAST.BRANCHID,WCBASIC.WCID,PROCESSMAST.PROCESSID,BPRODBASIC.DOCID,to_char(BPRODBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,BPRODBASIC.STARTTIME,BPRODBASIC.ENDTIME,to_char(BPRODBASIC.STARTDATE,'dd-MON-yyyy') STARTDATE,to_char(BPRODBASIC.ENDDATE,'dd-MON-yyyy') ENDDATE,BPRODBASIC.ETYPE,BPRODBASIC.TOTALINPUT,BPRODBASIC.TOTALOUTPUT,BPRODBASIC.TOTALWASTAGE,BPRODBASIC.TOTCONSQTY,BPRODBASIC.TOTRMVALUE,BPRODBASIC.TOTCONSVALUE,BPRODBASIC.TOTMACHINEVALUE,BPRODBASIC.TOTINPUTVALUE,BPRODBASIC.TOTRMQTY,BPRODBASIC.SHIFT,BPRODBASIC.BATCHCOMP,BPRODBASIC.BATCH,BPRODBASIC.PRODQTY,BPRODBASIC.SCHQTY,LPRODBASIC.DOCId as prodlog,PSBASIC.DOCID as Prodsch from BPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BPRODBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON BPRODBASIC.WCID=WCBASIC.WCBASICID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=BPRODBASIC.PROCESSID LEFT OUTER JOIN LPRODBASIC ON LPRODBASIC.LPRODBASICID=BPRODBASIC.PRODLOGID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=BPRODBASIC.PSCHNO where BPRODBASIC.BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProIndetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select BPRODINPDETID,IDRUMNO,ITEMMASTER.ITEMID,IBINID,ICDRUMNO,IBATCHNO,IBATCHQTY,IQTY,BPRODINPDET.IITEMID from BPRODINPDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODINPDET.IITEMID  WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProOutDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select BPRODOUTDET.ODRUMNO,BPRODOUTDET.TOLOCATION,BPRODOUTDETID,ITEMMASTER.ITEMID,to_char(DSDT,'dd-MON-yyyy') DSDT,to_char(DEDT,'dd-MON-yyyy') DEDT,STIME,ETIME,OBATCHNO,DRUMMAST.DRUMNO,OSTOCK,BPRODOUTDET.OQTY,OXQTY,BPRODOUTDET.STATUS,LOCDETAILS.LOCID,OITEMID from BPRODOUTDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODOUTDET.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=BPRODOUTDET.TOLOCATION LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=BPRODOUTDET.ODRUMNO  WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProConsDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select BPRODCONSDETID,ITEMMASTER.ITEMID,CBINID,CUNIT,CONSQTY,CVALUE,BPRODCONSDET.CITEMID from BPRODCONSDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODCONSDET.CITEMID   WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProwasteDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select BPRODWASTEDET.WLOCATION,BPRODWASTEDETID,ITEMMASTER.ITEMID,WBINID,LOCDETAILS.LOCID,WQTY,WBATCHNO,WITEMID from BPRODWASTEDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODWASTEDET.WITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=BPRODWASTEDET.WLOCATION   WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetBatchProInpDet(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select BPRODBASICID,IITEMID,IBINID,IDRUMNO,IBATCHNO,IBATCHQTY,IQTY from BPRODINPDET where BPRODBASICID ='" + id + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetBatchProConsDet(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select BPRODBASICID ,CITEMID,CBINID,CUNIT,CONSQTY,CVALUE from BPRODCONSDET where BPRODBASICID ='" + id + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetBatchProOutDet(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select BPRODBASICID ,OITEMID,to_char(BPRODOUTDET.DSDT,'dd-MON-yyyy')DSDT,to_char(BPRODOUTDET.DEDT,'dd-MON-yyyy')DEDT,STIME,ETIME,OBATCHNO,ODRUMNO,OSTOCK,OQTY,OXQTY,STATUS,TOLOCATION from BPRODOUTDET where BPRODBASICID ='" + id + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetBatchProWasteDet(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select BPRODBASICID ,WITEMID,WBINID,WLOCATION,WQTY,WBATCHNO from BPRODWASTEDET where BPRODBASICID ='" + id + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ShiftDeatils()
    {
        string SvSql = string.Empty;
        SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable BindProcess()
    {
        string SvSql = string.Empty;
        SvSql = "select PROCESSID,PROCESSMASTID from PROCESSMAST";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable SeacrhItem(string value)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
        if (!string.IsNullOrEmpty(value) && value != "0")
        {
            SvSql += " Where ITEMID='" + value + "'";
        }
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable DrumDeatils()
    {
        string SvSql = string.Empty;
        SvSql = "select DRUMMASTID,DRUMNO from DRUMMAST where DRUMTYPE='PRODUCTION'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable Getstkqty(string branch, string loc, string ItemId)
    {
        string SvSql = string.Empty;
        SvSql = "select SUM(BALANCE_QTY) as QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + loc + "' AND BRANCH_ID='" + branch + "' AND ITEM_ID='" + ItemId + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable Getlot(string Itemid, string barchid, string Locid)
    {
        string SvSql = string.Empty;
        SvSql = "select LOT_NO,INVENTORY_ITEM_ID from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + Locid + "' AND BRANCH_ID='" + barchid + "' AND ITEM_ID='" + Itemid + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetLotstkqty(string branch, string loc, string ItemId,string Lotno)
    {
        string SvSql = string.Empty;
        SvSql = "select SUM(BALANCE_QTY) as QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + loc + "' AND BRANCH_ID='" + branch + "' AND ITEM_ID='" + ItemId + "' AND LOT_NO='" + Lotno + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }


    public string StatusChange(string tag, int id)
    {

        try
        {
            string svSQL = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                svSQL = "UPDATE BPRODBASIC SET STATUS ='ISACTIVE' WHERE BPRODBASICID='" + id + "'";
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
}

