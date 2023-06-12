using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services
{
    public class WorkOrderShortCloseService : IWorkOrderShortClose
    {

        private readonly string _connectionString;
        DataTransactions datatrans;

        public WorkOrderShortCloseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public DataTable GetWorkOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select JOBASIC.DOCID,JOBASIC.BRANCHID,LOCDETAILS.LOCID,PARTYRCODE.PARTY,ORDTYPE from JOBASIC left Outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')  And JOBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
