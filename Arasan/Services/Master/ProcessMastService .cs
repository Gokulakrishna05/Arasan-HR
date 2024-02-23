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
                SvSql = "SELECT PROCESSMASTID, PROCESSID, PROCESSNAME, PRODHRTYPE,BATCHORAVGCOST FROM PROCESSMAST WHERE PROCESSMAST.IS_ACTIVE = 'Y' ORDER BY PROCESSMASTID DESC ";

            }
            else
            {
                SvSql = "SELECT PROCESSMASTID,PROCESSID, PROCESSNAME, PRODHRTYPE,BATCHORAVGCOST FROM PROCESSMAST WHERE PROCESSMAST.IS_ACTIVE = 'N' ORDER BY PROCESSMASTID DESC ";
                SvSql = "SELECT PROCESSMASTID,PROCESSID, PROCESSNAME, PRODHRTYPE,BATCHORAVGCOST FROM PROCESSMAST WHERE PROCESSMAST.IS_ACTIVE = 'N' ORDER BY PROCESSMASTID DESC ";

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

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {

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
    }
}
