using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class BatchCreationService : IBatchCreation
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BatchCreationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<BatchCreation> GetAllBatchCreation()
        {
            List<BatchCreation> cmpList = new List<BatchCreation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select   BRANCHMAST.BRANCHID,DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,BCPRODBASICID from BCPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BCPRODBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BatchCreation cmp = new BatchCreation
                        {

                            ID = rdr["BCPRODBASICID"].ToString(),

                            Branch = rdr["BRANCHID"].ToString(),
                            WorkCenter = rdr["WCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),

                            DocDate = rdr["DOCDATE"].ToString(),
                          

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string BatchCRUD(BatchCreation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("BATCHCREATIONPROC", objConn);
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
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.BatchNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("WPROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                  
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("SEQYN", OracleDbType.NVarchar2).Value = cy.Seq;
                    objCmd.Parameters.Add("IORATIOFROM", OracleDbType.NVarchar2).Value = cy.IOFrom;
                    objCmd.Parameters.Add("IORATIOTO", OracleDbType.NVarchar2).Value = cy.IOTo;
                    objCmd.Parameters.Add("MTONO", OracleDbType.NVarchar2).Value = cy.MTO;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.Prod;
                    objCmd.Parameters.Add("PTYPE", OracleDbType.NVarchar2).Value = cy.Shall;
                    objCmd.Parameters.Add("BTYPE", OracleDbType.NVarchar2).Value = cy.Leaf;

                    objCmd.Parameters.Add("REFDOCID", OracleDbType.NVarchar2).Value = cy.RefBatch;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narr;
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
                        foreach (BatchItem cp in cy.BatchLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ProcessId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("BATCHDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("BCPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("BWCID", OracleDbType.NVarchar2).Value = cp.WorkId;
                                    objCmds.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cp.ProcessId;
                                    objCmds.Parameters.Add("PSEQ", OracleDbType.NVarchar2).Value = cp.Seq;
                                    objCmds.Parameters.Add("INSREQ", OracleDbType.NVarchar2).Value = cp.Req;
                                   
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                   
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (BatchInItem cp in cy.BatchInLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("BCINPUTDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("BCPRODBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("IPROCESSID", OracleDbType.NVarchar2).Value = cp.Process;
                                    objCmds.Parameters.Add("IITEMID", OracleDbType.NVarchar2).Value = cp.Item;
                                    objCmds.Parameters.Add("IUNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("IQTY", OracleDbType.NVarchar2).Value = cp.Qty;

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
        public DataTable GetProcess(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID,PROCESSMAST.PROCESSID from WCBASIC LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=WCBASIC.PROCESSID where WCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcessid(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID,PROCESSMAST.PROCESSID from WCBASIC LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=WCBASIC.PROCESSID where WCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcessid()
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID,PROCESSMASTID from PROCESSMAST ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,WCID,DOCID,DOCDATE,WPROCESSID,ENTEREDBY,SEQYN,IORATIOFROM,IORATIOTO,MTONO,PSCHNO,PTYPE,BTYPE,REFDOCID,NARR,BCPRODBASICID from BCPRODBASIC Where BCPRODBASICID='"+ id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreationDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,BWCID,PROCESSID,PSEQ,INSREQ from BCPRODDETAIL where BCPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkCenterGr(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  WCID,WCBASICID from WCBASIC where PROCESSID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
