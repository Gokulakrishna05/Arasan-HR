using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class StopService : IStop
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StopService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

      

        public string StopCRUD(Stop ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(STOPID) as cnt FROM STOPMAST WHERE STOPID = LTRIM(RTRIM('" + ss.SID + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "STOPID Already Existed";
                        return msg;
                    }

                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("STOPPROC", objConn);

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
                    objCmd.Parameters.Add("STOPID", OracleDbType.NVarchar2).Value = ss.SID;
                    objCmd.Parameters.Add("STOPDESC", OracleDbType.NVarchar2).Value = ss.SDESC;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";

                    if (ss.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = ss.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = ss.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

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

        public DataTable GetviewStop(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select STOPID,STOPDESC,STOPMASTID,IS_ACTIVE from STOPMAST WHERE STOPMAST.STOPMASTID = " + id + "";
           DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStop(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select STOPID,STOPDESC from STOPMAST where STOPMASTID = '" + id + "' ";
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
                    svSQL = "UPDATE STOPMAST SET IS_ACTIVE ='N' WHERE STOPMASTID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE STOPMAST SET IS_ACTIVE ='Y' WHERE STOPMASTID='" + id + "'";
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

        public DataTable GetAllStop(string strStatus)    
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select STOPID,STOPDESC,STOPMASTID,IS_ACTIVE from STOPMAST WHERE STOPMAST.IS_ACTIVE = 'Y' ORDER BY STOPMASTID DESC";
            }
            else
            {
                SvSql = "select STOPID,STOPDESC,STOPMASTID,IS_ACTIVE from STOPMAST WHERE STOPMAST.IS_ACTIVE = 'N' ORDER BY STOPMASTID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }


}

