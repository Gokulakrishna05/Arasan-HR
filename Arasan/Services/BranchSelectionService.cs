using Arasan.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class BranchSelectionService : IBranchSelectionService
    {
        private readonly string _connectionString;
        public BranchSelectionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetBranch( )
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST where STATUS='ACTIVE'   order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation(string branch)
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCDETAILS.LOCID ,EMPLOYEELOCATION.LOCID loc from EMPLOYEELOCATION left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=EMPLOYEELOCATION.LOCID  Where EMPID='" + branch + "' GROUP BY LOCDETAILS.LOCID,EMPLOYEELOCATION.LOCID";
            //if(branch != "" || branch != "0")
            //{
            //    SvSql += " Where BRANCHID='" + branch + "'";
            //}
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
