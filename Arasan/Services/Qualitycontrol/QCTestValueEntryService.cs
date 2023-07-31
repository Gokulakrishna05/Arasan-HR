using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Qualitycontrol
{
    public class QCTestValueEntryService : IQCTestValueEntryService
    {
        private readonly string _connectionString;
        public QCTestValueEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<QCTestValueEntry> GetAllQCTestValueEntry()
        {
            List<QCTestValueEntry> cmpList = new List<QCTestValueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCH,DOCID,DOCDATE,WCID,SHIFTNO,PROCESSLOTNO,DRUMNO,PRODDATE,DSAMPLE,DSAMPLETIME,ITEMID,ENTEREDBY,REMARKS,QTVEBASICID from QTVEBASIC";
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
            return cmpList;
        }
        public string QCTestValueEntryCRUD(QCTestValueEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

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
                    objCmd.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cy.Drum;
                    objCmd.Parameters.Add("PRODDATE", OracleDbType.NVarchar2).Value = cy.Prodate;
                    objCmd.Parameters.Add("DSAMPLE", OracleDbType.NVarchar2).Value = cy.Sample;
                    objCmd.Parameters.Add("DSAMPLETIME", OracleDbType.NVarchar2).Value = cy.Sampletime;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("RATEPHR", OracleDbType.NVarchar2).Value = cy.Rate;
                    objCmd.Parameters.Add("NOZZLENO", OracleDbType.NVarchar2).Value = cy.Nozzle;
                    objCmd.Parameters.Add("AIRPRESS", OracleDbType.NVarchar2).Value = cy.Air;
                    objCmd.Parameters.Add("ADDCH", OracleDbType.NVarchar2).Value = cy.AddCharge;
                    objCmd.Parameters.Add("BCT", OracleDbType.NVarchar2).Value = cy.Ctemp;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
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
                                    if (cp.Isvalid == "Y" && cp.Description != "0")
                                    {

                                        svSQL = "Insert into QTVEDETAIL (QTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT) VALUES ('" + Pid + "','" + cp.Description + "','" + cp.Value + "','" + cp.Unit + "','" + cp.Startvalue + "','" + cp.Endvalue + "','" + cp.Test + "','" + cp.Manual + "','" + cp.Actual + "','" + cp.TestResult + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


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

                                    if (cp.Isvalid == "Y" && cp.Description != "0")
                                    {

                                        svSQL = "Insert into QTVEDETAIL (QTVEBASICID,TDESC,VALUEORMANUAL,UNIT,STARTVALUE,ENDVALUE,TESTVALUE,MANUALVALUE,ACTTESTVALUE,TESTRESULT) VALUES ('" + Pid + "','" + cp.Description + "','" + cp.Value + "','" + cp.Unit + "','" + cp.Startvalue + "','" + cp.Endvalue + "','" + cp.Test + "','" + cp.Manual + "','" + cp.Actual + "','" + cp.TestResult + "')";
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
            SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}
