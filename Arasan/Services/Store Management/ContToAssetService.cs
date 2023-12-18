using Arasan.Interface;
using Arasan.Models;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
namespace Arasan.Services
{
    public class ContToAssetService :IContToAsset
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ContToAssetService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public string ConstoassetCRUD(ContToAsset cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                string user = datatrans.GetDataString("Select USERID from EMPMAST where EMPMASTID='" + cy.Entered + "' ");
                if (cy.Type == "DIRECT ADDITION")
                {
                    if (cy.ID == null)
                    {
                        datatrans = new DataTransactions(_connectionString);
                        int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Aad-' AND ACTIVESEQUENCE = 'T'");
                        string DocNo = string.Format("{0}{1}", "Aad-", (idc + 1).ToString());

                        string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Aad-' AND ACTIVESEQUENCE ='T'";
                        try
                        {
                            datatrans.UpdateStatus(updateCMd);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        cy.DocId = DocNo;
                    }
                    using (OracleConnection objConn = new OracleConnection(_connectionString))
                    {
                        OracleCommand objCmd = new OracleCommand("CONSTOASSETPROC", objConn);
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
                        objCmd.Parameters.Add("FROMLOC", OracleDbType.NVarchar2).Value = cy.Location;
                        objCmd.Parameters.Add("DESTLOC", OracleDbType.NVarchar2).Value = cy.ToLoc;
                        objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                        objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                        objCmd.Parameters.Add("REASONCODE", OracleDbType.NVarchar2).Value = cy.Reason;
                         objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                        objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gro;
                        objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                        objCmd.Parameters.Add("CREATEDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                        objCmd.Parameters.Add("CREATEDON", OracleDbType.NVarchar2).Value = DateTime.Now; 
                        objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = user;
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
                            if (cy.Itlst != null)
                            {
                                if (cy.ID == null)
                                {
                                    foreach (ConItem cp in cy.Itlst)
                                    {
                                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                        {
                                            string item = datatrans.GetDataString("Select BINNO from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "'");

                                            svSQL = "Insert into CONTOASSETDETAIL (CONTOASSETBASICID,ITEMID,ITEMMASTERID,VALMETHODF,VALMETHODT,FBINID,TBINID,CLSTK,QTY,UNIT,FCOSTRATE,TCOSTRATE,FITEMVALUE,TITEMVALUE) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.ItemId + "','W','W','"+item+"','0','" + cp.Stock + "','" + cp.Quantity + "','" + cp.Unit + "','" + cp.rate + "','" + cp.rate + "','" + cp.Amount + "','" + cp.Amount + "') RETURNING CONTOASSETDETAILID INTO :LASTCID";
                                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                            objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmds.ExecuteNonQuery();
                                            string detid = objCmds.Parameters["LASTCID"].Value.ToString();

                                            double qty = Convert.ToDouble(cp.Quantity);
                                            DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID ='" + cp.ItemId + "',INVENTORY_ITEM.LOCATION_ID='" + cy.Location + "'");

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
                                                        OracleCommand objCmdss = new OracleCommand(Sql, objConn);

                                                        objCmdss.ExecuteNonQuery();
                                                        OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                        objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                                        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                        objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "Asset Transfer";
                                                        objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                        objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                                        objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "Asset Transfer";
                                                        objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.ToLoc;
                                                        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                        objCmdIn.ExecuteNonQuery();

                                                    }
                                                }

                                            }


                                            DataTable dt1 = datatrans.GetData("Select ASSTOCKVALUEID,PLUSORMINUS,ITEMID,LOCID,DOCDATE,QTY,STOCKVALUE,DOCTIME,MASTERID from ASSTOCKVALUE where ASSTOCKVALUE.ITEMID ='" + cp.ItemId + "' AND ASSTOCKVALUE.LOCID='" + cy.Location + "'");
                                            if (dt1.Rows.Count > 0)
                                            {


                                                svSQL = "Insert into ASSTOCKVALUE (ITEMID,LOCID,QTY,STOCKVALUE,PLUSORMINUS,DOCDATE,DOCTIME,MASTERID,T1SOURCEID,BINID,PROCESSID,FROMLOCID,STOCKTRANSTYPE) VALUES ('" + cp.ItemId + "','" + cy.ToLoc + "','" + cp.Quantity + "','" + dt1.Rows[0]["STOCKVALUE"].ToString() + "','p','" + cy.Docdate + "','10:00:00 AM','" + dt1.Rows[0]["MASTERID"].ToString() + "','" + detid + "','0','0','"+cy.Location+"','Asset Transfer') RETURNING ASSTOCKVALUEID INTO :LASTCID";

                                                objCmds = new OracleCommand(svSQL, objConn);
                                                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                                objCmds.ExecuteNonQuery();
                                                string detailid = objCmds.Parameters["LASTCID"].Value.ToString();

                                                
                                                svSQL = "Insert into ASSTOCKVALUE2 (ASSTOCKVALUEID,RATE,DOCID,MDCTRL,NARRATION,ALLOWDELETE,ISSCTRL,RECCTRL) VALUES ('" + detailid + "','" + dt1.Rows[0]["STOCKVALUE"].ToString() + "','" + cy.DocId + "','T','" + cy.Narr + "','T','T','F')";

                                                objCmds = new OracleCommand(svSQL, objConn);
                                                objCmds.ExecuteNonQuery();

                                            }
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
            
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetAllConAsset(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select LOCDETAILS.LOCID,loc.LOCID as location,DOCID,to_char(CONTOASSETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REASONCODE,CONTOASSETBASICID, CONTOASSETBASIC.IS_ACTIVE from CONTOASSETBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = CONTOASSETBASIC.FROMLOC LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID = CONTOASSETBASIC.DESTLOC  where CONTOASSETBASIC.IS_ACTIVE = 'Y' ORDER BY CONTOASSETBASICID DESC";

            }
            else
            {
                SvSql = "select LOCDETAILS.LOCID,loc.LOCID as location,DOCID,to_char(CONTOASSETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REASONCODE,CONTOASSETBASICID, CONTOASSETBASIC.IS_ACTIVE from CONTOASSETBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = CONTOASSETBASIC.FROMLOC LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID = CONTOASSETBASIC.DESTLOC  where CONTOASSETBASIC.IS_ACTIVE = 'N' ORDER BY CONTOASSETBASICID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
