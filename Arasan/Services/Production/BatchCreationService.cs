using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class BatchCreationService : IBatchCreation
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BatchCreationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<BatchCreation> GetAllBatchCreation()
        {
            List<BatchCreation> cmpList = new List<BatchCreation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select   BRANCHMAST.BRANCHID,DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,BCPRODBASICID from BCPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BCPRODBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BatchCreation cmp = new BatchCreation
                        {

                            ID = rdr["BCPRODBASICID"].ToString(),

                            Branch = rdr["BRANCHID"].ToString(),
                            WorkCenter = rdr["WCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),

                            DocDate = rdr["DOCDATE"].ToString(),
                          

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
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
        public DataTable GetProcess(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID,PROCESSMAST.PROCESSID from WCBASIC LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=WCBASIC.PROCESSID where WCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
