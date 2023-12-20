using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class AssetTransferService : IAssetTransfer
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AssetTransferService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
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

        public DataTable BindBinID()
        {
            string SvSql = string.Empty;
            SvSql = "Select BINBASICID,BINID from BINBASIC";
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ASITEMTRANLOC SET IS_ACTIVE ='N' WHERE ASITEMTRANLOCID='" + id + "'";
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
        public DataTable GetAllListAssetTransferItemItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ASITEMTRANLOCID,BRANCHMAST.BRANCHID,DOCID,to_char(ASITEMTRANLOC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID FROM ASITEMTRANLOC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ASITEMTRANLOC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=ASITEMTRANLOC.FROMLOC WHERE ASITEMTRANLOC.IS_ACTIVE='Y' ORDER BY ASITEMTRANLOC.ASITEMTRANLOCID DESC";
            }
            else
            {
                SvSql = "SELECT ASITEMTRANLOCID,BRANCHMAST.BRANCHID,DOCID,to_char(ASITEMTRANLOC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID FROM ASITEMTRANLOC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ASITEMTRANLOC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=ASITEMTRANLOC.FROMLOC WHERE ASITEMTRANLOC.IS_ACTIVE='N' ORDER BY ASITEMTRANLOC.ASITEMTRANLOCID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string AssetTransferCRUD(AssetTransfer ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Tfr-' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "Tfr-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Tfr-' AND ACTIVESEQUENCE ='T'  ";
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
                    OracleCommand objCmd = new OracleCommand("ASITEMTRANLOCPROC", objConn);
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("FROMLOC", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DESTLOC", OracleDbType.NVarchar2).Value = ss.ToLocation;
                    objCmd.Parameters.Add("FBINYN", OracleDbType.NVarchar2).Value = ss.BinId;
                    objCmd.Parameters.Add("TBINYN", OracleDbType.NVarchar2).Value = ss.ToBinId;
                    objCmd.Parameters.Add("REASONCODE", OracleDbType.NVarchar2).Value = ss.Reason;
                    objCmd.Parameters.Add("BROWSEORDER", OracleDbType.NVarchar2).Value = ss.Order;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = ss.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = ss.Net;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narration;
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
                        foreach (AssetTransferItem cp in ss.Assetlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {

                                OracleCommand objCmds = new OracleCommand("ASITEMTRANDETPROC", objConn);
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
                                objCmds.Parameters.Add("ASITEMTRANLOCID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                objCmds.Parameters.Add("CLSTK", OracleDbType.NVarchar2).Value = cp.Current;
                                objCmds.Parameters.Add("FCOSTRATE", OracleDbType.NVarchar2).Value = cp.rate;
                                objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objCmds.ExecuteNonQuery();
                                Object SPid = objCmds.Parameters["OUTID"].Value;

                                DataTable dt = datatrans.GetData("Select ASSTOCKVALUEID,PLUSORMINUS,ITEMID,LOCID,DOCDATE,QTY,STOCKVALUE,DOCTIME,MASTERID from ASSTOCKVALUE where ASSTOCKVALUE.ITEMID ='" + cp.ItemId + "' AND ASSTOCKVALUE.LOCID='" + ss.Location + "'");
                                if (dt.Rows.Count > 0)
                                {


                                    svSQL = "Insert into ASSTOCKVALUE (ITEMID,LOCID,QTY,STOCKVALUE,PLUSORMINUS,DOCDATE,DOCTIME,MASTERID,T1SOURCEID,BINID,PROCESSID,FROMLOCID,STOCKTRANSTYPE) VALUES ('" + cp.ItemId + "','" + ss.ToLocation + "','" + cp.Quantity + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','p','" + ss.Docdate + "','10:00:00 PM','" + dt.Rows[0]["MASTERID"].ToString() + "','" + SPid + "','0','0','"+ ss.Location +"','Asset Transfer') RETURNING ASSTOCKVALUEID INTO :LASTCID";

                                    objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmds.ExecuteNonQuery();
                                    string detailid = objCmds.Parameters["LASTCID"].Value.ToString();

                                    svSQL = "Insert into ASSTOCKVALUE2 (ASSTOCKVALUEID,RATE,DOCID,MDCTRL,NARRATION,ALLOWDELETE,ISSCTRL,RECCTRL) VALUES ('" + detailid + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + ss.DocId + "','T','" + ss.Narration + "','T','T','F')";

                                    objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();

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

        public DataTable GetAssetTransfer(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ASITEMTRANLOCID,BRANCHMAST.BRANCHID,DOCID,to_char(ASITEMTRANLOC.DOCDATE,'dd-MON-yyyy')DOCDATE,BINBASIC.BINID,Bin.BINID AS Bin,LOCDETAILS.LOCID,Loc.LOCID AS Loc,REASONCODE,BROWSEORDER,GROSS,NET,NARRATION FROM ASITEMTRANLOC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ASITEMTRANLOC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=ASITEMTRANLOC.FROMLOC LEFT OUTER JOIN LOCDETAILS Loc ON Loc.LOCDETAILSID=ASITEMTRANLOC.DESTLOC LEFT OUTER JOIN BINBASIC ON BINBASIC.BINBASICID=ASITEMTRANLOC.FBINYN LEFT OUTER JOIN BINBASIC Bin ON Bin.BINBASICID=ASITEMTRANLOC.TBINYN  Where ASITEMTRANLOC.ASITEMTRANLOCID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAssetTransferItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ASITEMTRANDETID,ITEMMASTER.ITEMID,UNIT,CLSTK,QTY,FCOSTRATE,AMOUNT FROM ASITEMTRANDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=ASITEMTRANDET.ITEMID  Where ASITEMTRANLOCID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
