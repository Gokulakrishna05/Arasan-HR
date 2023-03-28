using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services;

public class ProductionEntryService : IProductionEntry
{
    private readonly string _connectionString;
    DataTransactions datatrans;
    public ProductionEntryService(IConfiguration _configuratio)
    {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
    }
    public IEnumerable<ProductionEntry> GetAllProductionEntry()
    {
        List<ProductionEntry> cmpList = new List<ProductionEntry>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {

            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "Select  BRANCHMAST.BRANCHID, ETYPE,NPRODBASIC. DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,NPRODBASICID,ISCURING from NPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=NPRODBASIC.BRANCH ";
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProductionEntry cmp = new ProductionEntry
                    {
                        ID = rdr["NPRODBASICID"].ToString(),
                        Branch = rdr["BRANCHID"].ToString(),
                        EntryType = rdr["ETYPE"].ToString(),
                        DocId = rdr["DOCID"].ToString(),
                        Shiftdate = rdr["DOCDATE"].ToString(),
                        IsCuring= rdr["ISCURING"].ToString()

                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
    }

    public DataTable GetInwardEntry()
    {
        string SvSql = string.Empty;
        SvSql = "Select BRANCHMAST.BRANCHID, ETYPE,NPRODBASIC. DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,NPRODBASICID from NPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=NPRODBASIC.BRANCH WHERE NPRODBASIC.ISCURING='Y'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    //public IEnumerable<ProductionEntry> GetInwardEntry()
    //{
    //    List<ProductionEntry> cmpList = new List<ProductionEntry>();
    //    using (OracleConnection con = new OracleConnection(_connectionString))
    //    {

    //        using (OracleCommand cmd = con.CreateCommand())
    //        {
    //            con.Open();
    //            cmd.CommandText = "Select BRANCHMAST.BRANCHID, ETYPE,NPRODBASIC. DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,NPRODBASICID from NPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=NPRODBASIC.BRANCH WHERE NPRODBASIC.ISCURING='Y'";
    //            OracleDataReader rdr = cmd.ExecuteReader();
    //            while (rdr.Read())
    //            {
    //                ProductionEntry cmp = new ProductionEntry
    //                {
    //                    ID = rdr["NPRODBASICID"].ToString(),
    //                    Branch = rdr["BRANCHID"].ToString(),
    //                    EntryType = rdr["ETYPE"].ToString(),
    //                    DocId = rdr["DOCID"].ToString(),
    //                    Shiftdate = rdr["DOCDATE"].ToString()
    //                };
    //                cmpList.Add(cmp);
    //            }
    //        }
    //    }
    //    return cmpList;
    //}
    public DataTable GetInward()
    {
        string SvSql = string.Empty;
        SvSql = "select DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,CURINPBASICID,BRANCHMAST.BRANCHID from CURINPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=CURINPBASIC.BRANCHID WHERE CURINPBASIC.ISOUTWARD='N'  Order by CURINPBASICID DESC ";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetInwardItem(string inid)
    {
        string SvSql = string.Empty;
        SvSql = "Select ITEMMASTER.ITEMID,CURINPDETAIL.BATCHQTY,CURINPDETAIL.CURINPDETAILID,CURINPDETAIL.CURINPBASICID ,to_char(CURINPDETAIL.DUEDATE,'dd-MON-yyyy') DUEDATE,CURINPDETAIL.DRUMNO,CURINPDETAIL.BATCHNO from CURINPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=CURINPDETAIL.ITEMID ";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public string ProductionEntryCRUD(ProductionEntry cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty; string svSQL = "";

            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                OracleCommand objCmd = new OracleCommand("PRODUCTIONENTRYPROC", objConn);
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
                objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Shiftdate);
                objCmd.Parameters.Add("ETYPE", OracleDbType.NVarchar2).Value = cy.EntryType;
                objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                //objCmd.Parameters.Add("STARTDATE", OracleDbType.Date).Value = DateTime.Parse(cy.startdate);
                //objCmd.Parameters.Add("ENDDATE", OracleDbType.Date).Value = DateTime.Parse(cy.enddate);
                objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                objCmd.Parameters.Add("SCHQTY", OracleDbType.NVarchar2).Value = cy.SchQty;
                objCmd.Parameters.Add("PRODQTY", OracleDbType.NVarchar2).Value = cy.ProdQty;
                objCmd.Parameters.Add("PRODLOGID", OracleDbType.NVarchar2).Value = cy.ProdLogId;
                objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.ProdSchNo;
                objCmd.Parameters.Add("ITEMTYPE", OracleDbType.NVarchar2).Value = cy.Selection;
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
                    foreach (ProIn cp in cy.inputlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("PRODINPUTDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("IITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("IBINID", OracleDbType.NVarchar2).Value = cp.BinId;
                                objCmds.Parameters.Add("ICDRUMNO", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmds.Parameters.Add("IBATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                objCmds.Parameters.Add("IBATCHQTY", OracleDbType.NVarchar2).Value = cp.batchqty;
                                objCmds.Parameters.Add("ICSOCTKBUP", OracleDbType.NVarchar2).Value = cp.StockAvailable;
                                objCmds.Parameters.Add("IQTY", OracleDbType.NVarchar2).Value = cp.IssueQty;
                                objCmds.Parameters.Add("MLOADADD", OracleDbType.NVarchar2).Value = cp.MillLoadAdd;
                                objCmds.Parameters.Add("IOUTPUTYN", OracleDbType.NVarchar2).Value = cp.Output;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                objConns.Close();
                            }



                        }
                    }
                    foreach (ProInCons cp in cy.inconslst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("PRODCONSDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
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
                    foreach (output cp in cy.outlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("PRODOUTPUTDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
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
                    foreach (wastage cp in cy.wastelst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("PRODWASTEDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
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

    public string CuringInwardEntryCRUD(ProductionEntry cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty; string svSQL = "";

            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                OracleCommand objCmd = new OracleCommand("CURINGINWARDPPROC", objConn);
               
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
                objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.BranchId;
                objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = DateTime.Parse(cy.Shiftdate);
                objCmd.Parameters.Add("LOCDETAILSID", OracleDbType.NVarchar2).Value = cy.LOCID;
                objCmd.Parameters.Add("SHIFT", OracleDbType.Date).Value = cy.shiftid;
                objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WCID;
                objCmd.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = cy.starttime;

                objCmd.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = cy.endtime;
                objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
                objCmd.Parameters.Add("NPRODBASICID", OracleDbType.NVarchar2).Value = cy.PROID;
                objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    Object Pid = objCmd.Parameters["OUTID"].Value;
                    if (cy.ID != null)
                    {
                        Pid = cy.ID;
                    }
                   
                    
                    foreach (output cp in cy.outlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("CURINPDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("CURINPBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("FDATE", OracleDbType.NVarchar2).Value = DateTime.Parse(cy.Shiftdate);
                                objCmds.Parameters.Add("FTIME", OracleDbType.Date).Value = cy.starttime;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.Date).Value = cp.ItemId;
                                objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cp.drumno;
                                objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.batchno;
                                objCmds.Parameters.Add("BATCHQTY", OracleDbType.NVarchar2).Value = cp.OutQty;
                                objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = cp.Shed;
                                DataTable dt = datatrans.GetData("select CAPACITY,BINBASICID,BINID,OCCUPIED from BINBASIC WHERE BINBASICID='"+ cp.Shed + "'");
                                int curday = datatrans.GetDataId("select CURINGDAY from ITEMMASTER where ITEMMASTERID='"+ cp.ItemId + "'");
                                DateTime fdate= DateTime.Parse(cy.Shiftdate);
                                var dueDate = fdate.Date.AddDays(curday);
                                objCmds.Parameters.Add("CURDAY", OracleDbType.NVarchar2).Value = curday;
                                objCmds.Parameters.Add("BINMASTERID", OracleDbType.NVarchar2).Value = cp.Shed;
                                objCmds.Parameters.Add("CAPACITY", OracleDbType.NVarchar2).Value = dt.Rows[0]["CAPACITY"].ToString();
                                objCmds.Parameters.Add("OCCUPIED", OracleDbType.NVarchar2).Value = dt.Rows[0]["OCCUPIED"].ToString();
                                objCmds.Parameters.Add("DUEDATE", OracleDbType.NVarchar2).Value = dueDate;
                                objCmds.Parameters.Add("CBINID", OracleDbType.NVarchar2).Value = dt.Rows[0]["BINID"].ToString();
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
    public DataTable EditProEntry(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select WCBASIC.WCBASICID,BRANCHMAST.BRANCHMASTID,BRANCHMAST.BRANCHID,WCBASIC.WCID,PROCESSMAST.PROCESSID,NPRODBASIC.DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,NPRODBASIC.STARTTIME,NPRODBASIC.ENDTIME,to_char(NPRODBASIC.STARTDATE,'dd-MON-yyyy') STARTDATE,to_char(NPRODBASIC.ENDDATE,'dd-MON-yyyy') ENDDATE,ETYPE,TOTALINPUT,TOTALOUTPUT,TOTALWASTAGE,TOTCONSQTY,TOTRMVALUE,TOTCONSVALUE,TOTMACHINEVALUE,TOTINPUTVALUE,TOTRMQTY,SHIFT from NPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=NPRODBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON NPRODBASIC.WCID=WCBASIC.WCBASICID LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=NPRODBASIC.PROCESSID where NPRODBASICID=" + PROID  + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProIndetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,IBINID,ICDRUMNO,IBATCHNO,IBATCHQTY,ICSOCTKBUP,IQTY,MLOADADD,IOUTPUTYN from NPRODINPDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=NPRODINPDET.IITEMID  WHERE NPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProOutDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,to_char(DSDT,'dd-MON-yyyy') DSDT,to_char(DEDT,'dd-MON-yyyy') DEDT,STIME,ETIME,OBATCHNO,DRUMMAST.DRUMNO,OSTOCK,OQTY,OXQTY,STATUS,LOCDETAILS.LOCID from NPRODOUTDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=NPRODOUTDET.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=NPRODOUTDET.TOLOCATION LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=NPRODOUTDET.ODRUMNO  WHERE NPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }

    public DataTable ProOutInwardDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,to_char(DSDT,'dd-MON-yyyy') DSDT,to_char(DEDT,'dd-MON-yyyy') DEDT,STIME,ETIME,OBATCHNO,DRUMMAST.DRUMNO,OSTOCK,OQTY,OXQTY,STATUS,LOCDETAILS.LOCID from NPRODOUTDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=NPRODOUTDET.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=NPRODOUTDET.TOLOCATION LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=NPRODOUTDET.ODRUMNO  WHERE NPRODBASICID =" + PROID + " AND NPRODOUTDET.TOLOCATION='10044000011739'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProConsDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,CBINID,CUNIT,CONSQTY,CVALUE from NPRODCONSDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=NPRODCONSDET.CITEMID   WHERE NPRODBASICID =" + PROID + "";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable ProwasteDetail(string PROID)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,WBINID,LOCDETAILS.LOCID,WQTY,WBATCHNO from NPRODWASTEDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=NPRODWASTEDET.WITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=NPRODWASTEDET.WLOCATION   WHERE NPRODBASICID =" + PROID + "";
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
