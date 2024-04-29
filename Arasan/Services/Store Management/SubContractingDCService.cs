using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Org.BouncyCastle.Security.Certificates;
using Dapper;

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
        public DataTable GetSubContractDrumDetails(string Itemid,string loc)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT S.DRUMNO,SUM(S.PLUSQTY-S.MINUSQTY) as QTY, S.LOTNO   from LSTOCKVALUE S  WHERE S.ITEMID='" + Itemid + "' and S.LOCID='"+loc+ "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0 GROUP BY  S.DRUMNO,S.LOTNO    ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,LSTOCKVALUE.ITEMID as item FROM LSTOCKVALUE LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=LSTOCKVALUE.ITEMID  WHERE LOCID='" + value+ "' HAVING SUM(LSTOCKVALUE.PLUSQTY-LSTOCKVALUE.MINUSQTY) > 0 GROUP BY ITEMMASTER.ITEMID,LSTOCKVALUE.ITEMID UNION select ITEMMASTER.ITEMID,STOCKVALUE.ITEMID as item FROM STOCKVALUE LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=STOCKVALUE.ITEMID  WHERE LOCID='" + value+ "' HAVING SUM(DECODE(STOCKVALUE.PlusOrMinus,'p',STOCKVALUE.qty,-STOCKVALUE.qty)) > 0 GROUP BY ITEMMASTER.ITEMID,STOCKVALUE.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,ITEM_ID as item FROM INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID  WHERE ITEMMASTER.IGROUP='PACKING MATERIALS' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyItem(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,WIPITEMID FROM WCBASIC LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=WCBASIC.WIPITEMID WHERE PARTYID='" + ItemId + "' GROUP BY ITEMMASTER.ITEMID,WIPITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,VALMETHDES,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE,QCCOMPFLAG,BINBASIC.BINID,IGROUP from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID LEFT OUTER JOIN BINBASIC ON BINBASICID=ITEMMASTER.BINNO Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier( )
        {
            string SvSql = string.Empty;
            SvSql = "Select PartyMASTID ,P.PartyID,P.ADD1,P.ADD2,P.CITY, W.ConvLociD,W.ConvItemID From PartyMast P,WCBASIC w Where PartyCat  in ('SUB CONTRACTOR', 'BOTH') AND w.PARTYID = P.PARTYMASTID Order By 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCDETAILSID ,LocID,LocationType From LOCDETAILS Where LocID in ('AP','FG GODOWN-SVKS FACTORY','AP I','AP II','APS PACKING','APS','PYRO PACKING') Union Select LOCDETAILSID ,  LocID,LocationType From LOCDETAILS Where LocID in ('STORES') Order by 2";
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
                SvSql = "SELECT SUBCONTDCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,TOTQTY,SUBCONTDCBASIC.IS_ACTIVE,SUBCONTDCBASIC.STATUS FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBCONTDCBASIC.LOCID WHERE SUBCONTDCBASIC.IS_ACTIVE='Y' ORDER BY SUBCONTDCBASIC.SUBCONTDCBASICID DESC";
            }
            else
            {
                SvSql = "SELECT SUBCONTDCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,TOTQTY,SUBCONTDCBASIC.IS_ACTIVE,SUBCONTDCBASIC.STATUS FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBCONTDCBASIC.LOCID WHERE SUBCONTDCBASIC.IS_ACTIVE='N' ORDER BY SUBCONTDCBASIC.SUBCONTDCBASICID DESC";
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
                string loc = datatrans.GetDataString("SELECT LOCID FROM LOCDETAILS WHERE LOCDETAILSID='"+ss.Locationid+"'");
                ss.Narration = "Delivered To " + loc +",";
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
                                objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objCmds.ExecuteNonQuery();
                                Object detid = objCmds.Parameters["OUTID"].Value;
                                string[] Ddrum = new string[] {};
                                string[] dlot = new string[] {};
                                 if (cp.Drumsdesc!="" && cp.Drumsdesc!=null)
                                {
                                    Ddrum = cp.Drumsdesc.Split(',');
                                    

                                }
                                if (cp.Lotno != "" && cp.Lotno != null)
                                {
                                    dlot = cp.Lotno.Split('#');
                                }
                                string[] Dqty = cp.dqty.Split('-');
                                    string[] Drate = cp.drate.Split('-');
                                    for (int i = 0; i < Dqty.Length; i++)
                                    {
                                    string dddrum = "";
                                    if (Ddrum.Length > 0)
                                    {
                                        dddrum = Ddrum[i];
                                    }
                                        string ddqty = Dqty[i];
                                        string ddrate = Drate[i];
                                        string ddlot = dlot[i];


                                        if (cp.Isvalid == "Y")
                                        {

                                            svSQL = "Insert into SUBCONTDCBATCH(SUBCONTDCBASICID,PARENTRECORDID,BITEMID,BITEMMASTERID,DRUMNO,BQTY,BRATE,BAMOUNT,BLOCID,TLOT) VALUES ('" + Pid + "','" + detid + "','" + cp.ItemId + "','" + cp.ItemId + "','" + dddrum + "','" + ddqty + "','" + ddrate + "','0','" + ss.Location + "','" + ddlot + "')";
                                            objCmds = new OracleCommand(svSQL, objConn);
                                            objCmds.ExecuteNonQuery();


                                        string type = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.ItemId + "'");

                                        if (type == "YES")
                                        {
                                            string SvSql1 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE) VALUES ('0','0','F','" + detid + "','754368046','" + ss.DocId + "','" + ss.Docdate + "','" + ddlot + "' ,'0','" + ddqty + "','" + dddrum + "','" + cp.rate + "','" + cp.Amount + "','" + cp.ItemId + "','" + ss.Location + "','0','0','SUB DC' )";
                                            OracleCommand objCmdss = new OracleCommand(SvSql1, objConn);
                                            objCmdss.ExecuteNonQuery();


                                             SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,DOCTIME,STOCKTRANSTYPE) VALUES ('" + detid + "','m','" + cp.ItemId + "','" + ss.Docdate + "','" + ddqty + "' ,'"+ ss.Location + "','0','0','0','0','0','0','0','0','" + cp.Amount + "','11:00:00 PM','Conversion Issue') RETURNING STOCKVALUEID INTO :STKID";
                                            OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                            objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmdsss.ExecuteNonQuery();
                                             
                                            string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                            string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + ss.DocId + "','" + ss.Narration + "','" + cp.rate + "')";
                                            OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                            objCmddts.ExecuteNonQuery();
                                            string partyloc = datatrans.GetDataString("SELECT ILOCATION FROM WCBASIC  WHERE PARTYID='" + ss.Supplier + "'");
                                            SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,DOCTIME,STOCKTRANSTYPE) VALUES ('" + detid + "','p','" + cp.ItemId + "','" + ss.Docdate + "','" + ddqty + "' ,'" + partyloc + "','0','0','0','0','0','0','0','0','" + cp.Amount + "','11:00:00 PM','Conv to be Accomplis') RETURNING STOCKVALUEID INTO :STKID";
                                            objCmdsss = new OracleCommand(SvSql1, objConn);
                                            objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmdsss.ExecuteNonQuery();

                                            stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                            SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + ss.DocId + "','" + ss.Narration + "','" + cp.rate + "')";
                                            objCmddts = new OracleCommand(SvSql2, objConn);
                                            objCmddts.ExecuteNonQuery();
                                        }
                                        else
                                        {

                                           string SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,STOCKTRANSTYPE) VALUES ('" + detid + "','m','" + cp.ItemId + "','" + ss.Docdate + "','" + ddqty + "' ,'" + ss.Location + "','0','0','0','0','0','0','0','0','" + cp.Amount + "','Conversion Issue') RETURNING STOCKVALUEID INTO :STKID";
                                            OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                            objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmdsss.ExecuteNonQuery();

                                            string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                            string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + ss.DocId + "','" + ss.Narration + "','" + cp.rate + "')";
                                            OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                            objCmddts.ExecuteNonQuery();
                                            string partyloc = datatrans.GetDataString("SELECT ILOCATION FROM WCBASIC  WHERE PARTYID='" + ss.Supplier + "'");
                                            SvSql1 = "Insert into STOCKVALUE (T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,STOCKTRANSTYPE) VALUES ('" + detid + "','p','" + cp.ItemId + "','" + ss.Docdate + "','" + ddqty + "' ,'" + partyloc + "','0','0','0','0','0','0','0','0','" + cp.Amount + "','Conv to be Accomplis') RETURNING STOCKVALUEID INTO :STKID";
                                            objCmdsss = new OracleCommand(SvSql1, objConn);
                                            objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            objCmdsss.ExecuteNonQuery();

                                            stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                            SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + ss.DocId + "','" + ss.Narration + "','" + cp.rate + "')";
                                            objCmddts = new OracleCommand(SvSql2, objConn);
                                            objCmddts.ExecuteNonQuery();
                                        }

                                            }
                                      
                                    }
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
            SvSql = "SELECT BRANCHMAST.BRANCHID,SUBCONTDCBASIC.BRANCH,SUBCONTDCBASIC.DOCID,to_char(SUBCONTDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,SUBCONTDCBASIC.PARTYID,SUBCONTDCBASIC.ADD1,SUBCONTDCBASIC.ADD2,SUBCONTDCBASIC.CITY,LOCDETAILS.LOCID,SUBCONTDCBASIC.LOCID as loc,THROUGH,ENTEREDBY,TOTQTY,NARRATION FROM SUBCONTDCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SUBCONTDCBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBCONTDCBASIC.LOCID LEFT OUTER JOIN PARTYMAST on SUBCONTDCBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSubContractViewDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,SUBCONTDCDETAIL.ITEMID as item,UNIT,QTY,RATE,AMOUNT,SUBCONTDCDETAILID FROM SUBCONTDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTDCDETAIL.ITEMID WHERE SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetReceiptViewDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,SUBCONTEDET.RITEM,RUNIT,ERQTY,ERATE,EAMOUNT,SUBCONTEDETID FROM SUBCONTEDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTEDET.RITEM  WHERE SUBCONTDCBASICID='" + id + "'";
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
        public DataTable ViewSubContractDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUBCONTDCBASICID,PARENTRECORDID,BITEMID,BITEMMASTERID,DRUMNO,BQTY,BRATE,BAMOUNT,BLOCID,TLOT FROM SUBCONTDCBATCH   WHERE PARENTRECORDID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public string ApproveSubContractingDCCRUD(SubContractingDC cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();

                    foreach (SubContractingItem cp in cy.SCDIlst)
                    {
                        
                        if (cp.item != "0")
                        {
                            DataTable inv = datatrans.GetData("Select TLOT,BQTY,BITEMID,DRUMNO from SUBCONTDCBATCH where PARENTRECORDID ='" + cp.detid + "'");
                            
                                /////////////////////////Inventory Update
                              
                                if (inv.Rows.Count > 0)
                                {

                                    for (int i = 0; i < inv.Rows.Count; i++)
                                    {
                                    if (inv.Rows[i]["DRUMNO"].ToString() == null || inv.Rows[i]["DRUMNO"].ToString() == "")
                                    {
                                        DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where ITEM_ID='"+ inv.Rows[i]["BITEMID"].ToString() + "' AND LOT_NO='"+ inv.Rows[i]["TLOT"].ToString() + "' and LOCATION_ID='"+cy.Locationid+"'");





                                            double rqty = Convert.ToDouble(dt.Rows[0]["BALANCE_QTY"].ToString());
                                            double ddqty = Convert.ToDouble(inv.Rows[i]["BQTY"].ToString());

                                        
                                        if (rqty >= ddqty)
                                        {
                                            double bqty = rqty - ddqty;

                                            string Sql = string.Empty;
                                            Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[0]["INVENTORY_ITEM_ID"].ToString() + "'";
                                            OracleCommand objCmdss = new OracleCommand(Sql, objConn);

                                            objCmdss.ExecuteNonQuery();
                                            OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                            objCmdIn.CommandType = CommandType.StoredProcedure;
                                            objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.item;
                                            objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.detid;
                                            objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                            objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[0]["GRNID"].ToString();
                                            objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[0]["INVENTORY_ITEM_ID"].ToString();
                                            objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "SubContracting DC";
                                            objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                            objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = ddqty;
                                            objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "SubContracting DC";
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
                                    else
                                    {
                                        string drum = datatrans.GetDataString("Select DRUMMASTID from DRUMMAST where DRUMNO='" + inv.Rows[i]["DRUMNO"].ToString() + "'");

                                        DataTable dt = datatrans.GetData("Select DRUM_STOCK.BALANCE_QTY,DRUM_STOCK.ITEMID,DRUM_STOCK.LOCID,DRUM_STOCK.DRUM_NO,DRUM_ID,DRUM_STOCK_ID,WCID from DRUM_STOCK where DRUM_STOCK.ITEMID='" + cp.item + "' AND DRUM_STOCK.LOCID='"+cy.Locationid+"' and DRUM_NO='" + inv.Rows[i]["DRUMNO"].ToString() + "' and BALANCE_QTY!=0 order by DOC_DATE ASC");
                                        if (dt.Rows.Count > 0)
                                        {
                                            double ddqty = Convert.ToDouble(inv.Rows[0]["BQTY"].ToString());
                                            double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());

                                                double bqty = rqty - ddqty;

                                                string Sql = string.Empty;
                                                Sql = "Update DRUM_STOCK SET  BALANCE_QTY='" + bqty + "' WHERE DRUM_STOCK_ID='" + dt.Rows[0]["DRUM_STOCK_ID"].ToString() + "'";
                                                OracleCommand objCmds = new OracleCommand(Sql, objConn);
                                                objCmds.ExecuteNonQuery();




                                                OracleCommand objCmdIn = new OracleCommand("DRUMSTKDETPROC", objConn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = dt.Rows[0]["DRUM_STOCK_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.item;
                                                objCmdIn.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = drum;
                                                objCmdIn.Parameters.Add("DRUM", OracleDbType.NVarchar2).Value = inv.Rows[i]["DRUMNO"].ToString();
                                                objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.detid;
                                                objCmdIn.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                                objCmdIn.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "SubContracting DC";
                                                objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = dt.Rows[0]["LOCID"].ToString();
                                                objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = dt.Rows[0]["WCID"].ToString();

                                                objCmdIn.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = ddqty;
                                                objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = inv.Rows[i]["TLOT"].ToString();
                                                objCmdIn.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "";

                                                objCmdIn.ExecuteNonQuery();

                                                



                                            }
                                        
                                    }

                                }
                            }
                       
                        }
                    }
                    foreach (ReceiptDetailItem cp in cy.RECDlst)
                    {

                        if (cp.item != "0")
                        {
                            using (OracleConnection objConnI = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                objCmdI.CommandType = CommandType.StoredProcedure;
                                objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.item;
                                objCmdI.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.detid;
                                objCmdI.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value =  cp.Quantity; 
                                objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value =  cp.Quantity; 
                                objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Locationid;
                                objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branchid;

                                objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = "0";
                                objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objConnI.Open();
                                objCmdI.ExecuteNonQuery();
                                Object Invid = objCmdI.Parameters["OUTID"].Value;



                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.item;
                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = cp.detid;
                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "SUBCON DC";
                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "SUBCON DC ";
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
                                objCmdIn.ExecuteNonQuery();
                            }
                        }
                    }
                            string Sqla = string.Empty;
                    Sqla = "Update SUBCONTDCBASIC SET  STATUS='Approve' WHERE SUBCONTDCBASICID='" + cy.ID + "'";
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
        public string PackMatSubConDCCRUD(SubContractingDC cy)
        {
            string msg = "";
            if ( cy.ID != null )
            {
                cy.ID = null;
            }
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                //if (cy.ID != null)
                //{
                //    cy.ID = null;
                //}

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Ndc-' AND ACTIVESEQUENCE = 'T'  ");
                string Did = string.Format("{0}{1}", "Ndc-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Ndc-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                 

               
                //string ENTER = datatrans.GetDataString("Select EMPNAME from EMPMAST where EMPMASTID='" + cy.Approved + "' ");
                string APPROV = datatrans.GetDataString("Select EMPNAME from EMPMAST where EMPMASTID='" + cy.Enterd + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RDELPROC", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                   
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = Did;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("DELTYPE", OracleDbType.NVarchar2).Value = "Non-Returnable DC";
                    objCmd.Parameters.Add("THROUGH", OracleDbType.NVarchar2).Value = cy.Through;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.party;
                    objCmd.Parameters.Add("STKTYPE", OracleDbType.NVarchar2).Value = "Stock";
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("DELDATE", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("APPBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("APPBY2", OracleDbType.NVarchar2).Value = APPROV;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = 'Y';
                    objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("DCREFID", OracleDbType.NVarchar2).Value = cy.pakid;

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
                        foreach (PackMatItem cp in cy.packlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                string itemname = datatrans.GetDataString("Select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "' ");

                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("RDELDETAILPROC", objConns);
                                    
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("RDELBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = itemname;
                                    objCmds.Parameters.Add("CITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cp.stock;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("PURFTRN", OracleDbType.NVarchar2).Value = "";
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
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

        public async Task<IEnumerable<Subcondcdet>> GetSubcondc(string id )
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<Subcondcdet>(" SELECT  TAAIERP.SUBCONTDCBASIC.SUBCONTDCBASICID, TAAIERP.SUBCONTDCBASIC.BRANCH, to_char(SUBCONTDCBASIC.DOCDATE,'dd-MM-yy')DOCDATE, TAAIERP.SUBCONTDCBASIC.DOCID, TAAIERP.SUBCONTDCBASIC.REFNO, TAAIERP.SUBCONTDCBASIC.WCID, TAAIERP.SUBCONTDCBASIC.FROMLOCATION, TAAIERP.SUBCONTDCBASIC.ENTEREDBY, TAAIERP.SUBCONTDCBASIC.NARRATION, TAAIERP.SUBCONTDCBASIC.LOCID, TAAIERP.SUBCONTDCBASIC.TOTQTY, TAAIERP.SUBCONTDCBASIC.TOTRECQTY, TAAIERP.SUBCONTDCBASIC.EBY, TAAIERP.SUBCONTDCBASIC.USERID, PARTYMAST.PARTYID,  TAAIERP.SUBCONTDCBASIC.PARTYMASTID, TAAIERP.SUBCONTDCBASIC.TOLOCATION, TAAIERP.SUBCONTDCBASIC.LOCDETAILSID, SUBCONTDCBASIC.ADD1 ||','||SUBCONTDCBASIC.ADD2 ||','||SUBCONTDCBASIC.CITY ||'-'||PARTYMAST.PINCODE as ADDRESS, TAAIERP.SUBCONTDCBASIC.THROUGH, TAAIERP.SUBCONTDCDETAIL.SUBCONTDCDETAILID, TAAIERP.SUBCONTDCDETAIL.SUBCONTDCBASICID AS EXPR2, ITEMMASTER.ITEMID,ITEMMASTER.HSN, TAAIERP.SUBCONTDCDETAIL.UNIT, TAAIERP.SUBCONTDCDETAIL.QTY,SUBCONTDCDETAIL.NOOFBAGS,PARTYMAST.GSTNO, TAAIERP.SUBCONTDCDETAIL.RATE, TAAIERP.SUBCONTDCDETAIL.AMOUNT, TAAIERP.SUBCONTEDET.SUBCONTEDETID,TAAIERP.SUBCONTEDET.SUBCONTDCBASICID AS EXPR3, TAAIERP.SUBCONTEDET.RITEM, TAAIERP.SUBCONTEDET.RITEMMASTERID, TAAIERP.SUBCONTEDET.RUNIT, TAAIERP.SUBCONTEDET.ERATE, TAAIERP.SUBCONTEDET.EAMOUNT, TAAIERP.SUBCONTEDET.ERQTY, TAAIERP.SUBCONTEDET.APPRATE, TAAIERP.SUBCONTEDET.EFRATE, TAAIERP.SUBCONTEDET.RSUBQTY, TAAIERP.SUBCONTEDET.RECQTY FROM TAAIERP.SUBCONTDCBASIC  INNER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID =SUBCONTDCBASIC.PARTYID INNER JOIN TAAIERP.SUBCONTDCDETAIL ON TAAIERP.SUBCONTDCBASIC.SUBCONTDCBASICID = TAAIERP.SUBCONTDCDETAIL.SUBCONTDCBASICID  INNER JOIN TAAIERP.SUBCONTEDET ON TAAIERP.SUBCONTDCBASIC.SUBCONTDCBASICID = TAAIERP.SUBCONTEDET.SUBCONTDCBASICID INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID =SUBCONTDCDETAIL.ITEMID  where SUBCONTDCBASIC.SUBCONTDCBASICID='" + id + "' and SUBCONTEDET.SUBCONTDCBASICID ='" + id + "' and SUBCONTDCDETAIL.SUBCONTDCBASICID ='" + id + "'", commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<Subcondcdetet>> GetSubcondcdet(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<Subcondcdetet>(" SELECT  SUBCONTEDET.SUBCONTEDETID,TAAIERP.SUBCONTEDET.SUBCONTDCBASICID AS EXPR3, ITEMMASTER.ITEMID, TAAIERP.SUBCONTEDET.RITEMMASTERID, TAAIERP.SUBCONTEDET.RUNIT, TAAIERP.SUBCONTEDET.ERATE, TAAIERP.SUBCONTEDET.EAMOUNT, TAAIERP.SUBCONTEDET.APPRATE, TAAIERP.SUBCONTEDET.EFRATE, TAAIERP.SUBCONTEDET.RSUBQTY, TAAIERP.SUBCONTEDET.ERQTY,ITEMMASTER.HSN FROM TAAIERP.SUBCONTDCBASIC INNER JOIN TAAIERP.SUBCONTEDET ON TAAIERP.SUBCONTDCBASIC.SUBCONTDCBASICID = TAAIERP.SUBCONTEDET.SUBCONTDCBASICID  INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = TAAIERP.SUBCONTEDET.RITEM where SUBCONTEDET.SUBCONTDCBASICID ='" + id + "'", commandType: CommandType.Text);
            }
        }
        public DataTable GetPackMatViewDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,UNIT,CLSTOCK,QTY,PURFTRN,RATE,AMOUNT,RDELDETAILID from RDELDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = RDELDETAIL.CITEMID  WHERE RDELDETAIL.RDELBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
