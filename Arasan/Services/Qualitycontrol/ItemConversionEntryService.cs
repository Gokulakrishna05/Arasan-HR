using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Qualitycontrol
{
    public class ItemConversionEntryService : IItemConversionEntryService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ItemConversionEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DRUM_STOCK_ID,ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID AS Item FROM DRUM_STOCK LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=DRUM_STOCK.ITEMID WHERE LOCID ='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTERID,ITEMID FROM ITEMMASTER WHERE IGROUP IN('RAW MATERIAL','Consumables')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetStockDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DRUM_STOCK_ID,BALANCE_QTY FROM DRUM_STOCK WHERE ITEMID ='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetUnitDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTERID,UNITMAST.UNITID FROM ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT WHERE ITEMMASTERID ='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumStockDetail(string id, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUM_NO,DRUM_ID,BALANCE_QTY,DRUM_STOCK_ID from DRUM_STOCK where BALANCE_QTY  > 0 AND LOCID= '" + item + "' AND ITEMID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DRUMSTOCKDETID,LOTNO FROM DRUM_STOCKDET WHERE DRUMSTKID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBinDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTERID,BINBASIC.BINID,LATPURPRICE FROM ITEMMASTER LEFT OUTER JOIN BINBASIC ON BINBASIC.BINBASICID=ITEMMASTER.BINNO WHERE ITEMMASTERID  ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<ItemConversionEntry> GetAllItemConversionEntry(string st, string ed)
        {
            List<ItemConversionEntry> cmpList = new List<ItemConversionEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    if (st != null && ed != null)
                    {
                        cmd.CommandText = "SELECT ICEBASICID,DOCID,to_char(ICEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,ITEMMASTER.ITEMID FROM ICEBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=ICEBASIC.FROMLOC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=ICEBASIC.FITEMID WHERE ICEBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "' ORDER BY ICEBASICID DESC";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT ICEBASICID,DOCID,to_char(ICEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,ITEMMASTER.ITEMID FROM ICEBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=ICEBASIC.FROMLOC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=ICEBASIC.FITEMID WHERE ICEBASIC.DOCDATE > sysdate-30 ORDER BY ICEBASICID DESC";

                    }
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemConversionEntry cmp = new ItemConversionEntry
                        {

                            ID = rdr["ICEBASICID"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string ItemConversionEntryCRUD(ItemConversionEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'ICE#' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "ICE#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='ICE#' AND ACTIVESEQUENCE ='T'";
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
                    OracleCommand objCmd = new OracleCommand("ICEBASICPROC", objConn);
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("FROMLOC", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("FITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("TITEMID", OracleDbType.NVarchar2).Value = cy.ToItem;
                    objCmd.Parameters.Add("CPURPOSE", OracleDbType.NVarchar2).Value = cy.Purpose;
                    objCmd.Parameters.Add("FUNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("TOTQTY", OracleDbType.NVarchar2).Value = cy.Total;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("APPROVEDBY", OracleDbType.NVarchar2).Value = cy.Approved;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
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
                        foreach (ConEntryItem ca in cy.ICEILst)
                        {
                            if (ca.Isvalid == "Y" && ca.drum != "0")
                            {
                                string BinID = datatrans.GetDataString("Select BINBASICID from BINBASIC where BINID='" + ca.binid + "' ");
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("ICEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("ICEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.drum;
                                    objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = ca.batchno;
                                    objCmds.Parameters.Add("FBINID", OracleDbType.NVarchar2).Value = BinID;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ca.qty;
                                    objCmds.Parameters.Add("BATCHRATE", OracleDbType.NVarchar2).Value = ca.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = ca.total;
                                    objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cy.TotAmount;
                                    objCmds.Parameters.Add("RITEMMASTERID", OracleDbType.NVarchar2).Value = cy.ToItem;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                                string Sql = string.Empty;
                                Sql = " UPDATE DRUM_STOCK SET ITEMID = '" + cy.ToItem + "' WHERE LOCID= '" + cy.Location + "' AND DRUM_NO= '" + ca.drum + "' AND ITEMID ='" + cy.Item + "'";
                                OracleCommand objCmdst = new OracleCommand(Sql, objConn);
                                objCmdst.ExecuteNonQuery();
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

        public DataTable GetEditDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTERID,BINBASIC.BINID FROM ITEMMASTER LEFT OUTER JOIN BINBASIC ON BINBASIC.BINBASICID=ITEMMASTER.BINNO WHERE ITEMMASTERID  ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
