using Arasan.Interface.Master;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Production
{
    public class ReasonCodeService : IReasonCodeService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ReasonCodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        //public IEnumerable<ReasonCode> GetAllReasonCode()
        //{
        //    List<ReasonCode> cmpList = new List<ReasonCode>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = " Select MODBY,REASONBASICID from REASONBASIC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ReasonCode cmp = new ReasonCode
        //                {

        //                    ID = rdr["REASONBASICID"].ToString(),
        //                    ModBy = rdr["MODBY"].ToString(),
                           

        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}

        public DataTable GetReasonCode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSID ,REASONBASICID from REASONBASIC  where REASONBASICID= '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetReasonItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select REASON,RTYPE,DESCRIPTION,STOPID from REASONDETAIL where REASONBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getstop()
        {
            string SvSql = string.Empty;
            SvSql = "select STOPDESC,STOPMASTID from STOPMAST order by STOPMASTID desc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getprocess()
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSMASTID,PROCESSID from PROCESSMAST order by PROCESSMASTID desc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ReasonCodeCRUD(ReasonCode cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("REASONBASICPROC", objConn);

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

                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value ="Y";
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

                        foreach (ReasonItem cp in cy.ReLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Reason != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("REASONDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("REASONBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = cp.Reason;
                                    objCmds.Parameters.Add("RTYPE", OracleDbType.NVarchar2).Value = cp.Category;
                                    objCmds.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = cp.Description;
                                    objCmds.Parameters.Add("STOPID", OracleDbType.NVarchar2).Value = cp.GroupId;
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


        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE REASONBASIC SET IS_ACTIVE ='N' WHERE REASONBASICID='" + id + "'";
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
                    svSQL = "UPDATE REASONBASIC SET IS_ACTIVE ='Y' WHERE REASONBASICID='" + id + "'";
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
        public DataTable GetAllReason(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select PROCESSMAST.PROCESSID,REASONBASICID,IS_ACTIVE from REASONBASIC LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID = REASONBASIC.REASONBASICID  where REASONBASIC.IS_ACTIVE='Y' ORDER BY REASONBASICID DESC ";

            }
            else
            {
                SvSql = "select PROCESSMAST.PROCESSID,REASONBASICID,IS_ACTIVE from REASONBASIC LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID = REASONBASIC.REASONBASICID  where REASONBASIC.IS_ACTIVE='N' ORDER BY REASONBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
