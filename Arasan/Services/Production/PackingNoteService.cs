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

                //if (cy.ID == null)
                //{
                //    DateTime theDate = DateTime.Now;
                //    DateTime todate; DateTime fromdate;
                //    string t; string f;
                //    if (DateTime.Now.Month >= 4)
                //    {
                //        todate = theDate.AddYears(1);
                //    }
                //    else
                //    {
                //        todate = theDate;
                //    }
                //    if (DateTime.Now.Month >= 4)
                //    {
                //        fromdate = theDate;
                //    }
                //    else
                //    {
                //        fromdate = theDate.AddYears(-1);
                //    }
                //    t = todate.ToString("yy");
                //    f = fromdate.ToString("yy");
                //    string disp = string.Format("{0}-{1}", f, t);

                //    int idc = GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'PN' AND IS_ACTIVE = 'Y'");
                //    cy.DocId = string.Format("{0}/{3}/{1} - {2} ", "TAAI", "PacN", (idc + 1).ToString(), disp);

                //    string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='PN' AND IS_ACTIVE ='Y'";
                //    try
                //    {
                //        UpdateStatus(updateCMd);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}
              
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
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("DRUMLOCATION", OracleDbType.NVarchar2).Value = cy.DrumLoc;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("PACKCONSYN", OracleDbType.NVarchar2).Value = cy.PackYN;
                    objCmd.Parameters.Add("STARTDATE", OracleDbType.Date).Value = DateTime.Parse(sdate);
                    objCmd.Parameters.Add("ENDDATE", OracleDbType.Date).Value = DateTime.Parse(endate);
                    objCmd.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = stime;
                    objCmd.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = endtime;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("PACLOTNO", OracleDbType.NVarchar2).Value = cy.LotNo;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remark;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.ProdSchNo;
                    objCmd.Parameters.Add("OITEMID", OracleDbType.NVarchar2).Value = Item;
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
                            if (cp.Isvalid == "Y" && cp.DrumNo != "0")
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
                                    objCmds.Parameters.Add("IDRUMNO", OracleDbType.NVarchar2).Value = cp.DrumNo;
                                    objCmds.Parameters.Add("IBATCHNO", OracleDbType.NVarchar2).Value = cp.BatchNo;
                                    objCmds.Parameters.Add("IBATCHQTY", OracleDbType.NVarchar2).Value = cp.BatchQty;
                                    objCmds.Parameters.Add("COMBNO", OracleDbType.NVarchar2).Value = cp.Comp;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM_ID,ITEMID,IN_DATE,DRUM_ID,DRUM_NO,TSOURCEID,STOCKTRANSTYPE,LOCID,QTY,BALANCE_QTY,OUT_ID,BATCHNO,BATCH_QTY,ISPRODINV,DRUM_STOCK_ID from DRUM_STOCK where DRUM_STOCK.DRUM_ID='" + cp.DrumNo + "'");

                                string DrumID = datatrans.GetDataString("Select DRUM_NO from DRUM_STOCK where DRUM_ID='" + cp.DrumNo + "' ");

                                //double qty = ca.Qty;

                                //double rqty = Convert.ToDouble(dt.Rows[0]["BALANCE_QTY"].ToString());
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                        {
                                            string Sql = string.Empty;
                                            Sql = "UPDATE DRUM_STOCK SET BALANCE_QTY ='0' WHERE DRUM_STOCK_ID='" + dt.Rows[i]["DRUM_STOCK_ID"].ToString() + "'";
                                            OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                            objConnT.Open();
                                            objCmds.ExecuteNonQuery();
                                            objConnT.Close();
                                        }

                                        using (OracleConnection objConns = new OracleConnection(_connectionString))

                                        {

                                            OracleCommand objCmds = new OracleCommand("DRUMSTKPROC", objConns);
                                            objCmds.CommandType = CommandType.StoredProcedure;

                                            objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                            objCmds.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = "0";
                                            objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
                                            objCmds.Parameters.Add("IN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                            objCmds.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = cp.DrumNo;
                                            objCmds.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = DrumID;
                                            objCmds.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = "0";
                                            objCmds.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "PACKNOTE";
                                            objCmds.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.WorkId;
                                            objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.BatchQty;
                                            objCmds.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = cp.BatchQty;
                                            objCmds.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["DRUM_STOCK_ID"].ToString();
                                            objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cp.BatchNo;
                                            objCmds.Parameters.Add("BATCH_QTY", OracleDbType.NVarchar2).Value = "0";
                                            objCmds.Parameters.Add("ISPRODINV", OracleDbType.NVarchar2).Value = "Y";
                                            objConns.Open();
                                            objCmds.ExecuteNonQuery();

                                            objConns.Close();

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
        public DataTable GetSchedule( )
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
            SvSql = "select ITEMMASTER.ITEMID from DRUM_STOCK left outer join ITEMMASTER on ITEMMASTERID =DRUM_STOCK.ITEMID  where LOCID= '" + id + "' GROUP BY ITEMMASTER.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable DrumDeatils(string id, string item)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUM_NO,DRUM_ID  from DRUM_STOCK where BALANCE_QTY  > 0 AND ITEMID= '" + id + "' AND LOCID ='" + item + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select QTY from DRUM_STOCK where DRUM_ID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
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

    }

}
