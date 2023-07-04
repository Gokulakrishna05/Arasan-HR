using Arasan.Interface.Master;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace Arasan.Services
{
    public class EmailConfigService : IEmailConfig
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public EmailConfigService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<EmailConfig> GetAllEmailConfig(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }
            List<EmailConfig> cmpList = new List<EmailConfig>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {

                    con.Open();
                    cmd.CommandText = "Select EMAILCONFIG_ID,SMTP_HOST,PORT_NO,EMAIL_ID,PASSWORD,SSL,SIGNATURE from EMAIL_CONFIG  WHERE EMAIL_CONFIG.STATUS = '" + status + "' order by EMAILCONFIG_ID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                   
                    while (rdr.Read())
                    {
                        EmailConfig cmp = new EmailConfig
                        {
                            ID = rdr["EMAILCONFIG_ID"].ToString(),
                            SMTP = rdr["SMTP_HOST"].ToString(),
                            Port = rdr["PORT_NO"].ToString(),
                            Email = rdr["EMAIL_ID"].ToString(),
                            Password = rdr["PASSWORD"].ToString(),
                            SSL = rdr["SSL"].ToString(),
                            Signature = rdr["SIGNATURE"].ToString()
                            



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public string EmailConfigCRUD(EmailConfig ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(SMTP_HOST) as cnt FROM DRUMMAST WHERE DRUMNO = LTRIM(RTRIM('" + ss.SMTP + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "SMTP Already Existed";
                        return msg;
                    }
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EMAILCONFIGPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DRUM_PROCEDURE";*/

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

                    objCmd.Parameters.Add("SMTP_HOST", OracleDbType.NVarchar2).Value = ss.SMTP;
                    objCmd.Parameters.Add("PORT_NO", OracleDbType.NVarchar2).Value = ss.Port;
                    objCmd.Parameters.Add("EMAIL_ID", OracleDbType.NVarchar2).Value = ss.Email;
                    objCmd.Parameters.Add("PASSWORD", OracleDbType.NVarchar2).Value = ss.Password;
                    objCmd.Parameters.Add("SSL", OracleDbType.NVarchar2).Value = ss.SSL;
                    objCmd.Parameters.Add("SIGNATURE", OracleDbType.NVarchar2).Value = ss.Signature;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
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

        public DataTable GetEmailConfig(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EMAILCONFIG_ID,SMTP_HOST,PORT_NO,EMAIL_ID,PASSWORD,SSL,SIGNATURE from EMAIL_CONFIG  where EMAILCONFIG_ID = '" + id + "' ";
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
                    svSQL = "UPDATE EMAIL_CONFIG SET STATUS ='INACTIVE' WHERE EMAILCONFIG_ID ='" + id + "'";
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
                    svSQL = "UPDATE EMAIL_CONFIG SET STATUS ='ACTIVE' WHERE EMAILCONFIG_ID ='" + id + "'";
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
