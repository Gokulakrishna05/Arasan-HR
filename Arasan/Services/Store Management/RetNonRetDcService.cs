using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class RetNonRetDcService : IRetNonRetDc 
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RetNonRetDcService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }


        public string RetNonRetDcCRUD(RetNonRetDc cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                //if (cy.ID != null)
                //{
                //    cy.ID = null;
                //}

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Rdc-' AND ACTIVESEQUENCE = 'T'  ");
                string Did = string.Format("{0}{1}", "Rdc-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Rdc-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //string PARTY = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYID='" + cy.Party + "' ");
                //string WID = datatrans.GetDataString("Select WCBASICID from WCBASIC where WCID='" + cy.work + "' ");

                string PART = datatrans.GetDataString("Select PARTYNAME from PARTYMAST where PARTYMASTID='" + cy.Party + "' ");
                string ENTER = datatrans.GetDataString("Select EMPNAME from EMPMAST where EMPMASTID='" + cy.Approved + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RDELPROC", objConn);
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Did;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DDate;
                    objCmd.Parameters.Add("DELTYPE", OracleDbType.NVarchar2).Value = cy.DcType;
                    objCmd.Parameters.Add("THROUGH", OracleDbType.NVarchar2).Value = cy.Through;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("STKTYPE", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value =cy.RefDate;
                    objCmd.Parameters.Add("DELDATE", OracleDbType.NVarchar2).Value = cy.Delivery;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("APPBY", OracleDbType.NVarchar2).Value = cy.Approved;
                    objCmd.Parameters.Add("APPBY2", OracleDbType.NVarchar2).Value = cy.Approval2;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = 'Y';
                    objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = ENTER;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = PART;

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
                        foreach (RetNonRetDcItem cp in cy.RetLst)
                        {
                            if (cp.Isvalid == "Y" && cp.item != "0")
                            {
                                string itemname = datatrans.GetDataString("Select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.item + "' ");
                              
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("RDELDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("RDELBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = itemname;
                                    objCmds.Parameters.Add("CITEMID", OracleDbType.NVarchar2).Value = cp.item;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cp.Current;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("PURFTRN", OracleDbType.NVarchar2).Value = cp.Transaction;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;

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
        public string ApproveRetNonRetDcCRUD(RetNonRetDc cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                     
                    foreach (RetNonRetDcItem cp in cy.RetLst)
                    {
                        if (cp.saveItemId != "0")
                        {

                            /////////////////////////Inventory Update
                            if (cy.Stock == "Stock")
                            {
                                double qty = Convert.ToDouble(cp.Qty);
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID ='" + cp.saveItemId + "',INVENTORY_ITEM.LOCATION_ID='" + cy.Locationid + "'");

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
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveItemId;
                                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value =cp.detid;
                                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "Returable DC";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "Returable DC";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Locationid;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                            objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                            objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                            objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.ExecuteNonQuery();

                                        }
                                    }

                                }
                            }
                            else
                            {
                                DataTable dt = datatrans.GetData("Select ASSTOCKVALUEID,PLUSORMINUS,ITEMID,LOCID,DOCDATE,QTY,STOCKVALUE,DOCTIME,MASTERID from ASSTOCKVALUE where ASSTOCKVALUE.ITEMID ='" + cp.saveItemId + "' AND ASSTOCKVALUE.LOCID='" + cy.Locationid + "'");
                                if (dt.Rows.Count > 0)
                                {

                                  
                                        svSQL = "Insert into ASSTOCKVALUE (ITEMID,LOCID,QTY,STOCKVALUE,PLUSORMINUS,DOCDATE,DOCTIME,MASTERID,T1SOURCEID,BINID,PROCESSID,FROMLOCID,STOCKTRANSTYPE) VALUES ('" + cp.saveItemId + "','" + cy.Locationid + "','" + cp.Qty + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','m','" + cy.ADDate + "','11:00:00 PM','" + dt.Rows[0]["MASTERID"].ToString() + "','"+cp.detid+ "','0','0','0','Returable DC') RETURNING ASSTOCKVALUEID INTO :LASTCID";
                                       
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                        objCmds.ExecuteNonQuery();
                                        string detailid = objCmds.Parameters["LASTCID"].Value.ToString();

                                        string narr = "Delivered To " + cy.Party;
                                        svSQL = "Insert into ASSTOCKVALUE2 (ASSTOCKVALUEID,RATE,DOCID,MDCTRL,NARRATION,ALLOWDELETE,ISSCTRL,RECCTRL) VALUES ('" + detailid + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cy.Did + "','T','"+ narr+"','T','T','F')";

                                        objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                   
                                }
                            }
                        }
                    }
                    string Sqla = string.Empty;
                    Sqla = "Update RDELBASIC SET  STATUS='Approve' WHERE RDELBASICID='" + cy.ID + "'";
                    OracleCommand objCmdssa = new OracleCommand(Sqla, objConn);

                    objCmdssa.ExecuteNonQuery();
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

        public DataTable GetParty()
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,PARTYMASTID from PARTYMAST order by PARTYMASTID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPartyitems(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYID,ADD1,ADD2,CITY FROM PARTYMAST WHERE PARTYNAME = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYID,ADD1,ADD2,CITY FROM PARTYMAST WHERE PARTYMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        //public DataTable GetSubGroup(string id)
        //{
        //    string SvSql = string.Empty;
        //    //SvSql = "select SGCODE from ITEMSUBGROUP WHERE SGDESC = '" + id + "' ";
        //    SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMDESC = '" + id + "' ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public DataTable GetRetItemDetail(string id)
        {
           
            string SvSql = string.Empty;
            //SvSql = " select UNITMAST.UNITID,ITEMID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where ITEMMASTERID = '" + id + "' ";
            SvSql = " select UNITMAST.UNITID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT where ITEMID  = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRetItem(string id)
        {
           
            string SvSql = string.Empty;
            //SvSql = "   select UNITMAST.UNITID,ITEMID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID  where ITEMMASTER.ITEMDESC   = '" + id + "' ";
            SvSql = "    select UNITMAST.UNITID,ITEMID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = ITEMMASTER.PRIUNIT where ITEMMASTERID   = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetReturnable(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT FROMLOCID,DOCID,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DELTYPE,THROUGH,PARTYID,STKTYPE,REFNO, to_char(RDELBASIC.REFDATE,'dd-MON-yyyy')REFDATE,to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,NARRATION,APPBY,APPBY2 FROM RDELBASIC WHERE RDELBASIC.RDELBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable ViewGetReturnable(string id)
        {
            string SvSql = string.Empty;

            SvSql = "  SELECT LOCDETAILS.LOCID,FROMLOCID,DOCID,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DELTYPE,THROUGH,PARTYMAST.PARTYID ,STKTYPE,REFNO, to_char(RDELBASIC.REFDATE,'dd-MON-yyyy')REFDATE,to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,NARRATION,EMPMAST.EMPNAME,EMPMAST.EMPNAME FROM RDELBASIC LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY2 LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RDELBASIC.FROMLOCID WHERE RDELBASIC.RDELBASICID = '" + id + "' ";

           
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetReturnableItems(string id)
        {
            string SvSql = string.Empty;

            //SvSql = "select ITEMMASTER.ITEMID,UNIT,CLSTOCK,QTY,PURFTRN,RATE,AMOUNT,RDELDETAILID from RDELDETAIL left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID = RDELDETAIL.CITEMID WHERE RDELDETAIL.RDELBASICID  = '" + id + "' ";
            SvSql = "select RDELDETAIL.CITEMID,UNIT,CLSTOCK,QTY,PURFTRN,RATE,AMOUNT,RDELDETAILID from RDELDETAIL  WHERE RDELDETAIL.RDELBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetItemSubGroup(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select SGCODE from ITEMSUBGROUP WHERE ITEMSUBGROUPID  = '" + id + "' ";

        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetViewReturnableItems(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,RDELDETAIL.UNIT,RDELDETAIL.CLSTOCK,RDELDETAIL.QTY,RDELDETAIL.PURFTRN,RDELDETAIL.RATE,RDELDETAIL.AMOUNT from RDELDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = RDELDETAIL.ITEMID WHERE RDELDETAIL.RDELBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RDELBASIC SET IS_ACTIVE ='N' WHERE RDELBASICID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RDELBASIC SET IS_ACTIVE ='Y' WHERE RDELBASICID = '" + id + "'";
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
        public DataTable GetAllReturn(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select RDELBASIC.RDELBASICID,RDELBASIC.IS_ACTIVE,RDELBASIC.DOCID,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.DELTYPE,PARTYMAST.PARTYID,RDELBASIC.STATUS from RDELBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID where RDELBASIC.IS_ACTIVE = 'Y' ORDER BY RDELBASICID DESC";

            }
            else
            {
                SvSql = "select RDELBASIC.RDELBASICID,RDELBASIC.IS_ACTIVE,RDELBASIC.DOCID,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.DELTYPE,PARTYMAST.PARTYID,RDELBASIC.STATUS from RDELBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID where RDELBASIC.IS_ACTIVE = 'N' ORDER BY RDELBASICID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public async Task<IEnumerable<ReturnDetail>> GetReturns(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {

               // return await db.QueryAsync<ReturnDetail>("  SELECT LOCDETAILS.LOCID as FROMLOCID, RDELBASIC.DOCDATE, EMPMAST.EMPNAME as APPBY2,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.THROUGH,EMPMAST.EMPNAME as APPBY,ITEMMASTER.ITEMID, RDELDETAIL.QTY, RDELDETAIL.UNIT,RDELDETAIL.PURFTRN from RDELBASIC INNER JOIN ITEMMASTER ON RDELBASIC.RDELBASICID = ITEMMASTER.ITEMMASTERID INNER JOIN RDELDETAIL ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID INNER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RDELBASIC.FROMLOCID INNER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY INNER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY2 WHERE RDELDETAIL.RDELDETAILID = '" + id + "' and RDELBASIC.RDELBASICID  = '" + id + "' ", commandType: CommandType.Text);
               // return await db.QueryAsync<ReturnDetail>("   SELECT LOCDETAILS.LOCID as FROMLOCID,RDELBASIC.DELTYPE, EMPMAST.EMPNAME as APPBY2,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,RDELBASIC.THROUGH,EMPMAST.EMPNAME as APPBY,ITEMMASTER.ITEMID, RDELDETAIL.QTY, RDELDETAIL.UNIT,RDELDETAIL.PURFTRN from RDELBASIC INNER JOIN ITEMMASTER ON RDELBASIC.RDELBASICID = ITEMMASTER.ITEMMASTERID INNER JOIN RDELDETAIL ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID INNER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RDELBASIC.FROMLOCID INNER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY INNER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY2 WHERE RDELDETAIL.RDELDETAILID = '" + id + "' and RDELBASIC.RDELBASICID  = '" + id + "' ", commandType: CommandType.Text);

                //return await db.QueryAsync<ReturnDetail>(" SELECT RDELBASIC.RDELBASICID ,PARTYMAST.PARTYID, PARTYMAST.ADD1, PARTYMAST.CITY ,LOCDETAILS.LOCID as FROMLOCID,RDELBASIC.DELTYPE, EMPMAST.EMPNAME as APPBY2,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,RDELBASIC.THROUGH,EMPMAST.EMPNAME as APPBY,ITEMMASTER.ITEMID, RDELDETAIL.QTY, RDELDETAIL.UNIT,RDELDETAIL.PURFTRN from RDELBASIC INNER JOIN ITEMMASTER ON RDELBASIC.RDELBASICID = ITEMMASTER.ITEMMASTERID INNER JOIN RDELDETAIL ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID INNER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RDELBASIC.FROMLOCID INNER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY INNER JOIN EMPMAST ON EMPMAST.EMPMASTID = RDELBASIC.APPBY2 INNER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYNAME WHERE RDELDETAIL.RDELDETAILID = '" + id + "' and RDELBASIC.RDELBASICID  ='" + id + "'", commandType: CommandType.Text);
               
                
                return await db.QueryAsync<ReturnDetail>(" SELECT RDELBASIC.DOCID,RDELBASIC.RDELBASICID ,PARTYMAST.PARTYID, PARTYMAST.ADD1, PARTYMAST.CITY ,LOCDETAILS.LOCID as FROMLOCID,RDELBASIC.DELTYPE,E.EMPNAME as EBY, E2.EMPNAME as APPBY2,to_char(RDELBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,RDELBASIC.THROUGH,E1.EMPNAME as APPBY,ITEMMASTER.ITEMID, RDELDETAIL.QTY, RDELDETAIL.UNIT,RDELDETAIL.PURFTRN from RDELBASIC LEFT OUTER JOIN ITEMMASTER ON RDELBASIC.RDELBASICID = ITEMMASTER.ITEMMASTERID INNER JOIN RDELDETAIL ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID LEFT OUTER JOIN EMPMAST E ON E.EMPMASTID = RDELBASIC.APPBY LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID LEFT OUTER JOIN EMPMAST E1 ON E1.EMPMASTID = RDELBASIC.EBY LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RDELBASIC.FROMLOCID LEFT OUTER JOIN EMPMAST E2 ON E2.EMPMASTID = RDELBASIC.APPBY2 WHERE RDELDETAIL.RDELDETAILID = '" + id + "' and RDELBASIC.RDELBASICID = '" + id + "'", commandType: CommandType.Text);

            }

        }

    }
}
