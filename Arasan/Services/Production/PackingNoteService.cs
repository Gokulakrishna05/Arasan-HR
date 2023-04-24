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
        }
        public IEnumerable<PackingNote> GetAllPackingNote()
        {
            List<PackingNote> cmpList = new List<PackingNote>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select   BRANCHMAST.BRANCHID,DOCID,LOCDETAILS.LOCID,WCBASIC.WCID,PACKNOTEBASICID from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKNOTEBASIC.BRANCH  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION  LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PackingNote cmp = new PackingNote
                        {
                            ID = rdr["PACKNOTEBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            WorkId = rdr["WCID"].ToString(),
                            DrumLoc = rdr["LOCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                        };
                        cmpList.Add(cmp);
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
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkId;
            
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
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
        public DataTable GetDrumLocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCDETAILS.LOCID,NPRODBASICID,TOLOCATION,NPRODOUTDETID from NPRODOUTDET left outer join LOCDETAILS on LOCDETAILSID = NPRODOUTDET.TOLOCATION ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItembyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,OITEMID,NPRODBASICID from NPRODOUTDET left outer join ITEMMASTER on ITEMMASTERID =NPRODOUTDET.OITEMID  where TOLOCATION= '" + id +"'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumNo(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMMAST.DRUMNO,ODRUMNO,NPRODBASICID from NPRODOUTDET left outer join DRUMMAST on DRUMMASTID=NPRODOUTDET.ODRUMNO  where TOLOCATION= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select   NBATCHNO,OQTY,NPRODBASICID from NPRODOUTDET   where NBATCHNO= '" + id + "'";
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
            SvSql = "select  BRANCH, WCID,SHIFT,to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DRUMLOCATION,DOCID,PACKCONSYN,ENTEREDBY,PACLOTNO,REMARKS,PSCHNO,OITEMID,to_char(PACKNOTEBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,STARTTIME,to_char(PACKNOTEBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,ENDTIME from PACKNOTEBASIC    where PACKNOTEBASICID= '" + id + "'";
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
            SvSql = "select  BRANCHMAST.BRANCHID, WCBASIC.WCID,SHIFTMAST.SHIFTNO,to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, DRUMLOCATION,PACKNOTEBASIC.DOCID,PACKNOTEBASIC.PACKCONSYN,PACKNOTEBASIC.ENTEREDBY,PACLOTNO,PACKNOTEBASIC.REMARKS,PSBASIC.DOCID PS,ITEMMASTER.ITEMID,to_char(PACKNOTEBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,STARTTIME,to_char(PACKNOTEBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,ENDTIME from PACKNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=PACKNOTEBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN SHIFTMAST ON SHIFTMAST.SHIFTMASTID=PACKNOTEBASIC.SHIFT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKNOTEBASIC.DRUMLOCATION LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=PACKNOTEBASIC.PSCHNO where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditDrumDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  PACKNOTEBASICID,IDRUMNO,IBATCHNO,IBATCHQTY,COMBNO from PACKNOTEINPDETAIL    where PACKNOTEBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }

}
