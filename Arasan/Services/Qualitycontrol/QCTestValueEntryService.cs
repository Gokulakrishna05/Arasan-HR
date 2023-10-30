using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PdfSharp.Charting;

namespace Arasan.Services.Qualitycontrol
{
    public class QCTestValueEntryService : IQCTestValueEntryService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public QCTestValueEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<QCTestValueEntry> GetAllQCTestValueEntry(string st, string ed)
        {
            List<QCTestValueEntry> cmpList = new List<QCTestValueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                if (st != null && ed != null)
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select BRANCH,DOCID,to_char(QTVEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,SHIFTNO,PROCESSLOTNO,DRUMNO,PRODDATE,DSAMPLE,DSAMPLETIME,ITEMID,ENTEREDBY,QTVEBASIC.REMARKS,QTVEBASICID from QTVEBASIC left outer join WCBASIC on WCBASIC.WCBASICID=QTVEBASIC.WCID WHERE QTVEBASIC.DOCDATE BETWEEN ' " + st + "'  AND ' " + ed + "' order by QTVEBASICID desc ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            QCTestValueEntry cmp = new QCTestValueEntry
                            {
                                ID = rdr["QTVEBASICID"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                                Docdate = rdr["DOCDATE"].ToString(),
                                Work = rdr["WCID"].ToString(),
                                Shift = rdr["SHIFTNO"].ToString(),
                                Process = rdr["PROCESSLOTNO"].ToString(),
                                Prodate = rdr["PRODDATE"].ToString(),
                                Sample = rdr["DSAMPLE"].ToString(),
                                Sampletime = rdr["DSAMPLETIME"].ToString(),
                                Item = rdr["ITEMID"].ToString(),
                                Entered = rdr["ENTEREDBY"].ToString(),
                                Remarks = rdr["REMARKS"].ToString(),
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
                        cmd.CommandText = "Select BRANCH,DOCID,to_char(QTVEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,SHIFTNO,PROCESSLOTNO,DRUMNO,PRODDATE,DSAMPLE,DSAMPLETIME,ITEMID,ENTEREDBY,QTVEBASIC.REMARKS,QTVEBASICID from QTVEBASIC left outer join WCBASIC on WCBASIC.WCBASICID=QTVEBASIC.WCID WHERE QTVEBASIC.DOCDATE > sysdate-30 order by QTVEBASICID desc ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            QCTestValueEntry cmp = new QCTestValueEntry
                            {
                                ID = rdr["QTVEBASICID"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                                Docdate = rdr["DOCDATE"].ToString(),
                                Work = rdr["WCID"].ToString(),
                                Shift = rdr["SHIFTNO"].ToString(),
                                Process = rdr["PROCESSLOTNO"].ToString(),
                                Prodate = rdr["PRODDATE"].ToString(),
                                Sample = rdr["DSAMPLE"].ToString(),
                                Sampletime = rdr["DSAMPLETIME"].ToString(),
                                Item = rdr["ITEMID"].ToString(),
                                Entered = rdr["ENTEREDBY"].ToString(),
                                Remarks = rdr["REMARKS"].ToString(),
                            };

                            cmpList.Add(cmp);
                        }
                    }
                }
            }
            return cmpList;
        }
        public string QCTestValueEntryCRUD(QCTestValueEntry cy)
        {
            string msg = "";

            try
            {
                if (cy.ID != null)
                {
                    cy.ID = null;
                }
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'QTV2' AND ACTIVESEQUENCE = 'T' AND TRANSTYPE='QTVE'");
                    string Doc = string.Format("{0}{1}", "QTV2", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='QTV2' AND ACTIVESEQUENCE ='T' AND TRANSTYPE='QTVE'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocId = Doc;
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("QTVEBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "STATEPROC";*/

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Work;
                    objCmd.Parameters.Add("SHIFTNO", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("PROCESSLOTNO", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("CDRUMNO", OracleDbType.NVarchar2).Value = cy.Drum;
                    objCmd.Parameters.Add("PRODDATE", OracleDbType.NVarchar2).Value = cy.Prodate;
                    objCmd.Parameters.Add("SAMPLENO", OracleDbType.NVarchar2).Value = cy.Sample;
                    objCmd.Parameters.Add("STIME", OracleDbType.NVarchar2).Value = cy.Sampletime;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("RATEPHR", OracleDbType.NVarchar2).Value = cy.Rate;
                    objCmd.Parameters.Add("NOZZLENO", OracleDbType.NVarchar2).Value = cy.Nozzle;
                    objCmd.Parameters.Add("AIRPRESS", OracleDbType.NVarchar2).Value = cy.Air;
                    objCmd.Parameters.Add("ADDCH", OracleDbType.NVarchar2).Value = cy.AddCharge;
                    objCmd.Parameters.Add("BCT", OracleDbType.NVarchar2).Value = cy.Ctemp;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
                    objCmd.Parameters.Add("APPROID", OracleDbType.NVarchar2).Value = cy.APID;
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
                        if (cy.QCTestLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (QCTestValueEntryItem cp in cy.QCTestLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.description != "0")
                                    {

                                        svSQL = "Insert into QTVEDETAIL (QTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT) VALUES ('" + Pid + "','" + cp.description + "','" + cp.value + "','" + cp.unit + "','" + cp.startvalue + "','" + cp.endvalue + "','" + cp.test + "','" + cp.manual + "','" + cp.actual + "','" + cp.testresult + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                        svSQL = "Update APPRODOUTDET SET TESTRESULT='" + cp.testresult + "',MOVETOQC='Moved' WHERE APPRODUCTIONBASICID='" + cp.apid + "'";
                                        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                        objCmdd.ExecuteNonQuery();

                                    }

                                }

                            }
                            else
                            {
                                svSQL = "Delete QTVEDETAIL WHERE QTVEBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (QCTestValueEntryItem cp in cy.QCTestLst)
                                {

                                    if (cp.Isvalid == "Y" && cp.description != "0")
                                    {

                                        svSQL = "Insert into QTVEDETAIL (QTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT) VALUES ('" + Pid + "','" + cp.description + "','" + cp.value + "','" + cp.unit + "','" + cp.startvalue + "','" + cp.endvalue + "','" + cp.test + "','" + cp.manual + "','" + cp.actual + "','" + cp.testresult + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


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

        public DataTable GetQCTestValueEntryDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCH,DOCID,to_char(QTVEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCID,SHIFTNO,PROCESSLOTNO,CDRUMNO,to_char(QTVEBASIC.DOCDATE,'dd-MON-yyyy')PRODDATE,SAMPLENO,STIME,ITEMID,RATEPHR,NOZZLENO,AIRPRESS,ADDCH,BCT,ENTEREDBY,REMARKS,QTVEBASICID from QTVEBASIC WHERE QTVEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCTestDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select QTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT from QTVEDETAIL WHERE QTVEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAPOutDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select WCID,SHIFT,to_char(APPRODUCTIONBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE from APPRODUCTIONBASIC WHERE APPRODUCTIONBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAPOutItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,APPRODOUTDET.ITEMID as item,DRUMMAST.DRUMNO,FROMTIME from APPRODOUTDET  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=APPRODOUTDET.ITEMID  LEFT OUTER JOIN DRUMMAST ON DRUMMASTID=APPRODOUTDET.DRUMNO WHERE APPRODUCTIONBASICID='" + id + "' ";
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
                    svSQL = "UPDATE QTVEBASIC SET ISACTIVE ='N' WHERE QTVEBASICID='" + id + "'";
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
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER  ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TESTDESC,UNITMAST.UNITID,VALUEORMANUAL,STARTVALUE,ENDVALUE from TESTTDETAIL left outer join UNITMAST ON UNITMASTID =TESTTDETAIL.UNIT   WHERE TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetViewQCTestValueEntry(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCH,DOCID,to_char(QTVEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCID,SHIFTNO,PROCESSLOTNO,CDRUMNO,to_char(QTVEBASIC.DOCDATE,'dd-MON-yyyy')PRODDATE,SAMPLENO,STIME,ITEMID,RATEPHR,NOZZLENO,AIRPRESS,ADDCH,BCT,ENTEREDBY,REMARKS,QTVEBASICID from QTVEBASIC WHERE QTVEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetViewQCTestDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select QTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT from QTVEDETAIL WHERE QTVEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAPout1(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT APPROID, COUNT(*) as Ap FROM QTVEBASIC WHERE APPROID ='" + id + "' GROUP BY APPROID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAPout(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select APPRODUCTIONBASICID,ITEMMASTER.ITEMID,DRUMMAST.DRUMNO,FROMTIME,OUTQTY from APPRODOUTDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=APPRODOUTDET.ITEMID LEFT OUTER JOIN DRUMMAST ON DRUMMASTID=APPRODOUTDET.DRUMNO AND TESTRESULT is null where APPRODUCTIONBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDis(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select APPROID from FQTVEBASIC where APPROID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetResultItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CDRUMNO,STIME,ITEMID,QTVEBASICID from QTVEBASIC WHERE APPROID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetResultItemDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TESTRESULT from QTVEDETAIL WHERE QTVEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
