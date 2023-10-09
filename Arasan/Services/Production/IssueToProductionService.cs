using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services 
{
    public class IssueToProductionService :IIssueToProduction
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public IssueToProductionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "select * from WCBASIC where ACTIVE='Yes'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStockDetails(string id, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select  SUM(BALANCE_QTY) as SUM_QTY  from INVENTORY_ITEM where  LOCATION_ID= '" + id + "' AND ITEM_ID = '" + item + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStockDetail(string id, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select  ITEMMASTER.ITEMID,LOT_NO ,INVENTORY_ITEM.LOT_NO as lot,BALANCE_QTY,INVENTORY_ITEM.ITEM_ID as item from INVENTORY_ITEM left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID =INVENTORY_ITEM.ITEM_ID where  LOCATION_ID= '" + id + "' AND ITEM_ID = '" + item + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,INVENTORY_ITEM.ITEM_ID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID GROUP BY ITEMMASTER.ITEMID,INVENTORY_ITEM.ITEM_ID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string IssueToProductionCRUD(IssueToProduction cy)
        {
            string msg = "";

            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.Issuelst != null)
                    {
                        foreach (IssueItem cp in cy.Issuelst)
                        {
                            if (cp.Isvalid == "Y" && cp.itemid != "0")
                            {

                                ///////////////////////////// Input Inventory
                                double qty = cp.totalqty;
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INV_OUT_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRN_ID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.itemid + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.FromLoc + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.Branch + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
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
                                            OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                            objCmds.ExecuteNonQuery();
                                            



                                            OracleCommand objCmdIn = new OracleCommand("INVENTORYITEMPROC", objConn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.itemid;
                                            objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("GRN_DATE", OracleDbType.NVarchar2).Value =cy.Docdate;
                                            objCmdIn.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("TO_WCID", OracleDbType.NVarchar2).Value = cy.Toloc;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                            objCmdIn.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();

                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value =cp.lotnoid;
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                             
                                             
                                                objCmdIn.ExecuteNonQuery();
                                                Object Pid = objCmdIn.Parameters["OUTID"].Value;
                                                //string Pid = "0";
                                                if (cy.ID != null)
                                                {
                                                    Pid = cy.ID;
                                                }
                                                





                                            break;
                                        }
                                        else
                                        {
                                            qty = qty - rqty;

                                            /////////////////////////////////Outward Entry

                                            string Sql = string.Empty;
                                            Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + rqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                            OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                            objCmds.ExecuteNonQuery();



                                            OracleCommand objCmdIn = new OracleCommand("INVENTORYITEMPROC", objConn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.itemid;
                                            objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("GRN_DATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                                            objCmdIn.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.Date).Value = "0";
                                            objCmdIn.Parameters.Add("TO_WCID", OracleDbType.NVarchar2).Value = cy.Toloc;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                            objCmdIn.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INV_OUT_ID"].ToString();

                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = cp.lotno;
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                              
                                                objCmdIn.ExecuteNonQuery();
                                                Object Pid = objCmdIn.Parameters["OUTID"].Value;
                                                //string Pid = "0";
                                                if (cy.ID != null)
                                                {
                                                    Pid = cy.ID;
                                                }
                                               
                                               



                                        }



                                    }
                                }
                                ///////////////////////////// Input Inventory

                            }

                        }
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
    }
}
