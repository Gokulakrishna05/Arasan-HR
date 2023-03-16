using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services 
{
    public class ProductionLogService : IProductionLog
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductionLogService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<ProductionLog> GetAllProductionLog()
        {
            List<ProductionLog> cmpList = new List<ProductionLog>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,LPRODBASIC. DOCID,to_char(LPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,LPRODBASICID from LPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=LPRODBASIC.BRANCH LEFT OUTER JOIN WCBASIC ON WCBASICID=LPRODBASIC.WCID ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProductionLog cmp = new ProductionLog
                        {

                            ID = rdr["LPRODBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            WorkId = rdr["WCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                           Docdate = rdr["DOCDATE"].ToString()





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
        public DataTable GetReason()
        {
            string SvSql = string.Empty;
            SvSql = "Select REASON,REASONDETAILID from REASONDETAIL ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
        public DataTable BindProcess()
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSID,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
