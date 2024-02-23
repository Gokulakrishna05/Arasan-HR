using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PackingNoteService : IPackingNote
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PackingNoteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<PackingNote> GetAllPackingNote(string st, string ed)
        {
            List<PackingNote> cmpList = new List<PackingNote>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                if (st != null && ed != null)
                {

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select   BRANCHMAST.BRANCHID,DOCID,LOCDETAILS.LOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASICID  from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID WHERE PACKNOTEBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "' ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            PackingNote cmp = new PackingNote
                            {
                                ID = rdr["PACKNOTEBASICID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                ItemId = rdr["ITEMID"].ToString(),
                                DrumLoc = rdr["LOCID"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                            };
                            cmpList.Add(cmp);
                        }
                    }

                }
                else
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select   BRANCHMAST.BRANCHID,DOCID,LOCDETAILS.LOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASICID  from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID WHERE PACKNOTEBASIC.DOCDATE > sysdate-30 ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            PackingNote cmp = new PackingNote
                            {
                                ID = rdr["PACKNOTEBASICID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                ItemId = rdr["ITEMID"].ToString(),
                                DrumLoc = rdr["LOCID"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                            };
                            cmpList.Add(cmp);
                        }
                    }
                }

            }
            return cmpList;
        }
        public DataTable DrumDeatils(string id, string loc)
        {
            DataTable _Dt = new DataTable();
            return _Dt;
        }
        //public DataTable GetData(string sql)
        //{
        //    DataTable _Dt = new DataTable();
        //    try
        //    {
        //        OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
        //        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //        adapter.Fill(_Dt);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return _Dt;
        //}
        //public int GetDataId(String sql)
        //{
        //    DataTable _dt = new DataTable();
        //    int Id = 0;
        //    try
        //    {
        //        _dt = GetData(sql);
        //        if (_dt.Rows.Count > 0)
        //        {
        //            Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Id;
        //}

        //public bool UpdateStatus(string query)
        //{
        //    bool Saved = true;
        //    try
        //    {
        //        OracleConnection objConn = new OracleConnection(_connectionString);
        //        OracleCommand objCmd = new OracleCommand(query, objConn);
        //        objCmd.Connection.Open();
        //        objCmd.ExecuteNonQuery();
        //        objCmd.Connection.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        Saved = false;
        //    }
        //    return Saved;
        //}
        public string PackingNoteCRUD(PackingNote cy)
        {

            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PacN' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "PacN", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PacN' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string[] sdateList = cy.startdate.Split(" - ");
                string sdate = "";
                string stime = "";
                if (sdateList.Length > 0)
                {
                    sdate = sdateList[0];
                    stime = sdateList[1];
                }
                string[] edateList = cy.enddate.Split(" - ");
                string endate = "";
                string endtime = "";
                if (sdateList.Length > 0)
                {
                    endate = edateList[0];
                    endtime = edateList[1];
                }

                string Item = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cy.ItemId + "' ");
                //string loc = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + cy.WorkId + "' ");

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {

                    OracleCommand objCmd = new OracleCommand("PACKINGNOTEPROC", objConn);
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
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("TOLOCDETAILSID", OracleDbType.NVarchar2).Value = cy.WorkId;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkId;
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("DRUMLOCATION", OracleDbType.NVarchar2).Value = cy.DrumLoc;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = docid;
                    objCmd.Parameters.Add("PACKCONSYN", OracleDbType.NVarchar2).Value = cy.PackYN;
                    objCmd.Parameters.Add("STARTDATE", OracleDbType.NVarchar2).Value = sdate;
                    objCmd.Parameters.Add("ENDDATE", OracleDbType.NVarchar2).Value = endate;
                    objCmd.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = stime;
                    objCmd.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = endtime;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("PACLOTNO", OracleDbType.NVarchar2).Value = cy.LotNo;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remark;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.ProdSchNo;
                    objCmd.Parameters.Add("OITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
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
                        foreach (DrumDetail cp in cy.DrumDetlst)
                        {
                            if (cp.Isvalid == "Y" && cp.drum != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PACKNOTEDETPROC", objConns);
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
                                    objCmds.Parameters.Add("PACKNOTEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("IDRUMNO", OracleDbType.NVarchar2).Value = cp.drumid;
                                    objCmds.Parameters.Add("IBATCHNO", OracleDbType.NVarchar2).Value = cp.batch;
                                    objCmds.Parameters.Add("IBATCHQTY", OracleDbType.NVarchar2).Value = cp.qty;
                                    objCmds.Parameters.Add("COMBNO", OracleDbType.NVarchar2).Value = cp.comp;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objCmds.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    Object Pid1 = objCmds.Parameters["OUTID"].Value;
                                    objConns.Close();

                                    //DataTable dt = datatrans.GetData("Select ITEMID,DOC_DATE,DRUM_ID,DRUM_NO,TSOURCEID,STOCKTRANSTYPE,LOCID,QTY,BALANCE_QTY,OUT_ID,DRUM_STOCK_ID from DRUM_STOCK where BALANCE_QTY>0 AND DRUM_STOCK.DRUM_NO='" + cp.drum + "' and ITEMID='" + cy.ItemId + "' and LOCID='" + cy.DrumLoc + "'");

                                    ////string DrumID = datatrans.GetDataString("Select DRUM_ID from DRUM_STOCK where DRUM_NO='" + cp.drum + "' ");

                                    ////double qty = ca.Qty;

                                    ////double rqty = Convert.ToDouble(dt.Rows[0]["BALANCE_QTY"].ToString());
                                    //if (dt.Rows.Count > 0)
                                    //{
                                    //    for (int i = 0; i < dt.Rows.Count; i++)
                                    //    {
                                    //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                    //        {
                                    //            string Sql = string.Empty;
                                    //            Sql = "UPDATE DRUM_STOCK SET BALANCE_QTY ='0' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() + "'";
                                    //            OracleCommand objCmdsD = new OracleCommand(Sql, objConnT);
                                    //            objConnT.Open();
                                    //            objCmdsD.ExecuteNonQuery();
                                    //            objConnT.Close();
                                    //        }
                                    //        string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='" + cy.WorkId + "'");
                                    //        using (OracleConnection objConnsD = new OracleConnection(_connectionString))

                                    //        {

                                    //            OracleCommand objCmdsT = new OracleCommand("DRUMSTKPROC", objConnsD);
                                    //            objCmdsT.CommandType = CommandType.StoredProcedure;

                                    //            objCmdsT.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    //            objCmdsT.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
                                    //            objCmdsT.Parameters.Add("DOC_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    //            objCmdsT.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.drumid;
                                    //            objCmdsT.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = cp.drum;
                                    //            objCmdsT.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = Pid1;
                                    //            objCmdsT.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "PACKNOTE";
                                    //            objCmdsT.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WorkId;
                                    //            objCmdsT.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                    //            objCmdsT.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.qty;
                                    //            objCmdsT.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = cp.qty;
                                    //            objCmdsT.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["DRUM_STOCK_ID"].ToString();
                                    //            objCmdsT.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    //            objConnsD.Open();


                                    //            objCmdsT.ExecuteNonQuery();
                                    //            Object stid = objCmdsT.Parameters["OUTID"].Value;

                                    //            OracleCommand objCmdInp = new OracleCommand("DRUMSTKDETPROC", objConn);
                                    //            objCmdInp.CommandType = CommandType.StoredProcedure;
                                    //            objCmdInp.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    //            objCmdInp.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = stid;
                                    //            objCmdInp.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
                                    //            objCmdInp.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                    //            objCmdInp.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cp.drumid;
                                    //            objCmdInp.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = Pid1;
                                    //            objCmdInp.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "PACKNOTE";
                                    //            objCmdInp.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WorkId;
                                    //            objCmdInp.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;

                                    //            objCmdInp.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = cp.qty;
                                    //            objCmdInp.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = "0";
                                    //            objCmdInp.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    //            objCmdInp.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = cp.batch;
                                    //            objCmdInp.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "0";

                                    //            objCmdInp.ExecuteNonQuery();
                                    //            string Sql = string.Empty;
                                    //            Sql = "Update DRUMMAST SET  DRUMLOC='" + cy.WorkId + "',IS_EMPTY='N' WHERE DRUMNO='" + cp.drum + "'";
                                    //            OracleCommand objCmds1 = new OracleCommand(Sql, objConnsD);
                                    //            objCmds1.ExecuteNonQuery();


                                    //            objConnsD.Close();

                                    //        }
                                    //    }
                                    //}
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
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSchedule()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,PSBASICID from PSBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ShiftDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetDrumLocation()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select LOCDETAILS.LOCID,NPRODBASICID,TOLOCATION,NPRODOUTDETID from NPRODOUTDET left outer join LOCDETAILS on LOCDETAILSID = NPRODOUTDET.TOLOCATION ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetDrumLocation()
        {
            string SvSql = string.Empty;
            SvSql = "select  LOCID,LOCDETAILSID from LOCDETAILS  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItembyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID as item from DRUM_STOCK left outer join ITEMMASTER on ITEMMASTERID =DRUM_STOCK.ITEMID  where BALANCE_QTY >0 AND LOCID= '" + id + "' and CURINGDUEDATE  <= trunc(sysdate)  GROUP BY ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID as item from DRUM_STOCK left outer join ITEMMASTER on ITEMMASTERID =DRUM_STOCK.ITEMID  where BALANCE_QTY >0 AND LOCID= '" + id + "' GROUP BY ITEMMASTER.ITEMID,DRUM_STOCK.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string id, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUM_STOCK.DRUM_NO,DRUM_STOCK.DRUM_ID,BALANCE_QTY  from DRUM_STOCK where ITEMID= '" + id + "' AND LOCID ='" + item + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumLot(string id, string item, string drum)
        {
            string SvSql = string.Empty;
            SvSql = "select LOTNO   from DRUM_STOCKDET where ITEMID= '" + id + "' AND LOCID ='" + item + "' AND DRUMNO='" + drum + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetDrumDetails(string id, string item)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select QTY from DRUM_STOCK where DRUM_ID= '" + id + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetBatch(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  ODRUMNO,NBATCHNO, NPRODBASICID from NPRODOUTDET    where ODRUMNO= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackingNote(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  BRANCH, TOLOCDETAILSID,SHIFT,to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DRUMLOCATION,DOCID,PACKCONSYN,ENTEREDBY,PACLOTNO,REMARKS,PSCHNO,OITEMID,to_char(PACKNOTEBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,STARTTIME,to_char(PACKNOTEBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,ENDTIME from PACKNOTEBASIC    where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetItemDet(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select  LOCID,ITEMMASTER.ITEMID NPRODBASICID from NPRODOUTDET left outer join ITEMMASTER on ITEMMASTERID =NPRODOUTDET.OITEMID   where ODRUMNO= '" + id + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetDrumItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  PACKNOTEBASICID,IDRUMNO,IBATCHNO,IBATCHQTY,COMBNO from PACKNOTEINPDETAIL    where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditNote(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  BRANCHMAST.BRANCHID, tol.LOCID loc,SHIFTMAST.SHIFTNO,to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, LOCDETAILS.LOCID,PACKNOTEBASIC.DOCID,PACKNOTEBASIC.PACKCONSYN,PACKNOTEBASIC.ENTEREDBY,PACLOTNO,PACKNOTEBASIC.REMARKS,PSBASIC.DOCID PS,ITEMMASTER.ITEMID,to_char(PACKNOTEBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,STARTTIME,to_char(PACKNOTEBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,ENDTIME from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN SHIFTMAST ON SHIFTMAST.SHIFTMASTID=PACKNOTEBASIC.SHIFT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=PACKNOTEBASIC.PSCHNO left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION LEFT OUTER JOIN LOCDETAILS tol ON tol.LOCDETAILSID=PACKNOTEBASIC.TOLOCDETAILSID  where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditDrumDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  PACKNOTEBASICID,DRUMMAST.DRUMNO,IBATCHNO,IBATCHQTY,COMBNO from PACKNOTEINPDETAIL left outer join DRUMMAST ON DRUMMAST.DRUMMASTID=PACKNOTEINPDETAIL.IDRUMNO    where PACKNOTEBASICID= '" + id + "'";
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
                    svSQL = "UPDATE PACKNOTEBASIC SET STATUS ='ISACTIVE' WHERE PACKNOTEBASICID='" + id + "'";
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

        public DataTable GetAllPackingDeatils(string strStatus, string strfrom, string strTo)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select   BRANCHMAST.BRANCHID,DOCID,LOCDETAILS.LOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASICID  from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID WHERE PACKNOTEBASIC.IS_ACTIVE='Y' ORDER BY  PACKNOTEBASICID DESC";
            }
            else
            {
                SvSql = "Select   BRANCHMAST.BRANCHID,DOCID,LOCDETAILS.LOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASICID  from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID WHERE PACKNOTEBASIC.IS_ACTIVE='N' ORDER BY  PACKNOTEBASICID DESC";

            }
            if (strfrom == null && strTo == null)
            {
                SvSql = "Select   BRANCHMAST.BRANCHID,DOCID,to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASICID  from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID\r\n";
            }
            else
            {
                SvSql = "Select   BRANCHMAST.BRANCHID,PACKNOTEBASIC.DOCID,to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASICID  from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID\r\n";
                if (strfrom != null && strTo != null)
                {
                    SvSql += " and PACKNOTEBASIC.DOCDATE BETWEEN '" + strfrom + "' AND '" + strTo + "'";
                }
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }

}
