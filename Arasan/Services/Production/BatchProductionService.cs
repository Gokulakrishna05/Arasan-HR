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
    }
    public IEnumerable<BatchProduction> GetAllBatchProduction()
    {
        List<BatchProduction> cmpList = new List<BatchProduction>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        { 

            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "Select   BRANCHMAST.BRANCHID,DOCID,PROCESSMAST.PROCESSID,WCBASIC.WCID,BPRODBASICID from BPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BPRODBASIC.BRANCH  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BPRODBASIC.PROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BPRODBASIC.WCID";
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
                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
    }
    public string BatchProductionCRUD(BatchProduction cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty; string svSQL = "";

            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                OracleCommand objCmd = new OracleCommand("BATCHPRODUCTIONPROC", objConn);
                /*objCmd.Connection = objConn;
                objCmd.CommandText = "DIRECTPURCHASEPROC";*/
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
                objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Shiftdate);
                objCmd.Parameters.Add("ETYPE", OracleDbType.NVarchar2).Value = cy.EntryType;
                objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                objCmd.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = cy.BatchNo;
                objCmd.Parameters.Add("STARTDATE", OracleDbType.Date).Value = DateTime.Parse(cy.startdate);
                objCmd.Parameters.Add("ENDDATE", OracleDbType.Date).Value = DateTime.Parse(cy.enddate);
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
                    foreach (ProInputItem cp in cy.inplst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("BPRODNIPDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("BPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("IITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("IBINID", OracleDbType.NVarchar2).Value = cp.BinId;
                                objCmds.Parameters.Add("IDRUMNO", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmds.Parameters.Add("IBATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                objCmds.Parameters.Add("IBATCHQTY", OracleDbType.NVarchar2).Value = cp.batchqty;
                                //objCmds.Parameters.Add("ICSOCTKBUP", OracleDbType.NVarchar2).Value = cp.StockAvailable;
                                objCmds.Parameters.Add("IQTY", OracleDbType.NVarchar2).Value = cp.IssueQty;
                                //objCmds.Parameters.Add("MLOADADD", OracleDbType.NVarchar2).Value = cp.MillLoadAdd;
                                //objCmds.Parameters.Add("IOUTPUTYN", OracleDbType.NVarchar2).Value = cp.Output;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                objConns.Close();
                            }



                        }
                    }
                    foreach (BProInCons cp in cy.Binconslst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("BPRODCONSDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("BPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("CITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("CBINID", OracleDbType.NVarchar2).Value = cp.BinId;
                                objCmds.Parameters.Add("CUNIT", OracleDbType.NVarchar2).Value = cp.consunit;
                                objCmds.Parameters.Add("CONSQTY", OracleDbType.NVarchar2).Value = cp.consQty;
                                objCmds.Parameters.Add("CVALUE", OracleDbType.NVarchar2).Value = cp.ConsStock;

                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                objConns.Close();
                            }



                        }
                    }
                    foreach (Boutput cp in cy.Boutlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("BPRODOUTPUTDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("BPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("OITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("DSDT", OracleDbType.Date).Value = DateTime.Parse(cp.startdate);
                                objCmds.Parameters.Add("DEDT", OracleDbType.Date).Value = DateTime.Parse(cp.enddate);
                                objCmds.Parameters.Add("STIME", OracleDbType.NVarchar2).Value = cp.starttime;
                                objCmds.Parameters.Add("ETIME", OracleDbType.NVarchar2).Value = cp.endtime;
                                objCmds.Parameters.Add("OBATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                objCmds.Parameters.Add("ODRUMNO", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmds.Parameters.Add("OSTOCK", OracleDbType.NVarchar2).Value = cp.OutStock;
                                objCmds.Parameters.Add("OQTY", OracleDbType.NVarchar2).Value = cp.OutQty;
                                objCmds.Parameters.Add("OXQTY", OracleDbType.NVarchar2).Value = cp.ExcessQty;
                                objCmds.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = cp.status;
                                objCmds.Parameters.Add("TOLOCATION", OracleDbType.NVarchar2).Value = cp.toloc;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                objConns.Close();
                            }



                        }
                    }
                    foreach (Bwastage cp in cy.Bwastelst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("BPRODWASTEDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("BPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("WITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("WBINID", OracleDbType.NVarchar2).Value = cp.BinId;
                                objCmds.Parameters.Add("WLOCATION", OracleDbType.NVarchar2).Value = cp.toloc;
                                objCmds.Parameters.Add("WQTY", OracleDbType.NVarchar2).Value = cp.wastageQty;
                                objCmds.Parameters.Add("WBATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;

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
        SvSql = "select WCBASIC.WCBASICID,BRANCHMAST.BRANCHMASTID,BRANCHMAST.BRANCHID,WCBASIC.WCID,PROCESSMAST.PROCESSID,BPRODBASIC.DOCID,to_char(BPRODBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,BPRODBASIC.STARTTIME,BPRODBASIC.ENDTIME,to_char(BPRODBASIC.STARTDATE,'dd-MON-yyyy') STARTDATE,to_char(BPRODBASIC.ENDDATE,'dd-MON-yyyy') ENDDATE,ETYPE,TOTALINPUT,TOTALOUTPUT,TOTALWASTAGE,TOTCONSQTY,TOTRMVALUE,TOTCONSVALUE,TOTMACHINEVALUE,TOTINPUTVALUE,TOTRMQTY,SHIFT from BPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BPRODBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON BPRODBASIC.WCID=WCBASIC.WCBASICID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=BPRODBASIC.PROCESSID where BPRODBASICID=" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProIndetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,IBINID,ICDRUMNO,IBATCHNO,IBATCHQTY,IQTY from BPRODINPDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODINPDET.IITEMID  WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProOutDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,to_char(DSDT,'dd-MON-yyyy') DSDT,to_char(DEDT,'dd-MON-yyyy') DEDT,STIME,ETIME,OBATCHNO,DRUMMAST.DRUMNO,OSTOCK,OQTY,OXQTY,STATUS,LOCDETAILS.LOCID from BPRODOUTDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODOUTDET.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=BPRODOUTDET.TOLOCATION LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=BPRODOUTDET.ODRUMNO  WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProConsDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,CBINID,CUNIT,CONSQTY,CVALUE from BPRODCONSDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODCONSDET.CITEMID   WHERE BPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProwasteDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,WBINID,LOCDETAILS.LOCID,WQTY,WBATCHNO from BPRODWASTEDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=BPRODWASTEDET.WITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=BPRODWASTEDET.WLOCATION   WHERE BPRODBASICID =" + PROID + "";
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
}

