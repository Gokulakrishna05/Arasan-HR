using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class ProcessMastService : IProcessMastService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProcessMastService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllProcessMast(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT PROCESSMASTID, PROCESSID, PROCESSNAME, PRODHRTYPE,BATCHORAVGCOST,IS_ACTIVE FROM PROCESSMAST WHERE PROCESSMAST.IS_ACTIVE = 'Y' ORDER BY PROCESSMASTID DESC ";

            }
            else
            {
                SvSql = "SELECT PROCESSMASTID,PROCESSID, PROCESSNAME, PRODHRTYPE,BATCHORAVGCOST,IS_ACTIVE FROM PROCESSMAST WHERE PROCESSMAST.IS_ACTIVE = 'N' ORDER BY PROCESSMASTID DESC ";
                SvSql = "SELECT PROCESSMASTID,PROCESSID, PROCESSNAME, PRODHRTYPE,BATCHORAVGCOST,IS_ACTIVE FROM PROCESSMAST WHERE PROCESSMAST.IS_ACTIVE = 'N' ORDER BY PROCESSMASTID DESC ";

            }

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditProcessMast(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSID, PROCESSNAME,BATCHYN,QCYN,SNO, PRODHRTYPE,BATCHORAVGCOST from PROCESSMAST where PROCESSMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditProcessDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSMASTID,PARAMETERS,UNIT,PARAMVALUE from PROCESSDETAIL where PROCESSMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetViewEditWrkDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSMASTID,WCID from PROCESSWC where PROCESSMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWc()
        {
            string SvSql = string.Empty;
            SvSql = "select WCBASICID,WCID from WCBASIC  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetUnit()
        {
            string SvSql = string.Empty;
            SvSql = "Select UNITID,UNITMASTID from UNITMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ProcessMastCRUD(ProcessMast cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PROCESSMASTPROC", objConn);


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

                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.ProcessMastName;
                    objCmd.Parameters.Add("PROCESSNAME", OracleDbType.NVarchar2).Value = cy.ProcessMastName;
                    objCmd.Parameters.Add("BATCHYN", OracleDbType.NVarchar2).Value = cy.Batch;
                    objCmd.Parameters.Add("QCYN", OracleDbType.NVarchar2).Value = cy.Qc;
                    objCmd.Parameters.Add("SNO", OracleDbType.NVarchar2).Value = cy.Sno;
                    objCmd.Parameters.Add("PRODHRTYPE", OracleDbType.NVarchar2).Value = cy.Prodhrtype;
                    objCmd.Parameters.Add("BATCHORAVGCOST", OracleDbType.NVarchar2).Value = cy.Costtype;
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
                        if (cy.Prolst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Promst cp in cy.Prolst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into PROCESSDETAIL (PROCESSMASTID,PARAMETERS,UNIT,PARAMVALUE) VALUES ('" + Pid + "','" + cp.para + "','" + cp.unit + "','" + cp.paraval + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete PROCESSDETAIL WHERE PROCESSMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Promst cp in cy.Prolst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PROCESSDETAIL (PROCESSMASTID,PARAMETERS,UNIT,PARAMVALUE) VALUES ('" + Pid + "','" + cp.para + "','" + cp.unit + "','" + cp.paraval + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        if (cy.wclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Wcid cp in cy.wclst)
                                {
                                    if (cp.Isvalid1 == "Y" && cp.wc != "0")
                                    {
                                        svSQL = "Insert into PROCESSWC (PROCESSMASTID,WCID) VALUES ('" + Pid + "','" + cp.wc + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
                            }
                            else
                            {
                                svSQL = "Delete PROCESSWC WHERE PROCESSMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Wcid cp in cy.wclst)
                                {
                                    if (cp.Isvalid1 == "Y" && cp.wc != "0")
                                    {
                                        svSQL = "Insert into PROCESSWC (PROCESSMASTID,WCID) VALUES ('" + Pid + "','" + cp.wc + "')";
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

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PROCESSMAST SET IS_ACTIVE ='N' WHERE PROCESSMASTID='" + id + "'";
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
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PROCESSMAST SET IS_ACTIVE ='Y' WHERE PROCESSMASTID='" + id + "'";
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
