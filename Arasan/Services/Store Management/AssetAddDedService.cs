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
    public class AssetAddDedService :IAssetAddDed
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AssetAddDedService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public string AssetAddDedCRUD(AssetAddDed cy)
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
                        OracleCommand objCmd = new OracleCommand("ASSETADDITIONPROC", objConn);
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
                        objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                        objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                        objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                        objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = cy.Reason;
                        objCmd.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = cy.Type;
                        objCmd.Parameters.Add("BINYN", OracleDbType.NVarchar2).Value = cy.bin;
                        objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                        objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gro;
                        objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                        objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = user;
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
                                    foreach (AdDeItem cp in cy.Itlst)
                                    {
                                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                        {
                                            DataTable item = datatrans.GetData("Select LOTYN,DRUMYN,SERIALYN,EXPYN,BINNO from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "'");

                                            svSQL = "Insert into ASADDDETAIL (ASADDBASICID,ITEMID,ITEMMASTERID,LOCDETAILSID,UNIT,VALMETHOD,BINID,DRUMYN,LOTYN,SERIALYN,EXPYN,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.ItemId + "','" + cy.Location + "','" + cp.Unit + "','W','" + item.Rows[0]["BINNO"].ToString() + "','"+ item.Rows[0]["LOTYN"].ToString()+"','"+ item.Rows[0]["DRUMYN"].ToString()+"','"+ item.Rows[0]["SERIALYN"].ToString()+"','"+ item.Rows[0]["EXPYN"].ToString()+"','" + cp.Quantity + "','" + cp.rate + "','" + cp.Amount + "') RETURNING ASADDDETAILID INTO :LASTCID";
                                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                            objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmds.ExecuteNonQuery();
                                            string detid = objCmds.Parameters["LASTCID"].Value.ToString();

                                            DataTable dt = datatrans.GetData("Select ASSTOCKVALUEID,PLUSORMINUS,ITEMID,LOCID,DOCDATE,QTY,STOCKVALUE,DOCTIME,MASTERID from ASSTOCKVALUE where ASSTOCKVALUE.ITEMID ='" + cp.ItemId + "' AND ASSTOCKVALUE.LOCID='" + cy.Location + "'");
                                            if (dt.Rows.Count > 0)
                                            {


                                                svSQL = "Insert into ASSTOCKVALUE (ITEMID,LOCID,QTY,STOCKVALUE,PLUSORMINUS,DOCDATE,DOCTIME,MASTERID,T1SOURCEID,BINID,PROCESSID,FROMLOCID,STOCKTRANSTYPE) VALUES ('" + cp.ItemId + "','" + cy.Location + "','" + cp.Quantity + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','p','" + cy.Docdate + "','10:00:00 AM','" + dt.Rows[0]["MASTERID"].ToString() + "','" + detid + "','0','0','0','Asset Addition') RETURNING ASSTOCKVALUEID INTO :LASTCID";

                                                 objCmds = new OracleCommand(svSQL, objConn);
                                                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                                objCmds.ExecuteNonQuery();
                                                string detailid = objCmds.Parameters["LASTCID"].Value.ToString();

                                                string narr = "Upward Stock Adjustment owing to : RETURNABLE ENTRY";
                                                svSQL = "Insert into ASSTOCKVALUE2 (ASSTOCKVALUEID,RATE,DOCID,MDCTRL,NARRATION,ALLOWDELETE,ISSCTRL,RECCTRL) VALUES ('" + detailid + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cy.DocId + "','T','" + narr + "','T','T','F')";

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
                if (cy.Type == "DIRECT DEDUCTION")
                {
                    if (cy.ID == null)
                    {
                        datatrans = new DataTransactions(_connectionString);
                        int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Ade-' AND ACTIVESEQUENCE = 'T'");
                        string DocNo = string.Format("{0}{1}", "Ade-", (idc + 1).ToString());

                        string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Ade-' AND ACTIVESEQUENCE ='T'";
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
                        OracleCommand objCmd = new OracleCommand("ASSETDEDUCTIONPROC", objConn);
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
                        objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                        objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                        objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                        objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = cy.Reason;
                        objCmd.Parameters.Add("BINYN", OracleDbType.NVarchar2).Value = cy.bin;
                        objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                        objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gro;
                        objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                        objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = user;
                        objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = user;
                        objCmd.Parameters.Add("MATSUPP", OracleDbType.NVarchar2).Value = "OWN";
                        objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                        objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                        try
                        {
                            objConn.Open();
                            objCmd.ExecuteNonQuery();
                            Object Pid1 = objCmd.Parameters["OUTID"].Value;
                            //string Pid = "0";
                            if (cy.ID != null)
                            {
                                Pid1 = cy.ID;
                            }
                            if (cy.Itlst != null)
                            {
                                if (cy.ID == null)
                                {
                                    foreach (AdDeItem cp in cy.Itlst)
                                    {
                                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                        {
                                            string item = datatrans.GetDataString("Select BINNO from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "'");

                                            svSQL = "Insert into ASDEDDETAIL (ASDEDBASICID,ITEMID,ITEMMASTERID,UNIT,VALMETHOD,BINID,CLSTK,AVLSTK,QTY,RATE,AMOUNT) VALUES ('" + Pid1 + "','" + cp.ItemId + "','" + cp.ItemId + "','" + cp.Unit + "','W','" + item + "','" + cp.Stock + "','" + cp.Stock  + "','" + cp.Quantity + "','" + cp.rate + "','" + cp.Amount + "') RETURNING ASDEDDETAILID INTO :LASTCID";
                                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                            objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmds.ExecuteNonQuery();
                                            string detid = objCmds.Parameters["LASTCID"].Value.ToString();

                                            DataTable dt = datatrans.GetData("Select ASSTOCKVALUEID,PLUSORMINUS,ITEMID,LOCID,DOCDATE,QTY,STOCKVALUE,DOCTIME,MASTERID from ASSTOCKVALUE where ASSTOCKVALUE.ITEMID ='" + cp.ItemId + "' AND ASSTOCKVALUE.LOCID='" + cy.Location + "'");
                                            if (dt.Rows.Count > 0)
                                            {


                                                svSQL = "Insert into ASSTOCKVALUE (ITEMID,LOCID,QTY,STOCKVALUE,PLUSORMINUS,DOCDATE,DOCTIME,MASTERID,T1SOURCEID,BINID,PROCESSID,FROMLOCID,STOCKTRANSTYPE) VALUES ('" + cp.ItemId + "','" + cy.Location + "','" + cp.Quantity + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','m','" + cy.Docdate + "','11:00:00 PM','" + dt.Rows[0]["MASTERID"].ToString() + "','" + detid + "','0','0','0','Asset Deduction') RETURNING ASSTOCKVALUEID INTO :LASTCID";

                                                objCmds = new OracleCommand(svSQL, objConn);
                                                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                                objCmds.ExecuteNonQuery();
                                                string detailid = objCmds.Parameters["LASTCID"].Value.ToString();

                                                string narr = "Downward Stock Adjustment owing to : ADJUSTMENTS";
                                                svSQL = "Insert into ASSTOCKVALUE2 (ASSTOCKVALUEID,RATE,DOCID,MDCTRL,NARRATION,ALLOWDELETE,ISSCTRL,RECCTRL) VALUES ('" + detailid + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cy.DocId + "','T','" + narr + "','T','T','F')";

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

        public DataTable GetAllAddition(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select LOCDETAILS.LOCID,DOCID,to_char(ASADDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REASON,ASADDBASICID, ASADDBASIC.IS_ACTIVE from ASADDBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = ASADDBASIC.LOCID where ASADDBASIC.IS_ACTIVE = 'Y' ORDER BY ASADDBASICID DESC";

            }
            else
            {
                SvSql = "select LOCDETAILS.LOCID,DOCID,to_char(ASADDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REASON, ASADDBASIC.IS_ACTIVE from ASADDBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = ASADDBASIC.LOCID where ASADDBASIC.IS_ACTIVE = 'N' ORDER BY ASADDBASICID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllDeduction(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select LOCDETAILS.LOCID,DOCID,to_char(ASDEDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REASON,ASDEDBASICID, ASDEDBASIC.IS_ACTIVE from ASDEDBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = ASDEDBASIC.LOCID where ASDEDBASIC.IS_ACTIVE = 'Y' ORDER BY ASDEDBASICID DESC";

            }
            else
            {
                SvSql = "select LOCDETAILS.LOCID,DOCID,to_char(ASDEDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REASON, ASDEDBASIC.IS_ACTIVE from ASDEDBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = ASDEDBASIC.LOCID where ASDEDBASIC.IS_ACTIVE = 'N' ORDER BY ASDEDBASICID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
