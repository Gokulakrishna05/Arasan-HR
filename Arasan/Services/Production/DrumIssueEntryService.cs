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
            datatrans = new DataTransactions(_connectionString);
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
        public DataTable DrumDeatils(string id,string item)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUM_NO,DRUM_ID  from DRUM_STOCK where BALANCE_QTY  > 0 AND LOCID= '" + id +"' AND ITEMID ='" + item + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BATCHNO,QTY from DRUM_STOCK where DRUM_ID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStockDetails(string id,string item)
        {
            string SvSql = string.Empty;
            SvSql = "select  SUM(QTY) as SUM_QTY  from DRUM_STOCK where BALANCE_QTY  > 0 AND LOCID= '" + id + "' AND ITEMID = '" + item + "'";
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
        //public DataTable GetItemDetails(string value)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select ITEMID from DRUM_STOCK";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetDrumIssuseDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,to_char(DIEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCH,ITEMID,FROMLOC,TOLOC,FROMBINID,TOBINID,UNIT,LOTSTOCK,TYPE,STOCK,ENTEREDBY,APPROVEDBY,TOTQTY,REMARKS,ISSRATE,ISSVALUE,DIEBASICID from DIEBASIC Where DIEBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
       
        public string DrumIssueEntryCRUD(DrumIssueEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'DIE#' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "DIE#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='DIE#' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.Docid = docid;
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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Itemid;
                    objCmd.Parameters.Add("FROMLOC", OracleDbType.NVarchar2).Value = cy.FromLoc;
                    objCmd.Parameters.Add("TOLOC", OracleDbType.NVarchar2).Value = cy.Toloc;
                    //objCmd.Parameters.Add("FROMBINID", OracleDbType.NVarchar2).Value = cy.Frombin;
                    //objCmd.Parameters.Add("TOBINID", OracleDbType.NVarchar2).Value = cy.Tobin;
                    objCmd.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("LOTSTOCK", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.type;
                    objCmd.Parameters.Add("STOCK", OracleDbType.NVarchar2).Value = cy.Drum;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("APPROVEDBY", OracleDbType.NVarchar2).Value = cy.Approved;
                    objCmd.Parameters.Add("TOTQTY", OracleDbType.NVarchar2).Value = cy.Qty;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Purpose;
                    objCmd.Parameters.Add("ISSRATE", OracleDbType.NVarchar2).Value = cy.IRate;
                    objCmd.Parameters.Add("ISSVALUE", OracleDbType.NVarchar2).Value = cy.IValue;
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
                            if (ca.Isvalid == "Y" && ca.drum != "0")
                            {
                                string DrumID = datatrans.GetDataString("Select DRUM_ID from DRUM_STOCK where DRUM_NO='" + ca.drum + "' ");
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
                                    objCmds.Parameters.Add("NDRUMNO", OracleDbType.NVarchar2).Value = DrumID;
                                    objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.drum;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ca.qty;
                                    objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = ca.batchno;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }


                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM_ID,ITEMID,IN_DATE,DRUM_ID,DRUM_NO,TSOURCEID,STOCKTRANSTYPE,LOCID,QTY,BALANCE_QTY,OUT_ID,BATCHNO,BATCH_QTY,ISPRODINV,DRUM_STOCK_ID from DRUM_STOCK where DRUM_STOCK.DRUM_NO='" + ca.drum + "'");

                              
                                //double qty = ca.Qty;

                                //double rqty = Convert.ToDouble(dt.Rows[0]["BALANCE_QTY"].ToString());
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                        {
                                            string Sql = string.Empty;
                                            Sql = "UPDATE DRUM_STOCK SET BALANCE_QTY ='0' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() + "'";
                                            OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                            objConnT.Open();
                                            objCmds.ExecuteNonQuery();
                                            objConnT.Close();
                                        }

                                        using (OracleConnection objConns = new OracleConnection(_connectionString))

                                        {

                                            OracleCommand objCmds = new OracleCommand("DRUMSTKPROC", objConns);
                                            objCmds.CommandType = CommandType.StoredProcedure;
                                            objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmds.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = "0";
                                            objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Itemid;
                                            objCmds.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmds.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = DrumID;
                                            objCmds.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = ca.drum;
                                            objCmds.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = Pid;
                                            objCmds.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "DRUMISSUE";
                                            objCmds.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Toloc;
                                            objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ca.qty;
                                            objCmds.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = ca.qty;
                                            objCmds.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["DRUM_STOCK_ID"].ToString();
                                            objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = ca.batch;
                                            objCmds.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = "0";
                                            objCmds.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                            objConns.Open();
                                            objCmds.ExecuteNonQuery();

                                            objConns.Close();

                                        }
                                    }
                                }
                            }
                             
                        }

                        //string updateCMd = " UPDATE DRUM_STOCK SET BALANCE_QTY ='0' WHERE DRUM_ID ='" + cy.Drum + "' ";
                        //datatrans.UpdateStatus(updateCMd);
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
            SvSql = "select DIEBASICID,FBINID,TBINID,NDRUMNO,QTY,BATCHNO,BATCHRATE,AMOUNT from DIEDETAIL where DIEBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public IEnumerable<DrumIssueEntry> GetAllDrumIssueEntry(string st, string ed)
        {
            List<DrumIssueEntry> cmpList = new List<DrumIssueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                if (st != null && ed != null)
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "select DOCID,to_char(DIEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,TYPE,BRANCHMAST.BRANCHID,DIEBASICID FROM DIEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DIEBASIC.BRANCH  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DIEBASIC.FROMLOC  WHERE DIEBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'  order by DIEBASICID desc ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            DrumIssueEntry cmp = new DrumIssueEntry
                            {

                                ID = rdr["DIEBASICID"].ToString(),
                                FromLoc = rdr["LOCID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                type = rdr["TYPE"].ToString(),
                                Docid = rdr["DOCID"].ToString(),
                                Docdate = rdr["DOCDATE"].ToString(),

                            };
                            cmpList.Add(cmp);
                        }
                    }

                }
                else
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "select DOCID,to_char(DIEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,TYPE,BRANCHMAST.BRANCHID,DIEBASICID FROM DIEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DIEBASIC.BRANCH  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DIEBASIC.FROMLOC WHERE DIEBASIC.DOCDATE > sysdate-30 order by DIEBASICID desc ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            DrumIssueEntry cmp = new DrumIssueEntry
                            {

                                ID = rdr["DIEBASICID"].ToString(),
                                FromLoc = rdr["LOCID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                type = rdr["TYPE"].ToString(),
                                Docid = rdr["DOCID"].ToString(),
                                Docdate = rdr["DOCDATE"].ToString(),

                            };
                            cmpList.Add(cmp);
                        }
                    }
                }
            }
            return cmpList;
        }
        public DataTable EditDrumIssue(string DRUM)
        {
            string SvSql = string.Empty; 
            SvSql = " select DOCID,to_char(DIEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCHMAST.BRANCHID,ITEMMASTER.ITEMID,LOCDETAILS.LOCID ,tol.LOCID loc ,FROMBINID,TOBINID,UNIT,LOTSTOCK,TYPE,STOCK,ENTEREDBY,APPROVEDBY,TOTQTY,ISSRATE,ISSVALUE,DIEBASICID from DIEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DIEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=DIEBASIC.ITEMID  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DIEBASIC.FROMLOC LEFT OUTER JOIN LOCDETAILS tol ON tol.LOCDETAILSID=DIEBASIC.TOLOC where DIEBASICID='" + DRUM + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditDrumDetail(string DRUM)
        {
            string SvSql = string.Empty;
            SvSql = "select DIEBASICID,FBINID,TBINID,DRUMMAST.DRUMNO,QTY,BATCHNO,BATCHRATE,AMOUNT from DIEDETAIL left outer join DRUMMAST ON DRUMMAST.DRUMMASTID=DIEDETAIL.NDRUMNO where DIEBASICID='" + DRUM + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumStockDetail(string id,string item)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUM_NO,DRUM_ID ,DRUM_STOCK.BATCHNO,BATCHNO as batch,QTY from DRUM_STOCK where BALANCE_QTY  > 0 AND LOCID= '" + id + "' AND ITEMID ='" + item + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
