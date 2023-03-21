using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Production
{
    public class DrumIssueEntryService : IDrumIssueEntryService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DrumIssueEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable BindBinID()
        {
            string SvSql = string.Empty;
            SvSql = "Select BINBASICID,BINID from BINBASIC";
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
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
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
        public DataTable GetDrumIssuseDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,to_char(DIEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCH,ITEMID,FROMLOC,TOLOC,FROMBINID,TOBINID,UNIT,LOTSTOCK,TYPE,STOCK,ENTEREDBY,APPROVEDBY,TOTQTY,REMARKS,DIEBASICID from DIEBASIC Where DIEBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
        public IEnumerable<DrumIssueEntry> GetAllDrumIssueEntry()
        {
            List<DrumIssueEntry> cmpList = new List<DrumIssueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select DOCID,to_char(DIEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCHMAST.BRANCHID,FROMLOC,TOLOC,DIEBASICID FROM DIEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DIEBASIC.BRANCH";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DrumIssueEntry cmp = new DrumIssueEntry
                        {
                            ID = rdr["DIEBASICID"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Branch = rdr["BRANCH"].ToString(),
                            FromLoc = rdr["FROMLOC"].ToString(),
                            Toloc = rdr["TOLOC"].ToString(),
                            
                           
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string DrumIssueEntryCRUD(DrumIssueEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DIEBASICPROC", objConn);

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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Itemid;
                    objCmd.Parameters.Add("FROMLOC", OracleDbType.NVarchar2).Value = cy.FromLoc;
                    objCmd.Parameters.Add("TOLOC", OracleDbType.NVarchar2).Value = cy.Toloc;
                    objCmd.Parameters.Add("FROMBINID", OracleDbType.NVarchar2).Value = cy.Frombin;
                    objCmd.Parameters.Add("TOBINID", OracleDbType.NVarchar2).Value = cy.Tobin;
                    objCmd.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("LOTSTOCK", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.type;
                    objCmd.Parameters.Add("STOCK", OracleDbType.NVarchar2).Value = cy.Drum;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("APPROVEDBY", OracleDbType.NVarchar2).Value = cy.Approved;
                    objCmd.Parameters.Add("TOTQTY", OracleDbType.NVarchar2).Value = cy.Qty;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Purpose;
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
                        foreach (DrumIssueEntryItem ca in cy.Drumlst)
                        {
                            if (ca.Isvalid == "Y" && ca.FBinId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("DIEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("DIEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("FBINID", OracleDbType.NVarchar2).Value = ca.FBinId;
                                    objCmds.Parameters.Add("TBINID", OracleDbType.NVarchar2).Value = ca.TBinid;
                                    objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.Drum;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ca.Qty;
                                    objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = ca.Batch;
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

        public DataTable GetDIEDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
