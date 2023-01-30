using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class AccGroupService : IAccGroup
    {
        private readonly string _connectionString;
        public AccGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<AccGroup> GetAllAccGroup()
        {
            List<AccGroup> cmpList = new List<AccGroup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,DOCID,PMNAME,UNIQUEID,CPMNAME,ACCGRBASICID from ACCGRBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ACCGRBASIC.BRANCHID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccGroup cmp = new AccGroup
                        {
                            ID = rdr["ACCGRBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            CPMName = rdr["CPMNAME"].ToString(),
                            PmName = rdr["PMNAME"].ToString(),
                            Unique = rdr["UNIQUEID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                           

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string AccGroupCRUD(AccGroup cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACCGROUPPROC", objConn);
                  

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

                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("CPMNAME", OracleDbType.NVarchar2).Value = cy.CPMName;
                    objCmd.Parameters.Add("PMNAME", OracleDbType.NVarchar2).Value = cy.PmName;
                      objCmd.Parameters.Add("UNIQUEID", OracleDbType.NVarchar2).Value = cy.Unique;
                 
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
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
        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,CPMNAME,DOCID,PMNAME,UNIQUEID,ACCGRBASICID  from ACCGRBASIC where ACCGRBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
