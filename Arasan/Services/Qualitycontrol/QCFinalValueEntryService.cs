using Arasan.Interface.Qualitycontrol;
using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;
namespace Arasan.Services.Qualitycontrol
{
    public class QCFinalValueEntryService : IQCFinalValueEntryService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public QCFinalValueEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
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
        public DataTable GetProcess()
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID ,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable DrumDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMMAST.DRUMNO,NPRODBASICID,ODRUMNO,NPRODOUTDETID from NPRODOUTDET LEFT OUTER JOIN DRUMMAST  on DRUMMASTID=NPRODOUTDET.ODRUMNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,OITEMID,NPRODBASICID from NPRODOUTDET LEFT OUTER JOIN ITEMMASTER  on ITEMMASTERID=NPRODOUTDET.OITEMID Where ODRUMNO ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable BatchDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ODRUMNO,NBATCHNO,NPRODBASICID from NPRODOUTDET where ODRUMNO= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQC(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,TYPE,to_char(QCNOTIFICATION.CREATED_ON,'dd-MON-yyyy')CREATED_ON,ID from QCNOTIFICATION where QCNOTIFICATIONID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select WCID,PROCESSID,to_char(NPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE from NPRODBASIC where NPRODBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCOutDeatil(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,ODRUMNO,OBATCHNO from NPRODOUTDET LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=NPRODOUTDET.OITEMID where NPRODBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string QCFinalValueEntryCRUD(QCFinalValueEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
             

                if(cy.ID !=null)
                {
                   cy.ID = null;
                }
                //string[] sdateList = cy.startdate.Split(" - ");
                //string sdate = "";
                //string stime = "";
                //if (sdateList.Length > 0)
                //{
                //    sdate = sdateList[0];
                //    stime = sdateList[1];
                //}
                //string[] edateList = cy.enddate.Split(" - ");
                //string endate = "";
                //string endtime = "";
                //if (sdateList.Length > 0)
                //{
                //    endate = edateList[0];
                //    endtime = edateList[1];
                //}
                //string StatementType = string.Empty; string svSQL = "";
                string ITEMID = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cy.Itemid +"' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("FQTVEBASICPROC", objConn);
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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cy.DrumNo;
                    objCmd.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = cy.Batch;
                    objCmd.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cy.BatchNo;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ITEMID;
                    objCmd.Parameters.Add("PRODID", OracleDbType.NVarchar2).Value = cy.ProNo;
                    objCmd.Parameters.Add("RATEPHR", OracleDbType.NVarchar2).Value = cy.Rate;
                    objCmd.Parameters.Add("PRODDATE", OracleDbType.Date).Value = DateTime.Parse(cy.ProDate);
                    objCmd.Parameters.Add("SAMPLENO", OracleDbType.NVarchar2).Value = cy.SampleNo;
                    objCmd.Parameters.Add("NOZZLENO", OracleDbType.NVarchar2).Value = cy.NozzleNo;
                    objCmd.Parameters.Add("AIRPRESS", OracleDbType.NVarchar2).Value = cy.AirPress;
                    objCmd.Parameters.Add("ADDCH", OracleDbType.NVarchar2).Value = cy.Additive;
                    objCmd.Parameters.Add("STIME", OracleDbType.NVarchar2).Value = cy.Stime;
                    objCmd.Parameters.Add("BCT", OracleDbType.NVarchar2).Value = cy.CTemp;
                    objCmd.Parameters.Add("FINALRESULT", OracleDbType.NVarchar2).Value = cy.FResult;
                    objCmd.Parameters.Add("RESULTTYPE", OracleDbType.NVarchar2).Value = cy.RType;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Reamarks;
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
                        foreach (QCFVItem ca in cy.QCFVLst)
                        {
                            if (ca.Isvalid == "Y" && ca.Des != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("FQTVEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("FQTVEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("TDESC", OracleDbType.NVarchar2).Value = ca.Des;
                                    objCmds.Parameters.Add("VALUEORMANUAL", OracleDbType.NVarchar2).Value = ca.Value;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = ca.Unit;
                                    objCmds.Parameters.Add("STARTVALUE", OracleDbType.NVarchar2).Value = ca.Sta;
                                    objCmds.Parameters.Add("ENDVALUE", OracleDbType.NVarchar2).Value = ca.En;
                                    objCmds.Parameters.Add("TESTVALUE", OracleDbType.NVarchar2).Value = ca.Test;
                                    objCmds.Parameters.Add("MANUALVALUE", OracleDbType.NVarchar2).Value = ca.Manual;
                                    objCmds.Parameters.Add("ACTTESTVALUE", OracleDbType.NVarchar2).Value = ca.Actual;
                                    objCmds.Parameters.Add("TESTRESULT", OracleDbType.NVarchar2).Value = ca.Result;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                            }
                        }
                        foreach (QCFVItemDeatils cp in cy.QCFVDLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Vol != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("FQTVEGEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("FQTVEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("MINS", OracleDbType.NVarchar2).Value = cp.Time;
                                    objCmds.Parameters.Add("VOL25C", OracleDbType.NVarchar2).Value =cp.Vol;
                                    objCmds.Parameters.Add("VOL35C", OracleDbType.NVarchar2).Value = cp.Volat;
                                    objCmds.Parameters.Add("VOL45C", OracleDbType.NVarchar2).Value = cp.Volc;
                                    objCmds.Parameters.Add("VOLSTP", OracleDbType.NVarchar2).Value = cp.Stp;
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

                string updateCMd = " UPDATE QCNOTIFICATION SET IS_COMPLETED ='Y' WHERE QCNOTIFICATIONID ='"+ cy.ID + "' ";
                 datatrans.UpdateStatus(updateCMd);
              
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public IEnumerable<QCFinalValueEntry> GetAllQCFinalValueEntry()
        {
            List<QCFinalValueEntry> cmpList = new List<QCFinalValueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select BRANCHMAST.BRANCHID,PROCESSMAST.PROCESSID,WCBASIC.WCID,FQTVEBASICID FROM FQTVEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=FQTVEBASIC.BRANCH LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=FQTVEBASIC.PROCESSID LEFT OUTER JOIN WCBASIC ON WCBASICID=FQTVEBASIC.WCID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QCFinalValueEntry cmp = new QCFinalValueEntry
                        {

                            ID = rdr["FQTVEBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            WorkCenter = rdr["WCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

       

        public DataTable GetQCFVDeatil(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCH,DOCID,to_char(FQTVEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCID,PROCESSID,DRUMNO,BATCH,BATCHNO,ITEMID,PRODID,RATEPHR,to_char(FQTVEBASIC.PRODDATE,'dd-MON-yyyy')PRODDATE,SAMPLENO,NOZZLENO,AIRPRESS,ADDCH,STIME,BCT,FINALRESULT,RESULTTYPE,ENTEREDBY,REMARKS,FQTVEBASICID from FQTVEBASIC Where FQTVEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQCFVResultDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select FQTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT from FQTVEDETAIL where FQTVEBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQCFVGasDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select FQTVEBASICID,MINS,VOL25C,VOL35C,VOL45C,VOLSTP from FQTVEGEDETAIL where FQTVEBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
