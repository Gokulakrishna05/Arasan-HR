using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services;

public class StoresReturnService : IStoresReturnService
{
    private readonly string _connectionString;
    DataTransactions datatrans;
    public StoresReturnService(IConfiguration _configuratio)
    {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        datatrans = new DataTransactions(_connectionString);
    }
    public IEnumerable<StoresReturn> GetAllStoresReturn(string st, string ed)
    {
        List<StoresReturn> cmpList = new List<StoresReturn>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {

            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                if (st != null && ed != null)
                {
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,NARRATION,STORESRETBASICID from STORESRETBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESRETBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=STORESRETBASIC.FROMLOCID WHERE STORESRETBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "' ORDER BY STORESRETBASICID DESC";
                }
                else
                {
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,NARRATION,STORESRETBASICID from STORESRETBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESRETBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=STORESRETBASIC.FROMLOCID WHERE STORESRETBASIC.DOCDATE > sysdate-30 ORDER BY STORESRETBASICID DESC";

                }
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    StoresReturn cmp = new StoresReturn
                    {
                        ID = rdr["STORESRETBASICID"].ToString(),
                        Branch = rdr["BRANCHID"].ToString(),
                        Location = rdr["LOCID"].ToString(),
                        DocId = rdr["DOCID"].ToString(),
                        Docdate = rdr["DOCDATE"].ToString(),
                        RefNo = rdr["REFNO"].ToString(),
                        RefDate = rdr["REFDATE"].ToString(),
                        Narr = rdr["NARRATION"].ToString(),

                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
    }
    public IEnumerable<StoreItem> GetAllStoresReturnItem(string id)
    {
        List<StoreItem> cmpList = new List<StoreItem>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {

            using (OracleCommand cmd = con.CreateCommand())

            {
                con.Open();
                cmd.CommandText = "Select STORESRETDETAIL.QTY,STORESRETDETAIL.STORESRETDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from STORESRETDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESRETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where STORESRETDETAIL.STORESRETBASICID='" + id + "'";
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    StoreItem cmp = new StoreItem
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
    public DataTable GetSRItemDetails(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select STORESRETDETAIL.QTY,STORESRETDETAIL.STORESRETDETAILID,STORESRETDETAIL.ITEMID,STORESRETDETAIL.UNIT,UNITMAST.UNITID,RATE,AMOUNT,CONVFACTOR,FROMBINID,TOBINID from STORESRETDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESRETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESRETDETAIL.UNIT  where STORESRETDETAIL.STORESRETBASICID='" + id + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetStoresReturn(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select STORESRETBASIC.BRANCHID,STORESRETBASIC.DOCID,to_char(STORESRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,STORESRETBASIC.FROMLOCID,to_char(STORESRETBASIC.REFDATE,'dd-MON-yyyy')REFDATE,STORESRETBASIC.REFNO,STORESRETBASIC.NARRATION,STORESRETBASICID  from STORESRETBASIC where STORESRETBASIC.STORESRETBASICID=" + id + "";
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
    public string StoresReturnCRUD(StoresReturn cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty;
            //string svSQL = "";
            datatrans = new DataTransactions(_connectionString);


            int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'SRt-' AND ACTIVESEQUENCE = 'T'");
            string docid = string.Format("{0}{1}", "SRt-", (idc + 1).ToString());

            string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SRt-' AND ACTIVESEQUENCE ='T'";
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
                OracleCommand objCmd = new OracleCommand("STORESRETBASICPROC", objConn);
                /*objCmd.Connection = objConn;
                objCmd.CommandText = "STORESRETBASICPROC";*/

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
                objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                objCmd.Parameters.Add("TOLOCID", OracleDbType.NVarchar2).Value = "10001000000827";
                objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = docid;
                objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value =  cy.Docdate ;
                objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value =  cy.RefDate ;
                objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
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
                    foreach (StoreItem cp in cy.StrLst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("STORESRETDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("STORESRETBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unitid;
                                objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;

                                objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.ConFac;
                                objCmds.Parameters.Add("FROMBINID", OracleDbType.NVarchar2).Value = cp.FromBin;
                                objCmds.Parameters.Add("TOBINID", OracleDbType.NVarchar2).Value = cp.ToBin;
                                objCmds.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cp.Stock;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                Object did = objCmds.Parameters["OUTID"].Value;
                                objConns.Close();



                                DataTable dt1 = datatrans.GetData("Select WCBASICID from WCBASIC where ILOCATION='" + cy.Location + "' ");
                                string locid;
                                if (dt1.Rows.Count == 1)
                                {
                                    locid = dt1.Rows[0]["WCBASICID"].ToString();
                                }
                                else
                                {
                                    locid = "0";
                                }
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    objConnT.Open();


                                    ///////////////////////////// Input Inventory
                                    double qty = cp.Quantity;
                                    DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INV_OUT_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.ItemId + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.Location + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.Branch + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
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
                                                OracleCommand objCmdsa = new OracleCommand(Sql, objConnT);
                                                objCmdsa.ExecuteNonQuery();




                                                OracleCommand objCmdIn = new OracleCommand("INVENTORYITEMPROC", objConnT);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = did;
                                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();

                                                objCmdIn.Parameters.Add("GRN_DATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                                                objCmdIn.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
                                                objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = locid;
                                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                                                objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                objCmdIn.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();

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
                                                    objCmdIns.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
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
                                                OracleCommand objCmdsz = new OracleCommand(Sql, objConn);
                                                objCmdsz.ExecuteNonQuery();



                                                OracleCommand objCmdIn = new OracleCommand("INVENTORYITEMPROC", objConn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = did;
                                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();

                                                objCmdIn.Parameters.Add("GRN_DATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                                                objCmdIn.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.Date).Value = cy.Location;
                                                objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = locid;
                                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
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
                                                    objCmdIns.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
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






                                    objConnT.Close();
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
    public DataTable GetItem(string ItemId)
    {
        string SvSql = string.Empty;
        SvSql = "select ITEMMASTER.ITEMID,LSTOCKVALUE.ITEMID as item FROM LSTOCKVALUE LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=LSTOCKVALUE.ITEMID  WHERE LOCID='" + ItemId + "' HAVING SUM(LSTOCKVALUE.PLUSQTY-LSTOCKVALUE.MINUSQTY) > 0 GROUP BY ITEMMASTER.ITEMID,LSTOCKVALUE.ITEMID UNION select ITEMMASTER.ITEMID,STOCKVALUE.ITEMID as item FROM STOCKVALUE LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=STOCKVALUE.ITEMID  WHERE LOCID='" + ItemId + "' HAVING SUM(DECODE(STOCKVALUE.PlusOrMinus,'p',STOCKVALUE.qty,-STOCKVALUE.qty)) > 0 GROUP BY ITEMMASTER.ITEMID,STOCKVALUE.ITEMID ";
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
    public DataTable GetBin()
    {
        string SvSql = string.Empty;
        SvSql = "Select BINID,BINBASICID from BINBASIC ";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public string StatusChange(string tag, string id)
    {

        try
        {
            string svSQL = string.Empty;
            using (OracleConnection objConnT = new OracleConnection(_connectionString))
            {
                svSQL = "UPDATE STORESRETBASIC SET IS_ACTIVE ='N' WHERE STORESRETBASICID='" + id + "'";
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

    public DataTable GetAllListStoresReturnItems(string strStatus)
    {
        string SvSql = string.Empty;
        if (strStatus == "Y" || strStatus == null)
        {
            SvSql = "Select loc.LOCID as location,LOCDETAILS.LOCID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,NARRATION,STORESRETBASICID from STORESRETBASIC LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID=STORESRETBASIC.TOLOCID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESRETBASIC.FROMLOCID AND STORESRETBASIC.IS_ACTIVE='Y' ORDER BY STORESRETBASICID DESC";
        }
        else
        {
            SvSql = "Select loc.LOCID as location,LOCDETAILS.LOCID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,NARRATION,STORESRETBASICID from STORESRETBASIC LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID=STORESRETBASIC.TOLOCID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESRETBASIC.FROMLOCID AND STORESRETBASIC.IS_ACTIVE='N' ORDER BY STORESRETBASICID DESC";
        }
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
}
