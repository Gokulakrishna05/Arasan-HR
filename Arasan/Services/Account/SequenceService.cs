using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class SequenceService : ISequence
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SequenceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<Sequence> GetAllSequence()
        {
            List<Sequence> cmpList = new List<Sequence>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select PREFIX,TRANSTYPE,DESCRIPTION,LASTNO,to_char(STDATE,'dd-MON-yyyy')STDATE,to_char(EDDATE,'dd-MON-yyyy')EDDATE,SEQUENCEID from SEQUENCE where ACTIVESEQUENCE='T' ORDER BY SEQUENCEID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Sequence cmp = new Sequence
                        {
                            ID = rdr["SEQUENCEID"].ToString(),
                            Prefix = rdr["PREFIX"].ToString(),
                            Trans = rdr["TRANSTYPE"].ToString(),
                            Des = rdr["DESCRIPTION"].ToString(),
                            Start = rdr["STDATE"].ToString(),
                            End = rdr["EDDATE"].ToString(),
                            Last = rdr["LASTNO"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string SequenceCRUD(Sequence cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM SEQUENCE WHERE PREFIX =LTRIM(RTRIM('" + cy.Prefix + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "SEQUENCE Already Existed";
                        return msg;
                    }
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SEQUENCEPROC", objConn); 


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

                    objCmd.Parameters.Add("PREFIX", OracleDbType.NVarchar2).Value = cy.Prefix;
                    objCmd.Parameters.Add("TRANSTYPE", OracleDbType.NVarchar2).Value = cy.Trans;
                    objCmd.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = cy.Des;
                   
                    objCmd.Parameters.Add("STDATE", OracleDbType.NVarchar2).Value = cy.Start; 
                    objCmd.Parameters.Add("EDDATE", OracleDbType.NVarchar2).Value = cy.End;
                    objCmd.Parameters.Add("LASTNO", OracleDbType.NVarchar2).Value = cy.Last;
                    objCmd.Parameters.Add("ACTIVESEQUENCE", OracleDbType.NVarchar2).Value = "T";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
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
        public DataTable GetSequence(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PREFIX,TRANSTYPE,DESCRIPTION,LASTNO,to_char(STDATE,'dd-MON-yyyy')STDATE,to_char(EDDATE,'dd-MON-yyyy')EDDATE,SEQUENCEID from SEQUENCE where SEQUENCEID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllSeq(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "T" || strStatus == null)
            {
                SvSql = "Select PREFIX,TRANSTYPE,DESCRIPTION,LASTNO,to_char(STDATE,'dd-MON-yyyy')STDATE,to_char(EDDATE,'dd-MON-yyyy')EDDATE,SEQUENCEID,ACTIVESEQUENCE from SEQUENCE  WHERE SEQUENCE.ACTIVESEQUENCE = 'T' ORDER BY SEQUENCEID DESC";
            }
            else
            {
                SvSql = "Select PREFIX,TRANSTYPE,DESCRIPTION,LASTNO,to_char(STDATE,'dd-MON-yyyy')STDATE,to_char(EDDATE,'dd-MON-yyyy')EDDATE,SEQUENCEID,ACTIVESEQUENCE from SEQUENCE  WHERE SEQUENCE.ACTIVESEQUENCE = 'F' ORDER BY SEQUENCEID DESC";

            }
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
                    svSQL = "UPDATE SEQUENCE SET ACTIVESEQUENCE ='F' WHERE SEQUENCEID='" + id + "'";
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
                    svSQL = "UPDATE SEQUENCE SET ACTIVESEQUENCE ='T' WHERE SEQUENCEID='" + id + "'";
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
