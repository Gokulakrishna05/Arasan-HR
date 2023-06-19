using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class DirectAdditionService : IDirectAddition
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DirectAdditionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<DirectAddition> GetAllDirectAddition(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }
            List<DirectAddition> staList = new List<DirectAddition>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,DOCID,to_char(ADDBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,ADDBASICID,ADDBASIC.STATUS from ADDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ADDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=ADDBASIC.LOCID WHERE ADDBASIC.STATUS='"+ status +"'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectAddition sta = new DirectAddition
                        {
                            ID = rdr["ADDBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            ChellanNo = rdr["DCNO"].ToString(),
                            Reason = rdr["REASON"].ToString(),
                            //Gro = rdr["GROSS"].ToString(),
                            //Net = rdr["NET"].ToString(),
                            Entered = rdr["ENTBY"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),

                            status = rdr["STATUS"].ToString(),


                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

       

        public string DirectAdditionCRUD(DirectAddition ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ADDBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "ADDBASICPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = ss.Branch;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("DCNO", OracleDbType.NVarchar2).Value = ss.ChellanNo;
                    objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = ss.Reason;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = ss.Gro;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = ss.Net;
                    objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = ss.Entered;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }
                        foreach (DirectItem cp in ss.Itlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                               
                                    OracleCommand objCmds = new OracleCommand("ADDDETAILPROC", objConn);
                                    if (ss.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;

                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("ADDBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;

                                    //objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = cp.BinID;

                                    objCmds.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cp.Process;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objCmds.ExecuteNonQuery();
                                   Object Prid = objCmds.Parameters["OUTID"].Value;
                                //string Prid = "0";
                                /////////////////////////Inventory Details

                                OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                        objCmdI.CommandType = CommandType.StoredProcedure;
                                        objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                        objCmdI.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = Prid;
                                        objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Parse(ss.Docdate);
                                        objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                        objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                        objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                        objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                        objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                        objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = ss.Location;
                                        objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = ss.Branch;
                                        objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = "";
                                objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                        objCmdI.ExecuteNonQuery();
                                        Object Invid = objCmdI.Parameters["OUTID"].Value;

                                       
                                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveItemId;
                                            objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = Prid;
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "DADD";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "DADD";
                                            objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                            objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                            objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                            objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = ss.Location;
                                            objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = ss.Branch;
                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                            objCmdIn.ExecuteNonQuery();


                                objConn.Close();

                                    ////////////////////////Inventory Details

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
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

        public DataTable GetDirectAdditionDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NET,ADDBASICID  from ADDBASIC where ADDBASICID=" + id + "";
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
        public DataTable GetDAItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADDDETAIL.QTY,ADDDETAIL.ADDDETAILID,ADDDETAIL.ITEMID,UNITMAST.UNITID,RATE,AMOUNT,BINID,PROCESSID  from ADDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=ADDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where ADDDETAIL.ADDBASICID='" + id + "'"; 
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<DirectItem> GetAllDirectAdditionItem(string id)
        {
            List<DirectItem> cmpList = new List<DirectItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ADDDETAIL.QTY,ADDDETAIL.ADDDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from ADDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=ADDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where ADDDETAIL.ADDDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectItem cmp = new DirectItem
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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ADDBASIC SET STATUS ='ISACTIVE' WHERE ADDBASICID='" + id + "'";
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

    }
   
}
