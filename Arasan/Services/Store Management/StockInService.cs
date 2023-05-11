using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class StockInService : IStockIn
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
       
        public StockInService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<StockIn> GetAllStock()
        {
            List<StockIn> cmpList = new List<StockIn>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY,ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,INVENTORY_ITEM.ITEM_ID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=INVENTORY_ITEM.BRANCH_ID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID WHERE INVENTORY_ITEM.BALANCE_QTY !=0 AND INVENTORY_ITEM.LOCATION_ID='10001000000827' GROUP BY ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,INVENTORY_ITEM.ITEM_ID ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StockIn cmp = new StockIn
                        {

                            Branch = rdr["BRANCHID"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            Qty = rdr["QTY"].ToString(),
                            ItemID= rdr["ITEM_ID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                         };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetStockInItem(string Itemid)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY,ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=INVENTORY_ITEM.BRANCH_ID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID where ITEM_ID='" + Itemid + "' GROUP BY ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetIndentItem(string Itemid) 
        {
            string SvSql = string.Empty;
            SvSql = "Select PINDDETAIL.PINDDETAILID,PINDDETAIL.DEPARTMENT,ITEMMASTER.ITEMID,UNITMAST.UNITID,PINDDETAIL.QTY,PINDDETAIL.PINDBASICID,LOCDETAILS.LOCID,PINDDETAIL.PINDDETAILID ,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy') DUEDATE,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,PINDDETAIL.ITEMID as ITEM_ID from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINDDETAIL.UNIT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT LEFT OUTER JOIN PINDBASIC ON PINDBASIC.PINDBASICID=PINDDETAIL.PINDBASICID WHERE PINDDETAIL.ISISSUED='N' AND PINDDETAIL.BAL_QTY=0 AND PINDDETAIL.ITEMID='" + Itemid  + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string IssueToStockCRUD(StockIn cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                double totqty =Convert.ToDouble(cy.TotalQty);

                
                        /////////////////////////Inventory details
                        foreach (IndentList cp in cy.Indentlist)
                        {
                            if (cp.ItemId != "0")
                            {
                        double qty = Convert.ToDouble(cp.StockQty);
                        DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRN_ID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cy.ItemID + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.Location + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.Branch + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                if(rqty >= qty)
                                {
                                    double bqty = rqty - qty;
                                    /////////////////////////////////Outward Entry
                                    ///
                                    using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                    {
                                        string Sql = string.Empty;
                                        Sql = "Update PINDDETAIL SET  ISISSUED='Y',ISSUED_QTY='" + qty + "' WHERE PINDDETAILID='" + cp.IndentID + "'";
                                        OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                        objConnT.Open();
                                        objCmds.ExecuteNonQuery();
                                        objConnT.Close();
                                    }

                                    using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                    {
                                        string Sql = string.Empty;
                                        Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                        OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                        objConnT.Open();
                                        objCmds.ExecuteNonQuery();
                                        objConnT.Close();
                                    }

                                    using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                    {
                                        OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConnI);
                                        objCmdI.CommandType = CommandType.StoredProcedure;
                                        objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                        objCmdI.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                        objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Parse(dt.Rows[i]["GRN_DATE"].ToString());
                                        objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                        objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                        objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                        objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                        objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";/*cp.DamageQty*/;
                                        objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cp.LocationID;
                                        objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                        objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                        objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                        objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                        objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                        objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                        objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = dt.Rows[i]["LOT_NO"].ToString();
                                        objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                        objConnI.Open();
                                        objCmdI.ExecuteNonQuery();
                                        Object Invid = objCmdI.Parameters["OUTID"].Value;

                                        using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                        {
                                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnIn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                            objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUETOINDENT";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUETOINDENT";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cp.LocationID;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objConnIn.Open();
                                            objCmdIn.ExecuteNonQuery();
                                            objConnIn.Close();

                                        }

                                        objConnI.Close();
                                    }
                                    ///////////////////////////////Outward Entry

                                    break;
                                }
                                else
                                {
                                    qty = qty - rqty;
                                    using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                    {
                                        string Sql = string.Empty;
                                        Sql = "Update PINDDETAIL SET  ISSUED_QTY='" + rqty + "',BAL_QTY='" + qty + "' WHERE PINDDETAILID='" + cp.IndentID + "'";
                                        OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                        objConnT.Open();
                                        objCmds.ExecuteNonQuery();
                                        objConnT.Close();
                                    }
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

                                    using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                    {
                                        OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConnI);
                                        objCmdI.CommandType = CommandType.StoredProcedure;
                                        objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        objCmdI.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                        objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                        objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = dt.Rows[i]["GRN_DATE"].ToString();
                                        objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = rqty;
                                        objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = rqty;
                                        objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                        objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                        objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";/*cp.DamageQty*/;
                                        objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cp.LocationID;
                                        objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                        objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                        objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                        objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                        objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                        objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                        objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = "";
                                        objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                        objConnI.Open();
                                        objCmdI.ExecuteNonQuery();
                                        Object Invid = objCmdI.Parameters["OUTID"].Value;

                                        using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                        {
                                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnIn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                            objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "ISSUETOINDENT";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "ISSUETOINDENT";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objConnIn.Open();
                                            objCmdIn.ExecuteNonQuery();
                                            objConnIn.Close();

                                        }

                                        objConnI.Close();
                                    }
                                    ///////////////////////////////Outward Entry
                                    ///
                                   
                                }



                            }
                            ///////////////////////////////Inward Entry

                            ///////////////////////////////Inward Entry
                        }
                        /////////////////////////Inventory details
                    }
                }
                
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

    }
}
