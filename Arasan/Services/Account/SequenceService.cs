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
        public SequenceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Sequence> GetAllSequence()
        {
            List<Sequence> cmpList = new List<Sequence>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select PREFIX,TRANSTYPE,DESCRIPTION,LASTNO,to_char(STDT,'dd-MON-yyyy')STDT,to_char(EDDT,'dd-MON-yyyy')EDDT,SEQUENCEID from SEQUENCE";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Sequence cmp = new Sequence
                        {
                            ID = rdr["SEQUENCEID"].ToString(),
                            Prefix = rdr["PREFIX"].ToString(),
                            Trans = rdr["TRANSTYPE"].ToString(),
                            Des = rdr["DESCRIPTION"].ToString(),
                            Start = rdr["STDT"].ToString(),
                            End = rdr["EDDT"].ToString(),
                            Last = rdr["LASTNO"].ToString(),

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
                    objCmd.Parameters.Add("LASTNO", OracleDbType.NVarchar2).Value = cy.Last;
                    objCmd.Parameters.Add("STDT", OracleDbType.Date).Value = DateTime.Parse(cy.Start);
                    objCmd.Parameters.Add("EDDT", OracleDbType.Date).Value = DateTime.Parse(cy.End);
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
            SvSql = "Select PREFIX,TRANSTYPE,DESCRIPTION,LASTNO,to_char(STDT,'dd-MON-yyyy')STDT,to_char(EDDT,'dd-MON-yyyy')EDDT,SEQUENCEID from SEQUENCE where SEQUENCEID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
