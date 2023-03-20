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
                //objCmd.Parameters.Add("STARTDATE", OracleDbType.Date).Value = DateTime.Parse(cy.startdate);
                //objCmd.Parameters.Add("ENDDATE", OracleDbType.Date).Value = DateTime.Parse(cy.enddate);
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

