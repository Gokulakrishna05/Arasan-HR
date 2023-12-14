using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class SubContractingDCService : ISubContractingDC
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SubContractingDCService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public DataTable GetPartyDetails(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "select ADD1,ADD2,CITY from PARTYMAST WHERE partymast.partymastid=" + itemId + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSubContractDrumDetails(string Itemid)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DRUM,PLUSQTY as QTY,LOTNO,RATE FROM DRUM_STOCKDET WHERE ITEMID='" + Itemid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTERID,ITEMID FROM ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetPartyItem(string ItemId)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT WIPITEMID,WCBASICID FROM WCBASIC WHERE PARTYID='" + ItemId + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,VALMETHDES,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE,QCCOMPFLAG,BINBASIC.BINID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID LEFT OUTER JOIN BINBASIC ON BINBASICID=ITEMMASTER.BINNO Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GetDrumStock(string Itemid)
        {
            string SvSql = string.Empty;
            SvSql = "select SUM(PLUSQTY)-SUM(MINSQTY) as QTY from DRUM_STOCKDET where ITEMID='" + Itemid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            string stk = "0";
            if (dtt.Rows.Count > 0)
            {
                stk = dtt.Rows[0]["Qty"].ToString();
            }
            return stk;
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE SUBCONTDCBASIC SET IS_ACTIVE ='N' WHERE SUBCONTDCBASICID='" + id + "'";
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
        public DataTable GetAllListSubContractingDCItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SUBCONTDCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,TOTQTY FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBCONTDCBASIC.LOCID WHERE SUBCONTDCBASIC.IS_ACTIVE='Y' ORDER BY SUBCONTDCBASIC.SUBCONTDCBASICID DESC";
            }
            else
            {
                SvSql = "SELECT SUBCONTDCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,TOTQTY FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBCONTDCBASIC.LOCID WHERE SUBCONTDCBASIC.IS_ACTIVE='N' ORDER BY SUBCONTDCBASIC.SUBCONTDCBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SubContractingDCCRUD(SubContractingDC ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'DC23' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "DC23", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='DC23' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ss.DocId = DocId;
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SUBCONTDCBASICPROC", objConn);
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
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = ss.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = ss.Supplier;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = ss.Add1;
                    objCmd.Parameters.Add("ADD2", OracleDbType.NVarchar2).Value = ss.Add2;
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = ss.City;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("THROUGH", OracleDbType.NVarchar2).Value = ss.Through;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = ss.Entered;
                    objCmd.Parameters.Add("TOTQTY", OracleDbType.NVarchar2).Value = ss.TotalQty;
                    //objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = ss.Recived;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narration;
                    //objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //objConn.Close();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }
                        foreach (SubContractingItem cp in ss.SCDIlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {

                                OracleCommand objCmds = new OracleCommand("SUBCONTDCDETAILPROC", objConn);
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
                                objCmds.Parameters.Add("SUBCONTDCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.ExecuteNonQuery();



                            }
                        }
                        if (ss.RECDlst != null)
                        {
                            if (ss.ID == null)
                            {
                                foreach (ReceiptDetailItem cp in ss.RECDlst)
                                {
                                    if (cp.Isvalid1 == "Y")
                                    {
                                        svSQL = "Insert into SUBCONTEDET (SUBCONTDCBASICID,RITEM,RUNIT,ERQTY,ERATE,EAMOUNT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Unit + "','" + cp.Quantity + "','" + cp.rate + "','" + cp.Amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
                            }
                            else
                            {
                                svSQL = "Delete SUBCONTEDET WHERE SUBCONTDCBASICID='" + ss.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ReceiptDetailItem cp in ss.RECDlst)
                                {
                                    if (cp.Isvalid1 == "Y")
                                    {
                                        svSQL = "Insert into SUBCONTEDET (SUBCONTDCBASICID,RITEM,RUNIT,ERQTY,ERATE,EAMOUNT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Unit + "','" + cp.Quantity + "','" + cp.rate + "','" + cp.Amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
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

        public DataTable GetSubContractingDCDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BRANCHMAST.BRANCHID,SUBCONTDCBASIC.DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SUBCONTDCBASIC.PARTYID,SUBCONTDCBASIC.ADD1,SUBCONTDCBASIC.ADD2,SUBCONTDCBASIC.CITY,SUBCONTDCBASIC.LOCID,THROUGH,ENTEREDBY,TOTQTY,NARRATION FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH Where SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUBCONTDCDETAIL.ITEMID,UNIT,QTY,RATE,AMOUNT FROM SUBCONTDCDETAIL WHERE SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditReceiptDetailItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RITEM,RUNIT,ERQTY,ERATE,EAMOUNT FROM SUBCONTEDET  WHERE SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSubViewDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BRANCHMAST.BRANCHID,SUBCONTDCBASIC.DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,SUBCONTDCBASIC.ADD1,SUBCONTDCBASIC.ADD2,SUBCONTDCBASIC.CITY,LOCDETAILS.LOCID,THROUGH,ENTEREDBY,TOTQTY,NARRATION FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBCONTDCBASIC.LOCID LEFT OUTER JOIN PARTYMAST on SUBCONTDCBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSubContractViewDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,UNIT,QTY,RATE,AMOUNT FROM SUBCONTDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTDCDETAIL.ITEMID WHERE SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetReceiptViewDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,RUNIT,ERQTY,ERATE,EAMOUNT FROM SUBCONTEDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTEDET.RITEM  WHERE SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLotDetails(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LOTYN FROM ITEMMASTER WHERE ITEMID='" + itemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
